/*
 *	Argon, a roguelike engine.
 *	Copyright (C) 2025 - Maarten Driesen
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
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Argon.Client.Presentation;

public sealed partial class DialogPage : Page {
    public DialogPage() {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the keyboard shortcuts on the load page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnKeyDown(object sender, KeyRoutedEventArgs args) {
        INavigator navigator = Guard.NotNull<INavigator>(this.Navigator(), "INavigator not available.");

        switch (args.Key) {
            // navigating back to a page only seems to work when the back stack is cleared
            case VirtualKey.Escape: navigator.NavigateRouteAsync(this, "-/Game"); break;
        }
    }
}
