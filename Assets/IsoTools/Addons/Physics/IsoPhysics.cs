using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	public static class IsoPhysics {

		//
		// Ignore collision helpers
		//

		public static bool GetIgnoreLayerCollision(int layer1, int layer2) {
			return UnityEngine.Physics.GetIgnoreLayerCollision(layer1, layer2);
		}

		public static void IgnoreCollision(IsoCollider iso_collider1, IsoCollider iso_collider2,
			bool ignore = true)
		{
			UnityEngine.Physics.IgnoreCollision(
				iso_collider1.realCollider, iso_collider2.realCollider, ignore);
		}

		public static void IgnoreLayerCollision(int layer1, int layer2,
			bool ignore = true)
		{
			UnityEngine.Physics.IgnoreLayerCollision(layer1, layer2, ignore);
		}

		//
		// Raycast
		//

		public static bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.Raycast(ray, out hit_info,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// RaycastNonAlloc
		//

		public static int RaycastNonAlloc(Ray ray, IsoRaycastHit[] results,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			do {
				var hit_count = UnityEngine.Physics.RaycastNonAlloc(ray, _raycastNonAllocBuffer,
					max_distance,
					layer_mask,
					query_trigger_interaction);
				if ( hit_count >= results.Length || hit_count < _raycastNonAllocBuffer.Length ) {
					return RaycastBufferToIsoRaycastHits(hit_count, results);
				} else {
					_raycastNonAllocBuffer = new RaycastHit[_raycastNonAllocBuffer.Length * 2];
				}
			} while ( true );
		}

		//
		// Linecast
		//

		public static bool Linecast(
			Vector3 start, Vector3 end, out IsoRaycastHit iso_hit_info,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.Linecast(start, end, out hit_info,
				layer_mask,
				query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// CheckBox
		//

		public static bool CheckBox(
			Vector3 center, Vector3 half_extents,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			return UnityEngine.Physics.CheckBox(
				center, half_extents,
				Quaternion.identity,
				layer_mask,
				query_trigger_interaction);
		}

		//
		// CheckCapsule
		//

		public static bool CheckCapsule(
			Vector3 start, Vector3 end, float radius,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			return UnityEngine.Physics.CheckCapsule(
				start, end, radius,
				layer_mask,
				query_trigger_interaction);
		}

		//
		// CheckSphere
		//

		public static bool CheckSphere(
			Vector3 position, float radius,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			return UnityEngine.Physics.CheckSphere(
				position, radius,
				layer_mask,
				query_trigger_interaction);
		}

		//
		// BoxCast
		//

		public static bool BoxCast(
			Vector3 center, Vector3 half_extents, Vector3 direction, out IsoRaycastHit iso_hit_info,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.BoxCast(
				center, half_extents, direction, out hit_info,
				Quaternion.identity,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// BoxCastNonAlloc
		//

		public static int BoxCastNonAlloc(
			Vector3 center, Vector3 half_extents, Vector3 direction, IsoRaycastHit[] results,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_count = UnityEngine.Physics.BoxCastNonAlloc(
				center, half_extents, direction, _raycastNonAllocBuffer,
				Quaternion.identity,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			return RaycastBufferToIsoRaycastHits(hit_count, results);
		}

		//
		// CapsuleCast
		//

		public static bool CapsuleCast(
			Vector3 point1, Vector3 point2, float radius, Vector3 direction, out IsoRaycastHit iso_hit_info,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.CapsuleCast(
				point1, point2, radius, direction, out hit_info,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// CapsuleCastNonAlloc
		//

		public static int CapsuleCastNonAlloc(
			Vector3 point1, Vector3 point2, float radius, Vector3 direction, IsoRaycastHit[] results,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_count = UnityEngine.Physics.CapsuleCastNonAlloc(
				point1, point2, radius, direction, _raycastNonAllocBuffer,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			return RaycastBufferToIsoRaycastHits(hit_count, results);
		}

		//
		// SphereCast
		//

		public static bool SphereCast(
			Vector3 origin, float radius, Vector3 direction, out IsoRaycastHit iso_hit_info,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.SphereCast(
				origin, radius, direction, out hit_info,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// SphereCastNonAlloc
		//

		public static int SphereCastNonAlloc(
			Vector3 origin, float radius, Vector3 direction, IsoRaycastHit[] results,
			float                   max_distance              = Mathf.Infinity,
			int                     layer_mask                = UnityEngine.Physics.DefaultRaycastLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var hit_count = UnityEngine.Physics.SphereCastNonAlloc(
				origin, radius, direction, _raycastNonAllocBuffer,
				max_distance,
				layer_mask,
				query_trigger_interaction);
			return RaycastBufferToIsoRaycastHits(hit_count, results);
		}

		//
		// OverlapBoxNonAlloc
		//

		public static int OverlapBoxNonAlloc(
			Vector3 center, Vector3 half_extents, IsoCollider[] results,
			int                     layer_mask                = UnityEngine.Physics.AllLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			do {
				var collider_count = UnityEngine.Physics.OverlapBoxNonAlloc(
					center, half_extents, _overlapNonAllocBuffer,
					Quaternion.identity,
					layer_mask,
					query_trigger_interaction);
				if ( collider_count >= results.Length || collider_count < _overlapNonAllocBuffer.Length ) {
					return OverlapBufferToIsoColliders(collider_count, results);
				} else {
					_overlapNonAllocBuffer = new Collider[_overlapNonAllocBuffer.Length * 2];
				}
			} while ( true );
		}

		//
		// OverlapCapsuleNonAlloc
		//

		public static int OverlapCapsuleNonAlloc(
			Vector3 point0, Vector3 point1, float radius, IsoCollider[] results,
			int                     layer_mask                = UnityEngine.Physics.AllLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var collider_count = UnityEngine.Physics.OverlapCapsuleNonAlloc(
				point0, point1, radius, _overlapNonAllocBuffer,
				layer_mask,
				query_trigger_interaction);
			return OverlapBufferToIsoColliders(collider_count, results);
		}

		//
		// OverlapSphereNonAlloc
		//

		public static int OverlapSphereNonAlloc(
			Vector3 position, float radius, IsoCollider[] results,
			int                     layer_mask                = UnityEngine.Physics.AllLayers,
			QueryTriggerInteraction query_trigger_interaction = QueryTriggerInteraction.UseGlobal)
		{
			var collider_count = UnityEngine.Physics.OverlapSphereNonAlloc(
				position, radius, _overlapNonAllocBuffer,
				layer_mask,
				query_trigger_interaction);
			return OverlapBufferToIsoColliders(collider_count, results);
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		static RaycastHit[] _raycastNonAllocBuffer = new RaycastHit[128];
		static int RaycastBufferToIsoRaycastHits(int hit_count, IsoRaycastHit[] results) {
			var min_hit_count = Mathf.Min(hit_count, results.Length);
			for ( var i = 0; i < min_hit_count; ++i ) {
				results[i] = new IsoRaycastHit(_raycastNonAllocBuffer[i]);
			}
			System.Array.Clear(_raycastNonAllocBuffer, 0, hit_count);
			return min_hit_count;
		}

		static Collider[] _overlapNonAllocBuffer = new Collider[128];
		static int OverlapBufferToIsoColliders(int collider_count, IsoCollider[] results) {
			var min_collider_count = Mathf.Min(collider_count, results.Length);
			for ( var i = 0; i < min_collider_count; ++i ) {
				results[i] = IsoPhysicsUtils.IsoConvertCollider(_overlapNonAllocBuffer[i]);
			}
			System.Array.Clear(_overlapNonAllocBuffer, 0, collider_count);
			return min_collider_count;
		}
	}
}