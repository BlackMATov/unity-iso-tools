using UnityEngine;
using System.Collections;

namespace IsoTools.Examples.Simple {
	[RequireComponent(typeof(IsoObject))]
	public class CubeAutoMovement : MonoBehaviour {
		public float stepTicks    = 0.5f;
		public float stepRndTicks = 0.5f;

		Vector3   _startPos     = Vector3.zero;
		IsoObject _isoObject    = null;
		Coroutine _movementCoro = null;

		void OnEnable() {
			_isoObject = GetComponent<IsoObject>();
			if ( !_isoObject ) {
				throw new UnityException("CubeAutoMovement. IsoObject component not found!");
			}
			_startPos     = _isoObject.position;
			_movementCoro = StartCoroutine(Move());
		}

		void OnDisable() {
			StopCoroutine(_movementCoro);
			_isoObject.position = _startPos;
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