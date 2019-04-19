namespace TenorSharp.Enums
{
	// ReSharper disable InconsistentNaming
	/// <summary>
	///     Reduce the Number of GIF formats returned in the GIF_OBJECT list.
	/// </summary>
	public enum MediaFilter
	{
		/// <summary>
		///     nanomp4, tinygif, tinymp4, gif, mp4, and nanogif
		/// </summary>
		basic,

		/// <summary>
		///     tinygif, gif, and mp4
		/// </summary>
		minimal,

		/// <summary>
		///     the default value
		/// </summary>
		off
	}
}