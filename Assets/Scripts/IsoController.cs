using UnityEngine;
using System.Collections;

public class IsoController : MonoBehaviour {
	void Update () {
		var iso_object = gameObject.GetComponent<IsoObject>();
		if ( iso_object ) {
			if ( Input.GetKeyUp(KeyCode.LeftArrow) ) {
				iso_object.Position += new Vector3(-1, 0, 0);
			}
			if ( Input.GetKeyUp(KeyCode.RightArrow) ) {
				iso_object.Position += new Vector3(1, 0, 0);
			}
			if ( Input.GetKeyUp(KeyCode.DownArrow) ) {
				iso_object.Position += new Vector3(0, -1, 0);
			}
			if ( Input.GetKeyUp(KeyCode.UpArrow) ) {
				iso_object.Position += new Vector3(0, 1, 0);
			}
			if ( Input.GetKeyUp(KeyCode.A) ) {
				iso_object.Position += new Vector3(0, 0, 1);
			}
			if ( Input.GetKeyUp(KeyCode.Z) ) {
				iso_object.Position += new Vector3(0, 0, -1);
			}
		}
	}
}