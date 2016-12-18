using UnityEngine;
using IsoTools.Physics;
using System.Collections;

namespace IsoTools.Examples.Kenney {
	public class AlienDestroyer : MonoBehaviour {
		static IsoRaycastHit[] _raycastBuffer = new IsoRaycastHit[16];

		void Update () {
			var iso_world = IsoWorld.GetWorld(0);
			if ( iso_world && Input.GetMouseButtonDown(0) ) {
				var iso_mouse_pos       = iso_world.MouseIsoPosition();
				var ray_from_iso_camera = iso_world.RayFromIsoCameraToIsoPoint(iso_mouse_pos);
				var hit_count           = IsoPhysics.RaycastNonAlloc(ray_from_iso_camera, _raycastBuffer);
				for ( var i = 0; i < hit_count; ++i ) {
					var alien_go = _raycastBuffer[i].collider.gameObject;
					if ( alien_go.GetComponent<AlienBallController>() ) {
						Destroy(alien_go);
					}
				}
			}
		}
	}
}