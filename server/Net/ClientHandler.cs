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

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Argon.Common;
using Argon.Common.Assets;
using Microsoft.Extensions.Logging;

namespace Argon.Server.Net;

public class ClientHandler {
    private ILogger _logger = LogHelper.Logger;

    private TcpClient _client;

    internal ClientHandler(TcpClient client) {
            _client = client;
    }

    internal async void Run(CancellationToken token) {
        CreatureAsset creature = new("cat", "cat", "cat.jpg");
        string message = JsonSerializer.Serialize(creature);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using NetworkStream stream = _client.GetStream();
        StreamWriter writer = new(stream, Encoding.UTF8) { AutoFlush = true };
    
        try {
            while (!token.IsCancellationRequested) {
                    await writer.WriteLineAsync(message);
                    _logger.LogInformation("sent message: '{message}'", message);
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
        } catch (IOException) {
            _logger.LogInformation("client has disconnected");                
        }
    }
}
