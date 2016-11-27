using UnityEngine;
using IsoTools.Physics.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Physics {
	[RequireComponent(typeof(IsoObject))]
	public abstract class IsoCollider : IsoPhysicsHelperHolder {
		protected abstract Collider CreateRealCollider(GameObject target);

		IsoFakeCollider _fakeCollider;

		Collider _realCollider = null;
		public Collider realCollider {
			get { return _realCollider; }
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
					? IsoPhysicsUtils.IsoConvertRigidbody(realCollider.attachedRigidbody)
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

		public float contactOffset {
			get {
				return realCollider
					? realCollider.contactOffset
					: 0.0f;
			} set {
				if ( realCollider ) {
					realCollider.contactOffset = value;
				}
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
			_fakeCollider           = fakeObject.AddComponent<IsoFakeCollider>().Init(this);
			_realCollider           = CreateRealCollider(fakeObject);
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
				Destroy(_realCollider);
			}
			if ( _fakeCollider ) {
				Destroy(_fakeCollider);
			}
			DestroyUnnecessaryCheck();
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
}