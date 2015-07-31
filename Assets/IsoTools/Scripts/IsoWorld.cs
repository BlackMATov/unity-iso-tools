using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class IsoWorld : MonoBehaviour {

		bool               _dirty      = true;
		HashSet<IsoObject> _visibles   = new HashSet<IsoObject>();
		HashSet<IsoObject> _isoObjects = new HashSet<IsoObject>();

		class SectorInfo {
			public List<IsoObject> objects = new List<IsoObject>();
		}

		List<SectorInfo>   _sectors         = new List<SectorInfo>();
		float              _objsSectorSize  = 0.0f;
		Vector3            _objsMinNumPos   = Vector3.zero;
		Vector3            _objsMaxNumPos   = Vector3.zero;
		Vector3            _objsNumPosCount = Vector3.zero;
		
		// ------------------------------------------------------------------------
		//
		// Public
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		public float _tileSize = 32.0f;
		public float tileSize {
			get { return _tileSize; }
			set {
				_tileSize = Mathf.Max(value, Mathf.Epsilon);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _minDepth = 1.0f;
		public float minDepth {
			get { return _minDepth; }
			set {
				_minDepth = value;
				ChangeSortingProperty();
			}
		}
		
		[SerializeField]
		public float _maxDepth = 100.0f;
		public float maxDepth {
			get { return _maxDepth; }
			set {
				_maxDepth = value;
				ChangeSortingProperty();
			}
		}

		public void MarkDirty() {
			_dirty = true;
			MarkEditorWorldDirty();
		}

		public void MarkDirty(IsoObject iso_object) {
			if ( iso_object && _visibles.Contains(iso_object) ) {
				MarkDirty();
				iso_object.Moved = true;
			}
		}

		public Vector2 IsoToScreen(Vector3 pos) {
			return new Vector2(
				(pos.x - pos.y),
				(pos.x + pos.y) * 0.5f + pos.z) * tileSize;
		}

		public Vector3 ScreenToIso(Vector2 pos) {
			return new Vector3(
				(pos.x * 0.5f + pos.y),
				(pos.y - pos.x * 0.5f),
				0.0f) / tileSize;
		}

		public Vector3 ScreenToIso(Vector2 pos, float iso_z) {
			return IsoUtils.Vec3ChangeZ(
				ScreenToIso(new Vector2(pos.x, pos.y - iso_z * tileSize)),
				iso_z);
		}


		public void AddIsoObject(IsoObject iso_object) {
			_isoObjects.Add(iso_object);
		}

		public void RemoveIsoObject(IsoObject iso_object) {
			_isoObjects.Remove(iso_object);
		}

		// ------------------------------------------------------------------------
		//
		// Private
		//
		// ------------------------------------------------------------------------

		bool IsGameObjectVisible(GameObject obj) {
			var renderer = obj.GetComponent<Renderer>();
			if ( renderer && renderer.isVisible ) {
				return true;
			}
			var obj_transform = obj.transform;
			for ( var i = 0; i < obj_transform.childCount; ++i ) {
				var child_obj = obj_transform.GetChild(i).gameObject;
				if ( IsGameObjectVisible(child_obj) ) {
					return true;
				}
			}
			return false;
		}

		bool IsIsoObjectVisible(IsoObject iso_object) {
			return IsGameObjectVisible(iso_object.gameObject);
		}

		void MarkEditorWorldDirty() {
		#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
		#endif
		}

		void FixAllTransforms() {
			foreach ( var iso_object in _isoObjects ) {
				iso_object.FixTransform();
			}
		}

		void ChangeSortingProperty() {
			MarkDirty();
			FixAllTransforms();
		}

		bool IsDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			var a_max = a_min + a_size;
			var b_max = b_min + b_size;
			var a_yesno = a_max.x > b_min.x && a_max.y > b_min.y && b_max.z > a_min.z;
			var b_yesno = b_max.x > a_min.x && b_max.y > a_min.y && a_max.z > b_min.z;
			if ( a_yesno && b_yesno ) {
				var da_p = new Vector3(a_max.x - b_min.x, a_max.y - b_min.y, b_max.z - a_min.z);
				var db_p = new Vector3(b_max.x - a_min.x, b_max.y - a_min.y, a_max.z - b_min.z);
				var dp_p = a_size + b_size - IsoUtils.Vec3Abs(da_p - db_p);
				if ( dp_p.x <= dp_p.y && dp_p.x <= dp_p.z ) {
					return da_p.x > db_p.x;
				} else if ( dp_p.y <= dp_p.x && dp_p.y <= dp_p.z ) {
					return da_p.y > db_p.y;
				} else {
					return da_p.z > db_p.z;
				}
			}
			return a_yesno;
		}

		bool IsDepends(IsoObject a, IsoObject b) {
			return IsDepends(a.position, a.size, b.position, b.size);
		}

		/*
		void CheckAlives() {
			var new_visibles = new HashSet<IsoObject>();
			var new_objects = new List<IsoObject>();
			foreach ( var iso_object in _isoObjects ) {
				if ( IsIsoObjectVisible(iso_object) ) {
					new_visibles.Add(iso_object);
					if ( !_visibles.Contains(iso_object) ) {
						new_objects.Add(iso_object);
					}
				}
			}
			if ( !_visibles.IsSupersetOf(new_visibles) ) {
				MarkDirty();
			}
			_visibles = new_visibles;
			Debug.LogFormat("AllObjects: {0}, Visibles: {1}", _isoObjects.Count, _visibles.Count);
		}

		void SetupObjects() {
			_objects.Clear();
			foreach ( var obj in _visibles ) {
				_objects.Add(obj);
			}
		}

		void SetupObjectDepends(IsoObject obj_ao) {
			obj_ao.Visited = false;
			obj_ao.Depends.Clear();
			for ( var obj_bi = 0; obj_bi < _objects.Count; ++obj_bi ) {
				var obj_bo = _objects[obj_bi];
				if ( obj_ao != obj_bo && IsDepends(obj_ao.position, obj_ao.size, obj_bo.position, obj_bo.size) ) {
					obj_ao.Depends.Add(obj_bi);
				}
			}
		}

		void SetupAllObjectDepends() {
			foreach ( var obj_ao in _objects ) {
				obj_ao.Visited = false;
				obj_ao.Depends.Clear();
				for ( var obj_bi = 0; obj_bi < _objects.Count; ++obj_bi ) {
					var obj_bo = _objects[obj_bi];
					if ( obj_ao != obj_bo && IsDepends(obj_ao.position, obj_ao.size, obj_bo.position, obj_bo.size) ) {
						obj_ao.Depends.Add(obj_bi);
					}
				}
			}
		}
		*/

		int SectorIndex(Vector3 num_pos) {
			return Mathf.FloorToInt(
				num_pos.x + _objsNumPosCount.x * (num_pos.y + num_pos.z * _objsNumPosCount.y));
		}
		
		Vector3 SectorNumPos(int index) {
			var mz = _objsNumPosCount.x * _objsNumPosCount.y;
			var my = _objsNumPosCount.x;
			var vz = Mathf.FloorToInt(index / mz);
			var vy = Mathf.FloorToInt((index - vz * mz) / my);
			var vx = Mathf.FloorToInt(index - vz * mz - vy * my);
			return new Vector3(vx, vy, vz);
		}
		
		SectorInfo FindSector(Vector3 num_pos) {
			if ( num_pos.x < 0 || num_pos.y < 0 || num_pos.z < 0 ) {
				return null;
			}
			if ( num_pos.x >= _objsNumPosCount.x || num_pos.y >= _objsNumPosCount.y || num_pos.z >= _objsNumPosCount.z ) {
				return null;
			}
			return _sectors[SectorIndex(num_pos)];
		}
		
		void LookUpSectorDepends(Vector3 num_pos, System.Action<SectorInfo> act) {
			var ms = FindSector(num_pos);
			if ( ms != null ) {
				act(ms);
				var s1 = FindSector(num_pos + new Vector3(-1,  0, 0));
				var s2 = FindSector(num_pos + new Vector3( 0, -1, 0));
				var s3 = FindSector(num_pos + new Vector3(-1, -1, 0));
				if ( s1 != null ) act(s1);
				if ( s2 != null ) act(s2);
				if ( s3 != null ) act(s3);
				for ( var i = 0; i <= _objsNumPosCount.z; ++i ) {
					var ss1 = FindSector(num_pos + new Vector3( 0 - i,  0 - i, i + 1));
					var ss2 = FindSector(num_pos + new Vector3(-1 - i,  0 - i, i + 1));
					var ss3 = FindSector(num_pos + new Vector3( 0 - i, -1 - i, i + 1));
					var ss4 = FindSector(num_pos + new Vector3(-1 - i, -1 - i, i + 1));
					var ss5 = FindSector(num_pos + new Vector3(-2 - i, -1 - i, i + 1));
					var ss6 = FindSector(num_pos + new Vector3(-1 - i, -2 - i, i + 1));
					var ss7 = FindSector(num_pos + new Vector3(-2 - i, -2 - i, i + 1));
					if ( ss1 != null ) act(ss1);
					if ( ss2 != null ) act(ss2);
					if ( ss3 != null ) act(ss3);
					if ( ss4 != null ) act(ss4);
					if ( ss5 != null ) act(ss5);
					if ( ss6 != null ) act(ss6);
					if ( ss7 != null ) act(ss7);
				}
			}
		}

		void LookUpSectorRDepends(Vector3 num_pos, System.Action<SectorInfo> act) {
			var ms = FindSector(num_pos);
			if ( ms != null ) {
				act(ms);
				var s1 = FindSector(num_pos + new Vector3( 1,  0, 0));
				var s2 = FindSector(num_pos + new Vector3( 0,  1, 0));
				var s3 = FindSector(num_pos + new Vector3( 1,  1, 0));
				if ( s1 != null ) act(s1);
				if ( s2 != null ) act(s2);
				if ( s3 != null ) act(s3);
				for ( var i = 0; i <= _objsNumPosCount.z; ++i ) {
					var ss1 = FindSector(num_pos + new Vector3( 0 + i,  0 + i, -i - 1));
					var ss2 = FindSector(num_pos + new Vector3( 1 + i,  0 + i, -i - 1));
					var ss3 = FindSector(num_pos + new Vector3( 0 + i,  1 + i, -i - 1));
					var ss4 = FindSector(num_pos + new Vector3( 1 + i,  1 + i, -i - 1));
					var ss5 = FindSector(num_pos + new Vector3( 2 + i,  1 + i, -i - 1));
					var ss6 = FindSector(num_pos + new Vector3( 1 + i,  2 + i, -i - 1));
					var ss7 = FindSector(num_pos + new Vector3( 2 + i,  2 + i, -i - 1));
					if ( ss1 != null ) act(ss1);
					if ( ss2 != null ) act(ss2);
					if ( ss3 != null ) act(ss3);
					if ( ss4 != null ) act(ss4);
					if ( ss5 != null ) act(ss5);
					if ( ss6 != null ) act(ss6);
					if ( ss7 != null ) act(ss7);
				}
			}
		}

		void SetupSectorSize() {
			_objsSectorSize = 0.0f;
			var objsSum = 0;
			foreach ( var obj in _visibles ) {
				++objsSum;
				_objsSectorSize += IsoUtils.Vec3MaxF(obj.size);
			}
			_objsSectorSize = Mathf.Round(Mathf.Max(3.0f, _objsSectorSize / objsSum));
		}

		void SetupObjects() {
			_objsMinNumPos = Vector3.zero;
			_objsMaxNumPos = Vector3.one;
			foreach ( var obj in _visibles ) {
				var max_size = IsoUtils.Vec3Max(Vector3.one, obj.size);
				var min_npos = IsoUtils.Vec3DivFloor(obj.position, _objsSectorSize);
				var max_npos = IsoUtils.Vec3DivCeil(obj.position + max_size, _objsSectorSize);
				_objsMinNumPos = IsoUtils.Vec3Min(_objsMinNumPos, min_npos);
				_objsMaxNumPos = IsoUtils.Vec3Max(_objsMaxNumPos, max_npos);
				obj.MinSector = min_npos;
				obj.MaxSector = max_npos;
			}
			_objsNumPosCount = _objsMaxNumPos - _objsMinNumPos;
		}
		
		void SetupSectors() {
			_sectors.Clear();
			_sectors.Capacity = Mathf.FloorToInt(_objsNumPosCount.x * _objsNumPosCount.y * _objsNumPosCount.z);
			while ( _sectors.Count < _sectors.Capacity ) {
				_sectors.Add(new SectorInfo());
			}
			foreach ( var obj in _visibles ) {
				obj.MinSector -= _objsMinNumPos;
				obj.MaxSector -= _objsMinNumPos;
				IsoUtils.LookUpCube(obj.MinSector, obj.MaxSector, p => {
					var sector = FindSector(p);
					if ( sector != null ) {
						sector.objects.Add(obj);
					}
				});
			}
		}

		/// <summary>
		///
		/// </summary>

		float t_up = 0.0f;
		float t_pl = 0.0f;

		void OnGUI() {
			GUILayout.Label("UpdateVisibles  : " + (t_up * 1000).ToString());
			GUILayout.Label("PlaceAllVisibles: " + (t_pl * 1000).ToString());
		}

		void StepSort() {
			var s_up = Time.realtimeSinceStartup;
			UpdateVisibles();
			var e_up = Time.realtimeSinceStartup;
			t_up = e_up - s_up;
			if ( _dirty ) {
				var s_pl = Time.realtimeSinceStartup;
				PlaceAllVisibles();
				var e_pl = Time.realtimeSinceStartup;
				t_pl = e_pl - s_pl;
				_dirty = false;
			}
		}

		void UpdateVisibles() {
			var old_all_visibles = _visibles;
			var new_all_visibles = CalculateAllVisibles();
			_visibles = new_all_visibles;

			SetupSectorSize();
			SetupObjects();
			SetupSectors();

			var new_missings = new HashSet<IsoObject>(old_all_visibles);
			new_missings.ExceptWith(new_all_visibles);
			if ( new_missings.Count > 0 ) {
				MarkDirty();
				foreach ( var iso_object in new_missings ) {
					ClearIsoObjectDepends(iso_object);
				}
			}

			var new_visibles = new HashSet<IsoObject>(new_all_visibles);
			new_visibles.ExceptWith(old_all_visibles);
			if ( new_visibles.Count > 0 ) {
				MarkDirty();
				foreach ( var iso_object in new_visibles ) {
					SetupIsoObjectDepends(iso_object);
				}
			}

			foreach ( var iso_object in _visibles ) {
				if ( iso_object.Moved ) {
					SetupIsoObjectDepends(iso_object);
					iso_object.Moved = false;
				}
			}

			/*
			Debug.LogFormat(
				"New: {0}, Missings: {1}, Visibles: {2}, All: {3}",
				new_visibles.Count, new_missings.Count, _visibles.Count, _isoObjects.Count);*/
		}

		HashSet<IsoObject> CalculateAllVisibles() {
			var all_visibles = new HashSet<IsoObject>();
			foreach ( var iso_object in _isoObjects ) {
				if ( IsIsoObjectVisible(iso_object) ) {
					iso_object.Visited = false;
					all_visibles.Add(iso_object);
				}
			}
			return all_visibles;
		}

		void ClearIsoObjectDepends(IsoObject iso_object) {
			foreach ( var other_iso_object in iso_object.TheirDepends ) {
				other_iso_object.SelfDepends.Remove(iso_object);
			}
			iso_object.SelfDepends.Clear();
			iso_object.TheirDepends.Clear();
		}

		/*
		void SetupIsoObjectDepends(IsoObject iso_object) {
			ClearIsoObjectDepends(iso_object);
			foreach ( var other_iso_object in _visibles ) {
				if ( iso_object != other_iso_object ) {
					if ( IsDepends(iso_object, other_iso_object) ) {
						iso_object.SelfDepends.Add(other_iso_object);
						other_iso_object.TheirDepends.Add(iso_object);
					}
					if ( IsDepends(other_iso_object, iso_object) ) {
						other_iso_object.SelfDepends.Add(iso_object);
						iso_object.TheirDepends.Add(other_iso_object);
					}
				}
			}
		}*/
		void SetupIsoObjectDepends(IsoObject obj_a) {
			ClearIsoObjectDepends(obj_a);
			IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, num_pos => {
				LookUpSectorDepends(num_pos, sec => {
					foreach ( var obj_b in sec.objects ) {
						if ( obj_a != obj_b && IsDepends(obj_a, obj_b) ) {
							obj_a.SelfDepends.Add(obj_b);
							obj_b.TheirDepends.Add(obj_a);
						}
					}
				});
			});
			/*
			foreach ( var obj_b in _visibles ) {
				if ( obj_a != obj_b ) {
					if ( IsDepends(obj_b, obj_a) ) {
						obj_b.SelfDepends.Add(obj_a);
						obj_a.TheirDepends.Add(obj_b);
					}
				}
			}*/
			IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, num_pos => {
				LookUpSectorRDepends(num_pos, sec => {
					foreach ( var obj_b in sec.objects ) {
						if ( obj_a != obj_b && IsDepends(obj_b, obj_a) ) {
							obj_b.SelfDepends.Add(obj_a);
							obj_a.TheirDepends.Add(obj_b);
						}
					}
				});
			});
		}
		
		int place_num = 0;
		
		void PlaceAllVisibles() {
			place_num = 0;
			var depth = minDepth;
			foreach ( var iso_object in _visibles ) {
				depth = RecursivePlaceIsoObject(iso_object, depth);
			}
			//Debug.LogFormat("Place: {0}, All: {1}", _visibles.Count, _isoObjects.Count);
			//Debug.LogFormat("PlaceNum: {0}", place_num);
		}

		float RecursivePlaceIsoObject(IsoObject iso_object, float depth) {
			++place_num;
			if ( iso_object.Visited ) {
				return depth;
			}
			iso_object.Visited = true;
			foreach ( var depend in iso_object.SelfDepends ) {
				depth = RecursivePlaceIsoObject(depend, depth);
			}
			PlaceIsoObject(iso_object, depth);
			return depth + (maxDepth - minDepth) / _visibles.Count;
		}

		void PlaceIsoObject(IsoObject iso_object, float depth) {
			var trans = iso_object.transform;
			trans.position = IsoUtils.Vec3ChangeZ(trans.position, depth);
		}

		// ------------------------------------------------------------------------
		//
		// Messages
		//
		// ------------------------------------------------------------------------

		void Start() {
			ChangeSortingProperty();
			StepSort();
		}

		void LateUpdate() {
			StepSort();
		}

		void OnEnable() {
			_visibles.Clear();
			_isoObjects = new HashSet<IsoObject>(FindObjectsOfType<IsoObject>());
			MarkDirty();
		}

		void OnDisable() {
			_visibles.Clear();
			_isoObjects.Clear();
		}

		#if UNITY_EDITOR
		void Reset() {
			tileSize = 32.0f;
			minDepth = 1.0f;
			maxDepth = 100.0f;
		}
		
		void OnValidate() {
			tileSize = _tileSize;
			minDepth = _minDepth;
			maxDepth = _maxDepth;
		}

		void OnRenderObject() {
			if ( Camera.current && Camera.current.name == "SceneCamera" ) {
				StepSort();
			}
		}
		#endif
	}
} // namespace IsoTools