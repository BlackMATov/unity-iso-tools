using UnityEngine;
using IsoTools.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public abstract class IsoCollider : MonoBehaviour {
		protected abstract Collider CreateRealCollider(GameObject target);

		Collider _realCollider = null;
		protected Collider realCollider {
			get { return _realCollider; }
		}

		protected GameObject fakeObject {
			get {
				var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
				return helper ? helper.isoFakeObject : null;
			}
		}

		[SerializeField]
		public PhysicMaterial _material  = null;
		public PhysicMaterial material {
			get { return _material; }
			set {
				_material = value;
				if ( realCollider ) {
					realCollider.material = value;
				}
			}
		}

		[SerializeField]
		public bool _isTrigger = false;
		public bool isTrigger {
			get { return _isTrigger; }
			set {
				_isTrigger = value;
				if ( realCollider ) {
					realCollider.isTrigger = value;
				}
			}
		}

		public IsoRigidbody attachedRigidbody {
			get {
				return realCollider
					? IsoUtils.IsoConvertRigidbody(realCollider.attachedRigidbody)
					: null;
			}
		}

		public Bounds bounds {
			get {
				return realCollider
					? realCollider.bounds
					: new Bounds();
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position) {
			return realCollider
				? realCollider.ClosestPointOnBounds(position)
				: Vector3.zero;
		}

		public bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info, float max_distance) {
			var hit_info = new RaycastHit();
			var result = realCollider
				? realCollider.Raycast(ray, out hit_info, max_distance)
				: false;
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		void Awake() {
			var fake_collider_go = new GameObject();
			fake_collider_go.transform.SetParent(fakeObject.transform, false);
			fake_collider_go.AddComponent<IsoFakeCollider>().Init(this);
			_realCollider           = CreateRealCollider(fake_collider_go);
			_realCollider.material  = material;
			_realCollider.isTrigger = isTrigger;
		}

		void OnEnable() {
			if ( realCollider ) {
				realCollider.enabled = true;
			}
		}

		void OnDisable() {
			if ( realCollider ) {
				realCollider.enabled = false;
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
			material  = null;
			isTrigger = false;
			EditorUtility.SetDirty(this);
		}

		protected virtual void OnValidate() {
			material  = _material;
			isTrigger = _isTrigger;
		}
		#endif
	}
} // namespace IsoTools