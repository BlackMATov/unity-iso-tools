using UnityEngine;
using System.Collections;

namespace IsoTools.Examples.Kenney {
	public class AlienBallSpawner : MonoBehaviour {

		public int        maxAlienCount   = 10;
		public GameObject alienBallPrefab = null;
		
		void Start() {
			if ( !alienBallPrefab ) {
				throw new UnityException("AlienBallSpawner. Alien ball prefab not found!");
			}
			var iso_world = IsoWorld.GetWorld(0);
			StartCoroutine(SpawnAlienBall(iso_world));
		}
		
		IEnumerator SpawnAlienBall(IsoWorld iso_world) {
			while ( true ) {
				var aliens = GameObject.FindObjectsOfType<AlienBallController>();
				if ( aliens.Length < maxAlienCount ) {
					var dx = Random.Range(2.0f, 3.0f);
					var dy = Random.Range(2.0f, 3.0f);
					var alien_ball_go = Instantiate(alienBallPrefab, iso_world.transform);
					var alien_iso_obj = alien_ball_go.GetComponent<IsoObject>();
					alien_iso_obj.position = new Vector3(dx, dy, 5.0f);
				}
				yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
			}
		}
	}
}