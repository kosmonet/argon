/*
 *	Argon, a roguelike engine.
 *	Copyright (C) 2023 - Maarten Driesen
 * 
 *	This program is free software; you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation; either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	This program is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Argon.Common.Files;
using Argon.Common.Assets;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Argon.Editor;

/// <summary>
/// 
/// </summary>
public static class Editor {
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public static MauiApp CreateMauiApp() {
		string root = Environment.GetCommandLineArgs()[1];
		Debug.WriteLine(Path.GetDirectoryName(root));
		var files = new ArgonFileSystem(Path.Combine(root, "..", "temp"));
		files.AddModule(root);
		files.AddModule(Path.Combine(root, "..", "test.zip"));
		var assets = new AssetManager();
		assets.RegisterLoader(new CreatureLoader(files));
		assets.RegisterLoader(new ItemLoader(files));
		assets.RegisterLoader(new ModuleLoader(files));
		Debug.WriteLine(assets.GetAsset<ItemAsset>("fork"));

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Logging.AddDebug();
		return builder.Build();
	}
}