﻿using UnityEngine;
using System.Collections.Generic;

namespace IsoTools {
[ExecuteInEditMode]
public class IsoWorld : MonoBehaviour {
	
	/// <summary>Isometric tile size.</summary>
	public float TileSize   = 32.0f;
	/// <summary>Start sorting depth value.</summary>
	public float StartDepth = 0.0f;
	/// <summary>Step sorting depth value.</summary>
	public float StepDepth  = 0.1f;

	class ObjectInfo {
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

	bool             _dirty        = true;
	List<int>        _depends      = new List<int>();
	List<ObjectInfo> _objects      = new List<ObjectInfo>();
	float            _lastTileSize = 0.0f;
	
	// ------------------------------------------------------------------------
	/// <summary>
	/// Marks world for resorting.
	/// </summary>
	// ------------------------------------------------------------------------
	public void MarkDirty() {
		_dirty = true;
	}
	
	// ------------------------------------------------------------------------
	/// <summary>
	/// Convert isometric coordinates to screen coordinates
	/// </summary>
	/// <returns>Screen coordinates</returns>
	/// <param name="pos">Isometric coordinates.</param>
	// ------------------------------------------------------------------------
	public Vector2 IsoToScreen(Vector3 pos) {
		return new Vector2(
			(pos.x - pos.y),
			(pos.x + pos.y) * 0.5f + pos.z) * TileSize;
	}
	
	// ------------------------------------------------------------------------
	/// <summary>
	/// Convert screen coordinates to isometric coordinates
	/// </summary>
	/// <returns>Isometric coordinates</returns>
	/// <param name="pos">Screen coordinates.</param>
	// ------------------------------------------------------------------------
	public Vector3 ScreenToIso(Vector2 pos) {
		return new Vector3(
			(pos.x * 0.5f + pos.y),
			(pos.y - pos.x * 0.5f),
			0.0f) / TileSize;
	}
	
	// ------------------------------------------------------------------------
	/// <summary>
	/// Convert screen coordinates to isometric coordinates with specified isometric height
	/// </summary>
	/// <returns>Isometric coordinates</returns>
	/// <param name="pos">Screen coordinates.</param>
	/// <param name="iso_z">Point isometric height.</param>
	// ------------------------------------------------------------------------
	public Vector3 ScreenToIso(Vector2 pos, float iso_z) {
		var iso_pos = ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize));
		iso_pos.z = iso_z;
		return iso_pos;
	}

	void _fixTileSize() {
		_scanObjects();
		foreach ( var obj in _objects ) {
			obj.IsoObject.FixTransform();
		}
		_objects.Clear();
		_lastTileSize = TileSize;
	}

	void _fixDirty() {
		_scanObjects();
		_scanDepends();
		_manualSort();
		_dirty = false;
	}

	void _fixDisable() {
		_scanObjects();
		foreach ( var obj in _objects ) {
			obj.IsoObject.ResetIsoWorld();
		}
		_objects.Clear();
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
		_objects.Clear();
		_depends.Clear();
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

	void Start() {
		_fixTileSize();
		_fixDirty();
	}

	void LateUpdate() {
		if ( _lastTileSize != TileSize ) {
			_fixTileSize();
		}
		if ( _dirty ) {
			_fixDirty();
		}
	}

	void OnDisable() {
		_fixDisable();
	}
}
} // namespace IsoTools