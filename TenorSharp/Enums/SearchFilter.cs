// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace TenorSharp.Enums;

/// <summary>
/// Parameter to switch between a GIF search and a Sticker search
/// </summary>
public enum SearchFilter
{
	/// <summary>
	/// A Normal GIF Search
	/// </summary>
	off,

	/// <summary>
	/// Parameter that enables sticker search rather than GIF search.<br/>
	/// Do not use media_filter when requesting sticker searches.<br/>
	/// Doing so will filter out the transparent formats.
	/// </summary>
	sticker,
}