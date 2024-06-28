using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UN2WT {
	public static class Utility {
		public static string GetVectorString(Vector3 vector) {
			return string.Format("{0} {1} {2}", vector.x.ToString(), vector.y.ToString(), vector.z.ToString());
		}

		public static string GetVectorString(Vector2 vector) {
			return string.Format("{0} {1}", vector.x.ToString(), vector.y.ToString());
		}
	}
}