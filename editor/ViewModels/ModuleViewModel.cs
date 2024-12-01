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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Argon.Editor.ViewModels;

public partial class ModuleViewModel(ModuleAsset module) : ObservableObject {
	private static readonly ILogger _logger = LogHelper.Logger;

	/// <summary>
	/// The id of a module.
	/// </summary>
	[ObservableProperty]
	private string _id = module.Id;

	/// <summary>
	/// The title of a module.
	/// </summary>
	[ObservableProperty]
	private string _title = module.Title;

	/// <summary>
	/// The subtitle of a module.
	/// </summary>
	[ObservableProperty]
	private string _subtitle = module.Subtitle;

	/// <summary>
	/// The short description of a module.
	/// </summary>
	[ObservableProperty]
	private string? _description = module.Description;

	[RelayCommand]
	public void DeleteModule() {
		_logger.LogInformation("delete module: {id}", Id);
	}

	[RelayCommand]
	public void OpenModule() {
		_logger.LogInformation("open module: {id}", Id);
	}
}
