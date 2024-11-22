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

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Argon.Editor.Views;
using Argon.Common;
using System;
using Microsoft.Extensions.Logging;

namespace Argon.Editor;

/// <summary>
/// The main class of the Argon editor.
/// </summary>
public partial class Editor : Application {
	private readonly ILogger _logger = LogHelper.Logger;
	private readonly Settings _settings = new();

	public Editor() {

	}

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		// Line below is needed to remove Avalonia data validation. Without this line
		// you will get duplicate validations from both Avalonia and CommunityToolkit.
		BindingPlugins.DataValidators.RemoveAt(0);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new ModuleWindow();
		}

		base.OnFrameworkInitializationCompleted();
	}

	/// <summary>
	/// Main method, initializes Avalonia.
	/// </summary>
	/// <param name="args">The command-line arguments.</param>
	[STAThread]
	public static void Main(string[] args) {
		var builder = AppBuilder.Configure<Editor>().UsePlatformDetect().WithInterFont().LogToTrace();
		builder.StartWithClassicDesktopLifetime(args);
	}
}