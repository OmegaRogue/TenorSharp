using Newtonsoft.Json;

namespace TenorSharp.ResponseObjects
{
	public class HttpError
	{
		/// <summary>
		///     an optional numeric code
		/// </summary>
		[JsonProperty("code")]
		public int Code;

		/// <summary>
		///     a string message describing the error
		/// </summary>
		[JsonProperty("error")]
		public string Error;
	}
}