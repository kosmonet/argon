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

using Argon.Common.Files;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Argon.Common.Assets;

public class CreatureLoader : IAssetLoader {
	private readonly ArgonFileSystem _files;
	private readonly string _kind = "creatures";
	private readonly ConcurrentDictionary<string, CreatureAsset> _assets = new();

	public Type AssetType { get; } = typeof(CreatureAsset);

	public CreatureLoader(ArgonFileSystem fileSystem) {
		_files = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	}

	/// <summary>
	/// Loads a CreatureAsset. If the asset is not present in the cache, an 
	/// attempt will be made to load it from disk. If it is not found, an
	/// ArgumentNullException will be thrown.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Asset LoadAsset(string id) {
		if (!_assets.ContainsKey(id)) {
			FileInfo file = _files.LoadFile(_kind, $"{id}.json");
			using var reader = new StreamReader(file.FullName);
			CreatureAsset? asset = JsonSerializer.Deserialize<CreatureAsset>(reader.ReadToEnd());
			_assets[id] = Guard.NonNull(asset, $"No asset with id <{id}> found.");
		}

		return _assets[id];
	}

	public void SaveAsset(Asset asset) {
		CreatureAsset creature = (CreatureAsset)asset;
		_assets[creature.Id] = creature;

		FileInfo file = _files.SaveFile(_kind, $"{creature.Id}.json");
		File.WriteAllText(file.FullName, JsonSerializer.Serialize(creature));
	}
}
