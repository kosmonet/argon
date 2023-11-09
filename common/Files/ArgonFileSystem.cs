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
	private readonly ILogger _logger = LogHelper.GetLogger();

	private readonly string _tempFolder;
	private string _saveFolder;

	/// <summary>
	/// Creates a new file system with the given temporary and save folder 
	/// paths. The folders are created if they do not exist yet. The contents 
	/// of the temp folder are cleared before use.
	/// </summary>
	/// <param name="tempFolder">path to the temporary folder</param>
	public ArgonFileSystem(string tempFolder) {
		_tempFolder = Guard.NonNullOrEmpty(tempFolder, "Temp folder must not be empty or null.");
		_saveFolder = _tempFolder;

		if (Path.Exists(_tempFolder)) {
			ClearFolder(_tempFolder);
			_logger.LogInformation("Set temporary folder to {folder}", _tempFolder);
		} else {
			Directory.CreateDirectory(_tempFolder);
			_logger.LogInformation("Created temporary folder {folder}", _tempFolder);
		}
	}

	public void SetSaveFolder(string path) {
		_saveFolder = Guard.NonNullOrEmpty(path, "Save path must not be empty or null.");
		if (Path.Exists(_saveFolder)) {
			_logger.LogInformation("Set save folder to {folder}", _saveFolder);
		} else {
			Directory.CreateDirectory(_saveFolder);
			_logger.LogInformation("Created save folder {folder}", _saveFolder);
		}
	}

	public void AddModule(string path) {
		if (Directory.Exists(path) || IsZipFile(path)) {
			_modules.Push(Guard.NonNullOrEmpty(path, "Module path must not be empty or null."));
		}
	}

	internal FileInfo LoadFile(params string[] path) {
		return LoadFile(Path.Combine(path));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path">the relative path to the requested file</param>
	/// <returns>a FileInfo object that points to the requested relative path</returns>
	/// <exception cref="FileNotFoundException"></exception>
	internal FileInfo LoadFile(string path) {
		_logger.LogInformation("Load file: {path}", path);

		// first check the temp folder
		var file = new FileInfo(Path.Combine(_tempFolder, path));
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
		var file = new FileInfo(Path.Combine(_tempFolder, path));
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
