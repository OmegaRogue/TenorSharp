using System;
using System.Linq;

using Moq;

using RestSharp;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests
{
	public class UnitTest1
	{
		private const           string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		private static readonly Mock<RestClient> testClient = new();
		private static readonly Random random = new();
		private static readonly string ApiKey = Environment.GetEnvironmentVariable("TENOR_TEST_API_KEY");
		private readonly        TenorClient _client = new(ApiKey);


		private readonly ITestOutputHelper _testOutputHelper;

		public UnitTest1(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;

			// testClient.Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(),
			// 									 It.IsAny<Action<IRestResponse, RestRequestAsyncHandle>>()))
			// 		  .Callback<IRestRequest, Action<IRestResponse, RestRequestAsyncHandle>>((request, callback) =>
			// 		   {
			// 			   callback(new RestResponse {StatusCode = HttpStatusCode.OK}, null);
			// 		   });
		}

		private static string RndString(int len)
		{
			return new(Enumerable.Range(1, len).Select(_ => Chars[random.Next(Chars.Length)]).ToArray());
		}

		private static string RndLenString()
		{
			var len = random.Next();
			return RndString(len);
		}

		[Fact]
		public void TestSearch()
		{
			var anonId = RndString(18);
			var q      = RndString(10);
			var limit  = random.Next(50);
			var pos    = random.Next();
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
				_client.AutoComplete(RndString(10), random.Next(50));
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
			try
			{
				_client.NewSession(RndString(18));

				// _client.GetGifs(RndString(10), random.Next(50), random.Next());
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
				_client.NewSession(RndString(18));

				// _client.RegisterShare(RndString(10), random.Next(50), random.Next());
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
			try
			{
				_client.NewSession(RndString(18));

				// _client.SearchSuggestions(RndString(10), random.Next(50), random.Next());
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
			try
			{
				_client.NewSession(RndString(18));

				// _client.TrendingTerms(RndString(10), random.Next(50), random.Next());
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Fact]
		public void TestGetRandomGifs()
		{
			var anonId = RndString(18);
			var q      = RndString(10);
			var limit  = random.Next(50);
			var pos    = random.Next(10);
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
				_testOutputHelper.WriteLine(e.Message);
				_testOutputHelper.WriteLine($"{e.ErrorCode}");
				_testOutputHelper.WriteLine(e.ToString());

				// throw;
			}
		}

		[Fact]
		public void TestGetNewAnonId()
		{
			try
			{
				_client.NewSession(RndString(18));

				// _client.GetNewAnonId(RndString(10), random.Next(50), random.Next());
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}
	}
}