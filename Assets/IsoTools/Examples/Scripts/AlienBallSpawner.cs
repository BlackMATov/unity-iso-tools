using UnityEngine;
using System.Collections;

namespace IsoTools.Examples {
	public class AlienBallSpawner : MonoBehaviour {

		public int        maxAlienCount   = 10;
		public GameObject alienBallPrefab = null;
		
		void Start() {
			StartCoroutine("SpawnAlienBall");
		}
		
		IEnumerator SpawnAlienBall() {
			while ( true ) {
				var aliens = GameObject.FindObjectsOfType<AlienBallController>();
				if ( aliens.Length < maxAlienCount ) {
					var dx = Random.Range(1.0f, 4.0f);
					var dy = Random.Range(1.0f, 4.0f);
					var alien_ball_go = Instantiate(alienBallPrefab);
					var alien_iso_obj = alien_ball_go.GetComponent<IsoObject>();
					alien_iso_obj.position = new Vector3(dx, dy, 5.0f);
				}
				yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
			}
		}
	}
} // namespace IsoTools.Examples