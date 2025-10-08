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

public sealed partial class GamePage : Page {
    /// <summary>
    /// Initializes the component.
    /// </summary>
    public GamePage() {
        InitializeComponent();
    }

    /// <summary>
    /// Handles key presses.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnKeyDown(object sender, KeyRoutedEventArgs args) {
        INavigator navigator = Guard.NotNull(this.Navigator(), "INavigator not available.");

        switch (args.Key) {
            case VirtualKey.I: navigator.NavigateRouteAsync(this, "Inventory"); break;
            case VirtualKey.D: navigator.NavigateRouteAsync(this, "Dialog"); break;
            case VirtualKey.M: navigator.NavigateRouteAsync(this, "Map"); break;
            case VirtualKey.T: navigator.NavigateRouteAsync(this, "Trade"); break;
            case VirtualKey.Escape: navigator.NavigateBackAsync(this); break;
        }
    }

    /// <summary>
    /// Adds a focus listener to this component when navigating to this page.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnNavigatedTo(NavigationEventArgs args) {
        // you're not really supposed to do this according to the documentation
        LostFocus += OnFocusLost;
    }

    /// <summary>
    /// Removes the focus listener on this component when navigating to another
    /// page.
    /// </summary>
    /// <param name="args"></param>
    protected override void OnNavigatedFrom(NavigationEventArgs args) {
        // you're not really supposed to do this according to the documentation
        LostFocus -= OnFocusLost;
    }

    /// <summary>
    /// Requests focus when focus is lost. Otherwise, key presses would go lost
    /// when clicking the mouse outside the game screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnFocusLost(object sender, RoutedEventArgs args) {
        Focus(FocusState.Programmatic);
    }
}