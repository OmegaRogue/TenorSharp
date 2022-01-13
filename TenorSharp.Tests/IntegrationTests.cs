using System;
using System.Linq;
using System.Threading.Tasks;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests;

public class IntegrationTests
{
	private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	// private static readonly Random      Random  = new();
	private static readonly string ApiKey = Environment.GetEnvironmentVariable("TENOR_TEST_API_KEY");
	private readonly TenorClient _client = new(ApiKey, mediaFilter: MediaFilter.basic, anonId: "00000000000000000");


	private readonly ITestOutputHelper _testOutputHelper;

	public IntegrationTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

	// private static string RndString(int len)
	// 	=> new(Enumerable.Range(1, len).Select(_ => Chars[Random.Next(Chars.Length)]).ToArray());
	//
	// private static string RndLenString()
	// {
	// 	var len = Random.Next();
	// 	return RndString(len);
	// }

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
				Assert.NotNull(_client.Search("lorem ipsum", limit, pos.ToString()));
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										"q: lorem ipsum\n"            +
										$"limit: {limit}\n"           +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public async Task TestSearchAsync(int limit, object pos, bool succeed)
	{
		try
		{
			if (!succeed)
				await Assert.ThrowsAsync<TenorException>(async ()
					=> await _client.SearchAsync("lorem ipsum", limit, $"{pos}"));
			else
				Assert.NotNull(await _client.SearchAsync("lorem ipsum", limit, pos.ToString()));
			await _client.SearchAsync("lorem ipsum", limit, pos.ToString());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										"q: lorem ipsum\n"            +
										$"limit: {limit}\n"           +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public void TestTrending(int limit, object pos, bool succeed)
	{
		try
		{
			if (!succeed)
				Assert.Throws<TenorException>(() => _client.Trending(limit, pos.ToString()));
			else
				Assert.NotNull(_client.Trending(limit, pos.ToString()));
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public async Task TestTrendingAsync(int limit, object pos, bool succeed)
	{
		try
		{
			await _client.TrendingAsync(limit, pos.ToString());
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

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public void TestAutoComplete(int limit, bool succeed)
	{
		try
		{
			_client.AutoComplete("lorem ipsum", limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public async Task TestAutoCompleteAsync(int limit, bool succeed)
	{
		try
		{
			await _client.AutoCompleteAsync("lorem ipsum", limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public void TestGetGifs(int limit, object pos, bool succeed)
	{
		try
		{
			var result = _client.Search("lorem ipsum", pos: 0);

			_client.GetGifs(limit, pos.ToString(), result.GifResults.Select(o => o.Id).ToArray());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public async Task TestGetGifsAsync(int limit, object pos, bool succeed)
	{
		try
		{
			var result = await _client.SearchAsync("lorem ipsum", pos: 0);

			await _client.GetGifsAsync(limit, pos.ToString(), result.GifResults.Select(o => o.Id).ToArray());
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
		try
		{
			var result = _client.Search("lorem ipsum", pos: 0);
			var id     = result.GifResults.First().Id;

			_client.RegisterShare(id, "lorem ipsum");
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
		try
		{
			var result = await _client.SearchAsync("lorem ipsum", pos: 0);
			var id     = result.GifResults.First().Id;

			await _client.RegisterShareAsync(id, "lorem ipsum");
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public void TestSearchSuggestions(int limit, bool succeed)
	{
		try
		{
			_client.SearchSuggestions("lorem ipsum", limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public async Task TestSearchSuggestionsAsync(int limit, bool succeed)
	{
		try
		{
			await _client.SearchSuggestionsAsync("lorem ipsum", limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public void TestTrendingTerms(int limit, bool succeed)
	{
		try
		{
			_client.TrendingTerms(limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										$"limit: {limit}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, false)]
	[InlineData(1,  true)]
	[InlineData(49, true)]
	[InlineData(50, true)]
	[InlineData(51, false)]
	public async Task TestTrendingTermsAsync(int limit, bool succeed)
	{
		try
		{
			await _client.TrendingTermsAsync(limit);
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										$"limit: {limit}");
			_testOutputHelper.WriteLine(e.ToString());
			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public void TestGetRandomGifs(int limit, object pos, bool succeed)
	{
		try
		{
			_client.GetRandomGifs("lorem ipsum", limit, pos.ToString());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										"q: lorem ipsum\n"            +
										$"limit: {limit}\n"           +
										$"pos: {pos}");
			_testOutputHelper.WriteLine(e.ToString());

			throw;
		}
	}

	[Theory]
	[InlineData(-1, 0, false)]
	[InlineData(1,  0, true)]
	[InlineData(49, 0, true)]
	[InlineData(50, 0, true)]
	[InlineData(51, 0, false)]
	public async Task TestGetRandomGifsAsync(int limit, object pos, bool succeed)
	{
		try
		{
			await _client.GetRandomGifsAsync("lorem ipsum", limit, pos.ToString());
		}
		catch (TenorException e)
		{
			_testOutputHelper.WriteLine("anonId: 00000000000000000\n" +
										"q: lorem ipsum\n"            +
										$"limit: {limit}\n"           +
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