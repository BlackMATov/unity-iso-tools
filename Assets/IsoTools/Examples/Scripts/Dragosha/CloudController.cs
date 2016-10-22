using UnityEngine;

namespace IsoTools.Examples.Dragosha {
	[RequireComponent(typeof(SpriteRenderer))]
	public class CloudController : MonoBehaviour {

		public float minY     = 8.0f;
		public float maxY     = 12.0f;

		public float minSpeed = 0.3f;
		public float maxSpeed = 1.0f;

		float          _speed       = 0.0f;
		SpriteRenderer _sprRenderer = null;

		void Start() {
			_sprRenderer = GetComponent<SpriteRenderer>();
			if ( !_sprRenderer ) {
				throw new UnityException("CloudController. SpriteRenderer component not found!");
			}
			Rewind(false);
		}

		void Update() {
			var pos = transform.position;
			pos.x += _speed * Time.deltaTime;

			var screen_pnt = Camera.main.WorldToScreenPoint(
				new Vector3(pos.x - _sprRenderer.bounds.size.x, pos.y, pos.z));
			if ( screen_pnt.x > Screen.width ) {
				Rewind(true);
			} else {
				transform.position = pos;
			}
		}

		void Rewind(bool reset_position) {
			_speed = Random.Range(minSpeed, maxSpeed);
			if ( reset_position ) {
				transform.position = new Vector3(
					-_sprRenderer.bounds.size.x,
					Random.Range(minY, maxY),
					transform.position.z);
			}
		}
	}
}