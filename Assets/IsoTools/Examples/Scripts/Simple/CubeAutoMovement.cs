using UnityEngine;
using System.Collections;

namespace IsoTools.Examples.Simple {
	[RequireComponent(typeof(IsoObject))]
	public class CubeAutoMovement : MonoBehaviour {
		public float stepTicks    = 0.5f;
		public float stepRndTicks = 0.5f;

		IsoObject _isoObject = null;

		void Start() {
			_isoObject = GetComponent<IsoObject>();
			if ( !_isoObject ) {
				throw new UnityException("CubeAutoMovement. IsoObject component not found!");
			}
			StartCoroutine(Move());
		}

		WaitForSeconds RndWait() {
			return new WaitForSeconds(stepTicks + Random.Range(0.0f, stepRndTicks));
		}

		IEnumerator Move() {
			while ( true ) {
				yield return RndWait();
				_isoObject.position += new Vector3(1, 0, 0);
				yield return RndWait();
				_isoObject.position += new Vector3(0, 1, 0);
				yield return RndWait();
				_isoObject.position += new Vector3(-1, 0, 0);
				yield return RndWait();
				_isoObject.position += new Vector3(0, -1, 0);
			}
		}
	}
}