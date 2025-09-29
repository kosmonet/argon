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
using Microsoft.UI.Xaml.Input;
using SkiaSharp;
using Uno.WinUI.Graphics2DSK;
using Windows.Foundation;
using Windows.System;

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
    private int tileSize = 32;
    private int playerX = 220;
    private int playerY = 230;

    internal SkiaGameCanvas() {
        // this is necessary to allow key presses to bubble up to the main page
        IsTabStop = true;

        // but we handle them here for now
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs args) {
        switch (args.Key) {
            case VirtualKey.Up: 
                playerY = Math.Max(0, playerY - 1);
                break;
            case VirtualKey.Down: 
                playerY = Math.Min(255, playerY + 1);
                break;
            case VirtualKey.Left: 
                playerX = Math.Max(0, playerX - 1);
                break;
            case VirtualKey.Right: 
                playerX = Math.Min(255, playerX + 1);
                break;
        }

        // render forceren
        Invalidate();
    }

    protected override void RenderOverride(SKCanvas canvas, Size area) {
        // var watch = System.Diagnostics.Stopwatch.StartNew();

        // width and height of the view in tiles
        int width = (int)area.Width/tileSize;
        int height = (int)area.Height/tileSize;

        byte[,] terrainMap = loader.terrainMap;
        byte[,] heightMap = loader.heightMap;

        // hoogte van player bepalen
        int center = heightMap[playerX, playerY];

        // positie van hoek van scherm bepalen
        int cornerX = Math.Min(Math.Max(0, playerX - width / 2), heightMap.GetLength(1) - width);
        int cornerY = Math.Min(Math.Max(0, playerY - height / 2), heightMap.GetLength(0) - height);
        Rectangle view = new Rectangle(cornerX, cornerY, width, height);

        // eerste pass, geblurde bitmap tekenen (oil painting filter zou misschien beter zijn)
        SKBitmap terrain = DrawTerrainColor(heightMap, terrainMap, view, center);
        canvas.DrawBitmap(terrain, 0, 0, blurPaint);

        // tweede pass, hoogtelijntjes tekenen
        DrawLines(heightMap, canvas, view, center);

        // derde pass, begroeiing tekenen
        DrawText(terrainMap, heightMap, canvas, view, center);

        //vierde pass, player tekenen
        DrawPlayer(canvas, view);

        // watch.Stop();
        // Console.Out.WriteLine("duration: " + watch.ElapsedMilliseconds);
    }
    
    private void DrawPlayer(SKCanvas canvas, Rectangle view) {
        SKTextAlign align = SKTextAlign.Center;
        SKFont font = new(SKTypeface.Default, 9*tileSize/16, 1, 0);
        int x = Math.Max(Math.Min(playerX, view.width/2), playerX - view.x);
        int y = Math.Max(Math.Min(playerY, view.height/2), playerY - view.y);
        canvas.DrawText("@", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, white);
    }

    private void DrawText(byte[,] terrainMap, byte[,] heightMap, SKCanvas canvas, Rectangle view, int center) {
        SKTextAlign align = SKTextAlign.Center;
        SKFont font = new(SKTypeface.Default, 9*tileSize/16, 1, 0);

        for (int x = 0; x < view.width; x++) {
            for (int y = 0; y < view.height; y++) {
                int diff = Math.Abs(center - heightMap[x + view.x, y + view.y]);

                if (diff < 11) {
                    if (terrainMap[x + view.x, y + view.y] % 4 == 0) {
                        paint1.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText(",", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint1);
                    } else if (terrainMap[x + view.x, y + view.y] % 4 == 1) {
                        paint3.ImageFilter = SKImageFilter.CreateBlur(diff/3, diff/3);
                        canvas.DrawText(".", x*tileSize + tileSize/2, y*tileSize + tileSize/2, align, font, paint3);
                    } else if (terrainMap[x + view.x, y + view.y] % 4 == 2) {
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

    private void DrawLines(byte[,] heightMap, SKCanvas canvas, Rectangle view, int center) {
        SKPaint blurredBlack = new SKPaint { 
            ImageFilter =  SKImageFilter.CreateBlur(4, 4),
            Color = SKColors.Black
        };

        for (int x = 1; x < view.width; x++) {
            for (int y = 1; y < view.height; y++) {
                int diff = center - heightMap[x + view.x, y + view.y];
                if (Math.Abs(diff) < 11) {
                    // kijken of linkse tile lager ligt
                    if (heightMap[x + view.x, y + view.y] != heightMap[x + view.x - 1, y + view.y]) {
                        canvas.DrawLine(x*tileSize, y*tileSize, x*tileSize, y*tileSize+tileSize, blurredBlack);
                    }

                    // kijken of noordelijke tile lager ligt
                    if (heightMap[x + view.x, y + view.y] != heightMap[x + view.x, y + view.y - 1]) {
                        canvas.DrawLine(x*tileSize, y*tileSize, x*tileSize+tileSize, y*tileSize, blurredBlack);
                    }
                }
            }
        }
    }

    private SKBitmap DrawTerrainColor(byte[,] heightMap, byte[,] terrainMap, Rectangle view, int center) {
        SKBitmap terrain = new SKBitmap(view.width*tileSize, view.height*tileSize);
        SKCanvas canvas = new SKCanvas(terrain);

        for (int x = 0; x < view.width; x++) {
            for (int y = 0; y < view.height; y++) {
                // verschil t.o.v. center bepalen
                int diff = center - heightMap[x + view.x, y + view.y];
                int type = terrainMap[x + view.x, y + view.y];
                SKPaint paint;

                // inkleuren met juiste terreintype
                if (diff < -10) {
                    paint = white;
                } else if (diff > 10) {
                    paint = black;
                } else if (type < 32) {
                    paint = paint1;
                } else if (type < 64) {
                    paint = paint2;
                } else if (type < 96) {
                    paint = paint3;
                } else if (type < 128) {
                    paint = paint4;
                } else if (type < 160) {
                    paint = paint5;
                } else if (type < 192) {
                    paint = paint6;
                } else if (type < 224) {
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

                // en blokje tekenen op de canvas
                canvas.DrawRect(x*tileSize, y*tileSize, tileSize, tileSize, paint);
            }
        }

        return terrain;
    }

    private record Rectangle(int x, int y, int width, int height);
}
