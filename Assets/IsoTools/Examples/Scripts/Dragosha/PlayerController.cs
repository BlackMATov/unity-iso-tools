using UnityEngine;
using IsoTools.Physics;

namespace IsoTools.Examples.Dragosha {
	[RequireComponent(typeof(IsoRigidbody))]
	public class PlayerController : MonoBehaviour {

		public float          speed  = 1.0f;
		public SpriteRenderer sprite = null;

		IsoObject    _isoObject    = null;
		IsoRigidbody _isoRigidbody = null;

		void Start() {
			if ( !sprite ) {
				throw new UnityException("PlayerController. Sprite not found!");
			}
			_isoObject = GetComponent<IsoObject>();
			if ( !_isoObject ) {
				throw new UnityException("PlayerController. IsoObject component not found!");
			}
			_isoRigidbody = GetComponent<IsoRigidbody>();
			if ( !_isoRigidbody ) {
				throw new UnityException("PlayerController. IsoRigidbody component not found!");
			}
		}

		void Update () {
			if ( Input.GetKey(KeyCode.LeftArrow) ) {
				var velocity = _isoRigidbody.velocity;
				velocity.x = -speed;
				sprite.flipX = true;
				_isoRigidbody.velocity = velocity;
			}
			if ( Input.GetKey(KeyCode.RightArrow) ) {
				var velocity = _isoRigidbody.velocity;
				velocity.x = speed;
				sprite.flipX = false;
				_isoRigidbody.velocity = velocity;
			}
			if ( Input.GetKey(KeyCode.DownArrow) ) {
				var velocity = _isoRigidbody.velocity;
				velocity.y = -speed;
				_isoRigidbody.velocity = velocity;
			}
			if ( Input.GetKey(KeyCode.UpArrow) ) {
				var velocity = _isoRigidbody.velocity;
				velocity.y = speed;
				_isoRigidbody.velocity = velocity;
			}
		}
	}
}