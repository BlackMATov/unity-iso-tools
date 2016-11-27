#if PLAYMAKER
using UnityEngine;
using IsoTools.Physics;
using HutongGames.PlayMaker;

namespace IsoTools.PlayMaker.Internal {
	public abstract class IsoComponentAction<T> : FsmStateAction where T : Component {
		T          _cachedComponent;
		GameObject _cachedGameObject;

		public virtual void DoIsoTriggerEnter(IsoCollider collider) {}
		public virtual void DoIsoTriggerExit (IsoCollider collider) {}

		public virtual void DoIsoCollisionEnter(IsoCollision collision) {}
		public virtual void DoIsoCollisionExit (IsoCollision collision) {}

		protected IsoWorld isoWorld {
			get { return _cachedComponent as IsoWorld; }
		}

		protected IsoObject isoObject {
			get { return _cachedComponent as IsoObject; }
		}

		protected IsoRigidbody isoRigidbody {
			get { return _cachedComponent as IsoRigidbody; }
		}

		protected IsoCollider isoCollider {
			get { return _cachedComponent as IsoCollider; }
		}

		protected IsoBoxCollider isoBoxCollider {
			get { return _cachedComponent as IsoBoxCollider; }
		}

		protected IsoSphereCollider isoSphereCollider {
			get { return _cachedComponent as IsoSphereCollider; }
		}

		protected bool UpdateCache(GameObject go) {
			if ( go ) {
				if ( _cachedComponent == null || _cachedGameObject != go ) {
					_cachedComponent = go.GetComponent<T>();
					_cachedGameObject = go;
					if ( !_cachedComponent ) {
						LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name);
					}
				}
			} else {
				_cachedComponent = null;
				_cachedGameObject = null;
			}
			return _cachedComponent != null;
		}

		protected bool IsErrorVarClamp(float v, float min, float max) {
			return v < min || v > max;
		}

		protected string ErrorVarClampMsg(string name, float min, float max) {
			return string.Format(
				"{0} must be greater than {1} and less than {2}",
				name, min, max);
		}
	}
}
#endif