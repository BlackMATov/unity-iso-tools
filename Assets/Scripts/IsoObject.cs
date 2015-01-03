using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class IsoObject : MonoBehaviour {

	IsoWorld _iso_world     = null;
	Vector3  _lastPosition  = Vector3.zero;
	Vector3  _lastTransform = Vector3.zero;

	[SerializeField]
	Vector3 _position = Vector3.zero;
	public Vector3 Position {
		get { return _position; }
		set {
			if ( Alignment ) {
				_position.Set(Mathf.Round(value.x), Mathf.Round(value.y), Mathf.Round(value.z));
			} else {
				_position = value;
			}
			FixTransform();
			GetIsoWorld().MarkDirty();
			_lastPosition = _position;
		}
	}
	
	public Vector3 Size      = Vector3.one;
	public bool    Alignment = true;

	void Start() {
		GetIsoWorld().MarkDirty();
	}

	void Update() {
		if ( _lastPosition != _position ) {
			Position = _position;
		}
		if ( _lastTransform != gameObject.transform.position ) {
			FixIsoPosition();
		}
		/*
		if ( Selection.Contains(gameObject) ) {
			FixIsoPosition();
		} else {
			FixTransform();
		}*/
	}

	void OnRenderObject() {
		Update();
	}

	IsoWorld GetIsoWorld() {
		if ( !_iso_world ) {
			_iso_world = GameObject.FindObjectOfType<IsoWorld>();
		}
		if ( !_iso_world ) {
			throw new UnityException("IsoObject. IsoWorld not found!");
		}
		return _iso_world;
	}

	void FixTransform() {
		var pos = GetIsoWorld().IsoToScreen(Position);
		var depth = gameObject.transform.position.z;
		var trans = new Vector3(pos.x, pos.y, depth);
		gameObject.transform.position = trans;
		_lastPosition = trans;
	}

	void FixIsoPosition() {
		var trans = gameObject.transform.position;
		Position = GetIsoWorld().ScreenToIso(new Vector2(trans.x, trans.y), Position.z);
		_lastTransform = trans;
		if ( Application.isEditor ) {
			EditorUtility.SetDirty(this);
		}
	}
}