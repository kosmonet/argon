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
using Argon.Common;
using Argon.Server.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argon.Server.Services;

internal class NetworkService : BackgroundService {
    private static readonly ILogger _logger = LogHelper.Logger;

    protected override async Task ExecuteAsync(CancellationToken token) {
        IPAddress ipAddress = new([127,0,0,1]);
        IPEndPoint ipEndPoint = new(ipAddress, 58008);
        TcpListener listener = new(ipEndPoint);
        listener.Start();
        
        while (!token.IsCancellationRequested) {
            TcpClient client = await listener.AcceptTcpClientAsync(token);
            _logger.LogInformation("new client connected");
            ClientHandler handler = new(client);
            handler.Run(token);
        }
    }
}
