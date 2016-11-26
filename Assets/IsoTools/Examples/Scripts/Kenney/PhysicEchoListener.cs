﻿using UnityEngine;
using IsoTools.Physics;
using System.Collections;

namespace IsoTools.Examples.Kenney {
	[RequireComponent(typeof(IsoTriggerListener), typeof(IsoCollisionListener))]
	public class PhysicEchoListener : MonoBehaviour {
		void OnIsoTriggerEnter(IsoCollider iso_collider) {
			Debug.LogFormat(
				"OnIsoTriggerEnter. self:{0} other:{1}",
				gameObject.name, iso_collider.gameObject.name);
		}
		
		void OnIsoTriggerExit(IsoCollider iso_collider) {
			Debug.LogFormat(
				"OnIsoTriggerExit. self:{0} other:{1}",
				gameObject.name, iso_collider.gameObject.name);
		}

		void OnIsoCollisionEnter(IsoCollision iso_collision) {
			Debug.LogFormat(
				"OnIsoCollisionEnter. self:{0} other:{1}",
				gameObject.name, iso_collision.gameObject.name);
		}

		void OnIsoCollisionExit(IsoCollision iso_collision) {
			Debug.LogFormat(
				"OnIsoCollisionExit. self:{0} other:{1}",
				gameObject.name, iso_collision.gameObject.name);
		}
	}
}