using Newtonsoft.Json;

namespace TenorSharp.SearchResults
{
	public class Category
	{
		[JsonProperty("tags", Required = Required.Always)]
		public CategoryObject[] Tags { get; set; }
	}
}