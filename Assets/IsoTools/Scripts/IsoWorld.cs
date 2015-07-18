using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class IsoWorld : MonoBehaviour {

		class ObjectInfo {
			public int       index;
			public IsoObject isoObject;
			public Vector3   minSector;
			public Vector3   maxSector;

			public bool      visited;
			public int       beginDepend;
			public int       endDepend;

			public ObjectInfo(int index, IsoObject iso_object, Vector3 min_sector, Vector3 max_sector) {
				this.index       = index;
				this.isoObject   = iso_object;
				this.minSector   = min_sector;
				this.maxSector   = max_sector;
			}

			public void Init(int first_depend) {
				this.visited     = false;
				this.beginDepend = first_depend;
				this.endDepend   = first_depend;
			}
		}

		class SectorInfo {
			public List<int> objects = new List<int>();
		}

		bool               _dirty           = true;
		List<SectorInfo>   _sectors         = new List<SectorInfo>();
		List<ObjectInfo>   _objects         = new List<ObjectInfo>();
		List<int>          _depends         = new List<int>();
		HashSet<IsoObject> _visibles        = new HashSet<IsoObject>();
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
			if ( !_dirty && iso_object && _visibles.Contains(iso_object) ) {
				MarkDirty();
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

		void ApplyToAllIsoObjects(System.Action<IsoObject> act) {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();
			foreach ( var obj in iso_objects ) {
				act(obj);
			}
		}
		
		void FixAllTransforms() {
			ApplyToAllIsoObjects(obj => obj.FixTransform());
		}

		void ResetAllIsoWorld() {
			ApplyToAllIsoObjects(obj => obj.ResetIsoWorld());
		}

		void ChangeSortingProperty() {
			MarkDirty();
			FixAllTransforms();
		}

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

		void SetupVisibles(IsoObject[] iso_objects) {
			var new_visibles = new HashSet<IsoObject>();
			foreach ( var iso_object in iso_objects ) {
				if ( IsIsoObjectVisible(iso_object) ) {
					new_visibles.Add(iso_object);
				}
			}
			if ( !_visibles.IsSupersetOf(new_visibles) ) {
				MarkDirty();
			}
			_visibles = new_visibles;
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
			_objects.Clear();
			_objsMinNumPos = Vector3.zero;
			_objsMaxNumPos = Vector3.one;
			foreach ( var obj in _visibles ) {
				var max_size = IsoUtils.Vec3Max(Vector3.one, obj.size);
				var min_npos = IsoUtils.Vec3DivFloor(obj.position, _objsSectorSize);
				var max_npos = IsoUtils.Vec3DivCeil(obj.position + max_size, _objsSectorSize);
				_objsMinNumPos = IsoUtils.Vec3Min(_objsMinNumPos, min_npos);
				_objsMaxNumPos = IsoUtils.Vec3Max(_objsMaxNumPos, max_npos);
				_objects.Add(new ObjectInfo(_objects.Count, obj, min_npos, max_npos));
			}
			_objsNumPosCount = _objsMaxNumPos - _objsMinNumPos;
		}

		void SetupSectors() {
			_sectors.Clear();
			_sectors.Capacity = Mathf.FloorToInt(_objsNumPosCount.x * _objsNumPosCount.y * _objsNumPosCount.z);
			while ( _sectors.Count < _sectors.Capacity ) {
				_sectors.Add(new SectorInfo());
			}
			foreach ( var obj in _objects ) {
				obj.minSector -= _objsMinNumPos;
				obj.maxSector -= _objsMinNumPos;
				IsoUtils.LookUpCube(obj.minSector, obj.maxSector, p => {
					var sector = FindSector(p);
					if ( sector != null ) {
						sector.objects.Add(obj.index);
					}
				});
			}
		}

		void SetupObjectDepends() {
			_depends.Clear();
			foreach ( var obj_a in _objects ) {
				obj_a.Init(_depends.Count);
				var obj_ao = obj_a.isoObject;
				IsoUtils.LookUpCube(obj_a.minSector, obj_a.maxSector, num_pos => {
					LookUpSectorDepends(num_pos, sec => {
						foreach ( var obj_bi in sec.objects ) {
							var obj_bo = _objects[obj_bi].isoObject;
							if ( obj_ao != obj_bo && IsDepends(obj_ao.position, obj_ao.size, obj_bo.position, obj_bo.size) ) {
								_depends.Add(obj_bi);
								++obj_a.endDepend;
							}
						}
					});
				});
			}
		}

		void PlaceAllObjects() {
			var depth = minDepth;
			foreach ( var info in _objects ) {
				depth = PlaceObject(info, depth);
			}
			_sectors.Clear();
			_objects.Clear();
			_depends.Clear();
		}
		
		void PlaceObject(IsoObject obj, float depth) {
			var trans = obj.transform;
			trans.position = IsoUtils.Vec3ChangeZ(trans.position, depth);
		}
		
		float PlaceObject(ObjectInfo info, float depth) {
			if ( info.visited ) {
				return depth;
			}
			info.visited = true;
			for ( var i = info.beginDepend; i < info.endDepend && i < _depends.Count; ++i ) {
				var obj_index = _depends[i];
				var obj = _objects[obj_index];
				depth = PlaceObject(obj, depth);
			}
			PlaceObject(info.isoObject, depth);
			return depth + (maxDepth - minDepth) / _objects.Count;
		}

		void StepSort() {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();
			SetupVisibles(iso_objects);
			if ( _dirty ) {
				SetupSectorSize();
				SetupObjects();
				SetupSectors();
				SetupObjectDepends();
				PlaceAllObjects();
				_dirty = false;
			}
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
			MarkDirty();
		}

		void OnDisable() {
			ResetAllIsoWorld();
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