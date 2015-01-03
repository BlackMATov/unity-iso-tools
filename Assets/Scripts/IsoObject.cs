using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class IsoObject : MonoBehaviour {

	[SerializeField]
	private Vector3 _position     = Vector3.zero;
	private Vector3 _lastPosition = Vector3.zero;
	public  Vector3 Position {
		get { return _position; }
		set {
			if ( Alignment ) {
				_position.Set(Mathf.Round(value.x), Mathf.Round(value.y), value.z);
			} else {
				_position = value;
			}
			_lastPosition = _position;
			FixTransform();
		}
	}
	
	public Vector3 Size      = Vector3.one;
	public bool    Alignment = true;

	void Start() {
	}

	void Update() {
		if ( _lastPosition != _position ) {
			Position = _position;
		}
		if ( Selection.Contains(gameObject) ) {
			FixIsoPosition();
		} else {
			FixTransform();
		}
	}

	void FixTransform() {
		var iso_world = GameObject.FindObjectOfType<IsoWorld>();
		if ( !iso_world ) {
			return;
		}
		var pos = iso_world.IsoToScreen(Position);
		var depth = gameObject.transform.position.z;
		gameObject.transform.position = new Vector3(pos.x, pos.y, depth);
	}

	void FixIsoPosition() {
		var iso_world = GameObject.FindObjectOfType<IsoWorld>();
		if ( !iso_world ) {
			return;
		}
		var pos = gameObject.transform.position;
		Position = iso_world.ScreenToIso(new Vector2(pos.x, pos.y), Position.z);
		EditorUtility.SetDirty(this);
	}
}