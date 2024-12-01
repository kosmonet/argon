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
using Argon.Editor.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Argon.Editor.ViewModels;

internal partial class ModuleWindowViewModel : ObservableObject {
	/// <summary>
	/// A collection of modules.
	/// </summary>
	public ObservableCollection<ModuleViewModel> Modules { get; } = [];

	private readonly Settings _settings;

	internal ModuleWindowViewModel(Settings settings) {
		_settings = settings;

		foreach (string id in settings.Modules) {
/*			ModuleAsset module = _assets.GetAsset<ModuleAsset>(id);
			_moduleWindowViewModel.Modules.Add(new ModuleViewModel(module));
*/
		}
	}
}
