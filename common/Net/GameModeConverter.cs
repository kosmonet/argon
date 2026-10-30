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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Argon.Common.Net;

public class GameModeConverter : JsonConverter<GameMode> {
    public override GameMode Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) {
        string? value = reader.GetString();
        if(Enum.TryParse(value, true, out GameMode mode)) {
            return mode;
        } else {
            return GameMode.Host;
        }
    }

    public override void Write(Utf8JsonWriter writer, GameMode value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}
