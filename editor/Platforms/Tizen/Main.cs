using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace Argon.Editor;

internal class Program : MauiApplication {
	protected override MauiApp CreateMauiApp() => Editor.CreateMauiApp();

	static void Main(string[] args) {
		var app = new Program();
		app.Run(args);
	}
}