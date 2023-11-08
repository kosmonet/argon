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

using System.Text.Json.Serialization;

namespace Argon.Common.Assets;

public record Asset (string Id, string Kind) {
	/// <summary>
	/// The identifier of this <c>Asset</c>. Every <c>Asset</c> has a unique identifier.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; init; } = Guard.RequireNonNullOrEmpty(Id, "Id must not be null or empty.");

	/// <summary>
	/// The kind of <c>Asset</c>.
	/// </summary>
	[JsonPropertyName("kind")]
	public string Kind { get; init; } = Guard.RequireNonNullOrEmpty(Kind, "Kind must not be null or empty.");
}