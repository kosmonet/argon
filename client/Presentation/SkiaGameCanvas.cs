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

using Argon.Client.Graphics;
using SkiaSharp;
using Uno.WinUI.Graphics2DSK;
using Windows.Foundation;

namespace Argon.Client.Presentation;

internal class SkiaGameCanvas : SKCanvasElement {
    private SKPaint paint1 = new SKPaint { Color = SKColors.DarkGreen };
    private SKPaint paint2 = new SKPaint { Color = SKColors.Green };
    private SKPaint paint3 = new SKPaint { Color = SKColors.DarkOliveGreen };
    private SKPaint paint4 = new SKPaint { Color = SKColors.ForestGreen };
    private SKPaint paint5 = new SKPaint { Color = SKColors.SeaGreen };
    private SKPaint paint6 = new SKPaint { Color = SKColors.Olive };
    private SKPaint paint7 = new SKPaint { Color = SKColors.OliveDrab };
    private SKPaint paint8 = new SKPaint { Color = SKColors.DarkSeaGreen };
    private SKPaint white = new SKPaint { Color = SKColors.White };
    private SKPaint black = new SKPaint { Color = SKColors.Black };
    private SKPaint blurPaint = new SKPaint { ImageFilter =  SKImageFilter.CreateBlur(8, 8) };
    private ImageLoader loader = new();
    private int tileSize = 16;

    protected override void RenderOverride(SKCanvas canvas, Size area) {
        int width = (int)area.Width/tileSize;
        int height = (int)area.Height/tileSize;

        byte[,] terrainMap = loader.terrainMap;
        byte[,] heightMap = loader.heightMap;

        // hoogte van het centrum bepalen
        int center = heightMap[width/2, height/2];

        // eerste pass, geblurde bitmap tekenen (oil painting filter zou misschien nog properder zijn)
        canvas.DrawBitmap(DrawTerrainColor(heightMap, terrainMap, width, height, center), 0, 0, blurPaint);

        // tweede pass, hoogtelijntjes tekenen
        DrawLines(heightMap, canvas, width, height, center);

        // derde pass, begroeiing tekenen
        DrawText(terrainMap, heightMap, canvas, width, height, center);
    }
    
     private void DrawText(byte[,] terrainMap, byte[,] heightMap, SKCanvas canvas, int width, int height, int center) {
        SKTextAlign align = SKTextAlign.Center;
        SKFont font = new(SKTypeface.Default, 9*tileSize/16, 1, 0);

        for (int x = 1; x < width; x++) {
            for (int y = 1; y < height; y++) {
                int diff = Math.Abs(center - heightMap[x, y]);

                if (diff < 11) {
                    if (terrainMap[x, y] % 4 == 0) {
                        paint1.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText(",", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint1);
                    } else if (terrainMap[x, y] % 4 == 1) {
                        paint3.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText(".", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint3);
                    } else if (terrainMap[x, y] % 4 == 2) {
                        paint5.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText("ψ", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint5);
                    } else {
                        paint7.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText("ϡ", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint7);
                    }
                }
            }
        }

        paint1.ImageFilter = null;
        paint2.ImageFilter = null;
        paint3.ImageFilter = null;
        paint4.ImageFilter = null;
    }

    private void DrawLines(byte[,] heightMap, SKCanvas canvas, int width, int height, int center) {
        SKPaint blurredBlack = new SKPaint { 
            ImageFilter =  SKImageFilter.CreateBlur(4, 4),
            Color = SKColors.Black
        };

        for (int x = 1; x < width; x++) {
            for (int y = 1; y < height; y++) {
                int diff = center - heightMap[x, y];
                if (Math.Abs(diff) < 11) {
                    // kijken of linkse tile lager ligt
                    if (heightMap[x, y] != heightMap[x, y]) {
                        canvas.DrawLine(x*tileSize, y*tileSize, x*tileSize, y*tileSize+tileSize, blurredBlack);
                    }

                    // kijken of noordelijke tile lager ligt
                    if (heightMap[x, y] != heightMap[x, y]) {
                        canvas.DrawLine(x*tileSize, y*tileSize, x*tileSize+tileSize, y*tileSize, blurredBlack);
                    }
                }
            }
        }
    }

    private SKBitmap DrawTerrainColor(byte[,] heightMap, byte[,] terrainMap, int width, int height, int center) {
        SKBitmap terrain = new SKBitmap(width*tileSize, height*tileSize);
        SKCanvas canvas = new SKCanvas(terrain);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // verschil t.o.v. center bepalen
                int diff = center - heightMap[x, y];
                SKPaint paint;

                // inkleuren met juiste terreintype
                if (diff < -10) {
                    paint = white;
                } else if (diff > 10) {
                    paint = black;
                } else if (terrainMap[x, y] < 32) {
                    paint = paint1;
                } else if (terrainMap[x, y] < 64) {
                    paint = paint2;
                } else if (terrainMap[x, y] < 96) {
                    paint = paint3;
                } else if (terrainMap[x, y] < 128) {
                    paint = paint4;
                } else if (terrainMap[x, y] < 160) {
                    paint = paint5;
                } else if (terrainMap[x, y] < 192) {
                    paint = paint6;
                } else if (terrainMap[x, y] < 224) {
                    paint = paint7;
                } else {
                    paint = paint8;
                }

                // diepte aangeven met lichter/donkerder
                if (diff > 0) {
                    byte red = (byte)(paint.Color.Red - diff*paint.Color.Red/11);
                    byte green = (byte)(paint.Color.Green - diff*paint.Color.Green/11);
                    byte blue = (byte)(paint.Color.Blue - diff*paint.Color.Blue/11);
                    paint = new SKPaint { Color = new SKColor(red, green, blue) };
                } else if (diff < 0) {
                    byte red = (byte)(paint.Color.Red - diff*(255-paint.Color.Red)/11);
                    byte green = (byte)(paint.Color.Green - diff*(255-paint.Color.Green)/11);
                    byte blue = (byte)(paint.Color.Blue - diff*(255-paint.Color.Blue)/11);
                    paint = new SKPaint { Color = new SKColor(red, green, blue) };
                }

                if (diff == 0) {
                    SKColor color = new SKColor(255, 0, 0);
                    paint = new SKPaint { Color = color };
                }

                // en blokje tekenen op de canvas
                canvas.DrawRect(x*tileSize, y*tileSize, tileSize, tileSize, paint);
            }
        }

        return terrain;
    }
}
