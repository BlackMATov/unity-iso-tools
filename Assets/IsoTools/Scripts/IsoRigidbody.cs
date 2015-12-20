using UnityEngine;
using IsoTools.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoRigidbody : MonoBehaviour {

		Rigidbody _realRigidbody = null;
		protected Rigidbody realRigidbody {
			get { return _realRigidbody; }
		}

		protected GameObject fakeObject {
			get {
				var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
				return helper ? helper.isoFakeObject : null;
			}
		}

		[SerializeField]
		public bool _isKinematic = false;
		public bool isKinematic {
			get { return _isKinematic; }
			set {
				_isKinematic = value;
				if ( realRigidbody ) {
					realRigidbody.isKinematic = value;
				}
			}
		}

		[SerializeField]
		public RigidbodyInterpolation _interpolation = RigidbodyInterpolation.None;
		public RigidbodyInterpolation interpolation {
			get { return _interpolation; }
			set {
				_interpolation = value;
				if ( realRigidbody ) {
					realRigidbody.interpolation = value;
				}
			}
		}

		[SerializeField]
		public CollisionDetectionMode _collisionDetectionMode = CollisionDetectionMode.Discrete;
		public CollisionDetectionMode collisionDetectionMode {
			get { return _collisionDetectionMode; }
			set {
				_collisionDetectionMode = value;
				if ( realRigidbody ) {
					realRigidbody.collisionDetectionMode = value;
				}
			}
		}

		public Vector3 centerOfMass {
			get { return realRigidbody ? realRigidbody.centerOfMass : Vector3.zero; }
			set {
				if ( realRigidbody ) {
					realRigidbody.centerOfMass = value;
				}
			}
		}

		public float drag {
			get { return realRigidbody ? realRigidbody.drag : 0.0f; }
			set {
				if ( realRigidbody ) {
					realRigidbody.drag = value;
				}
			}
		}

		public Vector3 inertiaTensor {
			get { return realRigidbody ? realRigidbody.inertiaTensor : Vector3.zero; }
			set {
				if ( realRigidbody ) {
					realRigidbody.inertiaTensor = value;
				}
			}
		}

		public float mass {
			get { return realRigidbody ? realRigidbody.mass : 0.0f; }
			set {
				if ( realRigidbody ) {
					realRigidbody.mass = value;
				}
			}
		}

		public float maxDepenetrationVelocity {
			get { return realRigidbody ? realRigidbody.maxDepenetrationVelocity : 0.0f; }
			set {
				if ( realRigidbody ) {
					realRigidbody.maxDepenetrationVelocity = value;
				}
			}
		}

		public float sleepThreshold {
			get { return realRigidbody ? realRigidbody.sleepThreshold : 0.0f; }
			set {
				if ( realRigidbody ) {
					realRigidbody.sleepThreshold = value;
				}
			}
		}

		public int solverIterationCount {
			get { return realRigidbody ? realRigidbody.solverIterationCount : 0; }
			set {
				if ( realRigidbody ) {
					realRigidbody.solverIterationCount = value;
				}
			}
		}
		
		public bool useGravity {
			get { return realRigidbody ? realRigidbody.useGravity : false; }
			set {
				if ( realRigidbody ) {
					realRigidbody.useGravity = value;
				}
			}
		}
		
		public Vector3 velocity {
			get { return realRigidbody ? realRigidbody.velocity : Vector3.zero; }
			set {
				if ( realRigidbody ) {
					realRigidbody.velocity = value;
				}
			}
		}
		
		public Vector3 worldCenterOfMass {
			get { return realRigidbody ? realRigidbody.worldCenterOfMass : Vector3.zero; }
		}

		public void AddExplosionForce(
			float explosion_force, Vector3 explosion_position, float explosion_radius)
		{
			AddExplosionForce(
				explosion_force, explosion_position, explosion_radius,
				0.0f, ForceMode.Force);
		}

		public void AddExplosionForce(
			float explosion_force, Vector3 explosion_position, float explosion_radius,
			float upwards_modifier)
		{
			AddExplosionForce(
				explosion_force, explosion_position, explosion_radius,
				upwards_modifier, ForceMode.Force);
		}

		public void AddExplosionForce(
			float explosion_force, Vector3 explosion_position, float explosion_radius,
			float upwards_modifier, ForceMode mode)
		{
			if ( realRigidbody ) {
				realRigidbody.AddExplosionForce(
					explosion_force, explosion_position, explosion_radius,
					upwards_modifier, mode);
			}
		}

		public void AddForce(Vector3 force) {
			AddForce(force, ForceMode.Force);
		}

		public void AddForce(Vector3 force, ForceMode mode) {
			if ( realRigidbody ) {
				realRigidbody.AddForce(force, mode);
			}
		}

		public void AddRelativeForce(Vector3 force) {
			AddRelativeForce(force, ForceMode.Force);
		}
		
		public void AddRelativeForce(Vector3 force, ForceMode mode) {
			if ( realRigidbody ) {
				realRigidbody.AddRelativeForce(force, mode);
			}
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position) {
			AddForceAtPosition(force, position, ForceMode.Force);
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode) {
			if ( realRigidbody ) {
				realRigidbody.AddForceAtPosition(force, position, mode);
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position) {
			return realRigidbody
				? realRigidbody.ClosestPointOnBounds(position)
				: Vector3.zero;
		}

		public bool IsSleeping() {
			return realRigidbody
				? realRigidbody.IsSleeping()
				: false;
		}

		public void SetDensity(float density) {
			if ( realRigidbody ) {
				realRigidbody.SetDensity(density);
			}
		}

		public void Sleep() {
			if ( realRigidbody ) {
				realRigidbody.Sleep();
			}
		}

		public void WakeUp() {
			if ( realRigidbody ) {
				realRigidbody.WakeUp();
			}
		}

		public bool SweepTest(Vector3 direction, out IsoRaycastHit iso_hit_info) {
			return SweepTest(direction, out iso_hit_info, Mathf.Infinity);
		}

		public bool SweepTest(Vector3 direction, out IsoRaycastHit iso_hit_info, float max_distance) {
			var hit_info = new RaycastHit();
			var result = realRigidbody
				? realRigidbody.SweepTest(direction, out hit_info, max_distance)
				: false;
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		public IsoRaycastHit[] SweepTestAll(Vector3 direction) {
			return SweepTestAll(direction, Mathf.Infinity);
		}

		public IsoRaycastHit[] SweepTestAll(Vector3 direction, float max_distance) {
			return realRigidbody
				? IsoUtils.IsoConvertRaycastHits(realRigidbody.SweepTestAll(direction, max_distance))
				: new IsoRaycastHit[0];
		}
		
		void Awake() {
			IsoUtils.GetOrCreateComponent<IsoFakeRigidbody>(fakeObject).Init(this);
			_realRigidbody                        = IsoUtils.GetOrCreateComponent<Rigidbody>(fakeObject);
			_realRigidbody.freezeRotation         = true;
			_realRigidbody.isKinematic            = isKinematic;
			_realRigidbody.interpolation          = interpolation;
			_realRigidbody.collisionDetectionMode = collisionDetectionMode;
		}

		void OnEnable() {
			if ( realRigidbody ) {
				realRigidbody.detectCollisions = true;
			}
		}
		
		void OnDisable() {
			if ( realRigidbody ) {
				realRigidbody.detectCollisions = false;
			}
		}
		
		void OnDestroy() {
			if ( _realRigidbody ) {
				Destroy(fakeObject.GetComponent<IsoFakeRigidbody>());
				Destroy(_realRigidbody);
				_realRigidbody = null;
			}
		}

		#if UNITY_EDITOR
		void Reset() {
			isKinematic            = false;
			interpolation          = RigidbodyInterpolation.None;
			collisionDetectionMode = CollisionDetectionMode.Discrete;
			EditorUtility.SetDirty(this);
		}
		
		void OnValidate() {
			if ( realRigidbody ) {
				realRigidbody.isKinematic            = isKinematic;
				realRigidbody.interpolation          = interpolation;
				realRigidbody.collisionDetectionMode = collisionDetectionMode;
			}
		}
		#endif
	}
} // namespace IsoTools