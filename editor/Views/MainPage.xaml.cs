/*
 *	Argon, a roguelike engine.
 *	Copyright (C) 2023 - Maarten Driesen
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

using System.Collections.ObjectModel;

namespace Argon.Editor.Views; 
public partial class MainPage : ContentPage {
	readonly ObservableCollection<string> maps = new();
	readonly ObservableCollection<string> assets = new();

	public MainPage() {
		InitializeComponent();

		maps.Add("Zolder");
		maps.Add("Heusden");
		maps.Add("Bolderberg");
		maps.Add("Boekt");
		maps.Add("Eversel");
		maps.Add("Viversel");
		maps.Add("Berkenbos");
		maps.Add("Voort");

		mapList.ItemsSource = maps;

		assets.Add("brood");
		assets.Add("kaas");
		assets.Add("eieren");
		assets.Add("boter");
		assets.Add("chocolade");
		assets.Add("appel");
		assets.Add("peer");
		assets.Add("salami");
		assets.Add("taart");
		assets.Add("pasta");
		assets.Add("pizza");
		assets.Add("bloem");
		assets.Add("suiker");
		assets.Add("zout");

		assetList.ItemsSource = assets;
	}
}