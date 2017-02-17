using UnityEngine;
using IsoTools.Physics.Internal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Physics {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoRigidbody : IsoPhysicsHelperHolder {

		IsoFakeRigidbody _fakeRigidbody;

		Rigidbody _realRigidbody = null;
		Rigidbody realRigidbody {
			get { return _realRigidbody; }
		}

		[SerializeField]
		public float _mass = 1.0f;
		public float mass {
			get { return _mass; }
			set {
				_mass = value;
				if ( realRigidbody ) {
					realRigidbody.mass = value;
				}
			}
		}

		[SerializeField]
		public float _drag = 0.0f;
		public float drag {
			get { return _drag; }
			set {
				_drag = value;
				if ( realRigidbody ) {
					realRigidbody.drag = value;
				}
			}
		}

		[SerializeField]
		public bool _useGravity = true;
		public bool useGravity {
			get { return _useGravity; }
			set {
				_useGravity = value;
				if ( realRigidbody ) {
					realRigidbody.useGravity = value;
				}
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

		public Vector3 inertiaTensor {
			get { return realRigidbody ? realRigidbody.inertiaTensor : Vector3.zero; }
			set {
				if ( realRigidbody ) {
					realRigidbody.inertiaTensor = value;
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

		public int solverIterations {
			get { return realRigidbody ? realRigidbody.solverIterations : 0; }
			set {
				if ( realRigidbody ) {
					realRigidbody.solverIterations = value;
				}
			}
		}

		public int solverVelocityIterations {
			get { return realRigidbody ? realRigidbody.solverVelocityIterations : 0; }
			set {
				if ( realRigidbody ) {
					realRigidbody.solverVelocityIterations = value;
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
			float explosion_force, Vector3 explosion_position, float explosion_radius,
			float     upwards_modifier = 0.0f,
			ForceMode mode             = ForceMode.Force)
		{
			if ( realRigidbody ) {
				realRigidbody.AddExplosionForce(
					explosion_force, explosion_position, explosion_radius,
					upwards_modifier, mode);
			}
		}

		public void AddForce(Vector3 force,
			ForceMode mode = ForceMode.Force)
		{
			if ( realRigidbody ) {
				realRigidbody.AddForce(force, mode);
			}
		}

		public void AddForceAtPosition(Vector3 force, Vector3 position,
			ForceMode mode = ForceMode.Force)
		{
			if ( realRigidbody ) {
				realRigidbody.AddForceAtPosition(force, position, mode);
			}
		}
		
		public void AddRelativeForce(Vector3 force,
			ForceMode mode = ForceMode.Force)
		{
			if ( realRigidbody ) {
				realRigidbody.AddRelativeForce(force, mode);
			}
		}

		public Vector3 ClosestPointOnBounds(Vector3 position) {
			return realRigidbody
				? realRigidbody.ClosestPointOnBounds(position)
				: Vector3.zero;
		}

		public Vector3 GetPointVelocity(Vector3 world_point) {
			return realRigidbody
				? realRigidbody.GetPointVelocity(world_point)
				: Vector3.zero;
		}

		public Vector3 GetRelativePointVelocity(Vector3 relative_point) {
			return realRigidbody
				? realRigidbody.GetRelativePointVelocity(relative_point)
				: Vector3.zero;
		}

		public bool IsSleeping() {
			return realRigidbody
				? realRigidbody.IsSleeping()
				: false;
		}

		public void MovePosition(Vector3 position) {
			if ( realRigidbody ) {
				realRigidbody.MovePosition(position);
			}
		}

		public void ResetCenterOfMass() {
			if ( realRigidbody ) {
				realRigidbody.ResetCenterOfMass();
			}
		}

		public void ResetInertiaTensor() {
			if ( realRigidbody ) {
				realRigidbody.ResetInertiaTensor();
			}
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

		public bool SweepTest(Vector3 direction, out IsoRaycastHit iso_hit_info,
			float                   max_distance              = Mathf.Infinity,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = realRigidbody
				? realRigidbody.SweepTest(direction, out hit_info, max_distance, query_trigger_interaction)
				: false;
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		public IsoRaycastHit[] SweepTestAll(Vector3 direction,
			float                   max_distance              = Mathf.Infinity,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			return realRigidbody
				? IsoPhysicsUtils.IsoConvertRaycastHits(
					realRigidbody.SweepTestAll(direction, max_distance, query_trigger_interaction))
				: new IsoRaycastHit[0];
		}

		public void WakeUp() {
			if ( realRigidbody ) {
				realRigidbody.WakeUp();
			}
		}
		
		void Awake() {
			_fakeRigidbody                        = fakeObject.AddComponent<IsoFakeRigidbody>().Init(this);
			_realRigidbody                        = fakeObject.AddComponent<Rigidbody>();
			_realRigidbody.freezeRotation         = true;
			_realRigidbody.mass                   = mass;
			_realRigidbody.drag                   = drag;
			_realRigidbody.useGravity             = useGravity;
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
				Destroy(_realRigidbody);
			}
			if ( _fakeRigidbody ) {
				Destroy(_fakeRigidbody);
			}
			DestroyUnnecessaryCheck();
		}

	#if UNITY_EDITOR
		void Reset() {
			mass                   = 1.0f;
			drag                   = 0.0f;
			useGravity             = true;
			isKinematic            = false;
			interpolation          = RigidbodyInterpolation.None;
			collisionDetectionMode = CollisionDetectionMode.Discrete;
			EditorUtility.SetDirty(this);
		}
		
		void OnValidate() {
			if ( realRigidbody ) {
				realRigidbody.mass                   = mass;
				realRigidbody.drag                   = drag;
				realRigidbody.useGravity             = useGravity;
				realRigidbody.isKinematic            = isKinematic;
				realRigidbody.interpolation          = interpolation;
				realRigidbody.collisionDetectionMode = collisionDetectionMode;
			}
		}
	#endif
	}
}