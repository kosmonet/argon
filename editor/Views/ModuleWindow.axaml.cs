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

using Argon.Editor.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DialogHostAvalonia;

namespace Argon.Editor.Views;

internal partial class ModuleWindow : Window {
	public ModuleWindow() {
		InitializeComponent();
	}

	public void OnCreateClicked(object? sender, RoutedEventArgs e) {
		DialogHost.IsOpen = false;

		if (DataContext is ModuleViewModel model) {
			model.CreateModule(Id.Text, Title.Text, Subtitle.Text, Description.Text);
		}
	}
}
