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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Argon.Common;
using Microsoft.Extensions.Logging;

namespace Argon.Editor;

/// <summary>
/// Keeps track of user settings.
/// </summary>
internal class Settings {
	/// <summary>
	/// A record with the actual configuration that can be saved to a JSON file.
	/// </summary>
	private record Configuration {
		[JsonInclude]
		[JsonPropertyName("modules")]
		public Dictionary<string, string> _modules = new();
	}

	private readonly ILogger _logger = LogHelper.Logger;
	private readonly Configuration _configuration;
	private readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Argon", "editor.conf");

	/// <summary>
	/// Initializes the settings.
	/// </summary>
	internal Settings() {
		// check if the settings file exists
		if (!File.Exists(filePath)) {
			// if it doesn't, create an empty settings file
			_logger.LogInformation("Creating configuration.");
			string folderPath = Guard.NotNullOrEmpty(Path.GetDirectoryName(filePath), "Application folder not available.");
			Directory.CreateDirectory(folderPath);

			// load a default configuration
			_configuration = new Configuration();

			// and write the configuration to the settings file
			Save();
		} else {
			// if it does, load configuration from the settings file
			_logger.LogInformation("Loading configuration.");
			using FileStream fileStream = File.OpenRead(filePath);
			_configuration = Guard.NotNull(JsonSerializer.Deserialize<Configuration>(fileStream), "Application folder not available.");
		}
	}

	/// <summary>
	/// Saves the configuration to the settings file.
	/// </summary>
	internal void Save() {
		JsonSerializerOptions options = new() { WriteIndented = true };
		using FileStream fileStream = File.Create(filePath);
		JsonSerializer.Serialize(fileStream, _configuration, options);
	}
}
