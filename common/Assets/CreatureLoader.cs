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

public class CreatureLoader : IAssetLoader<CreatureAsset> {
	private readonly ArgonFileSystem _files;
	private readonly string _kind = "creatures";

	public CreatureLoader(ArgonFileSystem fileSystem) {
		_files = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	}

	/// <summary>
	/// Loads a CreatureAsset from disk. If it is not found, an
	/// ArgumentNullException will be thrown.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public CreatureAsset LoadAsset(string id) {
		FileInfo file = _files.LoadFile(_kind, $"{id}.json");
		using var reader = new StreamReader(file.FullName);
		CreatureAsset? creature = JsonSerializer.Deserialize<CreatureAsset>(reader.ReadToEnd());
		return Guard.NotNull(creature, $"The creature with id <{id}> was not found.");
	}

	public void SaveAsset(CreatureAsset creature) {
		FileInfo file = _files.SaveFile(_kind, $"{creature.Id}.json");
		File.WriteAllText(file.FullName, JsonSerializer.Serialize(creature));
	}
}
