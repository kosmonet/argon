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
using Argon.Common.Net;
using Microsoft.Extensions.Logging;

namespace Argon.Server;

/// <summary>
/// A service to handle all server configuration.
/// </summary>
internal class ServerConfiguration {
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

        /// <summary>
        /// The location of the temp folder. The default value is the temp 
        /// folder defined by the operating system.
        /// </summary>
        [JsonInclude][JsonPropertyName("temp")]
        internal string TempFolder { get; init; } = Path.Combine(Path.GetTempPath(), "argon");

        /// <summary>
        /// The IP address of the server.
        /// </summary>
        [JsonInclude][JsonPropertyName("address")]
        internal string IpAddress {get; init; } = "127.0.0.1";

        /// <summary>
        /// The IP port to listen to for clients.
        /// TODO: make this an enum
        /// </summary>
        [JsonInclude][JsonPropertyName("port")]
        internal string IpPort {get; init; } = "58008";

        /// <summary>
        /// The game mode: host or solo.
        /// </summary>
        [JsonInclude][JsonPropertyName("mode")]
        internal string Mode {get; init; } = "host";
    }
    
    private static readonly ILogger _logger = LogHelper.Logger;

    internal string TempFolder { get => _configuration.TempFolder; }
    internal string DataFolder { get => _configuration.DataFolder; }
    internal string[] Modules { get => _configuration.Modules; }

	private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly Configuration _configuration;

    internal ServerConfiguration() {
        // to convert a json string to an IP address
        _options.Converters.Add(new IPAddressConverter());

        // get the path to the config folder
        string folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "argon");

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
            _configuration = new();
            Save(filePath);
        } else {
            // read the configuration if it does exist
    		using FileStream fileStream = File.OpenRead(filePath);
	    	_configuration = Guard.NotNull(JsonSerializer.Deserialize<Configuration>(fileStream), 
                "Application data folder not available.");
            _logger.LogInformation("set data folder to {folder}", _configuration.DataFolder);
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
