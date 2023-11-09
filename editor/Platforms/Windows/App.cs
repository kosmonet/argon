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

using System.Diagnostics;
using Argon.Common;
using System.Runtime.Versioning;

namespace Argon.Editor;
public partial class App : Application {
	protected override Window CreateWindow(IActivationState? activationState) {
		var window = base.CreateWindow(activationState);
		window.Created += Window_Created;
		return window;
	}

	private async void Window_Created(object? sender, EventArgs e) {
		const int defaultWidth = 1200;
		const int defaultHeight = 800;

		var window = (Window)Guard.NonNull(sender, "The sender must not be null.");
		window.Width = defaultWidth;
		window.Height = defaultHeight;
		window.X = -defaultWidth;
		window.Y = -defaultHeight;

		await window.Dispatcher.DispatchAsync(() => { });

		var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
		Debug.WriteLine($"disp.Width = {displayInfo.Width}");
		Debug.WriteLine($"disp.Height = {displayInfo.Height}");
		Debug.WriteLine($"disp.Density = {displayInfo.Density}");
		window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
		window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
	}
}
