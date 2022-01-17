using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using RestSharp;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;
using TenorSharp.SearchResults;

using Type = TenorSharp.Enums.Type;

// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace TenorSharp;

/// <summary>
/// A Client for the Tenor API
/// </summary>
public partial class TenorClient
{
	private const string BaseUri = "https://g.tenor.com/v1/";


	private readonly RestClient _client;


	/// <summary>
	/// initializes a new Tenor Client
	/// </summary>
	/// <param name="apiKey">client key for privileged API access</param>
	/// <param name="locale">specify default language to interpret search string</param>
	/// <param name="arRange">Filter the response <see cref="GifObject"/> list to only include GIFs with aspect ratios that fit with in the selected range.</param>
	/// <param name="contentFilter">specify the content safety filter level</param>
	/// <param name="mediaFilter">Reduce the Number of GIF formats returned in the <see cref="GifObject"/> list.</param>
	/// <param name="anonId">specify the anonymous_id tied to the given user</param>
	/// <param name="testClient">a custom HttpClient object to use instead of the default, mainly for testing purposes.</param>
	public TenorClient(
		string        apiKey,
		Locale        locale        = null,
		AspectRatio   arRange       = AspectRatio.all,
		ContentFilter contentFilter = ContentFilter.off,
		MediaFilter   mediaFilter   = MediaFilter.off,
		string        anonId        = null,
		HttpClient    testClient    = null
	) : this(new TenorConfiguration
			 {
				 ApiKey        = apiKey,
				 AnonId        = anonId,
				 ArRange       = arRange,
				 ContentFilter = contentFilter,
				 MediaFilter   = mediaFilter,
				 Locale        = locale ?? new Locale()
			 },
		testClient)
	{
	}

	/// <summary>
	/// initializes a new Tenor Client
	/// </summary>
	/// <param name="configuration">an object containing the configuration for the Client</param>
	/// <param name="testClient">a custom HttpClient object to use instead of the default, mainly for testing purposes.</param>
	public TenorClient(TenorConfiguration configuration = default, HttpClient testClient = null)
	{
		Configuration = configuration ?? new TenorConfiguration();
		var options = new RestClientOptions { ThrowOnDeserializationError = true, BaseUrl = new Uri(BaseUri) };
		_client = testClient == null ? new RestClient(options) : new RestClient(testClient, options);


		_client = _client.AddDefaultParameter("key",     Configuration.ApiKey);
		_client = _client.AddDefaultParameter("anon_id", Configuration.AnonId);
	}


	/// <summary>
	///     Gets a File as a Stream from a URL
	/// </summary>
	/// <param name="url">the URL</param>
	/// <returns>the Stream</returns>

	// ReSharper disable once MemberCanBePrivate.Global
	public static async Task<Stream> GetMediaStreamAsync(Uri url)
	{
		var req    = WebRequest.Create(url);
		var stream = (await req.GetResponseAsync()).GetResponseStream();
		return stream;
	}


	#region Configuration

	/// <summary>
	/// an object containing the configuration for the Client
	/// </summary>
	public TenorConfiguration Configuration { get; }

	/// <summary>
	/// Modifies the Content Filter of the Client
	/// </summary>
	public ContentFilter ContentFilter
	{
		get => Configuration.ContentFilter;
		set => Configuration.ContentFilter = value;
	}

	/// <summary>
	/// Modifies the Media Filter of the Client
	/// </summary>
	public MediaFilter MediaFilter
	{
		get => Configuration.MediaFilter;
		set => Configuration.MediaFilter = value;
	}

	/// <summary>
	/// Modifies the Aspect Ratio Range of the Client
	/// </summary>
	public AspectRatio AspectRatioRange
	{
		get => Configuration.ArRange;
		set => Configuration.ArRange = value;
	}


	#region Locale

	/// <summary>
	/// Modifies the Locale of the Client
	/// </summary>
	public Locale Locale
	{
		get => Configuration.Locale;
		set => Configuration.Locale = value;
	}

	/// <summary>
	/// Resets the Clients Locale to en_us
	/// </summary>
	public void ResetLocale() => Configuration.Locale = new Locale();

	#endregion

	#region Sessions

	/// <summary>
	///     Start new Session with an Anonymous ID
	/// </summary>
	/// <param name="anonId">the Anonymous ID</param>
	/// <exception cref="TenorException">Thrown when anonId is invalid</exception>TODO
	public void NewSession(string anonId) => Configuration.AnonId = anonId.Length switch
	{
		>= 16 and <= 32 => anonId,
		var _ => throw new TenorException("Anon_id must be between 16 and 32 characters.", 1)
	};

	/// <summary>
	/// Returns the Current Anonymous ID
	/// </summary>
	/// <returns>the current anonymous ID</returns>
	public string GetSession() => Configuration.AnonId;

	/// <summary>
	/// Clears the current Anonymous ID
	/// </summary>
	public void ClearSession() => Configuration.AnonId = null;

