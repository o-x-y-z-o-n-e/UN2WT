using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using RealtimeCSG;
using RealtimeCSG.Components;

namespace UN2WT {
	public class Map : MonoBehaviour {

		[Header("Settings")] public string OutputPath = "map.scn";
		public string Name = "New Map";
		public string[] Authors = default;
		public bool FogEnabled = true;
		public float FogDensity = 0.05F;
		[ColorUsage(false)] public Color FogColor = new Color(0.7F, 0.7F, 0.7F);
		public Texture2D SkyTexture = null;
		public float AmbientLightLevel = 0.2F;

		private string GetAuthorString() {
			StringBuilder authors = new StringBuilder();
			for(int i = 0; i < Authors.Length; i++) {
				if(Authors[i] != "") {
					if(i > 0) authors.Append(';');
					authors.Append(Authors[i]);
				}
			}

			return authors.ToString();
		}

		private void WriteSettings(Package package, XmlWriter writer) {
			writer.WriteStartElement("settings");
			{
				writer.WriteStartElement("fog");
				writer.WriteAttributeString("enabled", FogEnabled.ToString());
				writer.WriteAttributeString("density", FogDensity.ToString());
				writer.WriteAttributeString("color", string.Format("{0} {1} {2}", FogColor.r, FogColor.g, FogColor.b));
				writer.WriteEndElement();
			}
			{
				writer.WriteStartElement("sky");
				//writer.WriteAttributeString("texture", package.Resources.ProcessTexture(SkyTexture));
				writer.WriteAttributeString("texture", "env/skyboxes/space/star-fields/Starfield_08-512x512.png");
				writer.WriteEndElement();
			}
			{
				writer.WriteStartElement("lighting");
				writer.WriteAttributeString("ambient_light_level", AmbientLightLevel.ToString());
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
		
		private void WriteEvent(Event ev, Package package, XmlWriter writer) {
			writer.WriteStartElement("event");
			writer.WriteEndElement();
		}

		private void WriteEntity(EntityDescriptor entity, Package package, XmlWriter writer) {
			writer.WriteStartElement("entity");
			writer.WriteAttributeString("type", entity.Type);
			writer.WriteAttributeString("origin", Utility.GetVectorString(entity.transform.position));
			writer.WriteAttributeString("euler", Utility.GetVectorString(entity.transform.eulerAngles));
			entity.WriteParameters(package, writer);
			writer.WriteEndElement();
		}

		private void WriteModel(CSGModel model, Package package, XmlWriter writer) {
			if(model.ShowGeneratedMeshes == false) return;
			
			writer.WriteStartElement("geometry");
			writer.WriteAttributeString("name", model.name);
			writer.WriteAttributeString("origin", Utility.GetVectorString(model.transform.position));
			writer.WriteAttributeString("euler", Utility.GetVectorString(model.transform.eulerAngles));
			
			foreach(var mesh in model.generatedMeshes.MeshInstances) {
				if(mesh.CachedMeshFilter != null) {
					Mesh raw = mesh.CachedMeshFilter.sharedMesh;
					writer.WriteStartElement("mesh");
					if(mesh.CachedMeshRenderer != null) {
						Material material = mesh.CachedMeshRenderer.sharedMaterial;
						writer.WriteElementString("material", package.Resources.ProcessMaterial(material));
					}
					{
						writer.WriteStartElement("points");
						StringBuilder points = new StringBuilder();
						for(int i = 0; i < raw.vertices.Length; i++) {
							points.Append(Utility.GetVectorString(raw.vertices[i]));
							if(i < raw.vertices.Length - 1) points.Append(',');
						}

						writer.WriteString(points.ToString());
						writer.WriteEndElement();
					}
					{
						writer.WriteStartElement("normals");
						StringBuilder normals = new StringBuilder();
						for(int i = 0; i < raw.normals.Length; i++) {
							normals.Append(Utility.GetVectorString(raw.vertices[i]));
							if(i < raw.normals.Length - 1) normals.Append(',');
						}

						writer.WriteString(normals.ToString());
						writer.WriteEndElement();
					}
					{
						writer.WriteStartElement("uvs");
						StringBuilder uvs = new StringBuilder();
						for(int i = 0; i < raw.uv.Length; i++) {
							uvs.Append(Utility.GetVectorString(raw.uv[i]));
							if(i < raw.normals.Length - 1) uvs.Append(',');
						}

						writer.WriteString(uvs.ToString());
						writer.WriteEndElement();
					}
					{
						writer.WriteStartElement("indices");
						StringBuilder indices = new StringBuilder();
						for(int i = 0; i < raw.triangles.Length; i++) {
							indices.Append(raw.triangles[i].ToString());
							if(i < raw.triangles.Length - 1 && raw.triangles[i + 1] >= 0) indices.Append(',');
						}

						writer.WriteString(indices.ToString());
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				} else if(mesh.CachedMeshCollider != null) {
					Mesh raw = mesh.CachedMeshCollider.sharedMesh;
					writer.WriteStartElement("collider");
					{
						writer.WriteStartElement("points");
						StringBuilder points = new StringBuilder();
						for(int i = 0; i < raw.vertices.Length; i++) {
							points.Append(Utility.GetVectorString(raw.vertices[i]));
							if(i < raw.vertices.Length - 1) points.Append(',');
						}

						writer.WriteString(points.ToString());
						writer.WriteEndElement();
					}
					{
						writer.WriteStartElement("indices");
						StringBuilder indices = new StringBuilder();
						for(int i = 0; i < raw.triangles.Length; i++) {
							indices.Append(raw.triangles[i].ToString());
							if(i < raw.triangles.Length - 1 && raw.triangles[i + 1] >= 0) indices.Append(',');
						}

						writer.WriteString(indices.ToString());
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
			}

			writer.WriteEndElement();
		}

		public void WriteTo(Package package, ZipArchive archive) {
			ZipArchiveEntry entry = archive.CreateEntry(OutputPath);
			Stream stream = entry.Open();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			settings.CloseOutput = true;
			settings.OmitXmlDeclaration = false;
			settings.Encoding = new UTF8Encoding(false);
			using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
				writer.WriteStartElement("scene");
				writer.WriteAttributeString("name", Name);
				writer.WriteAttributeString("authors", GetAuthorString());
				WriteSettings(package, writer);
				foreach(var entity in GetComponentsInChildren<EntityDescriptor>()) WriteEntity(entity, package, writer);
				foreach(var model in GetComponentsInChildren<CSGModel>()) WriteModel(model, package, writer);
				writer.WriteEndElement();
				writer.Flush();
			}
			stream.Close();
		}

	}
}