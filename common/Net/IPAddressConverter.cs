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

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Argon.Common.Net;

public class IPAddressConverter : JsonConverter<IPAddress> {
    /// <summary>
    /// Parses a string into an address. If the string is empty, 127.0.0.1 is returned.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return IPAddress.Parse(reader.GetString() ?? "127.0.0.1");
    }

    /// <summary>
    /// Converts an IP address to a string representation.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}
