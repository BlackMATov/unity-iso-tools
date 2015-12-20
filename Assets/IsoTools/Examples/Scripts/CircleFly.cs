using UnityEngine;
using System.Collections;

namespace IsoTools.Examples {
	public class CircleFly : MonoBehaviour {
		public float flyRadius = 150.0f;
		public float flySpeed  = 1.0f;

		Vector3 _start_pos;
		float   _fly_timer;

		void Start() {
			_start_pos = transform.position;
			_fly_timer = 0.0f;
		}

		void Update () {
			_fly_timer += flySpeed * Time.deltaTime;
			transform.position = new Vector3(
				_start_pos.x + Mathf.Cos(_fly_timer) * flyRadius,
				_start_pos.y + Mathf.Sin(_fly_timer) * flyRadius,
				_start_pos.z);
		}
	}
} // namespace IsoTools.Examples