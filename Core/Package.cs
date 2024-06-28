using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using RealtimeCSG.Components;
using UnityEngine;

namespace UN2WT {
	public class Package : MonoBehaviour {

		public List<Map> Maps;

		[Header("Export")] public string ExportPath = "export.zip";

		public ResourceManager Resources => resourceManager;

		private ResourceManager resourceManager;

		public void Export() {
			try {
				FileStream file = File.Create(ExportPath);
				ZipArchive archive = new ZipArchive(file, ZipArchiveMode.Create);
				resourceManager = new ResourceManager();
				
				foreach(Map map in Maps) {
					map.WriteTo(this, archive);
				}
				
				foreach(var entry in resourceManager.Materials) {
					WriteMaterial(entry.Key, entry.Value, archive);
				}

				foreach(var entry in resourceManager.Textures) {
					WriteTexture(entry.Key, entry.Value, archive);
				}

				resourceManager = null;
				archive.Dispose();
				file.Close();
			} catch(Exception e) {
				Debug.LogError(e);
			}
		}

		private void WriteTexture(Texture2D texture, string path, ZipArchive archive) {
			ZipArchiveEntry entry = archive.CreateEntry(path);
			Stream stream = entry.Open();
			stream.Write(texture.EncodeToPNG());
			stream.Close();
		}
		
		private void WriteMaterial(Material material, string path, ZipArchive archive) {
			ZipArchiveEntry entry = archive.CreateEntry(path);
			Stream stream = entry.Open();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			settings.CloseOutput = true;
			settings.OmitXmlDeclaration = false;
			settings.Encoding = new UTF8Encoding(false);
			using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
				writer.WriteStartElement("material");
				writer.WriteElementString("albedo_map", resourceManager.ProcessTexture(material.mainTexture as Texture2D));
				writer.WriteElementString("uv_scale", Utility.GetVectorString(material.mainTextureScale));
				writer.WriteElementString("uv_offset", Utility.GetVectorString(material.mainTextureOffset));
				writer.WriteEndElement();
				writer.Flush();
			}
			stream.Close();
		}
		
		private string GetVectorString(Vector3 vector) {
			return string.Format("{0} {1} {2}", vector.x.ToString(), vector.y.ToString(), vector.z.ToString());
		}

		private string GetVectorString(Vector2 vector) {
			return string.Format("{0} {1}", vector.x.ToString(), vector.y.ToString());
		}

	}
}