using UnityEngine;
using System.Collections;

namespace IsoTools.Examples {
	[RequireComponent(typeof(IsoRigidbody))]
	public class AlienBallController : MonoBehaviour {

		IsoObject    _isoObject    = null;
		IsoRigidbody _isoRigidbody = null;
		
		void Start() {
			_isoObject = GetComponent<IsoObject>();
			if ( !_isoObject ) {
				throw new UnityException("AlienBallController. IsoObject component not found!");
			}
			_isoRigidbody = GetComponent<IsoRigidbody>();
			if ( !_isoRigidbody ) {
				throw new UnityException("AlienBallController. IsoRigidbody component not found!");
			}
			StartCoroutine("AddRndForce");
		}

		void Update() {
			if ( _isoObject.positionZ < 0.0f ) {
				Destroy(gameObject);
			}
		}

		IEnumerator AddRndForce() {
			while ( true ) {
				var dx = Random.Range(-2.0f, 2.0f);
				var dy = Random.Range(-2.0f, 2.0f);
				_isoRigidbody.AddForce(new Vector3(dx, dy, 0.0f), ForceMode.Impulse);
				yield return new WaitForSeconds(1);
			}
		}
	}
} // namespace IsoTools.Examples