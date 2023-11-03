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

using System.Text.Json;
using System.Xml;
using System.Diagnostics;

namespace Argon.Common.Assets;

public class AssetManager {
	public AssetManager(string path) {
		using (var reader = new StreamReader(Path.Combine(path, "aneirin.xml"))) {
			var document = new XmlDocument();
			document.LoadXml(reader.ReadToEnd());
			string id = document.DocumentElement.Attributes["id"].Value;
			string title = document.DocumentElement.SelectSingleNode("title").InnerText;
			string subtitle = document.DocumentElement.SelectSingleNode("subtitle").InnerText;

			var module = new ModuleAsset { Id = id, Title = title, Subtitle = subtitle };

			Debug.WriteLine($"id: {module.Id}");
			Debug.WriteLine($"title: {module.Title}");
			Debug.WriteLine($"subtitle: {module.Subtitle}");
			Debug.WriteLine("");
		}

		var items = Directory.EnumerateFiles(Path.Combine(path, "items"));

		var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		foreach (var item in items) {
			using var reader = new StreamReader(item);
			using var document = JsonDocument.Parse(reader.ReadToEnd());
			JsonElement kind = document.RootElement.GetProperty("kind");
			Asset asset;

			switch (kind.GetString()) {
				case "weapon":
					asset = JsonSerializer.Deserialize<ItemAsset>(document, options);
					break;
				default:
					asset = JsonSerializer.Deserialize<Asset>(document, options);
					break;
			}

			Debug.WriteLine(asset);
			Debug.WriteLine("");
		}

		var creature = new CreatureAsset("sdf", "sfe", "jiodsoi", "image");
		Debug.WriteLine(creature);
		Debug.WriteLine("");
	}

	public Asset GetAsset(string kind, string id) {
		return new Asset("sdf", "wer");
	}
}
