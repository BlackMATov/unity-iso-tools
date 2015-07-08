using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoRigidbody : MonoBehaviour {

		Rigidbody _realRigidbody = null;
		protected Rigidbody RealRigidbody {
			get { return _realRigidbody; }
		}

		protected GameObject IsoFakeObject {
			get {
				var helper = IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject);
				return helper ? helper.IsoFakeObject : null;
			}
		}

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

		public Vector3 CenterOfMass {
			get { return RealRigidbody ? RealRigidbody.centerOfMass : Vector3.zero; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.centerOfMass = value;
				}
			}
		}

		public float Drag {
			get { return RealRigidbody ? RealRigidbody.drag : 0.0f; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.drag = value;
				}
			}
		}

		public Vector3 InertiaTensor {
			get { return RealRigidbody ? RealRigidbody.inertiaTensor : Vector3.zero; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.inertiaTensor = value;
				}
			}
		}

		public float Mass {
			get { return RealRigidbody ? RealRigidbody.mass : 0.0f; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.mass = value;
				}
			}
		}

		public float MaxDepenetrationVelocity {
			get { return RealRigidbody ? RealRigidbody.maxDepenetrationVelocity : 0.0f; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.maxDepenetrationVelocity = value;
				}
			}
		}

		public float SleepThreshold {
			get { return RealRigidbody ? RealRigidbody.sleepThreshold : 0.0f; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.sleepThreshold = value;
				}
			}
		}

		public int SolverIterationCount {
			get { return RealRigidbody ? RealRigidbody.solverIterationCount : 0; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.solverIterationCount = value;
				}
			}
		}
		
		public bool UseGravity {
			get { return RealRigidbody ? RealRigidbody.useGravity : false; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.useGravity = value;
				}
			}
		}
		
		public Vector3 Velocity {
			get { return RealRigidbody ? RealRigidbody.velocity : Vector3.zero; }
			set {
				if ( RealRigidbody ) {
					RealRigidbody.velocity = value;
				}
			}
		}
		
		public Vector3 worldCenterOfMass {
			get { return RealRigidbody ? RealRigidbody.worldCenterOfMass : Vector3.zero; }
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
			if ( RealRigidbody ) {
				RealRigidbody.AddExplosionForce(
					explosion_force, explosion_position, explosion_radius,
					upwards_modifier, mode);
			}
		}

		public void AddForce(Vector3 force) {
			AddForce(force, ForceMode.Force);
		}

		public void AddForce(Vector3 force, ForceMode mode) {
			if ( RealRigidbody ) {
				RealRigidbody.AddForce(force, mode);
			}
		}

		public void AddRelativeForce(Vector3 force) {
			AddRelativeForce(force, ForceMode.Force);
		}
		
		public void AddRelativeForce(Vector3 force, ForceMode mode) {
			if ( RealRigidbody ) {
				RealRigidbody.AddRelativeForce(force, mode);
			}
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position) {
			AddForceAtPosition(force, position, ForceMode.Force);
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode) {
			if ( RealRigidbody ) {
				RealRigidbody.AddForceAtPosition(force, position, mode);
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position) {
			return RealRigidbody
				? RealRigidbody.ClosestPointOnBounds(position)
				: Vector3.zero;
		}

		public bool IsSleeping() {
			return RealRigidbody
				? RealRigidbody.IsSleeping()
				: false;
		}

		public void SetDensity(float density) {
			if ( RealRigidbody ) {
				RealRigidbody.SetDensity(density);
			}
		}

		public void Sleep() {
			if ( RealRigidbody ) {
				RealRigidbody.Sleep();
			}
		}

		public void WakeUp() {
			if ( RealRigidbody ) {
				RealRigidbody.WakeUp();
			}
		}

		public bool SweepTest(Vector3 direction, out IsoRaycastHit iso_hit_info) {
			return SweepTest(direction, out iso_hit_info, Mathf.Infinity);
		}

		public bool SweepTest(Vector3 direction, out IsoRaycastHit iso_hit_info, float max_distance) {
			RaycastHit hit_info;
			var result = RealRigidbody
				? RealRigidbody.SweepTest(direction, out hit_info, max_distance)
				: false;
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		public IsoRaycastHit[] SweepTestAll(Vector3 direction) {
			return SweepTestAll(direction, Mathf.Infinity);
		}

		public IsoRaycastHit[] SweepTestAll(Vector3 direction, float max_distance) {
			return RealRigidbody
				? IsoUtils.IsoConvertRaycastHits(RealRigidbody.SweepTestAll(direction, max_distance))
				: new IsoRaycastHit[0];
		}
		
		void Awake() {
			IsoFakeObject.AddComponent<IsoFakeRigidbody>().Init(this);
			_realRigidbody                        = IsoFakeObject.AddComponent<Rigidbody>();
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
				Destroy(IsoFakeObject.GetComponent<IsoFakeRigidbody>());
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