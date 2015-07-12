using UnityEngine;

namespace IsoTools.Examples {
	[RequireComponent(typeof(IsoRigidbody))]
	public class PlayerController : MonoBehaviour {

		public float speed = 2.0f;

		IsoRigidbody _isoRigidbody = null;

		void OnIsoCollisionEnter(IsoCollision iso_collision) {
			if ( iso_collision.gameObject ) {
				var alient = iso_collision.gameObject.GetComponent<AlienBallController>();
				if ( alient ) {
					Destroy(alient.gameObject);
				}
			}
		}

		void Start() {
			_isoRigidbody = GetComponent<IsoRigidbody>();
			if ( !_isoRigidbody ) {
				throw new UnityException("PlayerController. IsoRigidbody component not found!");
			}
		}

		void Update () {
			if ( Input.GetKey(KeyCode.LeftArrow) ) {
				_isoRigidbody.velocity = IsoUtils.Vec3ChangeX(_isoRigidbody.velocity, -speed);
			}
			else if ( Input.GetKey(KeyCode.RightArrow) ) {
				_isoRigidbody.velocity = IsoUtils.Vec3ChangeX(_isoRigidbody.velocity,  speed);
			}
			else if ( Input.GetKey(KeyCode.DownArrow) ) {
				_isoRigidbody.velocity = IsoUtils.Vec3ChangeY(_isoRigidbody.velocity, -speed);
			}
			else if ( Input.GetKey(KeyCode.UpArrow) ) {
				_isoRigidbody.velocity = IsoUtils.Vec3ChangeY(_isoRigidbody.velocity,  speed);
			}
		}
	}
} // namespace IsoTools.Examples