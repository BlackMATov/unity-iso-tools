using UnityEngine;

namespace IsoTools.Internal {
	public abstract class IsoBehaviour<T> : MonoBehaviour
		where T : IsoBehaviour<T>
	{
		static IsoAssocList<T> _behaviours = new IsoAssocList<T>();

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public bool IsActive() {
			return isActiveAndEnabled && gameObject.activeInHierarchy;
		}

		protected static int AllBehaviourCount {
			get { return _behaviours.Count; }
		}

		protected static T GetBehaviourByIndex(int index) {
			return _behaviours[index];
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