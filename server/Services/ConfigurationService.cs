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

using System.Text.Json;
using System.Text.Json.Serialization;
using Argon.Common;
using Microsoft.Extensions.Logging;

namespace Argon.Server.Services;

/// <summary>
/// A service to handle all server configuration.
/// </summary>
internal class ConfigurationService {
    /// <summary>
    /// Record to keep track of all server configuration.
    /// </summary>
    private record Configuration {
        /// <summary>
        /// The location of the game data folder. The default value is the data 
        /// folder in the same location as the server executable.
        /// </summary>
		[JsonInclude][JsonPropertyName("data")]
		internal string DataFolder { get; init; } = Path.Combine(AppContext.BaseDirectory, "data");

        /// <summary>
        /// The list of modules to load.
        /// </summary>
        [JsonInclude][JsonPropertyName("modules")]
		internal string[] Modules { get; init; } = [""];
    }
    
    private static readonly ILogger _logger = LogHelper.Logger;

	private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly Configuration _configuration;

    internal ConfigurationService() {
        // get the path to the config folder
        string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string folderPath = Path.Combine(configPath, "argon");

        // check if the config folder exists
        if(!Directory.Exists(folderPath)) {
            // create the folder if it doesn't
            _logger.LogInformation("creating config folder: {folder}", folderPath);
            Directory.CreateDirectory(folderPath);            
        }

        // get the path to the config file
        string filePath = Path.Combine(folderPath, "server.config");

        // check if the config file exists
        if(!File.Exists(filePath)) {
            // create the file with a default configuration if it doesn't exist
            _logger.LogInformation("creating config file: {file}", filePath);
            _configuration = new Configuration();
            Save(filePath);
        } else {
            // read the configuration if it does exist
    		using FileStream fileStream = File.OpenRead(filePath);
	    	_configuration = Guard.NotNull(JsonSerializer.Deserialize<Configuration>(fileStream), "Application data folder not available.");
            _logger.LogInformation("setting data folder: {folder}", _configuration.DataFolder);
        }

        // load all the modules in the order in which they appear in the config file
        foreach (string module in _configuration.Modules) {
            LoadModule(module);
        }
    }

    /// <summary>
    /// Load a module.
    /// </summary>
    /// <param name="module">The name of a module.</param>
    /// <exception cref="FileLoadException">When the given module isn't found.</exception>
    private void LoadModule(string module) {
        string modulePath = Path.Combine(_configuration.DataFolder, module, $"{module}.xml");
        if (File.Exists(modulePath)) {
            _logger.LogInformation("loading module: {module}", module);
        } else {
            throw new FileLoadException($"Could not find module: '{module}'.");
        }
    }

    /// <summary>
    /// Save the configuration to the given path.
    /// </summary>
    /// <param name="filePath"></param>
	private void Save(string filePath) {
		using FileStream fileStream = File.Create(filePath);
		JsonSerializer.Serialize(fileStream, _configuration, _options);
	}
}
