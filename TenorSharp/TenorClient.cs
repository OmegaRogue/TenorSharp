using Newtonsoft.Json;
using RestSharp;
using TenorSharp.Enums;
using TenorSharp.ResponseObjects;
using TenorSharp.SearchResults;

// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace TenorSharp
{
	public class TenorClient
	{
		/// <summary>
		///     client key for privileged API access
		/// </summary>
		private readonly string _apiKey;

		private readonly RestClient _client = new RestClient();

		/// <summary>
		///     specify the anonymous_id tied to the given user
		/// </summary>
		private string _anonId;

		/// <summary>
		///     Filter the response GIF_OBJECT list to only include GIFs with aspect ratios that fit with in the selected range.
		/// </summary>
		private AspectRatio _arRange = AspectRatio.all;

		/// <summary>
		///     specify the content safety filter level
		/// </summary>
		private ContentFilter _contentFilter = ContentFilter.off;

		/// <summary>
		///     specify default language to interpret search string
		/// </summary>
		private Locale _locale = new Locale("en_US");

		/// <summary>
		///     Reduce the Number of GIF formats returned in the GIF_OBJECT list.
		/// </summary>
		private MediaFilter _mediaFilter = MediaFilter.off;

		public TenorClient(string apiKey)
		{
			_apiKey = apiKey;
		}

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
		public Gif Search(string q, int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={_apiKey}&q={q}&locale={_locale}&contentfilter={_contentFilter}&ar_range={_arRange}&limit={limit}&pos={pos}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (_mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={_mediaFilter}";

			var result = _client.Execute(new RestRequest(Endpoints.Search + optionString)).Content;
			try
			{
				return JsonConvert.DeserializeObject<Gif>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
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
		public Gif Trending(int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={_apiKey}&locale={_locale}&contentfilter={_contentFilter}&ar_range={_arRange}&limit={limit}&pos={pos}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (_mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={_mediaFilter}";

			var result = _client.Execute(new RestRequest(Endpoints.Trending + optionString)).Content;

			try
			{
				return JsonConvert.DeserializeObject<Gif>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
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
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public Category Categories(Type type = Type.featured)
		{
			var optionString = $"key={_apiKey}&locale={_locale}&contentfilter={_contentFilter}&Type={type}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.Execute(new RestRequest(Endpoints.Categories + optionString)).Content;
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
			var optionString = $"key={_apiKey}&q={q}&locale={_locale}&limit={limit}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (_mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={_mediaFilter}";

			var result = _client.Execute(new RestRequest(Endpoints.SearchSuggestions + optionString)).Content;
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
			var optionString = $"key={_apiKey}&q={q}&locale={_locale}&limit={limit}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.Execute(new RestRequest(Endpoints.Autocomplete + optionString)).Content;
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
			var optionString = $"key={_apiKey}&locale={_locale}&limit={limit}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.Execute(new RestRequest(Endpoints.TrendingTerms + optionString)).Content;
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
			var optionString = $"key={_apiKey}&id={id}&locale={_locale}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (q != null)
				optionString += $"&q={q}";

			var result = _client.Execute(new RestRequest(Endpoints.RegisterShare + optionString)).Content;
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
		public Gif GetGifs(int limit = 20, int pos = 0,
			params string[] ids)
		{
			var optionString = $"key={_apiKey}&ids={string.Join(',', ids)}&limit={limit}&pos={pos}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (_mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={_mediaFilter}";


			var result = _client.Execute(new RestRequest(Endpoints.Gifs + optionString)).Content;
			try
			{
				return JsonConvert.DeserializeObject<Gif>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}

		/// <summary>
		///     Get an anonymous ID for a new user.
		///     Store the ID in the client's cache for use on any additional API calls made by the user, either in this session or
		///     any future sessions.
		/// </summary>
		/// <returns>a Random AnonId</returns>
		/// <exception cref="TenorException">thrown when the Tenor API returns an Error</exception>
		public string GetAnonId()
		{
			var optionString = $"key={_apiKey}";

			var result = _client.Execute(new RestRequest(Endpoints.AnonId + optionString)).Content;
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
		public Gif GetRandomGifs(string q, int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={_apiKey}&q={q}&locale={_locale}&contentfilter={_contentFilter}&ar_range={_arRange}&limit={limit}&pos={pos}";

			if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (_mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={_mediaFilter}";

			var result = _client.Execute(new RestRequest(Endpoints.Random + optionString)).Content;
			try
			{
				return JsonConvert.DeserializeObject<Gif>(result);
			}
			catch (JsonException)
			{
				var error = JsonConvert.DeserializeObject<HttpError>(result);
				throw new TenorException(error.Error, error.Code);
			}
		}


		public void NewSession(string anonId)
		{
			_anonId = anonId;
		}

		public string GetSession()
		{
			return _anonId;
		}

		public void SetLocale(string locale)
		{
			_locale = new Locale(locale);
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
	}
}