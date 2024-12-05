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

using System.Text.Json.Serialization;

namespace Argon.Common.Assets; 

public record CreatureAsset (string Id, string Name, string Image) : Asset (Id) {
	/// <summary>
	/// The name of this <c>Asset</c> as it is used to describe it in the game.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; init; } = Guard.NotNullOrEmpty(Name, "Name must not be null or empty.");

	/// <summary>
	/// The path to the image file used for this <c>Asset</c> in the game.
	/// </summary>
	[JsonPropertyName("image")]
	public string Image { get; init; } = Guard.NotNullOrEmpty(Image, "Image must not be null or empty.");
}