using Newtonsoft.Json;
using TenorSharp.ResponseObjects;

namespace TenorSharp.SearchResults
{
	public class Gif
	{
		/// <summary>
		///     a position identifier to use with the next API query to retrieve the next set of results, or null if there are no
		///     further results.
		/// </summary>
		[JsonProperty("next", Required = Required.Always)]
		public int NextGifs { get; set; }

		/// <summary>
		///     the most relevant GIFs for the requested search term - Sorted by relevancy Rank
		/// </summary>
		[JsonProperty("results", Required = Required.Always)]
		public GifObject[] GifResults { get; set; }
	}
}