	#endregion

	#endregion

	#region API Calls

	//TODO: Add Sticker Search
	/// <summary>
	///     Get a json object containing a list of the most relevant GIFs for a given search term(s), category(ies), emoji(s),
	///     or any combination thereof.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <param name="pos">
	///     get results starting at position "value".
	///     Use a non-zero "next" value returned by API results to get the next set of results.
	///     pos is not an index and may be an integer, float, or string
	/// </param>
	/// <returns>a Tenor Gif Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Gif> SearchAsync(string q, int limit = 20, string pos = "0")
	{
		var request = new RestRequest(Endpoints.Search)
					 .AddParameter("q",             q)
					 .AddParameter("limit",         limit)
					 .AddParameter("pos",           pos == "" ? "0" : pos)
					 .AddParameter("ar_range",      Configuration.ArRange)
					 .AddParameter("contentfilter", Configuration.ContentFilter)
					 .AddParameter("locale",        Configuration.Locale, ParameterType.QueryString);
		if (Configuration.MediaFilter != MediaFilter.off)
			request.AddParameter("media_filter", Configuration.MediaFilter);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);

		try
		{
			var result = await _client.GetAsync<Gif>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			result.Client = this;
			result.Term   = q;
			result.Count  = limit;
			result.Type   = SearchTypes.search;
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	//TODO: Add Trending Stickers
	/// <summary>
	///     Get a json object containing a list of the current global trending GIFs. The trending stream is updated regularly
	///     throughout the day.
	/// </summary>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <param name="pos">
	///     get results starting at position "value".
	///     Use a non-zero "next" value returned by API results to get the next set of results.
	///     pos is not an index and may be an integer, float, or string
	/// </param>
	/// <returns>a Tenor Gif Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Gif> TrendingAsync(int limit = 20, string pos = "0")
	{
		var request = new RestRequest(Endpoints.Trending)
					 .AddParameter("limit",         limit)
					 .AddParameter("pos",           pos == "" ? "0" : pos)
					 .AddParameter("ar_range",      Configuration.ArRange)
					 .AddParameter("contentfilter", Configuration.ContentFilter)
					 .AddParameter("locale",        Configuration.Locale, ParameterType.QueryString);
		if (Configuration.MediaFilter != MediaFilter.off)
			request.AddParameter("media_filter", Configuration.MediaFilter);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);
		try
		{
			var result = await _client.GetAsync<Gif>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			result.Client = this;
			result.Count  = limit;
			result.Type   = SearchTypes.trending;
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get a json object containing a list of GIF categories associated with the provided type.
	///     Each category will include a corresponding search URL to be used if the user clicks on the category.
	///     The search URL will include the apikey, anonymous id, and locale that were used on the original call to the
	///     categories endpoint.
	/// </summary>
	/// <param name="type">determines the type of categories returned</param>
	/// <returns>a Tenor Category Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Category> CategoriesAsync(Type type = Type.featured)
	{
		var request = new RestRequest(Endpoints.Categories)
					 .AddParameter("type",          type)
					 .AddParameter("contentfilter", Configuration.ContentFilter)
					 .AddParameter("locale",        Configuration.Locale, ParameterType.QueryString);


		try
		{
			var result = await _client.GetAsync<Category>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get a json object containing a list of alternative search terms given a search term.
	///     SearchAsync suggestions helps a user narrow their search or discover related search terms to find a more precise GIF.
	///     Results are returned in order of what is most likely to drive a share for a given term, based on historic user
	///     search and share behavior.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of Search Terms</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Terms> SearchSuggestionsAsync(string q, int limit = 20)
	{
		var request = new RestRequest(Endpoints.SearchSuggestions)
					 .AddParameter("q",      q)
					 .AddParameter("limit",  limit)
					 .AddParameter("locale", Configuration.Locale, ParameterType.QueryString);

		if (Configuration.MediaFilter != MediaFilter.off)
			request.AddParameter("media_filter", Configuration.MediaFilter);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);


		try
		{
			var result = await _client.GetAsync<Terms>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get a json object containing a list of completed search terms given a partial search term.
	///     The list is sorted by Tenor’s AI and the number of results will decrease as Tenor’s AI becomes more certain.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of <see cref="Terms">Search Terms</see></returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Terms> AutoCompleteAsync(string q, int limit = 20)
	{
		var request = new RestRequest(Endpoints.Autocomplete)
					 .AddParameter("q",      q)
					 .AddParameter("limit",  limit)
					 .AddParameter("locale", Configuration.Locale, ParameterType.QueryString);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);

		try
		{
			var result = await _client.GetAsync<Terms>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get a json object containing a list of the current trending search terms.
	///     The list is updated Hourly by Tenor’s AI.
	/// </summary>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of Search Terms</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	/// <exception cref="System.Exception">thrown when the Tenor API returns Invalid Data</exception>TODO
	public async Task<Terms> TrendingTermsAsync(int limit = 20)
	{
		var request = new RestRequest(Endpoints.TrendingTerms)
					 .AddParameter("limit",  limit)
					 .AddParameter("locale", Configuration.Locale, ParameterType.QueryString);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);
		try
		{
			var result = await _client.GetAsync<Terms>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Register a user’s sharing of a GIF.
	/// </summary>
	/// <param name="id">the “id” of a GIF_OBJECT</param>
	/// <param name="q">The search string that lead to this share</param>
	/// <returns>the Share Status</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<string> RegisterShareAsync(string id, string q = null)
	{
		if (string.IsNullOrEmpty(id))
			throw new ArgumentNullException(nameof(id), "Supplied ID was null or empty.");
		var request = new RestRequest(Endpoints.RegisterShare)
					 .AddParameter("id",     id)
					 .AddParameter("locale", Configuration.Locale, ParameterType.QueryString);

		if (q != null)
			request.AddParameter("q", q);

		try
		{
			var result = await _client.GetAsync<Register>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			return result.ShareStatus;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get the GIF(s) for the corresponding id(s)
	/// </summary>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <param name="pos">
	///     get results starting at position "value".
	///     Use a non-zero "next" value returned by API results to get the next set of results.
	///     pos is not an index and may be an integer, float, or string
	/// </param>
	/// <param name="ids">a comma separated list of GIF IDs (max: 50)</param>
	/// <returns>a Tenor Gif Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Gif> GetGifsAsync(
		int             limit = 20,
		string          pos   = "0",
		params string[] ids
	)
	{
		ids = ids.Where(s => !string.IsNullOrEmpty(s)).ToArray();
		if (ids.Length == 0)
			throw new ArgumentNullException(nameof(ids), "All IDs supplied were null or empty.");
		var request = new RestRequest(Endpoints.Gifs)
					 .AddParameter("ids",           string.Join(',', ids))
					 .AddParameter("limit",         limit).AddParameter("pos", pos == "" ? "0" : pos)
					 .AddParameter("ar_range",      Configuration.ArRange)
					 .AddParameter("contentfilter", Configuration.ContentFilter)
					 .AddParameter("locale",        Configuration.Locale, ParameterType.QueryString);
		if (Configuration.MediaFilter != MediaFilter.off)
			request.AddParameter("media_filter", Configuration.MediaFilter);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);

		try
		{
			var result = await _client.GetAsync<Gif>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");
			result.Client = this;
			result.Ids    = ids;
			result.Count  = limit;
			result.Type   = SearchTypes.search;
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get an anonymous ID for a new user.
	///     Store the ID in the client's cache for use on any additional API calls made by the user, either in this session or
	///     any future sessions.
	/// </summary>
	/// <returns>a Random AnonId</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<string> GetNewAnonIdAsync()
	{
		var request = new RestRequest(Endpoints.AnonId);
		try
		{
			var result = await _client.GetAsync<Session>(request);
			if (result == null)
				throw new NullReferenceException("API Returned null");

			return result.AnonId;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}


	/// <summary>
	///     Get a randomized list of GIFs for a given search term. This differs from the search endpoint which returns a rank
	///     ordered list of GIFs for a given search term.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <param name="pos">
	///     get results starting at position "value".
	///     Use a non-zero "next" value returned by API results to get the next set of results.
	///     pos is not an index and may be an integer, float, or string
	/// </param>
	/// <returns>a Tenor Gif Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>TODO
	public async Task<Gif> GetRandomGifsAsync(string q, int limit = 20, string pos = "0")
	{
		var request = new RestRequest(Endpoints.Random)
					 .AddParameter("q",             q)
					 .AddParameter("limit",         limit)
					 .AddParameter("pos",           pos == "" ? "0" : pos)
					 .AddParameter("ar_range",      Configuration.ArRange)
					 .AddParameter("contentfilter", Configuration.ContentFilter)
					 .AddParameter("locale",        Configuration.Locale, ParameterType.QueryString);
		if (Configuration.MediaFilter != MediaFilter.off)
			request.AddParameter("media_filter", Configuration.MediaFilter);
		if (limit is < 1 or > 50)
			throw new TenorException("Limit must be between 1 and 50.", 1);
		try
		{
			var response = await _client.ExecuteAsync<Gif>(request);
			var result   = response.Data;
			if (result == null)
				throw new NullReferenceException("API Returned null");
			result.Client = this;
			result.Term   = q;
			result.Count  = limit;
			result.Type   = SearchTypes.search;
			return result;
		}
		catch (DeserializationException e)
		{
			if (e.Response.Content == null || !e.Response.Content.Contains("\"error\""))
				throw;
			var tenorException = _client.Deserialize<TenorException>(e.Response).Data;
			if (tenorException != null)
				throw tenorException;
			throw;
		}
	}

	#endregion
}