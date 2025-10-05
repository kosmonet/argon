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

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Argon.Common;
using Argon.Common.Assets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argon.Client.Services;

public class NetworkService : BackgroundService {
    ILogger _logger = LogHelper.Logger;

    protected override async Task ExecuteAsync(CancellationToken token) {
        _logger.LogInformation("connecting to server");
        IPAddress ipAddress = new([127,0,0,1]);
        IPEndPoint ipEndPoint = new(ipAddress, 58008);

        using TcpClient client = new();
        try {
            await client.ConnectAsync(ipEndPoint);
            _logger.LogInformation("connected to server");
            using NetworkStream stream = client.GetStream();
            StreamReader reader = new(stream, Encoding.UTF8);

            while (!token.IsCancellationRequested) {
                string message = Guard.NotNull(await reader.ReadLineAsync(token),
                    "The received message was empty.");
                CreatureAsset? creature = JsonSerializer.Deserialize<CreatureAsset>(message);
                Console.WriteLine($"Message received: {creature}");
            }
        } catch (SocketException) {
            _logger.LogInformation("connection refused");
        }
    }
}
