using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoRigidbody : MonoBehaviour {

		[SerializeField]
		public bool _isKinematic = false;
		public bool IsKinematic {
			get { return _isKinematic; }
			set {
				_isKinematic = value;
				if ( RealRigidbody ) {
					RealRigidbody.isKinematic = value;
				}
			}
		}

		[SerializeField]
		public RigidbodyInterpolation _interpolation = RigidbodyInterpolation.None;
		public RigidbodyInterpolation Interpolation {
			get { return _interpolation; }
			set {
				_interpolation = value;
				if ( RealRigidbody ) {
					RealRigidbody.interpolation = value;
				}
			}
		}

		[SerializeField]
		public CollisionDetectionMode _collisionDetectionMode = CollisionDetectionMode.Discrete;
		public CollisionDetectionMode CollisionDetectionMode {
			get { return _collisionDetectionMode; }
			set {
				_collisionDetectionMode = value;
				if ( RealRigidbody ) {
					RealRigidbody.collisionDetectionMode = value;
				}
			}
		}

		Rigidbody _realRigidbody = null;
		public Rigidbody RealRigidbody {
			get { return _realRigidbody; }
		}
		
		void Awake() {
			var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
			_realRigidbody                        = helper.IsoFakeObject.AddComponent<Rigidbody>();
			_realRigidbody.freezeRotation         = true;
			_realRigidbody.isKinematic            = IsKinematic;
			_realRigidbody.interpolation          = Interpolation;
			_realRigidbody.collisionDetectionMode = CollisionDetectionMode;
		}

		void OnEnable() {
			if ( RealRigidbody ) {
				RealRigidbody.detectCollisions = true;
			}
		}
		
		void OnDisable() {
			if ( RealRigidbody ) {
				RealRigidbody.detectCollisions = false;
			}
		}
		
		void OnDestroy() {
			if ( _realRigidbody ) {
				Destroy(_realRigidbody);
				_realRigidbody = null;
			}
		}

		#if UNITY_EDITOR
		void Reset() {
			IsKinematic            = false;
			Interpolation          = RigidbodyInterpolation.None;
			CollisionDetectionMode = CollisionDetectionMode.Discrete;
			EditorUtility.SetDirty(this);
		}
		
		void OnValidate() {
			if ( RealRigidbody ) {
				RealRigidbody.isKinematic            = IsKinematic;
				RealRigidbody.interpolation          = Interpolation;
				RealRigidbody.collisionDetectionMode = CollisionDetectionMode;
			}
		}
		#endif
	}
} // namespace IsoTools