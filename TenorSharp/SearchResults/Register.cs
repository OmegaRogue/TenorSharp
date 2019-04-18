using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Register
	{
		[JsonProperty("status", Required = Required.Always)]
		public string ShareStatus { get; set; }
	}
}