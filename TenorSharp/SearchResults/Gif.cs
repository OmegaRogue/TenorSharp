using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;

namespace TenorSharp.SearchResults;

/// <summary>
/// A list of GIFs returned by a Tenor API Query.
/// </summary>
public class Gif
{
	internal TenorClient Client;
	internal int         Count;
	internal string[]    Ids;

	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

	internal string Term;

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
	public async Task<Gif> Next() => await Next(Count);

	/// <summary>
	///     returns the next set of GIFs for the used search term and a different result count
	/// </summary>
	/// <param name="count">the new result count</param>
	/// <returns>the next set of GIFs</returns>
	public async Task<Gif> Next(int count) => Type switch
	{
		SearchTypes.search => await Client.SearchAsync(Term, count, NextGifs),
		SearchTypes.trending => await Client.TrendingAsync(count, NextGifs),
		SearchTypes.getGifs => await Client.GetGifsAsync(count, NextGifs, Ids),
		SearchTypes.getRandom => await Client.GetRandomGifsAsync(Term, count, NextGifs),
		var _ => null
	};
}