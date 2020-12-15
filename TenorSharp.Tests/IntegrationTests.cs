using System;
using System.Linq;

using TenorSharp.Enums;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests
{
	public class IntegrationTests
	{
		private const           string      Chars   = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		private static readonly Random      Random  = new();
		private static readonly string      ApiKey  = Environment.GetEnvironmentVariable("TENOR_TEST_API_KEY");
		private readonly        TenorClient _client = new(ApiKey, mediaFilter: MediaFilter.basic);


		private readonly ITestOutputHelper _testOutputHelper;

		public IntegrationTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		private static string RndString(int len)
		{
			return new(Enumerable.Range(1, len).Select(_ => Chars[Random.Next(Chars.Length)]).ToArray());
		}

		private static string RndLenString()
		{
			var len = Random.Next();
			return RndString(len);
		}

		[Fact]
		public void TestSearch()
		{
			var anonId = RndString(18);
			var q      = RndString(10);
			var limit  = Random.Next(50);
			var pos    = Random.Next();
			try
			{
				_client.NewSession(anonId);
				_client.Search(q, limit, pos);
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
				_client.NewSession(RndString(18));
				_client.Trending();
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
				_client.NewSession(RndString(18));
				_client.Categories(Type.emoji);
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
				_client.NewSession(RndString(18));
				_client.Categories();
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
				_client.NewSession(RndString(18));
				_client.Categories(Type.trending);
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
				_client.NewSession(RndString(18));
				_client.AutoComplete(RndString(10), Random.Next(50));
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
			var limit  = Random.Next(50);
			var pos    = Random.Next(10);
			try
			{
				_client.NewSession(anonId);
				var result = _client.Search("test", limit, pos);

				_client.GetGifs(limit, pos, result.GifResults.Select(o => o.Id).ToArray());
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
			var limit  = Random.Next(50);
			var pos    = Random.Next(10);
			try
			{
				_client.NewSession(anonId);
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
		public void TestSearchSuggestions()
		{
			var anonId = RndString(18);
			var q      = RndString(10);
			var limit  = Random.Next(50);
			try
			{
				_client.NewSession(anonId);

				_client.SearchSuggestions(q, limit);
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
			var limit  = Random.Next(50);
			try
			{
				_client.NewSession(anonId);

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
		public void TestGetRandomGifs()
		{
			var anonId = RndString(18);
			var q      = RndString(10);
			var limit  = Random.Next(50);
			var pos    = Random.Next(10);
			try
			{
				_client.NewSession(anonId);
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
	}
}