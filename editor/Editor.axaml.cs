/*
 *	Argon, a roguelike engine.
 *	Copyright (C) 2024 - Maarten Driesen
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

using Argon.Common.Assets;
using Argon.Common.Files;
using Argon.Editor.Services;
using Argon.Editor.ViewModels;
using Argon.Editor.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System;
using System.IO;

namespace Argon.Editor;

/// <summary>
/// The main class of the Argon editor.
/// </summary>
public partial class Editor : Application {
	private readonly ConfigurationService _configuration;
	private readonly ArgonFileSystem _files;
	private readonly AssetManager _assets;
	private readonly ModuleService _modules;

	/// <summary>
	/// Initializes the editor. Avalonia initialization is done in a separate method.
	/// </summary>
	public Editor() {
		// load user settings
		_configuration = new ConfigurationService();

		// initialize the file system with the correct temporary folder path
		_files = new ArgonFileSystem(Path.Combine(_configuration.DataFolder, "temp"));

		// register all necessary loaders with the asset manager
		_assets = new AssetManager();
		_assets.RegisterLoader(new ModuleLoader(_files));
		_assets.RegisterLoader(new CreatureLoader(_files));

		// initialize the module service
		_modules = new ModuleService(_configuration, _files, _assets);
	}

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	/// <summary>
	/// Don't remove, used by Avalonia.
	/// </summary>
	public override void OnFrameworkInitializationCompleted() {
		// Line below is needed to remove Avalonia data validation. Without this line
		// you will get duplicate validations from both Avalonia and CommunityToolkit.
		BindingPlugins.DataValidators.RemoveAt(0);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new ModuleWindow() {
				DataContext = new ModuleViewModel(_modules, _assets)
			};
		}

		base.OnFrameworkInitializationCompleted();
	}

	/// <summary>
	/// Main method, initializes Avalonia.
	/// </summary>
	/// <param name="args">The command-line arguments.</param>
	[STAThread]
	public static void Main(string[] args) {
		BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
	}

	/// <summary>
	/// Avalonia configuration, don't remove; also used by visual designer.
	/// </summary>
	/// <returns>An AppBuilder instance.</returns>
	public static AppBuilder BuildAvaloniaApp() {
		return AppBuilder.Configure<Editor>().UsePlatformDetect().WithInterFont().LogToTrace();
	}
}