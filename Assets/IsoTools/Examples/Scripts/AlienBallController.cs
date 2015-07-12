using UnityEngine;
using System.Collections;

namespace IsoTools.Examples {
	[RequireComponent(typeof(IsoRigidbody))]
	public class AlienBallController : MonoBehaviour {
		
		IsoRigidbody _isoRigidbody = null;
		
		void Start() {
			_isoRigidbody = GetComponent<IsoRigidbody>();
			if ( !_isoRigidbody ) {
				throw new UnityException("AlienBallController. IsoRigidbody component not found!");
			}
			StartCoroutine("AddRndForce");
		}

		IEnumerator AddRndForce() {
			while ( true ) {
				var dx = Random.Range(0.0f, 2.0f);
				var dy = Random.Range(0.0f, 2.0f);
				_isoRigidbody.AddForce(new Vector3(dx, dy, 0.0f), ForceMode.Impulse);
				yield return new WaitForSeconds(1);
			}
		}
	}
} // namespace IsoTools.Examples