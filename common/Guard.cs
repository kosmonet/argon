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

namespace Argon.Common;

/// <summary>
/// Collection of methods to validate parameters.
/// </summary>
public class Guard {
	/// <summary>
	/// Checks if value is null. If so, throws an ArgumentNullException, otherwise returns value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	public static T NotNull<T> (T? value, string message) {
		ArgumentNullException.ThrowIfNull(value, message);
		return value;
	}

	/// <summary>
	/// Checks if value is null or empty. If so, throws an ArgumentException, otherwise returns value.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	public static string NotNullOrEmpty(string? value, string message) {
		ArgumentException.ThrowIfNullOrEmpty(value, message);
		return value;
	}
}
