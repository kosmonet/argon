using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace argon.common;

public static class ArgonLoggerFactory
{
	private static readonly ILogger Logger = new DebugLoggerProvider().CreateLogger("argon");

	public static ILogger GetLogger()
	{
		return Logger;
	}
}