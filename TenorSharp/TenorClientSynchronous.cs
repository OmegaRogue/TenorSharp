using System;
using System.IO;

using TenorSharp.SearchResults;

using Type = TenorSharp.Enums.Type;

namespace TenorSharp;

public partial class TenorClient
{
	/// <inheritdoc cref="SearchAsync"/>
	public Gif Search(string q, int limit = 20, string pos = "0")
		=> SearchAsync(q, limit, pos).GetAwaiter().GetResult();

	/// <inheritdoc cref="TrendingAsync"/>
	public Gif Trending(int limit = 20, string pos = "0") => TrendingAsync(limit, pos).GetAwaiter().GetResult();

	/// <inheritdoc cref="CategoriesAsync"/>
	public Category Categories(Type type = Type.featured) => CategoriesAsync(type).GetAwaiter().GetResult();

	/// <inheritdoc cref="SearchSuggestionsAsync"/>
	public Terms SearchSuggestions(string q, int limit = 20)
		=> SearchSuggestionsAsync(q, limit).GetAwaiter().GetResult();

	/// <inheritdoc cref="AutoCompleteAsync"/>
	public Terms AutoComplete(string q, int limit = 20) => AutoCompleteAsync(q, limit).GetAwaiter().GetResult();

	/// <inheritdoc cref="TrendingTermsAsync"/>
	public Terms TrendingTerms(int limit = 20) => TrendingTermsAsync(limit).GetAwaiter().GetResult();

	/// <inheritdoc cref="RegisterShareAsync"/>
	public string RegisterShare(string id, string? q = null) => RegisterShareAsync(id, q).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetGifsAsync"/>
	public Gif GetGifs(int limit = 20, string pos = "0", params string[] ids)
		=> GetGifsAsync(limit, pos, ids).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetNewAnonIdAsync"/>
	public string GetNewAnonId() => GetNewAnonIdAsync().GetAwaiter().GetResult();

	/// <inheritdoc cref="GetRandomGifsAsync"/>
	public Gif GetRandomGifs(string q, int limit = 20, string pos = "0")
		=> GetRandomGifsAsync(q, limit, pos).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetMediaStreamAsync"/>
	public static Stream GetMediaStream(Uri url) => GetMediaStreamAsync(url).GetAwaiter().GetResult();

	/// <inheritdoc cref="GetMediaStream(Uri)" />
	public static Stream GetMediaStream(string url) => GetMediaStream(new Uri(url));
}