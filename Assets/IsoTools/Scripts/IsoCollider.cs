using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public abstract class IsoCollider : MonoBehaviour {
		protected abstract Collider CreateRealCollider(GameObject target);

		Collider _realCollider = null;
		protected Collider RealCollider {
			get { return _realCollider; }
		}

		protected GameObject IsoFakeObject {
			get {
				var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
				return helper ? helper.IsoFakeObject : null;
			}
		}

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

		public IsoRigidbody AttachedRigidbody {
			get {
				return RealCollider
					? IsoUtils.IsoConvertRigidbody(RealCollider.attachedRigidbody)
					: null;
			}
		}

		public Bounds Bounds {
			get {
				return RealCollider
					? RealCollider.bounds
					: new Bounds();
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position) {
			return RealCollider
				? RealCollider.ClosestPointOnBounds(position)
				: Vector3.zero;
		}

		public bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info, float max_distance) {
			RaycastHit hit_info;
			var result = RealCollider
				? RealCollider.Raycast(ray, out hit_info, max_distance)
				: false;
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		void Awake() {
			var fake_collider_go = new GameObject();
			fake_collider_go.transform.SetParent(IsoFakeObject.transform, false);
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
			Material  = _material;
			IsTrigger = _isTrigger;
		}
		#endif
	}
} // namespace IsoTools