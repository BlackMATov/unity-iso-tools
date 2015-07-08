using UnityEngine;
using System.Collections;

namespace IsoTools { namespace Examples {
	public class IsoController : MonoBehaviour {
		void MoveIsoObject(Vector3 dir) {
			var iso_object = GetComponent<IsoObject>();
			var iso_rigidbody = GetComponent<IsoRigidbody>();
			if ( iso_rigidbody ) {
				iso_rigidbody.Velocity = dir;
			} else if ( iso_object) {
				iso_object.Position += dir * Time.deltaTime;
			}
		}
		void Update () {
			if ( Input.GetKey(KeyCode.LeftArrow) ) {
				MoveIsoObject(new Vector3(-1, 0, 0));
			}
			if ( Input.GetKey(KeyCode.RightArrow) ) {
				MoveIsoObject(new Vector3(1, 0, 0));
			}
			if ( Input.GetKey(KeyCode.DownArrow) ) {
				MoveIsoObject(new Vector3(0, -1, 0));
			}
			if ( Input.GetKey(KeyCode.UpArrow) ) {
				MoveIsoObject(new Vector3(0, 1, 0));
			}
			if ( Input.GetKey(KeyCode.A) ) {
				MoveIsoObject(new Vector3(0, 0, 1));
			}
			if ( Input.GetKey(KeyCode.Z) ) {
				MoveIsoObject(new Vector3(0, 0, -1));
			}
		}
	}
}} // namespace IsoTools::Examples