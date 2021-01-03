using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Internal {
	public abstract class IsoBehaviour : MonoBehaviour {
		static List<IsoWorld>  _tempWorlds = new List<IsoWorld>();

		// ---------------------------------------------------------------------
		//
		// Protected
		//
		// ---------------------------------------------------------------------

		protected IsoWorld FindFirstActiveWorld() {
			IsoWorld ret_value = null;
			GetComponentsInParent<IsoWorld>(false, _tempWorlds);
			for ( int i = 0, e = _tempWorlds.Count; i < e; ++i ) {
				var iso_world = _tempWorlds[i];
				if ( iso_world.IsActive() ) {
					ret_value = iso_world;
					break;
				}
			}
			_tempWorlds.Clear();
			return ret_value;
		}

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public bool IsActive() {
			return isActiveAndEnabled && gameObject.activeInHierarchy;
		}
	}
}