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
using Argon.Common.Assets;
using Argon.Common.Files;
using Microsoft.Extensions.Logging;

namespace Argon.Server;

internal class GameConfiguration {
    private static readonly ILogger _logger = LogHelper.Logger;

    internal string Title { get; } = "Argon";
    internal string Subtitle { get; } = "roguelike engine";

    internal GameConfiguration(ArgonFileSystem files, AssetManager assets, ServerConfiguration config) {
        // load all the modules in the order in which they appear in the config file
        foreach (string module in config.Modules) {
            LoadModule(module, files, config.DataFolder);
        }

        // load title an subtitle from the first module
        ModuleAsset parent = assets.GetAsset<ModuleAsset>(config.Modules[0]);
        Title = parent.Title;
        Subtitle = parent.Subtitle;
    }

    /// <summary>
    /// Load a module.
    /// </summary>
    /// <param name="module">The name of a module.</param>
    /// <exception cref="FileLoadException">When the given module isn't found.</exception>
    private void LoadModule(string module, ArgonFileSystem files, string dataFolder) {
        string folderPath = Path.Combine(dataFolder, module);
        string modulePath = Path.Combine(folderPath, $"{module}.xml");

        if (File.Exists(modulePath)) {
            _logger.LogInformation("loading module '{module}'", module);
            files.AddModule(folderPath);
        } else {
            throw new FileLoadException($"Could not find module: '{module}'.");
        }
    }
}
