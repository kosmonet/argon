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
using Argon.Client.Models;
using Argon.Client.Presentation;
using Microsoft.Extensions.Hosting;
using Windows.Graphics;

namespace Argon.Client;

/// <summary>
/// 
/// </summary>
internal partial class App : Application {
    /// <summary>
    /// Initializes the singleton application object. This is the first line of 
    /// authored code executed, and as such is the logical equivalent of main() 
    /// or WinMain().
    /// </summary>
    internal App() {
        InitializeComponent();
    }

    /// <summary>
    /// Makes the main window and sets the start page.
    /// </summary>
    /// <param name="args"></param>
    protected async override void OnLaunched(LaunchActivatedEventArgs args) {
        IApplicationBuilder builder = this.CreateBuilder(args)
        	.UseToolkitNavigation()
            .Configure(hostBuilder => hostBuilder.UseLocalization()
                // ModelMappings nodig om Models en Views automatisch te verbinden
                .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes));
        
        Window mainWindow = builder.Window;
        mainWindow.AppWindow.Resize(new SizeInt32 { Width = 1920, Height = 1080 });
        // mainWindow.SetWindowIcon();
        IHost host = await builder.NavigateAsync<Shell>();
        // await host.RunAsync();

        RunTcp();   // TODO: naar host.RunAsync() verplaatsen
    }

    /// <summary>
    /// Registers all the navigation paths between pages in the user interface.
    /// </summary>
    /// <param name="views"></param>
    /// <param name="routes"></param>
    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes) {
        views.Register(
            // Why is the Shell special?
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<StartPage, StartModel>(),
            new ViewMap<MainPage, MainModel>(),
            new ViewMap<NewPage, NewModel>(),
            new ViewMap<LoadPage, LoadModel>(),
            new ViewMap<OptionPage, OptionModel>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested:
                [
                    new ("Start", View: views.FindByView<StartPage>(), IsDefault:true),
                    new ("Main", View: views.FindByView<MainPage>()),
                    new ("New", View: views.FindByView<NewPage>()),
                    new ("Load", View: views.FindByView<LoadPage>()),
                    new ("Options", View: views.FindByView<OptionPage>())
                ]
            )
        );
    }

    private static async void RunTcp() {
        IPAddress ipAddress = new([127,0,0,1]);
        var ipEndPoint = new IPEndPoint(ipAddress, 11111);

        using TcpClient client = new();
        await client.ConnectAsync(ipEndPoint);
        await using NetworkStream stream = client.GetStream();

        // ReadBytes() komt uit Uno extensions
        var message = Encoding.UTF8.GetString(stream.ReadBytes());
        User? user = JsonSerializer.Deserialize<User>(message);
        Console.WriteLine($"Message received: {user}");
    }
    
    private record User {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
