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
using Microsoft.Extensions.Hosting;

namespace Argon.Server.Services;

internal class NetworkService : IHostedService {
    private readonly IHostApplicationLifetime _appLifetime;

    public NetworkService(IHostApplicationLifetime appLifetime) {
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _appLifetime.ApplicationStarted.Register(() => {
            Task.Run(async () => {
                try {
                    Console.Out.WriteLine("starting TCP");
                    await RunTcp();
                } catch (Exception ex) {
                    Console.Out.WriteLine("oeps");
                } finally {
                    // Stop the application once the work is done
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    internal static async Task RunTcp() {
        User user = new User{Name = "Jef", Username = "JJ", Email = "Jef@JJ.com"};
        IPAddress ipAddress = new([127,0,0,1]);
        var ipEndPoint = new IPEndPoint(ipAddress, 11111);
        TcpListener listener = new(ipEndPoint);

        try {    
            listener.Start();

            using TcpClient handler = await listener.AcceptTcpClientAsync();
            await using NetworkStream stream = handler.GetStream();

            string message = JsonSerializer.Serialize(user);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            await stream.WriteAsync(messageBytes);
            Console.WriteLine($"Sent message: {message}");
        } finally {
            listener.Stop();
        }
    }

    private record User {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
