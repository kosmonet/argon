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

using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;

namespace Argon.Client.Graphics;

internal class ImageLoader {
    internal byte[,] heightMap = new byte[256, 256];
    internal byte[,] terrainMap = new byte[256, 256];

    internal ImageLoader () {
        // load the height map from an 8-bit grayscale bitmap image
        byte[] heightBuffer = File.ReadAllBytes("Assets/Images/height.bmp");
        using(Stream file = new MemoryStream(heightBuffer)) {
            SKBitmap bitmap = SKBitmap.Decode(file);
            for(int x = 0; x < bitmap.Width; x++) {
                for(int y = 0; y < bitmap.Height; y++) {
                    heightMap[x,y] = bitmap.GetPixel(x, y).Red;
                }
            }
        }

        // load the terrain map from an 8-bit grayscale bitmap image
        byte[] terrainBuffer = File.ReadAllBytes("Assets/Images/diffuse.bmp");
        using(Stream file = new MemoryStream(terrainBuffer)) {
            SKBitmap bitmap = SKBitmap.Decode(file);
            for(int x = 0; x < bitmap.Width; x++) {
                for(int y = 0; y < bitmap.Height; y++) {
                    terrainMap[x,y] = bitmap.GetPixel(x, y).Red;
                }
            }
        }
    }
}
