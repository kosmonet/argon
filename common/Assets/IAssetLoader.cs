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

namespace Argon.Common.Assets; 

/// <summary>
/// Interface for a generic asset loader. It is up to the concrete implementations to decide where 
/// to save and load the assets (JSON, XML, database, ...).
/// </summary>
public interface IAssetLoader<T> where T : Asset {
	/// <summary>
	/// The type of asset this loader can handle.
	/// </summary>
	internal Type AssetType { get => typeof(T); }

	/// <summary>
	/// Loads an asset.
	/// </summary>
	/// <param name="id">The id of the asset.</param>
	/// <returns>An asset with the given id.</returns>
	internal T LoadAsset(string id);

	/// <summary>
	/// Saves an asset.
	/// </summary>
	/// <param name="asset">The asset to be saved.</param>
	internal void SaveAsset(T asset);
}
