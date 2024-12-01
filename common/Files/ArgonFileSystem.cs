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
using System.Collections.Concurrent;
using System.IO.Compression;

namespace Argon.Common.Files;

/// <summary>
/// A virtual file system for the Argon engine. Saves and modules are added 
/// read-only. All changes are written to a temporary folder until explicitly 
/// flushed to a save folder.
/// </summary>
public class ArgonFileSystem {
	private readonly ConcurrentStack<string> _modules = new();
	private readonly ILogger _logger = LogHelper.Logger;

	private readonly string _tempFolder;
	private string _saveFolder;

	/// <summary>
	/// Creates a new file system with the given temporary folder 
	/// path. The folder is created if it does not exist yet. The contents 
	/// of the temp folder are cleared before use.
	/// </summary>
	/// <param name="tempFolder">Path to the temporary folder.</param>
	public ArgonFileSystem(string tempFolder) {
		_tempFolder = tempFolder;
		_saveFolder = _tempFolder;

		if (Path.Exists(_tempFolder)) {
			ClearFolder(_tempFolder);
			_logger.LogInformation("set temporary folder to {folder}", _tempFolder);
		} else {
			Directory.CreateDirectory(_tempFolder);
			_logger.LogInformation("created temporary folder {folder}", _tempFolder);
		}
	}

	/// <summary>
	/// Sets the path of the save folder.
	/// </summary>
	/// <param name="path">Path to the save folder.</param>
	public void SetSaveFolder(string path) {
		_saveFolder = path;
		if (Path.Exists(_saveFolder)) {
			_logger.LogInformation("set save folder to {folder}", _saveFolder);
		} else {
			Directory.CreateDirectory(_saveFolder);
			_logger.LogInformation("created save folder {folder}", _saveFolder);
		}
	}

	/// <summary>
	/// Adds the files of a module to this file system. The file system does not check if the
	/// given path actually contains a module.
	/// </summary>
	/// <param name="path">The full path of the module.</param>
	public void AddModule(string path) {
		if (Directory.Exists(path) || IsZipFile(path)) {
			_modules.Push(path);
		}
	}

	/// <summary>
	/// Clears the file system. All modules are removed, only temp and save folder are retained.
	/// </summary>
	public void Clear() {
		_modules.Clear();
	}

	/// <summary>
	/// Loads a file from the given path.
	/// </summary>
	/// <param name="path">The relative path to the requested file.</param>
	/// <returns>A FileInfo object that points to the requested relative path.</returns>
	internal FileInfo LoadFile(params string[] path) {
		return LoadFile(Path.Combine(path));
	}

	/// <summary>
	/// Loads a file from the given path.
	/// </summary>
	/// <param name="path">The relative path to the requested file.</param>
	/// <returns>A FileInfo object that points to the requested relative path.</returns>
	/// <exception cref="FileNotFoundException"></exception>
	internal FileInfo LoadFile(string path) {
		_logger.LogInformation("load file: {path}", path);

		// first check the temp folder
		FileInfo file = new(Path.Combine(_tempFolder, path));
		if (file.Exists) {
			return file;
		}

		// then check the save folder
		file = new FileInfo(Path.Combine(_saveFolder, path));
		if (file.Exists) {
			return file;
		}

		// finally check each module from last to first
		foreach (string module in _modules) {
			// modules can be in folders or zip archives
			if (Directory.Exists(module)) {
				file = new FileInfo(Path.Combine(module, path));
				if (file.Exists) {
					return file;
				}
			} else {
				ZipArchive archive = ZipFile.OpenRead(module);
				// ugly hack because .Net does not automatically swap slashes when opening zip archives
				ZipArchiveEntry? entry = archive.GetEntry(path.Replace("\\", "/"));
				if (entry is not null) {
					string extracted = Path.Combine(_tempFolder, path);
					file = new FileInfo(Path.Combine(_tempFolder, path));
					if (file.DirectoryName is not null) {
						Directory.CreateDirectory(file.DirectoryName);
						entry.ExtractToFile(extracted);
						return file;
					}
				}
			}
		}

		throw new FileNotFoundException(path);
	}

	internal FileInfo SaveFile(params string[] path) {
		return SaveFile(Path.Combine(path));
	}

	internal FileInfo SaveFile(string path) {
		FileInfo file = new(Path.Combine(_tempFolder, path));
		ArgumentNullException.ThrowIfNull(file.DirectoryName);
		Directory.CreateDirectory(file.DirectoryName);
		return file;
	}

	private static void ClearFolder(string path) {
		foreach (string file in Directory.EnumerateFiles(path)) {
			File.Delete(file);
		}

		foreach (string directory in Directory.EnumerateDirectories(path)) {
			Directory.Delete(directory, true);
		}
	}

	private static bool IsZipFile(string path) {
		try {
			ZipFile.OpenRead(path);
			return true;
		} catch {
			return false;
		}
	}
}
