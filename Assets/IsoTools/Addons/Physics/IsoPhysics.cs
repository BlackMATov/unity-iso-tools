using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	public static class IsoPhysics {

		//
		// Raycast
		//

		public static bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info) {
			return Raycast(ray, out iso_hit_info,
				Mathf.Infinity, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info,
			float max_distance)
		{
			return Raycast(ray, out iso_hit_info,
				max_distance, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info,
			float max_distance, int layer_mask)
		{
			return Raycast(ray, out iso_hit_info,
				max_distance, layer_mask,
				QueryTriggerInteraction.UseGlobal);
		}

		public static bool Raycast(Ray ray, out IsoRaycastHit iso_hit_info,
			float max_distance, int layer_mask,
			QueryTriggerInteraction query_trigger_interaction)
		{
			var hit_info = new RaycastHit();
			var result = UnityEngine.Physics.Raycast(ray, out hit_info,
				max_distance, layer_mask, query_trigger_interaction);
			iso_hit_info = result ? new IsoRaycastHit(hit_info) : new IsoRaycastHit();
			return result;
		}

		//
		// RaycastAll
		//

		public static IsoRaycastHit[] RaycastAll(Ray ray) {
			return RaycastAll(ray,
				Mathf.Infinity, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static IsoRaycastHit[] RaycastAll(Ray ray,
			float max_distance)
		{
			return RaycastAll(ray,
				max_distance, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static IsoRaycastHit[] RaycastAll(Ray ray,
			float max_distance, int layer_mask)
		{
			return RaycastAll(ray,
				max_distance, layer_mask,
				QueryTriggerInteraction.UseGlobal);
		}

		public static IsoRaycastHit[] RaycastAll(Ray ray,
			float max_distance, int layer_mask,
			QueryTriggerInteraction query_trigger_interaction)
		{
			var hits_info = UnityEngine.Physics.RaycastAll(ray,
				max_distance, layer_mask, query_trigger_interaction);
			return IsoPhysicsUtils.IsoConvertRaycastHits(hits_info);
		}

		//
		// RaycastNonAlloc
		//

		public static int RaycastNonAlloc(Ray ray, IsoRaycastHit[] results) {
			return RaycastNonAlloc(ray, results,
				Mathf.Infinity, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static int RaycastNonAlloc(Ray ray, IsoRaycastHit[] results,
			float max_distance)
		{
			return RaycastNonAlloc(ray, results,
				max_distance, UnityEngine.Physics.DefaultRaycastLayers,
				QueryTriggerInteraction.UseGlobal);
		}

		public static int RaycastNonAlloc(Ray ray, IsoRaycastHit[] results,
			float max_distance, int layer_mask)
		{
			return RaycastNonAlloc(ray, results,
				max_distance, layer_mask,
				QueryTriggerInteraction.UseGlobal);
		}

		static RaycastHit[] _raycastNonAllocBuffer = new RaycastHit[128];
		public static int RaycastNonAlloc(Ray ray, IsoRaycastHit[] results,
			float max_distance, int layer_mask,
			QueryTriggerInteraction query_trigger_interaction)
		{
			var hit_count = UnityEngine.Physics.RaycastNonAlloc(ray, _raycastNonAllocBuffer,
				max_distance, layer_mask, query_trigger_interaction);
			var min_hit_count = Mathf.Min(hit_count, results.Length);
			for ( var i = 0; i < min_hit_count; ++i ) {
				results[i] = new IsoRaycastHit(_raycastNonAllocBuffer[i]);
			}
			System.Array.Clear(_raycastNonAllocBuffer, 0, _raycastNonAllocBuffer.Length);
			return min_hit_count;
		}
	}
}