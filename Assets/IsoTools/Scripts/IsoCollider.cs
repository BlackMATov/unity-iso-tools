using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public abstract class IsoCollider : MonoBehaviour {
		protected abstract Collider CreateRealCollider(GameObject target);

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
			var fake_collider_go = new GameObject();
			fake_collider_go.transform.SetParent(
				IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject).IsoFakeObject.transform, false);
			fake_collider_go.AddComponent<IsoFakeCollider>().Init(this);
			_realCollider           = CreateRealCollider(fake_collider_go);
			_realCollider.material  = Material;
			_realCollider.isTrigger = IsTrigger;
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
				Destroy(_realCollider.gameObject);
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