using UnityEngine;
using UnityEditor;

namespace IsoTools {
[ExecuteInEditMode]
public class IsoObject : MonoBehaviour {
	
	Transform _transform     = null;
	Vector3   _lastPosition  = Vector3.zero;
	Vector3   _lastTransform = Vector3.zero;

	[SerializeField]
	Vector3 _position = Vector3.zero;
	/// <summary>Isometric object position.</summary>
	public Vector3 Position {
		get { return _position; }
		set {
			_position = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			MartDirtyIsoWorld();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	[SerializeField]
	Vector3 _size = Vector3.one;
	/// <summary>Isometric object size.</summary>
	public Vector3 Size {
		get { return _size; }
		set {
			_size = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			MartDirtyIsoWorld();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	[SerializeField]
	bool _alignment = true;
	/// <summary>Auto alignment position by isometric tile size.</summary>
	public bool Alignment {
		get { return _alignment; }
		set {
			_alignment = value;
			if ( Alignment ) {
				FixAlignment();
			} else {
				FixTransform();
			}
			MartDirtyIsoWorld();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}
	
	[SerializeField]
	bool _sorting = true;
	/// <summary>Auto sorting tile.</summary>
	public bool Sorting {
		get { return _sorting; }
		set {
			_sorting = value;
			MartDirtyIsoWorld();
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		}
	}

	IsoWorld _iso_world = null;
	public IsoWorld GetIsoWorld() {
		if ( !_iso_world ) {
			_iso_world = GameObject.FindObjectOfType<IsoWorld>();
		}
		if ( !_iso_world ) {
			throw new UnityException("IsoObject. IsoWorld not found!");
		}
		return _iso_world;
	}

	public void ResetIsoWorld() {
		_iso_world = null;
	}

	public void FixAlignment() {
		_position.Set(
			Mathf.Round(_position.x),
			Mathf.Round(_position.y),
			Mathf.Round(_position.z));
		FixTransform();
		MartDirtyIsoWorld();
		if ( Application.isEditor ) {
			EditorUtility.SetDirty(this);
		}
	}

	public void FixTransform() {
		var iso_world = GetIsoWorld();
		if ( iso_world && _transform ) {
			Vector3 trans = iso_world.IsoToScreen(Position);
			trans.z = _transform.position.z;
			_transform.position = trans;
			_lastPosition = Position;
			_lastTransform = trans;
		}
	}

	public void FixIsoPosition() {
		var iso_world = GetIsoWorld();
		if ( iso_world && _transform ) {
			Vector2 trans = _transform.position;
			Position = iso_world.ScreenToIso(trans, Position.z);
			FixTransform();
		}
	}

	void MartDirtyIsoWorld() {
		var iso_world = GetIsoWorld();
		if ( iso_world && Sorting ) {
			iso_world.MarkDirty();
		}
	}

	void Awake() {
		_transform = gameObject.transform;
		_lastPosition = Position;
		_lastTransform = _transform.position;
		FixIsoPosition();
		MartDirtyIsoWorld();
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
		MartDirtyIsoWorld();
	}
}
} // namespace IsoTools