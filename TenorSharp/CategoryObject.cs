using System;
using Newtonsoft.Json;

namespace TenorSharp
{
	public class CategoryObject
	{
		/// <summary>
		///     a url to the media source for the category’s example GIF
		/// </summary>
		[JsonProperty("image")]
		public Uri Image;

		/// <summary>
		///     Category name to overlay over the image. The name will be translated to match the locale of the corresponding
		///     request
		/// </summary>
		[JsonProperty("name")]
		public string Name;

		/// <summary>
		///     the search url to request if the user selects the category
		/// </summary>
		[JsonProperty("path")]
		public Uri Path;

		/// <summary>
		///     The english search term that corresponds to the category
		/// </summary>
		[JsonProperty("searchterm")]
		public string SearchTerm;
	}
}