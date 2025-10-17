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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argon.Server.Services;

/// <summary>
/// 
/// </summary>
internal class GameService : BackgroundService {
    private static readonly ILogger _logger = LogHelper.Logger;

    public GameService(Server server) {
        
    }

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("GameService started");

        // vaste of variabele framerate?
        while (!token.IsCancellationRequested) {
            // TODO: PeriodicTimer - slaat ticks over als de loop niet op tijd was
            Console.Out.WriteLine("server gameloop tick");
            await Task.Delay(TimeSpan.FromMilliseconds(1000), token);
        }
    }
}
