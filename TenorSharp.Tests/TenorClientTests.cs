using System;
using System.Net.Http;
using System.Threading.Tasks;

using NSubstitute;

using NUnit.Framework;

using TenorSharp.Enums;

namespace TenorSharp.Tests;

[TestFixture]
[Ignore("WIP")]
public class TenorClientTests
{
	[SetUp]
	public void SetUp()
	{
		subHttpClient = Substitute.For<HttpClient>();
	}

	private HttpClient subHttpClient;

	private TenorClient CreateTenorClient()
	{
		return new TenorClient("TENOR_TEST_API_KEY",
			new Locale(),
			AspectRatio.all,
			ContentFilter.off,
			MediaFilter.off,
			"00000000000000000",
			subHttpClient);
	}

	[Theory]
	public void Search_StateUnderTest_ExpectedBehavior(int limit, int pos)
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";

		// Act
		var result = tenorClient.Search(q,
			limit,
			pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task SearchAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const int    pos         = 0;

		// Act
		var result = await tenorClient.SearchAsync(q,
						 limit,
						 pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void Search_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = tenorClient.Search(q,
			limit,
			pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task SearchAsync_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = await tenorClient.SearchAsync(q,
						 limit,
						 pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void Trending_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = tenorClient.Trending(limit, pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task TrendingAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = await tenorClient.TrendingAsync(limit, pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void Categories_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var              tenorClient = CreateTenorClient();
		const Enums.Type type        = default;

		// Act
		var result = tenorClient.Categories(type);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task CategoriesAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var              tenorClient = CreateTenorClient();
		const Enums.Type type        = default;

		// Act
		var result = await tenorClient.CategoriesAsync(type);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void SearchSuggestions_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;

		// Act
		var result = tenorClient.SearchSuggestions(q,
			limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task SearchSuggestionsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;

		// Act
		var result = await tenorClient.SearchSuggestionsAsync(q,
						 limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void AutoComplete_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;

		// Act
		var result = tenorClient.AutoComplete(q,
			limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task AutoCompleteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;

		// Act
		var result = await tenorClient.AutoCompleteAsync(q,
						 limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void TrendingTerms_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var       tenorClient = CreateTenorClient();
		const int limit       = 0;

		// Act
		var result = tenorClient.TrendingTerms(limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task TrendingTermsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var       tenorClient = CreateTenorClient();
		const int limit       = 0;

		// Act
		var result = await tenorClient.TrendingTermsAsync(limit);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void RegisterShare_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string id          = "1";
		const string q           = "lorem ipsum";

		// Act
		var result = tenorClient.RegisterShare(id,
			q);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task RegisterShareAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string id          = "1";
		const string q           = "lorem ipsum";

		// Act
		var result = await tenorClient.RegisterShareAsync(id,
						 q);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetGifs_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var       tenorClient = CreateTenorClient();
		const int limit       = 0;
		const int pos         = 0;
		var       ids         = new[] { "1", "2" };

		// Act
		var result = tenorClient.GetGifs(limit,
			pos,
			ids);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task GetGifsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var       tenorClient = CreateTenorClient();
		const int limit       = 0;
		const int pos         = 0;
		var       ids         = new[] { "1", "2" };

		// Act
		var result = await tenorClient.GetGifsAsync(limit,
						 pos,
						 ids);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetGifs_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const int    limit       = 0;
		const string pos         = "1";
		var          ids         = new[] { "1", "2" };

		// Act
		var result = tenorClient.GetGifs(limit,
			pos,
			ids);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task GetGifsAsync_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const int    limit       = 0;
		const string pos         = "1";
		var          ids         = new[] { "1", "2" };

		// Act
		var result = await tenorClient.GetGifsAsync(limit,
						 pos,
						 ids);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetNewAnonId_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		var result = tenorClient.GetNewAnonId();

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task GetNewAnonIdAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		var result = await tenorClient.GetNewAnonIdAsync();

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetRandomGifs_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const int    pos         = 0;

		// Act
		var result = tenorClient.GetRandomGifs(q,
			limit,
			pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task GetRandomGifsAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const int    pos         = 0;

		// Act
		var result = await tenorClient.GetRandomGifsAsync(q,
						 limit,
						 pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetRandomGifs_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = tenorClient.GetRandomGifs(q,
			limit,
			pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public async Task GetRandomGifsAsync_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string q           = "lorem ipsum";
		const int    limit       = 0;
		const string pos         = "1";

		// Act
		var result = await tenorClient.GetRandomGifsAsync(q,
						 limit,
						 pos);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void NewSession_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var          tenorClient = CreateTenorClient();
		const string anonId      = "00000000000000000";

		// Act
		tenorClient.NewSession(anonId);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetSession_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		var result = tenorClient.GetSession();

		// Assert
		Assert.Fail();
	}

	[Test]
	public void ResetLocale_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		tenorClient.ResetLocale();

		// Assert
		Assert.Fail();
	}

	[Test]
	public void ClearSession_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		tenorClient.ClearSession();

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetApiKey_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var tenorClient = CreateTenorClient();

		// Act
		var result = tenorClient.GetApiKey();

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetMediaStream_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var url = new Uri("https://www.youtube.com/watch?v=iik25wqIuFo");

		// Act
		var result = TenorClient.GetMediaStream(url);

		// Assert
		Assert.Fail();
	}

	[Test]
	public void GetMediaStream_StateUnderTest_ExpectedBehavior1()
	{
		// Arrange
		const string url = "https://www.youtube.com/watch?v=iik25wqIuFo";

		// Act
		var result = TenorClient.GetMediaStream(url);

		// Assert
		Assert.Fail();
	}
}