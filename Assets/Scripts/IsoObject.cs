using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class IsoObject : MonoBehaviour {

	[SerializeField]
	private Vector3 position = Vector3.zero;
	public  Vector3 Position
	{
		get { return position; }
		set {
			position = value;
			FixTransform();
		}
	}

	[SerializeField]
	private Vector3 size = Vector3.one;
	public  Vector3 Size
	{
		get { return size; }
		set {
			size = value;
			FixTransform();
		}
	}

	[SerializeField]
	private bool alignment = true;
	public  bool Alignment
	{
		get { return alignment; }
		set {
			alignment = value;
			FixTransform();
		}
	}
	
	void Start() {
	}

	void Update() {
		if ( Selection.Contains(gameObject) ) {
			FixIsoPosition();
		} else {
			FixTransform();
		}
	}

	void FixTransform() {
		var iso_world = GameObject.FindObjectOfType<IsoWorld>();
		if ( iso_world ) {
			if ( Alignment ) {
				position = new Vector3(
					Mathf.Round(Position.x),
					Mathf.Round(Position.y),
					Position.z);
			}
			var pos = iso_world.IsoToScreen(Position);
			var depth = gameObject.transform.position.z;
			gameObject.transform.position = new Vector3(pos.x, pos.y, depth);
		}
	}

	void FixIsoPosition() {
		var iso_world = GameObject.FindObjectOfType<IsoWorld>();
		if ( iso_world ) {
			var pos = gameObject.transform.position;
			Position = iso_world.ScreenToIso(new Vector2(pos.x, pos.y));
			EditorUtility.SetDirty(this);
		}
	}
}
