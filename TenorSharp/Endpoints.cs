namespace TenorSharp
{
	public static class Endpoints
	{
		/// <summary>
		///     Get a json object containing a list of the most relevant GIFs for a given search term(s), category(ies), emoji(s),
		///     or any combination thereof.
		/// </summary>
		public const string Search = @"https://api.tenor.com/v1/search?";

		/// <summary>
		///     Get a json object containing a list of the current global trending GIFs. The trending stream is updated regularly
		///     throughout the day.
		/// </summary>
		public const string Trending = @"https://api.tenor.com/v1/trending?";

		/// <summary>
		///     Get a json object containing a list of GIF categories associated with the provided type.
		///     Each category will include a corresponding search URL to be used if the user clicks on the category.
		///     The search URL will include the apikey, anonymous id, and locale that were used on the original call to the
		///     categories endpoint.
		///     Supported types:
		///     featured (default) - The current featured emotional / reaction based GIF categories including a preview GIF for
		///     each term.
		///     emoji - The current featured emoji GIF categories
		///     trending - The current trending search terms including a preview GIF for each term.
		/// </summary>
		public const string Categories = @"https://api.tenor.com/v1/categories?";

		/// <summary>
		///     Get a json object containing a list of alternative search terms given a search term.
		///     Search suggestions helps a user narrow their search or discover related search terms to find a more precise GIF.
		///     Results are returned in order of what is most likely to drive a share for a given term, based on historic user
		///     search and share behavior.
		/// </summary>
		public const string SearchSuggestions = @"https://api.tenor.com/v1/search_suggestions?";

		/// <summary>
		///     Get a json object containing a list of completed search terms given a partial search term.
		///     The list is sorted by Tenor’s AI and the number of results will decrease as Tenor’s AI becomes more certain.
		/// </summary>
		public const string Autocomplete = @"https://api.tenor.com/v1/autocomplete?";

		/// <summary>
		///     Get a json object containing a list of the current trending search terms.
		///     The list is updated Hourly by Tenor’s AI.
		/// </summary>
		public const string TrendingTerms = @"https://api.tenor.com/v1/trending_terms?";

		/// <summary>
		///     Register a user’s sharing of a GIF.
		/// </summary>
		public const string RegisterShare = @"https://api.tenor.com/v1/registershare?";

		/// <summary>
		///     Get the GIF(s) for the corresponding id(s)
		/// </summary>
		public const string Gifs = @"https://api.tenor.com/v1/gifs?";

		/// <summary>
		///     Get an anonymous ID for a new user.
		///     Store the ID in the client's cache for use on any additional API calls made by the user, either in this session or
		///     any future sessions.
		/// </summary>
		public const string AnonId = @"https://api.tenor.com/v1/anonid?";

		/// <summary>
		///     Get a randomized list of GIFs for a given search term.
		///     This differs from the search endpoint which returns a rank ordered list of GIFs for a given search term.
		/// </summary>
		public const string Random = @"https://api.tenor.com/v1/random?";
	}
}