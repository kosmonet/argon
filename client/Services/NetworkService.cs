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
using Argon.Common.Assets;
using Microsoft.Extensions.Hosting;

namespace Argon.Client.Services;

public class NetworkService : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken token) {
        Console.Out.WriteLine("starting TCP");
        IPAddress ipAddress = new([127,0,0,1]);
        IPEndPoint ipEndPoint = new(ipAddress, 58008);

        using TcpClient client = new();
        await client.ConnectAsync(ipEndPoint);
        using NetworkStream stream = client.GetStream();

        // ReadBytes() komt uit Uno extensions
        string message = Encoding.UTF8.GetString(stream.ReadBytes());
        CreatureAsset? creature = JsonSerializer.Deserialize<CreatureAsset>(message);
        Console.WriteLine($"Message received: {creature}");    
    }
}
