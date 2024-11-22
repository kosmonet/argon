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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Argon.Common;

/// <summary>
/// Contains common methods related to logging.
/// </summary>
public static class LogHelper {
	/// <summary>
	/// A logger that logs to the debug output by default.
	/// </summary>
	public static ILogger Logger { get; } = new DebugLoggerProvider().CreateLogger("argon");
}