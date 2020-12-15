using Newtonsoft.Json;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;

namespace TenorSharp.SearchResults
{
	public class Gif
	{
		internal TenorClient Client;
		internal int         Count;
		internal string[]    Ids;
		internal string      Term;

		internal SearchTypes Type;

		/// <summary>
		///     a position identifier to use with the next API query to retrieve the next set of results, or null if there are no
		///     further results.
		/// </summary>
		[JsonProperty("next", Required = Required.Always)]
		public string NextGifs { get; set; }

		/// <summary>
		///     the most relevant GIFs for the requested search term - Sorted by relevancy Rank
		/// </summary>
		[JsonProperty("results", Required = Required.Always)]
		public GifObject[] GifResults { get; set; }

		/// <summary>
		///     returns the next set of GIFs for the used search term and result count
		/// </summary>
		/// <returns>the next set of GIFs</returns>
		public Gif Next()
		{
			return Next(Count);
		}

		/// <summary>
		///     returns the next set of GIFs for the used search term and a different result count
		/// </summary>
		/// <param name="count">the new result count</param>
		/// <returns>the next set of GIFs</returns>
		public Gif Next(int count)
		{
			switch (Type)
			{
				case SearchTypes.search:
					return Client.Search(Term, count, NextGifs);
				case SearchTypes.trending:
					return Client.Trending(count, NextGifs);
				case SearchTypes.getGifs:
					return Client.GetGifs(count, NextGifs, Ids);
				case SearchTypes.getRandom:
					return Client.GetRandomGifs(Term, count, NextGifs);
				default:
					return null;
			}
		}
	}
}