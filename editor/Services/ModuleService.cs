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
using System;
using System.Diagnostics;

namespace Argon.Editor.Services;

/// <summary>
/// A service that keeps track of all known modules in the data folder.
/// </summary>
internal class ModuleService {
	/// <summary>
	/// A set containing the names and paths of all known game modules.
	/// </summary>
	internal ISet<string> Modules { get; } = new HashSet<string>();

	private readonly ArgonFileSystem _files;
	private readonly ILogger _logger = LogHelper.Logger;
	private readonly ConfigurationService _configuration;
	private readonly AssetManager _assets;

	internal ModuleService(ConfigurationService configuration, ArgonFileSystem files, AssetManager assets) {
		_files = files;
		_configuration = configuration;
		_assets = assets;

		// load all known modules in the data folder
		LoadModules(configuration.DataFolder);
	}

	/// <summary>
	/// Creates a new module in the data folder.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="title"></param>
	/// <param name="subtitle"></param>
	/// <param name="description"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	internal ModuleAsset CreateModule(string id, string title, string subtitle, string description) {
		_logger.LogInformation("create module {id}", id);
		string path = Path.Combine(_configuration.DataFolder,id);

		// check if there is already a folder with the same name as the module id
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
			_files.SetSaveFolder(path);
			ModuleAsset module = new(id, title, subtitle, description);
			_assets.AddAsset(module);
			_files.FlushToSave();
			return module;
		} else {
			throw new ArgumentException("A folder with the name <{id}> already exists.", id);
		}
	}

	private void LoadModules(string path) {
		foreach (string folder in Directory.GetDirectories(path)) {
			string name = Path.GetFileName(folder);
			string file = Path.Combine(folder, $"{name}.xml");
			if (Directory.GetFiles(folder).Contains(file)) {
				FileInfo module = new(file);
				if (ModuleLoader.VerifyAsset(module)) {
					_logger.LogInformation("module folder found: {name}", name);
					_files.AddModule(folder);
					Modules.Add(name);
				}
			}
		}
	}
}
