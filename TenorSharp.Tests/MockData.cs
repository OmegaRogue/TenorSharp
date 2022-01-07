namespace TenorSharp.Tests;

public struct MockData
{
	public const string ErrorAnonId = "{\"code\": 1, \"error\": \"Anon_id must be between 16 and 32 characters.\"}";

	public const string ErrorAnonIdLimit =
		"{\"code\": 1, \"error\": \"Anon_id must be between 16 and 32 characters and limit must be between 1 and 50.\"}";

	public const string ErrorLimit = "{\"code\": 1, \"error\": \"Limit must be between 1 and 50.\"}";

	public const string SearchRespone = "";

	public const string TrendinRespone = "";

	public const string CategoryRespone = "";

	public const string SearchSuggestionRespone = "";

	public const string AutoCompleteRespone = "";

	public const string TendingTermsRespone = "";

	public const string RegisterShareRespone = "";

	public const string AnonIdResponse = "";

	public const string GetRandomGifsRespone = "";
}