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

using Argon.Common.Files;
using System.Xml;

namespace Argon.Common.Assets;

/// <summary>
/// Class that loads module descriptions from an XML file.
/// </summary>
public class ModuleLoader : IAssetLoader<ModuleAsset> {
	private readonly ArgonFileSystem _files;

	public ModuleLoader(ArgonFileSystem fileSystem) { 
		_files = fileSystem;
	}

	public ModuleAsset LoadAsset(string id) {
		FileInfo file = _files.LoadFile($"{id}.xml");
		using var reader = new StreamReader(file.FullName);
		var document = new XmlDocument();
		document.LoadXml(reader.ReadToEnd());
		XmlNode root = document.DocumentElement ?? throw new ArgumentException(id);

		XmlNode titleNode = root.SelectSingleNode("title") ?? throw new ArgumentException(id);
		string title = titleNode.InnerText;

		XmlNode subNode = root.SelectSingleNode("subtitle") ?? throw new ArgumentException(id);
		string subtitle = subNode.InnerText;

		XmlNode descriptionNode = root.SelectSingleNode("description") ?? throw new ArgumentException(id);
		string description = descriptionNode.InnerText;

		return new ModuleAsset(id, title, subtitle, description);
	}

	public void SaveAsset(ModuleAsset module) {
		XmlDocument document = new();
		XmlElement root = document.CreateElement("module");
		root.SetAttribute("id", module.Id);
		document.AppendChild(root);
		XmlElement title = document.CreateElement("title");
		title.InnerText = module.Title;
		root.AppendChild(title);
		XmlElement subtitle = document.CreateElement("subtitle");
		subtitle.InnerText = module.Subtitle;
		root.AppendChild(subtitle);
		XmlElement description = document.CreateElement("description");
//		XmlCDataSection cdataSection = document.CreateCDataSection(module.Description);
//		description.AppendChild(cdataSection);
		description.InnerText = module.Description;
		root.AppendChild(description);

		FileInfo file = _files.SaveFile($"{module.Id}.xml");
		using var stream = file.Create();
		document.Save(stream);
	}

	/// <summary>
	/// Verifies that the given file is indeed a valid module description.
	/// </summary>
	/// <param name="file">The file to be checked.</param>
	/// <returns>True if it is a valid module, false otherwise.</returns>
	public static Boolean VerifyAsset(FileInfo file) {
		using var reader = new StreamReader(file.FullName);
		var document = new XmlDocument();
		document.LoadXml(reader.ReadToEnd());

		XmlNode? root = document.DocumentElement;

		if (root is not null) {
			return root.Name.Equals("module");
		} else {
			return false;
		}
	}
}
