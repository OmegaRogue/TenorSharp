using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using System.Text.Json;

using RestSharp;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;
using TenorSharp.SearchResults;

using Type = TenorSharp.Enums.Type;

// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace TenorSharp;

public class TenorClient 
{
	private const string BaseUri = "https://tenor.googleapis.com/v2/";

	private static readonly Locale DefaultLocale = new("en_US");

	private readonly RestRequest _anonIdRequest = new(Endpoints.AnonId);

	/// <summary>
	///     client key for privileged API access
	/// </summary>
	private readonly string _apiKey;


	private readonly RestRequest _autocompleteRequest =
		new(Endpoints.Autocomplete);

	private readonly RestRequest _categoryRequest =
		new(Endpoints.Categories);

	private readonly RestClient _client;

	private readonly RestRequest _gifsRequest = new(Endpoints.Gifs);

	private readonly RestRequest _rndGifRequest = new(Endpoints.Random);

	private readonly RestRequest _searchRequest = new(Endpoints.Search);

	private readonly RestRequest _shareRequest =
		new(Endpoints.RegisterShare);

	private readonly RestRequest _suggestionRequest =
		new(Endpoints.SearchSuggestions);

	private readonly RestRequest _trendingRequest =
		new(Endpoints.Trending);

	private readonly RestRequest _trendingTermsRequest =
		new(Endpoints.TrendingTerms);

	/// <summary>
	///     specify the anonymous_id tied to the given user
	/// </summary>
	private string _anonId;

	/// <summary>
	///     Filter the response GIF_OBJECT list to only include GIFs with aspect ratios that fit with in the selected range.
	/// </summary>
	private AspectRatio _arRange;

	/// <summary>
	///     specify the content safety filter level
	/// </summary>
	private ContentFilter _contentFilter;

	/// <summary>
	///     specify default language to interpret search string
	/// </summary>
	private Locale _locale;

	/// <summary>
	///     Reduce the Number of GIF formats returned in the GifObject list.
	/// </summary>
	private MediaFilter _mediaFilter;


	public TenorClient(
		string        apiKey,
		Locale        locale        = null,
		AspectRatio   arRange       = AspectRatio.all,
		ContentFilter contentFilter = ContentFilter.off,
		MediaFilter   mediaFilter   = MediaFilter.off,
		string        anonId        = null,
		RestClient    testClient    = null
	)
	{
		_locale = locale     ?? DefaultLocale;
		_client = testClient ?? new RestClient(BaseUri);

		_arRange       = arRange;
		_contentFilter = contentFilter;
		_mediaFilter   = mediaFilter;
		_apiKey        = apiKey;

		_anonId = anonId;

		_client = _client.AddDefaultParameter("key", apiKey, ParameterType.QueryString);
	}

	public TenorClient(TenorConfiguration configuration, RestClient testClient = null) : this(configuration.ApiKey,
		configuration.Locale,
		configuration.ArRange,
		configuration
		   .ContentFilter,
		configuration
		   .MediaFilter,
		configuration.AnonId,
		testClient)
	{
	}


	/// <inheritdoc cref="SearchAsync(string,int,int)"/>
	public Gif Search(string q, int limit = 20, int pos = 0) => Task.Run(() => SearchAsync(q, limit, pos)).GetAwaiter().GetResult();
	
	/// <inheritdoc cref="SearchAsync(string,int,string)"/>
	public async Task<Gif> SearchAsync(string q, int limit = 20, int pos = 0)
		=> await SearchAsync(q, limit, pos.ToString());
	
	/// <inheritdoc cref="SearchAsync(string,int,string)"/>
	public Gif Search(string q, int limit = 20, string pos = "0") => Task.Run(() => SearchAsync(q, limit, pos)).GetAwaiter().GetResult();
	
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
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Gif> SearchAsync(string q, int limit = 20, string pos = "0")
	{
		_searchRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
					  .AddOrUpdateParameter("limit",         limit,          ParameterType.QueryString)
					  .AddOrUpdateParameter("pos",           pos,            ParameterType.QueryString)
					  .AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
					  .AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
					  .AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
		if (_anonId != null)
			_searchRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
		if (_mediaFilter != MediaFilter.off)
			_searchRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);


