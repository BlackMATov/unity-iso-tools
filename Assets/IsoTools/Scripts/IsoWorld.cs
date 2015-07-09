using UnityEngine;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class IsoWorld : MonoBehaviour {

		class ObjectInfo {
			public int       Index;
			public IsoObject IsoObject;
			public Vector3   MinSector;
			public Vector3   MaxSector;

			public bool      Visited;
			public int       BeginDepend;
			public int       EndDepend;

			public ObjectInfo(int index, IsoObject iso_object, Vector3 min_sector, Vector3 max_sector) {
				Index     = index;
				IsoObject = iso_object;
				MinSector = min_sector;
				MaxSector = max_sector;
			}

			public void Init(int first_depend) {
				Visited     = false;
				BeginDepend = first_depend;
				EndDepend   = first_depend;
			}
		}

		class SectorInfo {
			public List<int> Objects = new List<int>();
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

		[SerializeField]
		public float _tileSize = 32.0f;
		/// <summary>Isometric tile size.</summary>
		public float TileSize {
			get { return _tileSize; }
			set {
				_tileSize = Mathf.Max(value, Mathf.Epsilon);
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _minDepth = 0.0f;
		/// <summary>Min sorting depth value.</summary>
		public float MinDepth {
			get { return _minDepth; }
			set {
				_minDepth = value;
				ChangeSortingProperty();
			}
		}
		
		[SerializeField]
		public float _maxDepth = 100.0f;
		/// <summary>Max sorting depth value.</summary>
		public float MaxDepth {
			get { return _maxDepth; }
			set {
				_maxDepth = value;
				ChangeSortingProperty();
			}
		}
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Marks world for resorting.
		/// </summary>
		// ------------------------------------------------------------------------
		public void MarkDirty() {
			_dirty = true;
			MarkEditorWorldDirty();
		}

		// ------------------------------------------------------------------------
		/// <summary>
		/// Marks world for resorting.
		/// </summary>
		/// <param name="iso_object">Isometric object for resorting.</param>
		// ------------------------------------------------------------------------ 
		public void MarkDirty(IsoObject iso_object) {
			if ( !_dirty && iso_object && _visibles.Contains(iso_object) ) {
				MarkDirty();
			}
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
			return IsoUtils.Vec3ChangeZ(
				ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize)),
				iso_z);
		}

		// ------------------------------------------------------------------------
		//
		// Private
		//
		// ------------------------------------------------------------------------

		bool IsIsoObjectVisible(IsoObject iso_object) {
			var renderers = iso_object.GetComponentsInChildren<Renderer>();
			foreach ( var child_renderer in renderers ) {
				if ( child_renderer.isVisible ) {
					return true;
				}
			}
			return false;
		}

		void MarkEditorWorldDirty() {
		#if UNITY_EDITOR
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
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
				_objsSectorSize += Mathf.Max(obj.Size.x, obj.Size.y, obj.Size.z);
			}
			_objsSectorSize = Mathf.Round(Mathf.Max(3.0f, _objsSectorSize / objsSum));
		}

		void SetupObjects() {
			_objects.Clear();
			_objsMinNumPos = Vector3.zero;
			_objsMaxNumPos = Vector3.one;
			foreach ( var obj in _visibles ) {
				var max_size = IsoUtils.Vec3Max(Vector3.one, obj.Size);
				var min_npos = IsoUtils.Vec3DivFloor(obj.Position, _objsSectorSize);
				var max_npos = IsoUtils.Vec3DivCeil(obj.Position + max_size, _objsSectorSize);
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
				obj.MinSector -= _objsMinNumPos;
				obj.MaxSector -= _objsMinNumPos;
				IsoUtils.LookUpCube(obj.MinSector, obj.MaxSector, p => {
					var sector = FindSector(p);
					if ( sector != null ) {
						sector.Objects.Add(obj.Index);
					}
				});
			}
		}

		void SetupObjectDepends() {
			_depends.Clear();
			foreach ( var obj_a in _objects ) {
				obj_a.Init(_depends.Count);
				var obj_ao = obj_a.IsoObject;
				IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, num_pos => {
					LookUpSectorDepends(num_pos, sec => {
						foreach ( var obj_bi in sec.Objects ) {
							var obj_bo = _objects[obj_bi].IsoObject;
							if ( obj_ao != obj_bo && IsDepends(obj_ao.Position, obj_ao.Size, obj_bo.Position, obj_bo.Size) ) {
								_depends.Add(obj_bi);
								++obj_a.EndDepend;
							}
						}
					});
				});
			}
		}

		void PlaceAllObjects() {
			var depth = MinDepth;
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
			if ( info.Visited ) {
				return depth;
			}
			info.Visited = true;
			for ( var i = info.BeginDepend; i < info.EndDepend && i < _depends.Count; ++i ) {
				var obj_index = _depends[i];
				var obj = _objects[obj_index];
				depth = PlaceObject(obj, depth);
			}
			PlaceObject(info.IsoObject, depth);
			return depth + (MaxDepth - MinDepth) / _objects.Count;
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
			TileSize = 32.0f;
			MinDepth = 0.0f;
			MaxDepth = 100.0f;
		}
		
		void OnValidate() {
			TileSize = _tileSize;
			MinDepth = _minDepth;
			MaxDepth = _maxDepth;
		}
		#endif
	}
} // namespace IsoTools