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

using Uno.UI.Hosting;

namespace Argon.Client;

/// <summary>
/// The main class for the (Skia) desktop platform. No other platforms
/// are currently supported.
/// </summary>
internal class Program {
    /// <summary>
    /// Main method. Initializes the Uno Platform and starts the client.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    [STAThread]
    public static void Main(string[] args) {
        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
            .UseWin32()
            .Build();
        host.Run();
    }
}
