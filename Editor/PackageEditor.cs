using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UN2WT {
	[CustomEditor(typeof(Package))]
	public class PackageEditor : Editor  {

		private Package package => (Package)target;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if(GUILayout.Button("Export")) {
				package.Export();
			}
		}
	}
}