		var result  = await _client.ExecuteAsync(_searchRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			var res = JsonSerializer.Deserialize<Gif>(content);
			if (res == null)
				throw new Exception("API Returned null");
			res.Client = this;
			res.Term   = q;
			res.Count  = limit;
			res.Type   = SearchTypes.search;
			return res;
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}
	
	
	/// <inheritdoc cref="TrendingAsync(int, string)"/>
	public Gif Trending(int limit = 20, string pos = "0") => Task.Run(() => TrendingAsync(limit, pos)).GetAwaiter().GetResult();

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
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Gif> TrendingAsync(int limit = 20, string pos = "0")
	{
		_trendingRequest.AddOrUpdateParameter("limit", limit, ParameterType.QueryString)
						.AddOrUpdateParameter("pos",           pos,            ParameterType.QueryString)
						.AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
						.AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
						.AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
		if (_anonId != null)
			_trendingRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
		if (_mediaFilter != MediaFilter.off)
			_trendingRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);
		var result  = await _client.ExecuteAsync(_trendingRequest);
		var content = result.Content;

		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			var res = JsonSerializer.Deserialize<Gif>(content);
			if (res == null)
				throw new Exception("API Returned null");
			res.Client = this;
			res.Count  = limit;
			res.Type   = SearchTypes.trending;
			return res;
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}
	
	/// <inheritdoc cref="CategoriesAsync"/>
	public Category Categories(Type type = Type.featured) => Task.Run(() => CategoriesAsync(type)).GetAwaiter().GetResult();

	/// <summary>
	///     Get a json object containing a list of GIF categories associated with the provided type.
	///     Each category will include a corresponding search URL to be used if the user clicks on the category.
	///     The search URL will include the apikey, anonymous id, and locale that were used on the original call to the
	///     categories endpoint.
	/// </summary>
	/// <param name="type">determines the type of categories returned</param>
	/// <returns>a Tenor Category Response</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Category> CategoriesAsync(Type type = Type.featured)
	{
		_categoryRequest.AddOrUpdateParameter("Type", type, ParameterType.QueryString)
						.AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
						.AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
		if (_anonId != null)
			_categoryRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);

		var result  = await _client.ExecuteAsync(_categoryRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Category>(content);
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}
	
	/// <inheritdoc cref="SearchSuggestionsAsync"/>
	public Terms SearchSuggestions(string q, int limit = 20) => Task.Run(() => SearchSuggestionsAsync(q, limit)).GetAwaiter().GetResult();

	/// <summary>
	///     Get a json object containing a list of alternative search terms given a search term.
	///     SearchAsync suggestions helps a user narrow their search or discover related search terms to find a more precise GIF.
	///     Results are returned in order of what is most likely to drive a share for a given term, based on historic user
	///     search and share behavior.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of Search Terms</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Terms> SearchSuggestionsAsync(string q, int limit = 20)
	{
		_suggestionRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
						  .AddOrUpdateParameter("limit",  limit,   ParameterType.QueryString)
						  .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);
		if (_anonId != null)
			_suggestionRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
		if (_mediaFilter != MediaFilter.off)
			_suggestionRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);


		var result  = await _client.ExecuteAsync(_suggestionRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Terms>(content);
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}
	
	/// <inheritdoc cref="AutoCompleteAsync"/>
	public Terms AutoComplete(string q, int limit = 20) => Task.Run(() => AutoCompleteAsync(q, limit)).GetAwaiter().GetResult();

	/// <summary>
	///     Get a json object containing a list of completed search terms given a partial search term.
	///     The list is sorted by Tenor’s AI and the number of results will decrease as Tenor’s AI becomes more certain.
	/// </summary>
	/// <param name="q">a search string</param>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of Search Terms</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Terms> AutoCompleteAsync(string q, int limit = 20)
	{
		_autocompleteRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
							.AddOrUpdateParameter("limit",  limit,   ParameterType.QueryString)
							.AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

		if (_anonId != null)
			_autocompleteRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);

		var result  = await _client.ExecuteAsync(_autocompleteRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Terms>(content);
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}

	/// <inheritdoc cref="TrendingTermsAsync"/>
	public Terms TrendingTerms(int limit = 20) => Task.Run(() => TrendingTermsAsync(limit)).GetAwaiter().GetResult();

	/// <summary>
	///     Get a json object containing a list of the current trending search terms.
	///     The list is updated Hourly by Tenor’s AI.
	/// </summary>
	/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
	/// <returns>an Array of Search Terms</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	/// <exception cref="Exception">thrown when the Tenor API returns Invalid Data</exception>
	public async Task<Terms> TrendingTermsAsync(int limit = 20)
	{
		_trendingTermsRequest.AddOrUpdateParameter("limit", limit, ParameterType.QueryString)
							 .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

		if (_anonId != null)
			_trendingTermsRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);


		var result  = await _client.ExecuteAsync(_trendingTermsRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Terms>(content);
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}

	/// <inheritdoc cref="RegisterShareAsync"/>
	public string RegisterShare(string id, string q = null)
		=> Task.Run(() => RegisterShareAsync(id, q)).GetAwaiter().GetResult();

	/// <summary>
	///     Register a user’s sharing of a GIF.
	/// </summary>
	/// <param name="id">the “id” of a GIF_OBJECT</param>
	/// <param name="q">The search string that lead to this share</param>
	/// <returns>the Share Status</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<string> RegisterShareAsync(string id, string q = null)
	{
		_shareRequest.AddOrUpdateParameter("id", id)
					 .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

		if (_anonId != null)
			_shareRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);


		if (q != null)
			_shareRequest.AddOrUpdateParameter("q", q);

		var result  = await _client.ExecuteAsync(_shareRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Register>(content)?.ShareStatus;
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}


	/// <inheritdoc cref="GetGifsAsync(int,int,string[])"/>
	public Gif GetGifs(
		int             limit = 20,
		int             pos   = 0,
		params string[] ids
	) => Task.Run(() => GetGifsAsync(limit, pos, ids)).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetGifsAsync(int,string,string[])"/>
	public async Task<Gif> GetGifsAsync(
		int             limit = 20,
		int             pos   = 0,
		params string[] ids
	)
		=> await GetGifsAsync(limit, pos.ToString(), ids);
	
	/// <inheritdoc cref="GetGifsAsync(int,string,string[])"/>
	public Gif GetGifs(
		int             limit = 20,
		string          pos   = "0",
		params string[] ids
	) => Task.Run(() => GetGifsAsync(limit, pos, ids)).GetAwaiter().GetResult();

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
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Gif> GetGifsAsync(
		int             limit = 20,
		string          pos   = "0",
		params string[] ids
	)
	{
		_gifsRequest.AddOrUpdateParameter("ids", string.Join(',', ids), ParameterType.QueryString)
					.AddOrUpdateParameter("limit",         limit).AddOrUpdateParameter("pos", pos)
					.AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
					.AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
					.AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
		if (_anonId != null)
			_gifsRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
		if (_mediaFilter != MediaFilter.off)
			_gifsRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);


		var result  = await _client.ExecuteAsync(_gifsRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			var res = JsonSerializer.Deserialize<Gif>(content);
			if (res == null)
				throw new Exception("API Returned null");
			res.Client = this;
			res.Count  = limit;
			res.Ids    = ids;
			res.Type   = SearchTypes.getGifs;
			return res;
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}

	


	/// <inheritdoc cref="GetNewAnonIdAsync"/>
	public string GetNewAnonId() => Task.Run(GetNewAnonIdAsync).GetAwaiter().GetResult();

	/// <summary>
	///     Get an anonymous ID for a new user.
	///     Store the ID in the client's cache for use on any additional API calls made by the user, either in this session or
	///     any future sessions.
	/// </summary>
	/// <returns>a Random AnonId</returns>
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<string> GetNewAnonIdAsync()
	{
		var result  = await _client.ExecuteAsync(_anonIdRequest);
		var content = result.Content;
		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			return JsonSerializer.Deserialize<Session>(content)?.AnonId;
		}
		catch (JsonException)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}

	
	/// <inheritdoc cref="GetRandomGifsAsync(string,int,int)"/>
	public Gif GetRandomGifs(string q, int limit = 20, int pos = 0)
		=> Task.Run(() => GetRandomGifsAsync(q, limit, pos)).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetRandomGifsAsync(string,int,string)" />
	public async Task<Gif> GetRandomGifsAsync(string q, int limit = 20, int pos = 0)
		=> await GetRandomGifsAsync(q, limit, pos.ToString());

	/// <inheritdoc cref="GetRandomGifsAsync(string,int,string)"/>
	public Gif GetRandomGifs(string q, int limit = 20, string pos = "0")
		=> Task.Run(() => GetRandomGifsAsync(q, limit, pos)).GetAwaiter().GetResult();

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
	/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
	public async Task<Gif> GetRandomGifsAsync(string q, int limit = 20, string pos = "0")
	{
		_rndGifRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString).AddOrUpdateParameter("limit", limit)
					  .AddOrUpdateParameter("pos",           pos)
					  .AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
					  .AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
					  .AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
		if (_mediaFilter != MediaFilter.off)
			_rndGifRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);
		if (_anonId != null)
			_rndGifRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);

		var result  = await _client.ExecuteAsync(_rndGifRequest);
		var content = result.Content;

		try
		{
			if (content == null)
				throw new Exception("API Returned null");
			var res = JsonSerializer.Deserialize<Gif>(content);
			if (res == null)
				throw new Exception("API Returned null");
			res.Client = this;
			res.Term   = q;
			res.Count  = limit;
			res.Type   = SearchTypes.getRandom;
			return res;
		}
		catch (JsonException e)
		{
			var error = JsonSerializer.Deserialize<HttpError>(content!);
			throw new TenorException(error!.Error, e, error!.Code);
		}
		catch (Exception e)
		{
			throw new TenorException(e.Message, e, e.HResult);
		}
	}


	


	/// <summary>
	///     Start new Session with an Anonymous ID
	/// </summary>
	/// <param name="anonId">the Anonymous ID</param>
	/// <exception cref="TenorException">Thrown when anonId is invalid</exception>
	public void NewSession(string anonId) => _anonId = anonId.Length switch
	{
		>= 16 and <= 32 => anonId,
		var _ => throw new TenorException("Anon_id must be between 16 and 32 characters.", 1)
	};

	public string GetSession() => _anonId;

	public void SetLocale(string locale) => Locale = new Locale(locale);


	[Obsolete("SetLocale is deprecated, please use Locale instead.")]
	public void SetLocale(Locale locale) => Locale = locale;

	[Obsolete("GetLocale is deprecated, please use Locale instead.")]
	public Locale GetLocale() => Locale;

	public void ResetLocale() => Locale = DefaultLocale;

	public void ClearSession() => _anonId = null;

	[Obsolete("SetContentFilter is deprecated, please use ContentFilter instead.")]
	public void SetContentFilter(ContentFilter filter) => ContentFilter = filter;


	public Locale        Locale           { get; set; }
	public ContentFilter ContentFilter    { get; set; }
	public MediaFilter   MediaFilter      { get; set; }
	public AspectRatio   AspectRatioRange { get; set; }

	[Obsolete("GetContentFilter is deprecated, please use ContentFilter instead.")]
	public ContentFilter GetContentFilter() => ContentFilter;

	[Obsolete("SetMediaFilter is deprecated, please use MediaFilter instead.")]
	public void SetMediaFilter(MediaFilter filter) => MediaFilter = filter;

	[Obsolete("GetMediaFilter is deprecated, please use MediaFilter instead.")]
	public MediaFilter GetMediaFilter() => MediaFilter;

	[Obsolete("SetAspectRatioRange is deprecated, please use AspectRatioRange instead.")]
	public void SetAspectRatioRange(AspectRatio ratio) => AspectRatioRange = ratio;

	[Obsolete("GetAspectRatioRange is deprecated, please use AspectRatioRange instead.")]
	public AspectRatio GetAspectRatioRange() => AspectRatioRange;

	public string GetApiKey() => _apiKey;

	/// <summary>
	///     Gets a File as a Stream from a URL
	/// </summary>
	/// <param name="url">the URL</param>
	/// <returns>the Stream</returns>
	public static Stream GetMediaStream(Uri url)
	{
		var req    = WebRequest.Create(url);
		var stream = req.GetResponse().GetResponseStream();
		return stream;
	}

	public static Stream GetMediaStream(string url) => GetMediaStream(new Uri(url));


	// .AddOrUpdateParameter("anon_id",       null,              ParameterType.QueryString)
	// .AddOrUpdateParameter("ar_range",      AspectRatio.all,   ParameterType.QueryString)
	// .AddOrUpdateParameter("contentfilter", ContentFilter.off, ParameterType.QueryString)
	// .AddOrUpdateParameter("locale",        DefaultLocale,     ParameterType.QueryString)
}