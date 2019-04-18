using System.Net.Http;
using Newtonsoft.Json;
using TenorSharp.Enums;
using TenorSharp.SearchResults;

namespace TenorSharp
{
	public class TenorClient
	{
		private readonly HttpClient _client = new HttpClient();

		public readonly string ApiKey;

		private string _anonId;

		private Locale _locale = new Locale("en_US");

		public TenorClient(string apiKey)
		{
			ApiKey = apiKey;
		}

		/// <summary>
		///     Get a json object containing a list of the most relevant GIFs for a given search term(s), category(ies), emoji(s),
		///     or any combination thereof.
		/// </summary>
		/// <param name="q">a search string</param>
		/// <param name="contentFilter">specify the content safety filter level</param>
		/// <param name="anonId">specify the anonymous_id tied to the given user</param>
		/// <param name="mediaFilter">Reduce the Number of GIF formats returned in the GIF_OBJECT list.</param>
		/// <param name="arRange">
		///     Filter the response GIF_OBJECT list to only include GIFs with aspect ratios that fit with in the
		///     selected range.
		/// </param>
		/// <param name="limit">fetch up to a specified number of results (max: 50).</param>
		/// <param name="pos">
		///     get results starting at position "value".
		///     Use a non-zero "next" value returned by API results to get the next set of results.
		///     pos is not an index and may be an integer, float, or string
		/// </param>
		/// <returns></returns>
		/// <exception cref="TenorException"></exception>
		public Gif Search(string q, ContentFilter contentFilter = ContentFilter.off,
			string anonId = null, MediaFilter mediaFilter = MediaFilter.off, AspectRatio arRange = AspectRatio.all,
			int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={ApiKey}&q={q}&locale={_locale}&contentfilter={contentFilter}&ar_range={arRange}&limit={limit}&pos={pos}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={mediaFilter}";

			var result = _client.GetAsync(Endpoints.Search + optionString).Result.Content
				.ReadAsStringAsync().Result;
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
		/// <param name="anonId"></param>
		/// <param name="mediaFilter"></param>
		/// <param name="arRange"></param>
		/// <param name="contentFilter"></param>
		/// <param name="limit"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		/// <exception cref="TenorException"></exception>
		public Gif Trending(string anonId = null, MediaFilter mediaFilter = MediaFilter.off,
			AspectRatio arRange = AspectRatio.all,
			ContentFilter contentFilter = ContentFilter.off,
			int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={ApiKey}&locale={_locale}&contentfilter={contentFilter}&ar_range={arRange}&limit={limit}&pos={pos}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={mediaFilter}";

			var result = _client.GetAsync(Endpoints.Trending + optionString).Result.Content
				.ReadAsStringAsync().Result;

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

		public Category Categories(string anonId = null, Type type = Type.featured,
			ContentFilter contentFilter = ContentFilter.off)
		{
			var optionString = $"key={ApiKey}&locale={_locale}&contentfilter={contentFilter}&Type={type}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.GetAsync(Endpoints.Categories + optionString).Result
				.Content.ReadAsStringAsync().Result;
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

		public Terms SearchSuggestions(string q, string anonId = null,
			MediaFilter mediaFilter = MediaFilter.off, int limit = 20)
		{
			var optionString = $"key={ApiKey}&q={q}&locale={_locale}&limit={limit}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={mediaFilter}";

			var result = _client.GetAsync(Endpoints.SearchSuggestions + optionString)
				.Result.Content.ReadAsStringAsync().Result;
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

		public Terms AutoComplete(string q,
			string anonId = null, int limit = 20)
		{
			var optionString = $"key={ApiKey}&q={q}&locale={_locale}&limit={limit}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.GetAsync(Endpoints.Autocomplete + optionString).Result
				.Content.ReadAsStringAsync().Result;
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

		public Terms TrendingTerms(string q,
			string anonId = null, int limit = 20)
		{
			var optionString = $"key={ApiKey}&q={q}&locale={_locale}&limit={limit}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			var result = _client.GetAsync(Endpoints.TrendingTerms + optionString).Result
				.Content.ReadAsStringAsync().Result;
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

		public string RegisterShare(string id,
			string anonId = null, string q = null)
		{
			var optionString = $"key={ApiKey}&id={id}&locale={_locale}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (q != null)
				optionString += $"&q={q}";

			var result = _client.GetAsync(Endpoints.RegisterShare + optionString)
				.Result.Content.ReadAsStringAsync().Result;
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

		public Gif GetGifs(string anonId = null, MediaFilter mediaFilter = MediaFilter.off, int limit = 20, int pos = 0,
			params string[] ids)
		{
			var optionString = $"key={ApiKey}&ids={string.Join(',', ids)}&limit={limit}&pos={pos}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={mediaFilter}";


			var result = _client.GetAsync(Endpoints.Gifs + optionString).Result.Content
				.ReadAsStringAsync().Result;
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

		public string GetAnonId()
		{
			var optionString = $"key={ApiKey}";

			var result = _client.GetAsync(Endpoints.AnonId + optionString).Result
				.Content.ReadAsStringAsync().Result;
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

		public Gif GetRandomGifs(string q, ContentFilter contentFilter = ContentFilter.off,
			string anonId = null, MediaFilter mediaFilter = MediaFilter.off, AspectRatio arRange = AspectRatio.all,
			int limit = 20, int pos = 0)
		{
			var optionString =
				$"key={ApiKey}&q={q}&locale={_locale}&contentfilter={contentFilter}&ar_range={arRange}&limit={limit}&pos={pos}";

			if (anonId != null)
				optionString += $"&anon_id={anonId}";
			else if (_anonId != null)
				optionString += $"&anon_id={_anonId}";

			if (mediaFilter != MediaFilter.off)
				optionString += $"&media_filter={mediaFilter}";

			var result = _client.GetAsync(Endpoints.Random + optionString).Result.Content
				.ReadAsStringAsync().Result;
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
	}
}