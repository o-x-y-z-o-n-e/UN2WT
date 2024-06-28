using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UN2WT {
	[CustomEditor(typeof(Map))]
	public class MapEditor : Editor  {

		private Map map => (Map)target;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
		}

	}
}