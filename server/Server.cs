using System.Globalization;

namespace Argon.Server;

internal class Server {
	static void Main(string[] args) {
		CultureInfo.CurrentCulture = new CultureInfo("en-US");

		String path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		FileInfo info = new(path);

		Console.WriteLine($"Full Name: {info.FullName}{Environment.NewLine}Directory: {info.Directory}{Environment.NewLine}Extension: {info.Extension}{Environment.NewLine}Create Date: {info.CreationTime}");
	}
}