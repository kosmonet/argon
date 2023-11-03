using Android.App;
using Android.Runtime;

namespace Argon.Editor; 
[Application]
public class MainApplication : MauiApplication {
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership) {
	}

	protected override MauiApp CreateMauiApp() => Editor.CreateMauiApp();
}