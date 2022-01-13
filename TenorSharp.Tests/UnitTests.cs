using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RichardSzalay.MockHttp;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests;

public class UnitTests
{
	private const           string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	private const           string ApiKey = "TENOR_TEST_API_KEY";
	private static readonly Random Random = new();
	private static readonly MockHttpMessageHandler MockHttp = new();

	private static readonly int[]    AnonIdLengths = { 15, 16, 18, 32, 33 };
	private static readonly int[]    Limits        = { -1, 0, 2, 6, 49, 50, 51 };
	private static readonly object[] Positions     = { 42, -1, 1.3255, Math.PI, "test" };

#pragma warning disable CA2211
	public static MatrixTheoryData<int, int, object> SearchMatrixData = new(AnonIdLengths, Limits, Positions);
#pragma warning restore CA2211

	private readonly TenorClient _client = new(ApiKey,
		mediaFilter: MediaFilter.basic,
		testClient: MockHttp.ToHttpClient());


	private readonly ITestOutputHelper _testOutputHelper;

	public List<MockedRequest> Requests = new();

	public UnitTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/*").With(message =>
		{
			message.Properties.TryGetValue("anon_id", out var anonID);
			return $"{anonID}".Length is > 32 or < 16 && anonID != null;
		}).Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(MockData.ErrorAnonId)));

		Requests.Add(MockHttp.When("https://g.tenor.com/v1/*").With(message =>
		{
			message.Properties.TryGetValue("limit", out var lim);
			return Convert.ToInt32(lim) is > 50 or < 1 && lim != null;
		}).Respond(HttpStatusCode.OK, "application/json", MockData.ErrorLimit));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/search").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.GifResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/trending").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.GifResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/categories").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.CategoryResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/search_suggestions").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.TermsResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/autocomplete").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.TermsResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/trending_terms").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.TermsResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/registershare").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.RegisterShareResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/gifs").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.GifResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/anonid").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.AnonIdResponse)));
		Requests.Add(MockHttp.When("https://g.tenor.com/v1/random").Respond(HttpStatusCode.OK,
			"application/json",
			JsonConvert.SerializeObject(MockData.GifResponse)));

		//MockHttp.When("https://g.tenor.com/v1/*").Respond(HttpStatusCode.OK,"application/json", JsonConvert.SerializeObject(MockData.GifResponse));
	}

	public static List<string> TestAnonIds
		=> new() { RndString(18), };

	public int[] GetMatches() => Requests.Select(request => MockHttp.GetMatchCount(request)).ToArray();

	private static string RndString(int len)
		=> new(Enumerable.Range(1, len).Select(_ => Chars[Random.Next(Chars.Length)]).ToArray());


	private static string RndLenString()
	{
		var len = Random.Next();
		return RndString(len);
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public void TestSearch(int limit, object pos, bool succeed)
	{
		try

		{
			if (!succeed)
				Assert.Throws<TenorException>(() => _client.Search("lorem ipsum", limit, $"{pos}"));
			else
				Assert.NotNull(_client.Search("lorem ipsum", limit, $"{pos}"));
		}
		catch
			(TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: 00000000000000000\n" +
										$"q: lorem ipsum\n"            +
										$"limit: {limit}\n"            +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestSearchAsync()
	{
		var anonId = RndString(18);
		var q      = RndString(10);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next();
		try
		{
			await _client.SearchAsync(q, limit, pos);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: {anonId}\n" +
										$"q: {q}\n"           +
										$"limit: {limit}\n"   +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestTrending()
	{
		try
		{
			_client.Trending();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestTrendingAsync()
	{
		try
		{
			await _client.TrendingAsync();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestCategoriesEmoji()
	{
		try
		{
			_client.Categories(Type.emoji);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestCategoriesEmojiAsync()
	{
		try
		{
			await _client.CategoriesAsync(Type.emoji);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestCategoriesFeatured()
	{
		try
		{
			_client.Categories();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestCategoriesFeaturedAsync()
	{
		try
		{
			await _client.CategoriesAsync();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestCategoriesTrending()
	{
		try
		{
			_client.Categories(Type.trending);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestCategoriesTrendingAsync()
	{
		try
		{
			await _client.CategoriesAsync(Type.trending);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestAutoComplete()
	{
		try
		{
			_client.AutoComplete(RndString(10), Random.Next(50));
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestAutoCompleteAsync()
	{
		try
		{
			await _client.AutoCompleteAsync(RndString(10), Random.Next(50));
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestGetGifs()
	{
		var anonId = RndString(18);
		try
		{
			var result = _client.Search("test", 20, 0);

			_client.GetGifs(20, 1, result.GifResults.Select(o => o.Id).ToArray());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestGetGifsAsync()
	{
		var anonId = RndString(18);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next(10);
		try
		{
			var result = await _client.SearchAsync("test", limit, pos);

			await _client.GetGifsAsync(limit, pos, result.GifResults.Select(o => o.Id).ToArray());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestRegisterShare()
	{
		var anonId = RndString(18);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next(10);
		try
		{
			var result = _client.Search("test", limit, pos);
			var id     = result.GifResults.First().Id;

			_client.RegisterShare(id, "test");
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestRegisterShareAsync()
	{
		var anonId = RndString(18);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next(10);
		try
		{
			var result = await _client.SearchAsync("test", limit, pos);
			var id     = result.GifResults.First().Id;

			await _client.RegisterShareAsync(id, "test");
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestSearchSuggestions()
	{
		var anonId = RndString(18);
		var q      = RndString(10);
		var limit  = Random.Next(1, 50);
		try
		{
			var test = _client.SearchSuggestions(q, limit);
			_testOutputHelper.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(test));
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestSearchSuggestionsAsync()
	{
		var anonId = RndString(18);
		var q      = RndString(10);
		var limit  = Random.Next(1, 50);
		try
		{
			await _client.SearchSuggestionsAsync(q, limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestTrendingTerms()
	{
		var anonId = RndString(18);
		var limit  = Random.Next(1, 50);
		try
		{
			_client.TrendingTerms(limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: {anonId}\n" +
										$"limit: {limit}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestTrendingTermsAsync()
	{
		var anonId = RndString(18);
		var limit  = Random.Next(1, 50);
		try
		{
			await _client.TrendingTermsAsync(limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: {anonId}\n" +
										$"limit: {limit}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public void TestGetRandomGifs()
	{
		var anonId = RndString(18);
		var q      = RndString(10);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next(10);
		try
		{
			_client.GetRandomGifs(q, limit, pos);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: {anonId}\n" +
										$"q: {q}\n"           +
										$"limit: {limit}\n"   +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());

			throw;
		}
	}

	[Fact]
	public async Task TestGetRandomGifsAsync()
	{
		var anonId = RndString(18);
		var q      = RndString(10);
		var limit  = Random.Next(1, 50);
		var pos    = Random.Next(10);
		try
		{
			await _client.GetRandomGifsAsync(q, limit, pos);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine($"anonId: {anonId}\n" +
										$"q: {q}\n"           +
										$"limit: {limit}\n"   +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());

			throw;
		}
	}

	[Fact]
	public void TestGetNewAnonId()
	{
		try
		{
			_client.GetNewAnonId();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Fact]
	public async Task TestGetNewAnonIdAsync()
	{
		try
		{
			await _client.GetNewAnonIdAsync();
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}
}