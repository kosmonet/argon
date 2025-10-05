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
using Argon.Server.Services;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argon.Server;

/// <summary>
/// The main class for the server. 
/// </summary>
internal class Server {
    private static readonly ILogger _logger = LogHelper.Logger;

    private readonly AppConfiguration _appConfig;
    private readonly GameConfiguration _gameConfig;
    private readonly ArgonFileSystem _files;
    private readonly AssetManager _assets;

    /// <summary>
    /// Main method of the Server.
    /// </summary>
    /// <param name="args">The command line parameters.</param>
    public static async Task Main(string[] args) {
        _logger.LogInformation("starting server");
        Server server = new();

        IHostBuilder builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => {
            services.AddHostedService<NetworkService>();
            services.AddHostedService<GameService>();

            // services.AddSingleton<AppConfiguration>();
        });

        IHost host = builder.Build();
        await host.RunAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    internal Server() {
        // load the config file
        _appConfig = new AppConfiguration();

        // initialize the file system
        _files = new ArgonFileSystem(_appConfig.TempFolder);

        // initialize the asset manager
        _assets = new AssetManager();
        _assets.RegisterLoader(new ModuleLoader(_files));

        // add all modules in the configuration to the file system
        _gameConfig = new GameConfiguration(_files, _assets, _appConfig);

        // initialize the scripting system
        InitializeScripting();
    }

    private static void InitializeScripting() {
        var codeToEval = @"return 42;";
        var result = CSharpScript.EvaluateAsync(codeToEval, ScriptOptions.Default);
        Console.WriteLine($"testing script finished: {result.Result}");
    }    
}
