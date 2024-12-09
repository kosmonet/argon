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
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Argon.Editor.ViewModels;

/// <summary>
/// The view model of the module window that is displayed when starting the application.
/// </summary>
public partial class ModuleWindowViewModel : ObservableObject {
	/// <summary>
	/// A collection of modules.
	/// </summary>
	internal ObservableCollection<ModuleViewModel> Modules { get; } = [];

	/// <summary>
	/// The view model for the add module dialog box.
	/// </summary>
	internal AddModuleViewModel AddViewModel { get; } = new AddModuleViewModel();

	/// <summary>
	/// The selected module. May be null if no module has been selected.
	/// </summary>
	internal ModuleViewModel? SelectedModule {get; set;}

	private readonly AssetManager _assets;
	private static readonly ILogger _logger = LogHelper.Logger;

	/// <summary>
	/// Initializes this view model.
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="assets"></param>
	/// <param name="files"></param>
	internal ModuleWindowViewModel(ModuleService modules, AssetManager assets) {
		_assets = assets;

		// register all modules in the catalog
		foreach (string id in modules.Modules) {
			ModuleAsset module = _assets.GetAsset<ModuleAsset>(id);
			Modules.Add(new ModuleViewModel(module));
		}
	}

	/// <summary>
	/// Opens an existing module for editing.
	/// </summary>
	[RelayCommand]
	public void OpenModule() {
		if (SelectedModule != null) {
			_logger.LogInformation("open module: {id}", SelectedModule.Module.Id);
		} else {
			_logger.LogInformation("opening module failed, no module selected");
		}
	}
}
