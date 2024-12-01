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

using Argon.Common;
using Argon.Common.Assets;
using Argon.Common.Files;
using Argon.Editor.Models;
using Argon.Editor.ViewModels;
using Argon.Editor.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Argon.Editor;

/// <summary>
/// The main class of the Argon editor.
/// </summary>
public partial class Editor : Application {
	private readonly ILogger _logger = LogHelper.Logger;
	private readonly Settings _settings = new();
	private readonly ArgonFileSystem _files;
	private readonly AssetManager _assets = new();
	private readonly ModuleWindowViewModel _moduleWindowViewModel;

	public Editor() {
		// initialize the file system with the correct temporary folder path
		_files = new ArgonFileSystem(Path.Combine(_settings.DataFolder, "temp"));

		// register all necessary loaders with the asset manager
		_assets.RegisterLoader(new ModuleLoader(_files));

		// create the view models
		_moduleWindowViewModel = new(_settings);

		foreach (string id in _settings.Modules) {
			_logger.LogInformation("add module: {id}", id);
			_files.AddModule(Path.Combine(_settings.DataFolder, id));
			ModuleAsset module = _assets.GetAsset<ModuleAsset>(id);
			_moduleWindowViewModel.Modules.Add(new ModuleViewModel(module));
		}
	}

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		// Line below is needed to remove Avalonia data validation. Without this line
		// you will get duplicate validations from both Avalonia and CommunityToolkit.
		BindingPlugins.DataValidators.RemoveAt(0);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new ModuleWindow(_moduleWindowViewModel);
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