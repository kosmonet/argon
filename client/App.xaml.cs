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

using Argon.Client.Models;
using Argon.Client.Presentation;
using Argon.Client.Services;
using Microsoft.Extensions.DependencyInjection;
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
    /// Sets up the internal plumbing of the client.
    /// </summary>
    /// <param name="args"></param>
    protected async override void OnLaunched(LaunchActivatedEventArgs args) {
        // it seems we have to use the IHostBuilder provided by Uno to make use of all the nice features
        IApplicationBuilder builder = this.CreateBuilder(args);
        builder.UseToolkitNavigation().Configure(ConfigureHostBuilder);

        // there's a version of builder.Configure that takes a Window as argument, but we'll take this one
        Window mainWindow = builder.Window;
        mainWindow.AppWindow.Resize(new SizeInt32 { Width = 1920, Height = 1080 });

        // and build the IHost
        IHost host = await builder.NavigateAsync<Shell>();
        // apparently, we don't need to host.RunAsync(), the builder took care of that?
    }

    /// <summary>
    /// Configures the IHostBuilder used by the client.
    /// </summary>
    /// <param name="builder"></param>
    private void ConfigureHostBuilder(IHostBuilder builder) {
        builder.UseLocalization()
            // ModelMappings needed to automagically connect Models and Views
            .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
            // add services
            .ConfigureServices(services => {
                services.AddHostedService<NetworkService>();
                services.AddHostedService<GameService>();
            }
        );
    }

    /// <summary>
    /// Registers all the navigation paths between pages in the user interface.
    /// </summary>
    /// <param name="views"></param>
    /// <param name="routes"></param>
    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes) {
        views.Register(
            // why is the Shell special?
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
}
