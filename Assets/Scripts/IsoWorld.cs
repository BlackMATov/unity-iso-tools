using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class IsoWorld : MonoBehaviour {

	private class ObjectInfo {
		public IsoObject IsoObject;
		public bool      Visited;
		public int       BeginDepend;
		public int       EndDepend;
		public ObjectInfo(IsoObject obj) {
			IsoObject = obj;
		}
		public void Reset(int first_depend) {
			Visited     = false;
			BeginDepend = first_depend;
			EndDepend   = first_depend;
		}
	}

	public float TileSize   = 32.0f;
	public float StartDepth = 0.0f;
	public float StepDepth  = 0.1f;

	List<int>        _depends = new List<int>();
	List<ObjectInfo> _objects = new List<ObjectInfo>();
	
	public Vector2 IsoToScreen(Vector3 pos) {
		return new Vector2(
			(pos.x - pos.y),
			(pos.x + pos.y) * 0.5f + pos.z) * TileSize;
	}

	public Vector3 ScreenToIso(Vector2 pos) {
		return new Vector3(
			(pos.x * 0.5f + pos.y),
			(pos.y - pos.x * 0.5f),
			0.0f) / TileSize;
	}

	void _scanObjects() {
		_objects.Clear();
		IsoObject[] iso_objects = GameObject.FindObjectsOfType<IsoObject>();
		foreach ( var iso_object in iso_objects ) {
			var info = new ObjectInfo(iso_object);
			_objects.Add(info);
		}
	}

	void _scanDepends() {
		_depends.Clear();
		foreach ( var obj_a in _objects ) {
			obj_a.Reset(_depends.Count);
			var obj_ao = obj_a.IsoObject;
			var max_ax = obj_ao.Position.x + obj_ao.Size.x;
			var max_ay = obj_ao.Position.y + obj_ao.Size.y;
			for ( int i = 0; i < _objects.Count; ++i ) {
				var obj_bo = _objects[i].IsoObject;
				if ( obj_ao != obj_bo ) {
					if ( obj_bo.Position.x < max_ax && obj_bo.Position.y < max_ay ) {
						var max_bz = obj_bo.Position.z + obj_bo.Size.z;
						if ( obj_ao.Position.z < max_bz ) {
							_depends.Add(i);
							++obj_a.EndDepend;
						}
					}
				}
			}
		}
	}

	void _manualSort() {
		var depth = StartDepth;
		foreach ( ObjectInfo info in _objects ) {
			_placeObject(info, ref depth);
		}
	}

	void _placeObject(IsoObject obj, float depth) {
		var pos = obj.gameObject.transform.position;
		obj.gameObject.transform.position = new Vector3(pos.x, pos.y, depth);
	}

	void _placeObject(ObjectInfo info, ref float depth) {
		if ( !info.Visited ) {
			info.Visited = true;
			for ( int i = info.BeginDepend; i < info.EndDepend && i < _depends.Count; ++i ) {
				var object_index = _depends[i];
				var obj = _objects[object_index];
				_placeObject(obj, ref depth);
			}
			_placeObject(info.IsoObject, depth);
			depth += StepDepth;
		}
	}
	
	void Start () {
	}

	void Update () {
		_scanObjects();
		_scanDepends();
		_manualSort();
	}
}