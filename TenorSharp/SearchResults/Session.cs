using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Session
	{
		[JsonProperty("anon_id", Required = Required.Always)]
		public string AnonId { get; set; }
	}
}