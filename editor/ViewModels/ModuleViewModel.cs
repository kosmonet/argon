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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Argon.Editor.ViewModels;

internal partial class ModuleViewModel(ModuleAsset asset, ConfigurationService configuration) : ObservableObject {
	[ObservableProperty] 
	private ModuleAsset _module = asset;

	private readonly ILogger _logger = LogHelper.Logger;
	private readonly ConfigurationService _configuration = configuration;

	/// <summary>
	/// Opens the selected module for editing.
	/// </summary>
	[RelayCommand]
	public void OpenModule() {
		_logger.LogInformation("open module: {id}", Module.Id);
	}

	/// <summary>
	/// Removes the selected module from the catalog.
	/// </summary>
	[RelayCommand]
	public void DeleteModule() {
		_logger.LogInformation("delete module: {id}", Module.Id);
		_configuration.Modules.Remove(Module.Id);
	}
}
