using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Terms
	{
		[JsonProperty("results", Required = Required.Always)]
		public string[] SearchTerms;
	}
}