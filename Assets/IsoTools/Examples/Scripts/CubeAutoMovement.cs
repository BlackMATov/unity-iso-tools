using UnityEngine;
using System.Collections;

namespace IsoTools.Examples {
	public class CubeAutoMovement : MonoBehaviour {
		public float stepTicks    = 0.5f;
		public float stepRndTicks = 0.5f;

		void Start() {
			StartCoroutine("Move");
		}

		WaitForSeconds RndWait() {
			return new WaitForSeconds(stepTicks + Random.Range(0.0f, stepRndTicks));
		}

		IEnumerator Move() {
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object ) {
				for (;;) {
					yield return RndWait();
					iso_object.position += new Vector3(1, 0, 0);
					yield return RndWait();
					iso_object.position += new Vector3(0, 1, 0);
					yield return RndWait();
					iso_object.position += new Vector3(-1, 0, 0);
					yield return RndWait();
					iso_object.position += new Vector3(0, -1, 0);
				}
			}
		}
	}
} // namespace IsoTools.Examples