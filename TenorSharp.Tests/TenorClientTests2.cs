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
	[Category("Integration Tests")]
	[Author("OmegaRogue", "omegarogue@omegavoid.codes")]
	[TestOf(typeof(TenorClient))]
	public class TenorClientTests2
	{
		[SetUp]
		public void SetUp() => _subHttpClient = Substitute.For<HttpClient>();

		private const string     AnonIdZero = "000000000000000000";
		private       HttpClient _subHttpClient;

		[DatapointSource]

		// ReSharper disable once UnusedMember.Global
		public string[] StringValues =
		{
			null,
			"",
			"1",
			"0000000000000",
			"0000000000000000000",
			"0000000000000000000000000000000000000000",
			"12846096"
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

			// Act
			tenorClient.ResetLocale();

			// Assert
			Assert.That(new Locale("en_us"), Is.EqualTo(tenorClient.Locale));
		}

		[Test]
		public void NewSession_AnonIdLength_Successful()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.NewSession("0000000000000000000");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase("")]
		[TestCase("1")]
		[TestCase("0000000000000")]
		[TestCase("0000000000000000000000000000000000000000")]
		[TestCase("12846096")]
		public void NewSession_AnonIdLength_Exception(string anonId)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

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
			void Act() => tenorClient.NewSession(null!);

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

		[TestCase(null, 20, "0")]
		[TestCase("",   20, "0")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  1,  "0")]
		[TestCase("1",  4,  "0")]
		[TestCase("1",  49, "0")]
		[TestCase("1",  50, "0")]
		[TestCase(null, 20, "0")]
		[TestCase("1",  20, null)]
		[TestCase("1",  20, "")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  20, "0")]
		public void SearchAsync_Success(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.SearchAsync(q, limit, pos).GetAwaiter().GetResult();

			// Assert
			Assert.That(Act, Throws.Nothing);
		}


		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void SearchAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.Search("1", limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[TestCase(20, "0")]
		[TestCase(20, "0")]
		[TestCase(20, "0")]
		[TestCase(20, "0")]
		[TestCase(1,  "0")]
		[TestCase(4,  "0")]
		[TestCase(49, "0")]
		[TestCase(50, "0")]
		[TestCase(20, "0")]
		[TestCase(20, null)]
		[TestCase(20, "")]
		[TestCase(20, "0")]
		[TestCase(20, "0")]
		public void TrendingAsync_Success(int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.Trending(limit, pos);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void TrendingAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.Trending(limit);

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

		[TestCase(null, 20)]
		[TestCase("",   20)]
		[TestCase("1",  20)]
		[TestCase("1",  1)]
		[TestCase("1",  4)]
		[TestCase("1",  49)]
		[TestCase("1",  50)]
		public void SearchSuggestionsAsync_Success(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.SearchSuggestions(q, limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void SearchSuggestionsAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.SearchSuggestions("test", limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[TestCase(null, 20)]
		[TestCase("",   20)]
		[TestCase("1",  20)]
		[TestCase("1",  1)]
		[TestCase("1",  4)]
		[TestCase("1",  49)]
		[TestCase("1",  50)]
		public void AutoCompleteAsync_Success(string q, int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.AutoComplete(q, limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void AutoCompleteAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.AutoComplete("test", limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[TestCase(20)]
		[TestCase(1)]
		[TestCase(4)]
		[TestCase(49)]
		[TestCase(50)]
		public void TrendingTermsAsync_Success(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.TrendingTerms(limit);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void TrendingTermsAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.TrendingTerms(limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[TestCase(null)]
		[TestCase("")]
		[TestCase("1")]
		public void RegisterShareAsync_Success(string q)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.RegisterShare("12846096", q);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Test]
		public void RegisterShareAsync_EmptyId_Exception()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.RegisterShare("");

			// Assert
			Assert.That(Act,
				Throws.TypeOf<ArgumentNullException>().With.Message
					  .EqualTo("Supplied ID was null or empty. (Parameter 'id')"));
		}

		[TestCase(20)]
		[TestCase(1)]
		[TestCase(4)]
		[TestCase(49)]
		[TestCase(50)]
		public void GetGifsAsync_Success(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetGifs(limit, "0", "12846096", "12846096");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void GetGifsAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetGifs(limit, "0", "12846096", "12846096");

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[TestCase(null)]
		[TestCase("")]
		public void GetGifsAsync_EmptyPos_Exception(string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetGifs(20, pos, "12846096", "12846096");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Test]
		public void GetGifsAsync_EmptyId_Exception()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetGifs(20, "0", "", "");

			// Assert
			Assert.That(Act,
				Throws.TypeOf<ArgumentNullException>().With.Message
					  .EqualTo("All IDs supplied were null or empty. (Parameter 'ids')"));
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

		[TestCase(null, 20, "0")]
		[TestCase("",   20, "0")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  1,  "0")]
		[TestCase("1",  4,  "0")]
		[TestCase("1",  49, "0")]
		[TestCase("1",  50, "0")]
		[TestCase(null, 20, "0")]
		[TestCase("1",  20, null)]
		[TestCase("1",  20, "")]
		[TestCase("1",  20, "0")]
		[TestCase("1",  20, "0")]
		public void GetRandomGifsAsync_Success(string q, int limit, string pos)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetRandomGifs(q, limit, pos);

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(51)]
		public void GetRandomGifsAsync_LimitOutOfRange_Exception(int limit)
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetRandomGifs("test", limit);

			// Assert
			Assert.That(Act,
				Throws.TypeOf<TenorException>().With.Property("Message")
					  .EqualTo("Limit must be between 1 and 50."));
		}

		[Test]
		public void GetRandomGifsAsync_EmptySearch_Exception()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetRandomGifs("");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}

		[Test]
		public void GetRandomGifsAsync_EmptyPos_Exception()
		{
			// Arrange
			var tenorClient = CreateTenorClient();

			// Act
			void Act() => tenorClient.GetRandomGifs("test", 20, "");

			// Assert
			Assert.That(Act, Throws.Nothing);
		}
	}
}