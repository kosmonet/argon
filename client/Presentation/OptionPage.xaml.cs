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

public sealed partial class OptionPage : Page {
    public OptionPage() {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the keyboard shortcuts on the start page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeyPressed(object sender, KeyRoutedEventArgs e) {
        INavigator navigator = Guard.NotNull<INavigator>(this.Navigator(), "INavigator not available.");

        switch (e.Key) {
            case VirtualKey.Enter: 
                Console.Out.WriteLine("saving configuration");
                navigator.GoBack(sender); break;
            case VirtualKey.Escape: navigator.GoBack(sender); break;
        }
    }
}

