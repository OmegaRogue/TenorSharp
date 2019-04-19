namespace TenorSharp.Enums
{
	// ReSharper disable InconsistentNaming
	/// <summary>
	///     Filter the response GIF_OBJECT list to only include GIFs with aspect ratios that fit with in the selected range.
	/// </summary>
	public enum AspectRatio
	{
		/// <summary>
		///     no constraints
		/// </summary>
		all,

		/// <summary>
		///     minimum: 0.42
		///     maximum: 2.36
		/// </summary>
		wide,

		/// <summary>
		///     minimum: 0.56
		///     maximum: 1.78
		/// </summary>
		standard
	}
}