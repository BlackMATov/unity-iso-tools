using UnityEngine;

namespace IsoTools {
	[ExecuteInEditMode]
	[RequireComponent(typeof(IsoPhysicHelper))]
	public abstract class IsoCollider : MonoBehaviour {

		public PhysicMaterial Material  = null;
		public bool           IsTrigger = false;

		public IsoObject IsoObject {
			get { return GetComponent<IsoPhysicHelper>().IsoObject; }
		}

		public IsoFakeObject IsoFakeObject {
			get { return GetComponent<IsoPhysicHelper>().IsoFakeObject; }
		}

		public    abstract void     EditorReset();
		protected abstract Collider CreateCollider();

		void Awake() {
			if ( Application.isPlaying ) {
				var collider = CreateCollider();
				if ( collider ) {
					collider.material  = Material;
					collider.isTrigger = IsTrigger;
				}
			}
			#if UNITY_EDITOR
			else if ( Application.isEditor ) {
				EditorReset();
			}
			#endif
		}
	}
} // namespace IsoTools