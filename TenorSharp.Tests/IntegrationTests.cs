using System;
using System.Linq;

using TenorSharp.Enums;

using Xunit;
using Xunit.Abstractions;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp.Tests
{
	[Collection("Integration Tests")]
	public class IntegrationTests
	{
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
		[InlineData("test",      50, 0)]
		[InlineData("",          50, 0)]
		[InlineData("dehfghfgh", 50, 0)]
		[InlineData("test",      50, 1)]
		[InlineData("test",      51, 0)]
		[InlineData("test",      0,  0)]
		[InlineData(null,        0,  0)]
		public void TestSearchInt(string q, int limit, int pos)
		{
			try
			{
				_client.Search(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData("test", 50, "")]
		[InlineData("test", 50, "null")]
		[InlineData("test", 50, null)]
		[InlineData("test", 50, "0.5")]
		public void TestSearchString(string q, int limit, string pos)
		{
			try
			{
				_client.Search(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData("test",      50, 0)]
		[InlineData("",          50, 0)]
		[InlineData("dehfghfgh", 50, 0)]
		[InlineData("test",      50, 1)]
		[InlineData("test",      51, 0)]
		[InlineData("test",      0,  0)]
		[InlineData(null,        0,  0)]
		public void TestSearchStickersInt(string q, int limit, int pos)
		{
			try
			{
				_client.SearchStickers(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData("test", 50, "")]
		[InlineData("test", 50, "null")]
		[InlineData("test", 50, null)]
		[InlineData("test", 50, "0.5")]
		public void TestSearchStickersString(string q, int limit, string pos)
		{
			try
			{
				_client.SearchStickers(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(50, 0, new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, 1, new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(51, 0, new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(51, 1, new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(0,  0, new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, 0, new[] {"null", "12846096", "8766184", "8766189"})]
		[InlineData(50, 0, new[] {null, "12846096", "8766184", "8766189"})]
		public void TestGetGifsInt(int limit, int pos, string[] ids)
		{
			try
			{
				_client.GetGifs(limit, pos, ids);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (ids.Any(s => !int.TryParse(s, out var _)))
				{
					Assert.Equal("Valid id is required.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(50, "0",    new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, "",     new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, "null", new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, null,   new[] {"17599391", "12846096", "8766184", "8766189"})]
		[InlineData(50, "0.5",  new[] {"17599391", "12846096", "8766184", "8766189"})]
		public void TestGetGifsString(int limit, string pos, string[] ids)
		{
			try
			{
				_client.GetGifs(limit, pos, ids);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(20, 0)]
		[InlineData(20, 1)]
		[InlineData(51, 0)]
		[InlineData(0,  0)]
		public void TestTrendingInt(int limit, int pos)
		{
			try
			{
				_client.Trending(limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(20, "0")]
		[InlineData(20, "")]
		[InlineData(20, "null")]
		[InlineData(20, null)]
		[InlineData(20, "0.5")]
		public void TestTrendingString(int limit, string pos)
		{
			try
			{
				_client.Trending(limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(20, 0)]
		[InlineData(20, 1)]
		[InlineData(51, 0)]
		[InlineData(0,  0)]
		public void TestTrendingStickersInt(int limit, int pos)
		{
			try
			{
				_client.TrendingStickers(limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData(20, "0")]
		[InlineData(20, "")]
		[InlineData(20, "null")]
		[InlineData(20, null)]
		[InlineData(20, "0.5")]
		public void TestTrendingStickersString(int limit, string pos)
		{
			try
			{
				_client.TrendingStickers(limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}


		[Theory]
		[InlineData("test", 50)]
		[InlineData("test", 51)]
		[InlineData("test", 0)]
		[InlineData("",     50)]
		[InlineData(null,   50)]
		public void TestSearchSuggestions(string q, int limit)
		{
			try
			{
				_client.SearchSuggestions(q, limit);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
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
		[InlineData("",     50)]
		[InlineData(null,   50)]
		[InlineData("test", 51)]
		[InlineData("test", 0)]
		public void TestAutoComplete(string q, int limit)
		{
			try
			{
				_client.AutoComplete(q, limit);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}


		[Theory]
		[InlineData("17599391", "test")]
		[InlineData("17599391", "")]
		[InlineData("17599391", null)]
		[InlineData("",         "test")]
		[InlineData("",         "")]
		[InlineData("",         null)]
		[InlineData(null,       "test")]
		[InlineData(null,       "")]
		[InlineData(null,       null)]
		public void TestRegisterShare(string id, string q)
		{
			try
			{
				_client.RegisterShare(id, q);
			}
			catch (TenorException e)
			{
				if (!int.TryParse(id, out var _))
				{
					Assert.Equal("Id is not a valid int.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}


		[Theory]
		[InlineData(50)]
		[InlineData(51)]
		[InlineData(0)]
		public void TestTrendingTerms(int limit)
		{
			try
			{
				_client.TrendingTerms(limit);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData("test", 50, 0)]
		[InlineData("",     50, 0)]
		[InlineData("null", 50, 0)]
		[InlineData("test", 50, 1)]
		[InlineData("test", 51, 0)]
		[InlineData("test", 0,  0)]
		public void TestGetRandomGifsInt(string q, int limit, int pos)
		{
			try
			{
				_client.GetRandomGifs(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
			}
		}

		[Theory]
		[InlineData("test", 50, "0")]
		[InlineData("test", 50, "")]
		[InlineData("test", 50, "0.5")]
		[InlineData("test", 50, "null")]
		[InlineData("test", 50, null)]
		public void TestGetRandomGifsString(string q, int limit, string pos)
		{
			try
			{
				_client.GetRandomGifs(q, limit, pos);
			}
			catch (TenorException e)
			{
				if (limit > 50 || limit < 1)
				{
					Assert.Equal("Limit must be between 1 and 50.", e.Message);
				}
				else if (!float.TryParse(pos, out var _))
				{
					Assert.Equal("Invalid value for parameter \"pos\"", e.Message);
				}
				else
				{
					_testOutputHelper.WriteLine(e.ToString());
					throw;
				}
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