using UnityEngine;
using System.Collections;

public class CircleFlyCamera : MonoBehaviour {

	public float FlyRadius = 150.0f;
	public float FlySpeed  = 1.0f;

	Vector3 _start_pos;
	float   _fly_timer;

	void Start() {
		_start_pos = transform.position;
		_fly_timer = 0.0f;
	}

	void Update () {
		_fly_timer += FlySpeed * Time.deltaTime;
		transform.position = new Vector3(
			_start_pos.x + Mathf.Cos(_fly_timer) * FlyRadius,
			_start_pos.y + Mathf.Sin(_fly_timer) * FlyRadius,
			_start_pos.z);
	}
}
