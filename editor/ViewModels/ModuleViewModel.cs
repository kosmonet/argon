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
using Argon.Editor.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Argon.Editor.ViewModels;

/// <summary>
/// The view model of the module window that is displayed when starting the application.
/// </summary>
public partial class ModuleViewModel : ObservableObject {
	/// <summary>
	/// A collection of modules.
	/// </summary>
	internal ObservableCollection<ModuleAsset> Modules { get; } = [];

	private static readonly ILogger _logger = LogHelper.Logger;

	private readonly AssetManager _assets;
	private readonly ModuleService _moduleService;

	/// <summary>
	/// Initializes this view model.
	/// </summary>
	/// <param name="modules"></param>
	/// <param name="assets"></param>
	internal ModuleViewModel(ModuleService modules, AssetManager assets) {
		_assets = assets;
		_moduleService = modules;

		// register all modules in the data folder
		foreach (string id in _moduleService.Modules) {
			Modules.Add(_assets.GetAsset<ModuleAsset>(id));
		}
	}

	/// <summary>
	/// Opens an existing module for editing.
	/// </summary>
	[RelayCommand]
	internal void OpenModule(ModuleAsset module) {
		if (module != null) {
			_logger.LogInformation("open module: {id}", module.Id);
		} else {
			_logger.LogInformation("no module selected to open");
		}
	}

	/// <summary>
	/// Creates a new module and opens it for editing.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="title"></param>
	/// <param name="subtitle"></param>
	/// <param name="description"></param>
	internal void CreateModule(string id, string title, string subtitle, string description) {
		ModuleAsset module = _moduleService.CreateModule(id, title, subtitle, description);
		OpenModule(module);
	}
}
