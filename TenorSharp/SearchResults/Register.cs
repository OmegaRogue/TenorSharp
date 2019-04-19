using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Register
	{
		/// <summary>
		///     set to “ok” if share registration was successful
		/// </summary>
		[JsonProperty("status", Required = Required.Always)]
		public string ShareStatus { get; set; }
	}
}