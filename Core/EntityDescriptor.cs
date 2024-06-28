using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

namespace UN2WT {
	public abstract class EntityDescriptor : MonoBehaviour {

		public abstract string Type { get; }
		public Dictionary<string, string> Parameters;

		public virtual void WriteParameters(Package package, XmlWriter writer) {
			/* TODO
			foreach(var entry in Parameters) {
				writer.WriteAttributeString(entry.Key, entry.Value);
			}
			*/
		}

	}
}