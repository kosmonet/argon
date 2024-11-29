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

namespace Argon.Common.Assets;

/// <summary>
/// 
/// </summary>
public class AssetManager {
	private readonly Dictionary<Type, IAssetLoader> _loaders = [];

	public void RegisterLoader(IAssetLoader loader) {
		if(loader is not null) {
			_loaders.Add(loader.AssetType, loader);
		}
	}

	public T GetAsset<T>(string id) where T : Asset {
		T asset = (T)_loaders[typeof(T)].LoadAsset(id);
		return asset;
	}

	public void AddAsset<T>(T asset) where T : Asset {
		_loaders[typeof(T)].SaveAsset(asset);
	}
}
