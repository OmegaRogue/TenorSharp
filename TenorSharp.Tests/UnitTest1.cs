using System;
using System.Linq;

using Moq;

using RestSharp;

using Xunit;
using Xunit.Abstractions;

namespace TenorSharp.Tests
{
	public class UnitTest1
	{
		private const           string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		private static readonly Mock<RestClient> testClient = new Mock<RestClient>();
		private static readonly Random random = new Random();
		private static readonly string ApiKey = Environment.GetEnvironmentVariable("TENOR_TEST_API_KEY");
		private readonly        TenorClient _client = new TenorClient(ApiKey);


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
			return new string(Enumerable.Range(1, len).Select(_ => Chars[random.Next(Chars.Length)]).ToArray());
		}

		private static string RndLenString()
		{
			var len = random.Next();
			return RndString(len);
		}

		[Fact]
		public void TestSearch()
		{
			try
			{
				_client.NewSession(RndString(18));
				_client.Search(RndString(10), random.Next(50), random.Next());
			}
			catch (TenorException e)
			{
				_testOutputHelper.WriteLine(e.ToString());
				throw;
			}
		}
	}
}