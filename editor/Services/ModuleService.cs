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

using Argon.Common;
using Argon.Common.Assets;
using Argon.Common.Files;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Argon.Editor.Services;

/// <summary>
/// A service that keeps track of all known modules in the data folder.
/// </summary>
internal class ModuleService {
	/// <summary>
	/// A set containing the names and paths of all known game modules.
	/// </summary>
	internal ISet<string> Modules { get; private set; } = new HashSet<string>();

	private ArgonFileSystem _files;
	private ILogger _logger = LogHelper.Logger;

	internal ModuleService(ConfigurationService configuration, ArgonFileSystem files) {
		_files = files;

		// load all known modules in the data folder
		LoadModules(configuration.DataFolder);
	}

	private void LoadModules(string path) {
		foreach (string folder in Directory.GetDirectories(path)) {
			string name = Path.GetFileName(folder);
			if (Directory.GetFiles(folder).Contains(Path.Combine(folder, $"{name}.xml"))) {
				FileInfo module = new FileInfo(Path.Combine(folder, $"{name}.xml"));
				if (ModuleLoader.VerifyAsset(module)) {
					_logger.LogInformation($"module folder found: {name}");
					_files.AddModule(folder);
					Modules.Add(name);
				}
			}
		}
	}
}
