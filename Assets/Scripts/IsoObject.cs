using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class IsoObject : MonoBehaviour {
	
	Transform _transform     = null;
	Vector3   _lastPosition  = Vector3.zero;
	Vector3   _lastTransform = Vector3.zero;

	[SerializeField]
	Vector3 _position = Vector3.zero;
	public Vector3 Position {
		get { return _position; }
		set {
			_position = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			IsoWorld.MarkDirty();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	[SerializeField]
	Vector3 _size = Vector3.one;
	public Vector3 Size {
		get { return _size; }
		set {
			_size = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			IsoWorld.MarkDirty();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	[SerializeField]
	bool _alignment = true;	
	public bool Alignment {
		get { return _alignment; }
		set {
			_alignment = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			IsoWorld.MarkDirty();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	IsoWorld _iso_world = null;
	public IsoWorld IsoWorld {
		get {
			if ( !_iso_world ) {
				_iso_world = GameObject.FindObjectOfType<IsoWorld>();
			}
			if ( !_iso_world ) {
				throw new UnityException("IsoObject. IsoWorld not found!");
			}
			return _iso_world;
		}
	}

	public void FixAlignment() {
		_position.Set(
			Mathf.Round(_position.x),
			Mathf.Round(_position.y),
			Mathf.Round(_position.z));
		FixTransform();
		IsoWorld.MarkDirty();
		if ( Application.isEditor ) {
			EditorUtility.SetDirty(this);
		}
	}

	public void FixTransform() {
		Vector3 trans = IsoWorld.IsoToScreen(Position);
		trans.z = _transform.position.z;
		_transform.position = trans;
		_lastPosition = Position;
		_lastTransform = trans;
	}

	public void FixIsoPosition() {
		Vector2 trans = _transform.position;
		Position = IsoWorld.ScreenToIso(trans, Position.z);
		FixTransform();
	}

	void Start() {
		_transform = gameObject.transform;
		_lastPosition = Position;
		_lastTransform = _transform.position;
		IsoWorld.MarkDirty();
	}
	
	void Update() {
		if ( _lastPosition != _position ) {
			Position = _position;
		}
		if ( _lastTransform != _transform.position ) {
			FixIsoPosition();
		}
	}

	void OnEnable() {
		IsoWorld.MarkDirty();
	}

	void OnDisable() {
		IsoWorld.MarkDirty();
	}
}