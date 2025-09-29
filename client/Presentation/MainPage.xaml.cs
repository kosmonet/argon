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

using Microsoft.UI.Xaml.Input;

namespace Argon.Client.Presentation;

public sealed partial class MainPage : Page {
    public MainPage() {
        InitializeComponent();
        LostFocus += OnFocusLost;
    }

    /// <summary>
    /// Handles key presses.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnKeyDown(object sender, KeyRoutedEventArgs args) {
        // Console.Out.WriteLine($"KeyDown: {args.Key}");
    }

    /// <summary>
    /// Requests focus when focus is lost.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnFocusLost(object sender, RoutedEventArgs args) {
        Focus(FocusState.Programmatic);
    }
}