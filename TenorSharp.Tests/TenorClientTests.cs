using System;
using System.Net.Http;
using System.Threading.Tasks;

using NSubstitute;

using NUnit.Framework;

using TenorSharp.ResponseObjects;

namespace TenorSharp.Tests
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public class TenorClientTests
	{
		[SetUp]
		public void SetUp() => subHttpClient = Substitute.For<HttpClient>();

		private const string     AnonIdZero = "000000000000000000";
		private       HttpClient subHttpClient;

		[DatapointSource]

		// ReSharper disable once UnusedMember.Global
		public string[] StringValues =
		{
			null, "", "1", "0000000000000", "0000000000000000000", "0000000000000000000000000000000000000000"
		};

		[DatapointSource]

		// ReSharper disable once UnusedMember.Global
		public int[] IntValues = { -1000, -100, -51, -50, -49, -4, -1, 0, 1, 4, 49, 50, 51, 100, 1000 };

		private static TenorClient CreateTenorClient() => new(new TenorConfiguration { ApiKey = "test" });

		[Test]
		public async Task GetMediaStreamAsync_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var url = new Uri(
				"https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png");

			// Act
			var result = await TenorClient.GetMediaStreamAsync(url);

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void ResetLocale()
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			tenorClient.Locale = new Locale("de_de");
			Assume.That(tenorClient.Locale, Is.Not.EqualTo(new Locale("en_us")));

			// Act
			tenorClient.ResetLocale();

			// Assert
			Assert.That(new Locale("en_us"), Is.EqualTo(tenorClient.Locale));
		}

		[Theory]
		public void NewSession_AnonIdLength_Successful(string anonId)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(anonId,        Is.Not.Null);
			Assume.That(anonId.Length, Is.GreaterThanOrEqualTo(16).And.LessThanOrEqualTo(32));

			// Act
			tenorClient.NewSession(anonId);

			// Assert
			Assert.That(() => tenorClient.NewSession(anonId), Throws.Nothing);
		}

		[Theory]
		public void NewSession_AnonIdLength_Exception(string anonId)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(anonId,        Is.Not.EqualTo(null));
			Assume.That(anonId.Length, Is.LessThan(16).Or.GreaterThan(32));

			// Act
			void Act() => tenorClient.NewSession(anonId);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Anon_id must be between 16 and 32 characters."));
		}

		[Test]
		public void NewSession_AnonIdNull_Exception()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.NewSession(null);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<NullReferenceException>());
		}

		[Test]
		public void GetSession_NoSession_ReturnsNull()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			var result = tenorClient.GetSession();

			// Assert
			Assert.That(result, Is.EqualTo(null));
		}

		[Test]
		public void GetSession_AnonIdZero_ReturnsSession()
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			tenorClient.NewSession(AnonIdZero);


			// Act
			var result = tenorClient.GetSession();

			// Assert
			Assert.That(result, Is.EqualTo(AnonIdZero));
		}

		[Test]
		public void ClearSession_AnonIdZero_ClearsSession()
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			tenorClient.NewSession(AnonIdZero);

			// Act
			tenorClient.ClearSession();

			// Assert
			Assert.That(tenorClient.Configuration.AnonId, Is.EqualTo(null));
		}

		[Theory]
		public void SearchAsync_Success(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.SearchAsync(q, limit, pos).GetAwaiter().GetResult();

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void SearchAsync_LimitOutOfRange_Exception(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));

			// Act
			void Act() => tenorClient.Search(q, limit, pos);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void TrendingAsync_Success(int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.Trending(limit, pos);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void TrendingAsync_LimitOutOfRange_Exception(int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));

			// Act
			void Act() => tenorClient.Trending(limit, pos);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void CategoriesAsync_Success(Enums.Type categoryType)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.Categories(categoryType);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void SearchSuggestionsAsync_Success(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.SearchSuggestions(q, limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void SearchSuggestionsAsync_LimitOutOfRange_Exception(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));

			// Act
			void Act() => tenorClient.SearchSuggestions(q, limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void AutoCompleteAsync_Success(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.AutoComplete(q, limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void AutoCompleteAsync_LimitOutOfRange_Exception(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));

			// Act
			void Act() => tenorClient.AutoComplete(q, limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void TrendingTermsAsync_Success(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.TrendingTerms(limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void TrendingTermsAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));

			// Act
			void Act() => tenorClient.TrendingTerms(limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void RegisterShareAsync_Success(string id, string q)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(id, Is.Not.Empty);

			// Act
			void Act() => tenorClient.RegisterShare(id, q);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void RegisterShareAsync_EmptyId_Exception(string q)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.RegisterShare("", q);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void GetGifsAsync_Success(int limit, string pos, string id)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.GetGifs(limit, pos, id, id);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void GetGifsAsync_LimitOutOfRange_Exception(int limit, string pos, string id)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));


			// Act
			void Act() => tenorClient.GetGifs(limit, pos, id, id);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void GetGifsAsync_EmptyPos_Exception(int limit, string pos, string id)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.GetGifs(limit, pos, id, id);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Test]
		public void GetNewAnonIdAsync()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			void Act() => tenorClient.GetNewAnonId();

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void GetRandomGifsAsync_Success(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(q,     Is.Not.EqualTo(""));
			Assume.That(pos,   Is.Not.EqualTo(""));
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.GetRandomGifs(q, limit, pos);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void GetRandomGifsAsync_LimitOutOfRange_Exception(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(limit, Is.LessThanOrEqualTo(0).Or.GreaterThan(50));
			Assume.That(pos,   Is.Not.EqualTo(""));
			Assume.That(q,     Is.Not.EqualTo(""));

			// Act
			void Act() => tenorClient.GetRandomGifs(q, limit, pos);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Theory]
		public void GetRandomGifsAsync_EmptySearch_Exception(int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(pos,   Is.Not.EqualTo(""));
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.GetRandomGifs("", limit, pos);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Theory]
		public void GetRandomGifsAsync_EmptyPos_Exception(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();
			Assume.That(q,     Is.Not.EqualTo(""));
			Assume.That(limit, Is.GreaterThan(0).And.LessThanOrEqualTo(50));

			// Act
			void Act() => tenorClient.GetRandomGifs(q, limit, "");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}
	}
}