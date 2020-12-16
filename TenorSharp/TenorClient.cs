using System;
using System.IO;
using System.Net;

using Newtonsoft.Json;

using RestSharp;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;
using TenorSharp.SearchResults;

using Type = TenorSharp.Enums.Type;

// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace TenorSharp
{
	public class TenorClient
	{
		private const string BaseUri = "https://api.tenor.com/v1/";

		private static readonly Locale DefaultLocale = new("en_US");

		private readonly RestRequest _anonIdRequest = new(Endpoints.AnonId, Method.GET, DataFormat.Json);

		/// <summary>
		///     client key for privileged API access
		/// </summary>
		private readonly string _apiKey;


		private readonly RestRequest _autocompleteRequest =
			new(Endpoints.Autocomplete, Method.GET, DataFormat.Json);

		private readonly RestRequest _categoryRequest =
			new(Endpoints.Categories, Method.GET, DataFormat.Json);

		private readonly RestClient _client;

		private readonly RestRequest _gifsRequest = new(Endpoints.Gifs, Method.GET, DataFormat.Json);

		private readonly RestRequest _rndGifRequest = new(Endpoints.Random, Method.GET, DataFormat.Json);

		private readonly RestRequest _searchRequest = new(Endpoints.Search, Method.GET, DataFormat.Json);

		private readonly RestRequest _shareRequest =
			new(Endpoints.RegisterShare, Method.GET, DataFormat.Json);

		private readonly RestRequest _suggestionRequest =
			new(Endpoints.SearchSuggestions, Method.GET, DataFormat.Json);

		private readonly RestRequest _trendingRequest =
			new(Endpoints.Trending, Method.GET, DataFormat.Json);

		private readonly RestRequest _trendingTermsRequest =
			new(Endpoints.TrendingTerms, Method.GET, DataFormat.Json);

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

			_client = (RestClient) _client.AddDefaultParameter("key", apiKey, ParameterType.QueryString);
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


		//	
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
		public Gif Search(string q, int limit = 20, string pos = "0", bool stickers = false)
		{
			_searchRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
						  .AddOrUpdateParameter("limit",         limit,          ParameterType.QueryString)
						  .AddOrUpdateParameter("pos",           pos,            ParameterType.QueryString)
						  .AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
						  .AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
						  .AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
			if (_anonId != null)
				_searchRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
			if (_mediaFilter != MediaFilter.off && !stickers)
				_searchRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);

			if (stickers)
				_searchRequest.AddQueryParameter("searchfilter", "sticker");


			var result = _client.Execute(_searchRequest).Content;
			try
			{
				var res = JsonConvert.DeserializeObject<Gif>(result);
				res.Client = this;
				res.Term   = q;
				res.Count  = limit;
				res.Type   = SearchTypes.search;
				return res;
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}


		/// <inheritdoc cref="Search(string,int,string, bool)" />
		public Gif Search(string q, int limit = 20, int pos = 0, bool stickers = false)
		{
			return Search(q, limit, pos.ToString());
		}

		public Gif SearchStickers(string q, int limit = 20, int pos = 0)
		{
			return Search(q, limit, pos, true);
		}

		public Gif SearchStickers(string q, int limit = 20, string pos = "0")
		{
			return Search(q, limit, pos, true);
		}

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
		public Gif Trending(int limit = 20, string pos = "0", bool stickers = false)
		{
			_trendingRequest.AddOrUpdateParameter("limit", limit, ParameterType.QueryString)
							.AddOrUpdateParameter("pos",           pos,            ParameterType.QueryString)
							.AddOrUpdateParameter("ar_range",      _arRange,       ParameterType.QueryString)
							.AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
							.AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
			if (_anonId != null)
				_trendingRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
			if (_mediaFilter != MediaFilter.off && !stickers)
				_trendingRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);

			if (stickers)
				_searchRequest.AddQueryParameter("searchfilter", "sticker");

			var result = _client.Execute(_trendingRequest).Content;

			try
			{
				var res = JsonConvert.DeserializeObject<Gif>(result);
				res.Client = this;
				res.Count  = limit;
				res.Type   = SearchTypes.trending;
				return res;
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}


		/// <inheritdoc cref="Trending(int,string, bool)" />
		public Gif Trending(int limit = 20, int pos = 0, bool stickers = false)
		{
			return Trending(limit, pos.ToString());
		}


		public Gif TrendingStickers(int limit = 20, int pos = 0)
		{
			return Trending(limit, pos, true);
		}

		public Gif TrendingStickers(int limit = 20, string pos = "0")
		{
			return Trending(limit, pos, true);
		}

		/// <summary>
		///     Get a json object containing a list of GIF categories associated with the provided type.
		///     Each category will include a corresponding search URL to be used if the user clicks on the category.
		///     The search URL will include the apikey, anonymous id, and locale that were used on the original call to the
		///     categories endpoint.
		/// </summary>
		/// <param name="type">determines the type of categories returned</param>
		/// <returns>a Tenor Category Response</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Category Categories(Type type = Type.featured)
		{
			_categoryRequest.AddOrUpdateParameter("Type", type, ParameterType.QueryString)
							.AddOrUpdateParameter("contentfilter", _contentFilter, ParameterType.QueryString)
							.AddOrUpdateParameter("locale",        _locale,        ParameterType.QueryString);
			if (_anonId != null)
				_categoryRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);

			var result = _client.Execute(_categoryRequest).Content;
			try
			{
				return JsonConvert.DeserializeObject<Category>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <summary>
		///     Get a json object containing a list of alternative search terms given a search term.
		///     Search suggestions helps a user narrow their search or discover related search terms to find a more precise GIF.
		///     Results are returned in order of what is most likely to drive a share for a given term, based on historic user
		///     search and share behavior.
		/// </summary>
		/// <param name="q">a search string</param>
		/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
		/// <returns>an Array of Search Terms</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Terms SearchSuggestions(string q, int limit = 20)
		{
			_suggestionRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
							  .AddOrUpdateParameter("limit",  limit,   ParameterType.QueryString)
							  .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);
			if (_anonId != null)
				_suggestionRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);
			if (_mediaFilter != MediaFilter.off)
				_suggestionRequest.AddOrUpdateParameter("media_filter", _mediaFilter, ParameterType.QueryString);


			var result = _client.Execute(_suggestionRequest)
								.Content;
			try
			{
				return JsonConvert.DeserializeObject<Terms>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <summary>
		///     Get a json object containing a list of completed search terms given a partial search term.
		///     The list is sorted by Tenor’s AI and the number of results will decrease as Tenor’s AI becomes more certain.
		/// </summary>
		/// <param name="q">a search string</param>
		/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
		/// <returns>an Array of Search Terms</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Terms AutoComplete(string q, int limit = 20)
		{
			_autocompleteRequest.AddOrUpdateParameter("q", q, ParameterType.QueryString)
								.AddOrUpdateParameter("limit",  limit,   ParameterType.QueryString)
								.AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

			if (_anonId != null)
				_autocompleteRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);

			var result = _client.Execute(_autocompleteRequest).Content;
			try
			{
				return JsonConvert.DeserializeObject<Terms>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <summary>
		///     Get a json object containing a list of the current trending search terms.
		///     The list is updated Hourly by Tenor’s AI.
		/// </summary>
		/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
		/// <returns>an Array of Search Terms</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Terms TrendingTerms(int limit = 20)
		{
			_trendingTermsRequest.AddOrUpdateParameter("limit", limit, ParameterType.QueryString)
								 .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

			if (_anonId != null)
				_trendingTermsRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);


			var result = _client.Execute(_trendingTermsRequest).Content;
			try
			{
				return JsonConvert.DeserializeObject<Terms>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <summary>
		///     Register a user’s sharing of a GIF.
		/// </summary>
		/// <param name="id">the “id” of a GIF_OBJECT</param>
		/// <param name="q">The search string that lead to this share</param>
		/// <returns>the Share Status</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public string RegisterShare(string id, string q = null)
		{
			_shareRequest.AddOrUpdateParameter("id", id)
						 .AddOrUpdateParameter("locale", _locale, ParameterType.QueryString);

			if (_anonId != null)
				_shareRequest.AddOrUpdateParameter("anon_id", _anonId, ParameterType.QueryString);


			if (q != null)
				_shareRequest.AddOrUpdateParameter("q", q);

			var result = _client.Execute(_shareRequest).Content;
			try
			{
				return JsonConvert.DeserializeObject<Register>(result).ShareStatus;
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
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
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Gif GetGifs(
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


			var result = _client.Execute(_gifsRequest).Content;
			try
			{
				var res = JsonConvert.DeserializeObject<Gif>(result);
				res.Client = this;
				res.Count  = limit;
				res.Ids    = ids;
				res.Type   = SearchTypes.getGifs;
				return res;
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <inheritdoc cref="GetGifs(int,string,string[])" />
		public Gif GetGifs(
			int             limit = 20,
			int             pos   = 0,
			params string[] ids
		)
		{
			return GetGifs(limit, pos.ToString(), ids);
		}

		/// <summary>
		///     Get an anonymous ID for a new user.
		///     Store the ID in the client's cache for use on any additional API calls made by the user, either in this session or
		///     any future sessions.
		/// </summary>
		/// <returns>a Random AnonId</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public string GetNewAnonId()
		{
			var result = _client.Execute(_anonIdRequest).Content;
			try
			{
				return JsonConvert.DeserializeObject<Session>(result).AnonId;
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
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
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Gif GetRandomGifs(string q, int limit = 20, string pos = "0")
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

			var result = _client.Execute(_rndGifRequest).Content;
			try
			{
				var res = JsonConvert.DeserializeObject<Gif>(result);
				res.Client = this;
				res.Term   = q;
				res.Count  = limit;
				res.Type   = SearchTypes.getRandom;
				return res;
			}
			catch (JsonException e)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, e, error.Code);
			}
		}

		/// <inheritdoc cref="GetRandomGifs(string,int,string)" />
		public Gif GetRandomGifs(string q, int limit = 20, int pos = 0)
		{
			return GetRandomGifs(q, limit, pos.ToString());
		}


		/// <summary>
		///     Start new Session with an Anonymous ID
		/// </summary>
		/// <param name="anonId">the Anonymous ID</param>
		/// <exception cref="TenorException">Thrown when anonId is invalid</exception>
		public void NewSession(string anonId)
		{
			if (anonId.Length >= 16 && anonId.Length <= 32)
				_anonId = anonId;
			else
				throw new TenorException("Anon_id must be between 16 and 32 characters.", 1);
		}

		public string GetSession()
		{
			return _anonId;
		}

		public void SetLocale(string locale)
		{
			SetLocale(new Locale(locale));
		}

		public void SetLocale(Locale locale)
		{
			_locale = locale;
		}

		public Locale GetLocale()
		{
			return _locale;
		}

		public void ResetLocale()
		{
			_locale = new Locale("en_US");
		}

		public void ClearSession()
		{
			_anonId = null;
		}

		public void SetContentFilter(ContentFilter filter)
		{
			_contentFilter = filter;
		}

		public ContentFilter GetContentFilter()
		{
			return _contentFilter;
		}

		public void SetMediaFilter(MediaFilter filter)
		{
			_mediaFilter = filter;
		}

		public MediaFilter GetMediaFilter()
		{
			return _mediaFilter;
		}

		public void SetAspectRatioRange(AspectRatio ratio)
		{
			_arRange = ratio;
		}

		public AspectRatio GetAspectRatioRange()
		{
			return _arRange;
		}

		public string GetApiKey()
		{
			return _apiKey;
		}

		/// <summary>
		///     Gets a File as a Stream from a URL
		/// </summary>
		/// <param name="url">the URL</param>
		/// <returns>the Stream</returns>
		public Stream GetMediaStream(Uri url)
		{
			var req    = WebRequest.Create(url);
			var stream = req.GetResponse().GetResponseStream();
			return stream;
		}

		public Stream GetMediaStream(string url)
		{
			return GetMediaStream(new Uri(url));
		}


		// .AddOrUpdateParameter("anon_id",       null,              ParameterType.QueryString)
		// .AddOrUpdateParameter("ar_range",      AspectRatio.all,   ParameterType.QueryString)
		// .AddOrUpdateParameter("contentfilter", ContentFilter.off, ParameterType.QueryString)
		// .AddOrUpdateParameter("locale",        DefaultLocale,     ParameterType.QueryString)
	}
}