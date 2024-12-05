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
using Argon.Editor.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Argon.Editor.ViewModels;

/// <summary>
/// The view model of the module window that is displayed when starting the application.
/// </summary>
public partial class ModuleWindowViewModel : ObservableObject {
	/// <summary>
	/// A collection of modules.
	/// </summary>
	internal ObservableCollection<ModuleViewModel> Modules { get; } = [];

	private readonly ConfigurationService _settings;
	private readonly AssetManager _assets;
	private readonly ArgonFileSystem _files;
	private static readonly ILogger _logger = LogHelper.Logger;

	/// <summary>
	/// Initialize this view model.
	/// </summary>
	/// <param name="provider">Container for all services required by this view model.</param>
	internal ModuleWindowViewModel(ConfigurationService configuration, AssetManager assets, ArgonFileSystem files) {
		_settings = configuration;
		_assets = assets;
		_files = files;

		// add all active modules to the file system and prepare them for display
		foreach (string id in _settings.Modules) {
			_logger.LogInformation("add module: {id}", id);
			_files.AddModule(Path.Combine(_settings.DataFolder, id));
			ModuleAsset module = _assets.GetAsset<ModuleAsset>(id);
			// TODO: have ModuleAssets as the observables
			Modules.Add(new ModuleViewModel(module, configuration));
		}
	}

	/// <summary>
	/// Adds an existing module in the data folder.
	/// </summary>
	[RelayCommand]
	public void AddModule() {
		_logger.LogInformation("add existing module");
	}

	/// <summary>
	/// Creates a new module in the data folder.
	/// </summary>
	[RelayCommand]
	public void CreateModule() {
		_logger.LogInformation("create new module");
	}
}
