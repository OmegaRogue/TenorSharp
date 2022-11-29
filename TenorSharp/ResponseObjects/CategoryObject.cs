using System;

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TenorSharp.ResponseObjects;

public class CategoryObject
{
	/// <summary>
	///     The english search term that corresponds to the category
	/// </summary>
	[JsonPropertyName("searchterm")]
	public string SearchTerm { get; set; }
	
	/// <summary>
	///     the search url to request if the user selects the category
	/// </summary>
	[JsonPropertyName("path")]
	public string Path { get; set; }
	
	/// <summary>
	///     a url to the media source for the category’s example GIF
	/// </summary>
	[JsonPropertyName("image")]
	public string Image { get; set; }

	/// <summary>
	///     Category name to overlay over the image. The name will be translated to match the locale of the corresponding
	///     request
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }
}