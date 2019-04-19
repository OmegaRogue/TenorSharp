using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Terms
	{
		/// <summary>
		///     An array of suggested search terms.
		/// </summary>
		[JsonProperty("results", Required = Required.Always)]
		public string[] SearchTerms;
	}
}