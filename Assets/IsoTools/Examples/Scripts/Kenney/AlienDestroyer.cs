using UnityEngine;
using System.Collections;

namespace IsoTools.Examples.Kenney {
	public class AlienDestroyer : MonoBehaviour {
		void Update () {
			var iso_world = IsoWorld.Instance;
			if ( iso_world && Input.GetMouseButtonDown(0) ) {
				var iso_mouse_pos       = iso_world.MouseIsoPosition();
				var ray_from_iso_camera = iso_world.RayFromIsoCameraToIsoPoint(iso_mouse_pos);
				var hits                = iso_world.RaycastAll(ray_from_iso_camera);
				for ( var i = 0; i < hits.Length; ++i ) {
					var alien_go = hits[i].collider.gameObject;
					if ( alien_go.GetComponent<AlienBallController>() ) {
						Destroy(alien_go);
					}
				}
			}
		}
	}
}