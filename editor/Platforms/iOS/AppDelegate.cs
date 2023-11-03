using Foundation;

namespace Argon.Editor; 
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate {
	protected override MauiApp CreateMauiApp() => Editor.CreateMauiApp();
}