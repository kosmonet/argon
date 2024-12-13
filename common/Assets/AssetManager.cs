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

using System.Runtime.Caching;

namespace Argon.Common.Assets;

/// <summary>
/// Manages game assets. Before adding or requesting an asset, the correct loader for that asset
/// should be registered. Assets are cached after they have been loaded. This class is quite dirty 
/// because it does various evil things with generics and casting.
/// </summary>
public class AssetManager {
	private readonly Dictionary<Type, Object> _loaders = [];
	private readonly MemoryCache _cache = MemoryCache.Default;
	private readonly CacheItemPolicy _policy = new();

	/// <summary>
	/// Registers a loader for a certain type of asset.
	/// </summary>
	/// <typeparam name="T">The type of asset the loader can handle.</typeparam>
	/// <param name="loader">The asset loader for the given type.</param>
	public void RegisterLoader<T>(IAssetLoader<T> loader) where T : Asset {
			_loaders.Add(loader.AssetType, loader);
	}

	/// <summary>
	/// Returns an asset of the requested type with the given id.
	/// </summary>
	/// <typeparam name="T">The type of asset to return.</typeparam>
	/// <param name="id">The id of the asset.</param>
	/// <returns>The asset of the given type and id.</returns>
	public T GetAsset<T>(string id) where T : Asset {
		if (_cache.Contains($"{typeof(T).Name}:{id}")) {
			return (T)_cache.Get($"{typeof(T).Name}:{id}");
		} else {
			IAssetLoader<T> loader = (IAssetLoader<T>)_loaders[typeof(T)];
			T asset = loader.LoadAsset(id);
			_cache.Set($"{typeof(T).Name}:{asset.Id}", asset, _policy);
			return asset;
		}
	}

	/// <summary>
	/// Adds an asset and saves it to the temp folder.
	/// </summary>
	/// <typeparam name="T">The type of the asset.</typeparam>
	/// <param name="asset">The asset to be added.</param>
	public void AddAsset<T>(T asset) where T : Asset {
		_cache.Set($"{typeof(T).Name}:{asset.Id}", asset, _policy);
		IAssetLoader<T> loader = (IAssetLoader<T>)_loaders[typeof(T)];
		loader.SaveAsset(asset);
	}

	/// <summary>
	/// Clears all entries from the internal cache.
	/// </summary>
	public void Clear() {
		_cache.Dispose();
	}
}
