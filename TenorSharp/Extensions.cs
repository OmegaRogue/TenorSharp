using RestSharp;

namespace TenorSharp
{
	public static class Extensions
	{
		public static IRestRequest RemoveParameter(this RestRequest request, Parameter p)
		{
			request.Parameters.Remove(p);
			return request;
		}

		public static IRestRequest RemoveParameterIfExists(this RestRequest request, Parameter p)
		{
			if (request.Parameters.Contains(p))
				request.Parameters.Remove(p);
			return request;
		}
	}
}