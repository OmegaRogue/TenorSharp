using System;
using System.Collections.Generic;

using TenorSharp.Enums;
using TenorSharp.ResponseObjects;
using TenorSharp.SearchResults;

namespace TenorSharp.Tests;

public struct MockData
{
	public const string ErrorAnonId = "{\"code\": 1, \"error\": \"Anon_id must be between 16 and 32 characters.\"}";

	public const string ErrorAnonIdLimit =
		"{\"code\": 1, \"error\": \"Anon_id must be between 16 and 32 characters and limit must be between 1 and 50.\"}";

	public const string ErrorLimit = "{\"code\": 1, \"error\": \"Limit must be between 1 and 50.\"}";


	public static readonly MediaObject MediaObjectResponse = new()
															 {
																 Dims = new[] { 10, 10 },
																 Preview =
																	 "https://www.youtube.com/watch?v=iik25wqIuFo",
																 Size = 0,
																 Url = new Uri(
																	 "https://www.youtube.com/watch?v=iik25wqIuFo"),
																 Duration = 0,
															 };

	public static readonly GifObject GifObjectResponse = new()
														 {
															 Created    = 0,
															 HasAudio   = false,
															 HasCaption = false,
															 Id         = "iik25wqIuFo",
															 ItemUrl =
																 new Uri("https://www.youtube.com/watch?v=iik25wqIuFo"),
															 Media =
																 new[]
																 {
																	 new Dictionary<GifFormat, MediaObject>
																	 {
																		 { GifFormat.mp4, MediaObjectResponse }
																	 }
																 },
															 Shares = 0,
															 Tags   = new[] { "foo", "bar" },
															 Title  = "lorem ipsum",
															 Url = new Uri(
																 "https://www.youtube.com/watch?v=iik25wqIuFo")
														 };

	public static readonly CategoryObject CategoryObjectResponse = new()
																   {
																	   Image =
																		   new Uri(
																			   "https://www.youtube.com/watch?v=iik25wqIuFo"),
																	   Name = "bar",
																	   Path = new Uri(
																		   "https://www.youtube.com/watch?v=iik25wqIuFo"),
																	   SearchTerm = "foo",
																   };


	public static readonly Gif GifResponse = new() { NextGifs = "1", GifResults = new[] { GifObjectResponse } };

	public static readonly Category CategoryResponse = new() { Tags = new[] { CategoryObjectResponse }, };

	public static readonly Terms TermsResponse = new() { SearchTerms = new[] { "foo", "bar" } };

	public static readonly Register RegisterShareResponse = new() { ShareStatus = "ok", };

	public static readonly Session AnonIdResponse = new() { AnonId = "00000000000000000" };
}