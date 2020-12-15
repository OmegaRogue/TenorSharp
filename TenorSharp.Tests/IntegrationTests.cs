using System;

using TenorSharp.Enums;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests
{
	public class IntegrationTests
	{
		private const           string Chars  = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		private static readonly string ApiKey = Environment.GetEnvironmentVariable("TENOR_TEST_API_KEY");

		private readonly TenorClient _client =
			new(ApiKey,
				new Locale("en_US"),
				AspectRatio.all,
				ContentFilter.medium,
				MediaFilter.basic,
				"ecb8d811a40547cf81e8db1014823e30");


		private readonly ITestOutputHelper _testOutputHelper;

		public IntegrationTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Theory]
		[InlineData("test", 50, 0)]
		public void TestSearch(string q, int limit, int pos)
		{
			try
			{
				_client.Search(q, limit, pos);
			}
			catch (TenorException e)
			{
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

		[Theory]
		[InlineData(Type.emoji)]
		[InlineData(Type.trending)]
		[InlineData(Type.featured)]
		public void TestCategories(Type type)
		{
			try
			{
				_client.Categories(type);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData("test", 50)]
		public void TestAutoComplete(string q, int limit)
		{
			try
			{
				_client.AutoComplete(q, limit);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData(50, 0, new[] {"17599391", "12846096", "8766184", "8766189"})]
		public void TestGetGifs(int limit, int pos, string[] ids)
		{
			try
			{
				_client.GetGifs(limit, pos, ids);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData("17599391", "test")]
		public void TestRegisterShare(string id, string q)
		{
			try
			{
				_client.RegisterShare(id, q);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData("test", 50)]
		public void TestSearchSuggestions(string q, int limit)
		{
			try
			{
				_client.SearchSuggestions(q, limit);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData(50)]
		public void TestTrendingTerms(int limit)
		{
			try
			{
				_client.TrendingTerms(limit);
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}

		[Theory]
		[InlineData("test", 50, 0)]
		public void TestGetRandomGifs(string q, int limit, int pos)
		{
			try
			{
				_client.GetRandomGifs(q, limit, pos);
			}
			catch (TenorException e)
			{
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