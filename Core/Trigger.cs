using System.Collections;
using System.Collections.Generic;
using UN2WT;
using UnityEngine;

namespace UN2WT {
	public class Trigger : EntityDescriptor {

		public override string Type => "trigger-box";

		public string EventName;
		public Vector3 Size;
		public Vector3 Offset;

	}
}