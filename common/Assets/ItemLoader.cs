/*
 *	Argon, a roguelike engine.
 *	Copyright (C) 2023-2024 - Maarten Driesen
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

using Argon.Common.Files;
using System.Text.Json;

namespace Argon.Common.Assets;

public class ItemLoader : IAssetLoader<ItemAsset> {
	private readonly ArgonFileSystem _files;
	private readonly string _kind = "items";

	public ItemLoader(ArgonFileSystem fileSystem) {
		_files = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	}

	public ItemAsset LoadAsset(string id) {
		FileInfo file = _files.LoadFile(_kind, $"{id}.json");
		using var reader = new StreamReader(file.FullName);
		using var document = JsonDocument.Parse(reader.ReadToEnd());
		JsonElement kind = document.RootElement.GetProperty("kind");

		ItemAsset? item = kind.GetString() switch {
			"weapon" => JsonSerializer.Deserialize<ItemAsset.Weapon>(document),
			"armor" => JsonSerializer.Deserialize<ItemAsset.Armor>(document),
			"clothing" => JsonSerializer.Deserialize<ItemAsset.Clothing>(document),
			"book" => JsonSerializer.Deserialize<ItemAsset.Book>(document),
			_ => JsonSerializer.Deserialize<ItemAsset>(document),
		};

		return Guard.NotNull(item, $"The item with id <{id}> was not found.");
	}

	public void SaveAsset(ItemAsset item) {
		FileInfo file = _files.SaveFile(_kind, $"{item.Id}.json");
		File.WriteAllText(file.FullName, JsonSerializer.Serialize(item));
	}
}
