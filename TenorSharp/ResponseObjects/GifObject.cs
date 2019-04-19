using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TenorSharp.Enums;

namespace TenorSharp.ResponseObjects
{
	public class GifObject
	{
		/// <summary>
		///     a unix timestamp representing when this post was created.
		/// </summary>
		[JsonProperty("created")]
		public double Created;

		/// <summary>
		///     true if this post contains audio (only video formats support audio, the gif image file format can not contain audio
		///     information).
		/// </summary>
		[JsonProperty("hasaudio")]
		public bool HasAudio;

		/// <summary>
		///     true if this post contains captions
		/// </summary>
		[JsonProperty("hascaption")]
		public bool HasCaption;

		/// <summary>
		///     Tenor result identifier
		/// </summary>
		[JsonProperty("id")]
		public string Id;

		/// <summary>
		///     the full URL to view the post on tenor.com.
		/// </summary>
		[JsonProperty("itemurl")]
		public Uri ItemUrl;

		/// <summary>
		///     An array of dictionaries with GifFormat as the key and MediaObject as the value
		/// </summary>
		[JsonProperty("media")]
		public Dictionary<GifFormat, MediaObject>[] Media;

		/// <summary>
		///     the amount of Shares the post has.
		/// </summary>
		[JsonProperty("shares")]
		public int Shares;

		/// <summary>
		///     an array of tags for the post
		/// </summary>
		[JsonProperty("tags")]
		public string[] Tags;

		/// <summary>
		///     the title of the post.
		/// </summary>
		[JsonProperty("title")]
		public string Title;

		/// <summary>
		///     a short URL to view the post on tenor.com.
		/// </summary>
		[JsonProperty("url")]
		public Uri Url;
	}
}