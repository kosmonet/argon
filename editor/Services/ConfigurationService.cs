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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Argon.Editor.Services;

/// <summary>
/// A service to keep track of user settings.
/// </summary>
internal class ConfigurationService {
	/// <summary>
	/// A record with the actual configuration that can be saved to a JSON file.
	/// </summary>
	private record Configuration {
		[JsonInclude]
		[JsonPropertyName("modules")]
		public HashSet<string> _modules = [];

		[JsonInclude]
		[JsonPropertyName("data")]
		public string _dataFolder = Path.Combine(AppContext.BaseDirectory, "data");
	}

	/// <summary>
	/// A set containing the names and paths of all known game modules.
	/// </summary>
	internal ISet<string> Modules { get { return _configuration._modules; } }

	/// <summary>
	/// The path to the data folder containing all game modules.
	/// </summary>
	internal string DataFolder { get { return _configuration._dataFolder; } }

	private readonly ILogger _logger = LogHelper.Logger;
	private readonly Configuration _configuration;
	private readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Argon", "editor.conf");
	private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

	/// <summary>
	/// Initializes the settings.
	/// </summary>
	internal ConfigurationService() {
		// check if the settings file exists
		if (!File.Exists(filePath)) {
			// if it doesn't, create an empty settings file
			_logger.LogInformation("creating configuration");
			string folderPath = Guard.NotNullOrEmpty(Path.GetDirectoryName(filePath), "Application folder not available.");
			Directory.CreateDirectory(folderPath);

			// load a default configuration
			_configuration = new Configuration();

			// and write the configuration to the settings file
			Save();
		} else {
			// if it does, load configuration from the settings file
			_logger.LogInformation("loading configuration");
			using FileStream fileStream = File.OpenRead(filePath);
			_configuration = Guard.NotNull(JsonSerializer.Deserialize<Configuration>(fileStream), "Application folder not available.");
		}

		_logger.LogInformation("set data folder to {folder}", _configuration._dataFolder);

	}

	/// <summary>
	/// Saves the configuration to the settings file.
	/// </summary>
	internal void Save() {
		using FileStream fileStream = File.Create(filePath);
		JsonSerializer.Serialize(fileStream, _configuration, _options);
	}
}
