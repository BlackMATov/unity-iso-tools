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

		GameObject _isoFakeCollider = null;
		public GameObject IsoFakeCollider {
			get { return _isoFakeCollider; }
		}

		public Collider RealCollider {
			get { return IsoFakeCollider ? IsoFakeCollider.GetComponent<Collider>() : null; }
		}

		void Awake() {
			_isoFakeCollider = new GameObject();
			_isoFakeCollider.transform.SetParent(
				IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject).IsoFakeObject.transform, false);
			_isoFakeCollider.AddComponent<IsoFakeCollider>().Init(this);
			var real_collider       = CreateCollider(_isoFakeCollider);
			real_collider.material  = Material;
			real_collider.isTrigger = IsTrigger;
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
			if ( _isoFakeCollider ) {
				Destroy(_isoFakeCollider);
				_isoFakeCollider = null;
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