using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Internal {
	public abstract class IsoBehaviour<T> : MonoBehaviour
		where T : IsoBehaviour<T>
	{
		static IsoAssocList<T> _behaviours = new IsoAssocList<T>();
		static List<IsoWorld>  _tempWorlds = new List<IsoWorld>();

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void Internal_SetDirtyInEditorMode() {
		#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
		#endif
		}

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

		protected static int AllBehaviourCount {
			get { return _behaviours.Count; }
		}

		protected static T GetBehaviourByIndex(int index) {
			return _behaviours[index];
		}

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public bool IsActive() {
			return isActiveAndEnabled && gameObject.activeInHierarchy;
		}

		// ---------------------------------------------------------------------
		//
		// Virtual
		//
		// ---------------------------------------------------------------------

		protected virtual void OnEnable() {
			var behaviour = this as T;
			if ( behaviour && behaviour.IsActive() ) {
				_behaviours.Add(behaviour);
			}
		}

		protected virtual void OnDisable() {
			var behaviour = this as T;
			if ( behaviour ) {
				_behaviours.Remove(behaviour);
			}
		}
	}
}