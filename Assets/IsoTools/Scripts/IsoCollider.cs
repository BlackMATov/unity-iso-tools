using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public abstract class IsoCollider : MonoBehaviour {
		protected abstract Collider CreateCollider(GameObject target);

		[SerializeField]
		public PhysicMaterial _material  = null;
		public PhysicMaterial Material {
			get { return _material; }
			set {
				_material = value;
				if ( RealCollider ) {
					RealCollider.material = value;
				}
			}
		}

		[SerializeField]
		public bool _isTrigger = false;
		public bool IsTrigger {
			get { return _isTrigger; }
			set {
				_isTrigger = value;
				if ( RealCollider ) {
					RealCollider.isTrigger = value;
				}
			}
		}

		Collider _realCollider = null;
		public Collider RealCollider {
			get { return _realCollider; }
		}

		void Awake() {
			var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
			_realCollider = CreateCollider(helper.IsoFakeObject);
			if ( _realCollider ) {
				_realCollider.material  = Material;
				_realCollider.isTrigger = IsTrigger;
			}
		}

		void OnEnable() {
			if ( RealCollider ) {
				RealCollider.enabled = true;
			}
		}

		void OnDisable() {
			if ( RealCollider ) {
				RealCollider.enabled = false;
			}
		}

		void OnDestroy() {
			if ( _realCollider ) {
				Destroy(_realCollider);
				_realCollider = null;
			}
		}

		#if UNITY_EDITOR
		protected virtual void Reset() {
			Material  = null;
			IsTrigger = false;
			EditorUtility.SetDirty(this);
		}

		protected virtual void OnValidate() {
			if ( RealCollider ) {
				RealCollider.material  = Material;
				RealCollider.isTrigger = IsTrigger;
			}
		}
		#endif
	}
} // namespace IsoTools