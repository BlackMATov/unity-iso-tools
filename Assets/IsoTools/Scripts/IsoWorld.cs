using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoWorld : MonoBehaviour {
		
		/// <summary>World tile types.</summary>
		public enum TileTypes {
			Isometric,
			UpDown
		}

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

		bool             _dirty            = true;

		List<SectorInfo> _sectors          = new List<SectorInfo>();
		Vector3          _sectorsNum       = Vector3.zero;
		Vector3          _sectorsMinNumPos = Vector3.zero;

		List<ObjectInfo> _objects          = new List<ObjectInfo>();
		List<int>        _depends          = new List<int>();

		TileTypes        _lastTileType     = TileTypes.Isometric;
		float            _lastTileSize     = 0.0f;
		float            _lastMinDepth     = 0.0f;
		float            _lastMaxDepth     = 0.0f;
		float            _lastSectorSize   = 0.0f;

		[SerializeField]
		public TileTypes _tileType = TileTypes.Isometric;
		/// <summary>World tile type.</summary>
		public TileTypes TileType {
			get { return _tileType; }
			set {
				_tileType = value;
				ChangeSortingProperty();
			}
		}

		[SerializeField]
		public float _tileSize = 32.0f;
		/// <summary>Isometric tile size.</summary>
		public float TileSize {
			get { return _tileSize; }
			set {
				_tileSize = Math.Max(value, Mathf.Epsilon);
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

		[SerializeField]
		public float _sectorSize = 3.0f;
		/// <summary>Sorting sector size.</summary>
		public float SectorSize {
			get { return _sectorSize; }
			set {
				_sectorSize = Mathf.Max(value, 1.0f);
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
		/// <param name="obj">Isometric object for resorting.</param>
		// ------------------------------------------------------------------------ 
		public void MarkDirty(IsoObject obj) {
			if ( obj && obj.Sorting ) {
				var renderer = obj.GetComponent<Renderer>();
				if ( renderer && renderer.isVisible ) {
					_dirty = true;
					MarkEditorWorldDirty();
				}
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
			switch ( TileType ) {
			case TileTypes.Isometric:
				return new Vector2(
					(pos.x - pos.y),
					(pos.x + pos.y) * 0.5f + pos.z) * TileSize;
			case TileTypes.UpDown:
				return new Vector2(
					pos.x,
					pos.y + pos.z) * TileSize;
			default:
				throw new UnityException("IsoWorld. TileType is wrong!");
			}
		}
		
		// ------------------------------------------------------------------------
		/// <summary>
		/// Convert screen coordinates to isometric coordinates
		/// </summary>
		/// <returns>Isometric coordinates</returns>
		/// <param name="pos">Screen coordinates.</param>
		// ------------------------------------------------------------------------
		public Vector3 ScreenToIso(Vector2 pos) {
			switch ( TileType ) {
			case TileTypes.Isometric:
				return new Vector3(
					(pos.x * 0.5f + pos.y),
					(pos.y - pos.x * 0.5f),
					0.0f) / TileSize;
			case TileTypes.UpDown:
				return new Vector3(
					pos.x,
					pos.y,
					0.0f) / TileSize;
			default:
				throw new UnityException("IsoWorld. TileType is wrong!");
			}
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
			switch ( TileType ) {
			case TileTypes.Isometric: {
					var iso_pos = ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize));
					iso_pos.z = iso_z;
					return iso_pos;
				}
			case TileTypes.UpDown: {
					var iso_pos = ScreenToIso(new Vector2(pos.x, pos.y - iso_z * TileSize));
					iso_pos.z = iso_z;
					return iso_pos;
				}
			default:
				throw new UnityException("IsoWorld. TileType is wrong!");
			}
		}

		void MarkEditorWorldDirty() {
		#if UNITY_EDITOR
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		#endif
		}
		
		void FixAllTransforms() {
			ApplyToAllIsoObjects(obj => obj.FixTransform());
		}

		void ChangeSortingProperty() {
			MarkDirty();
			FixAllTransforms();
			_lastTileType   = TileType;
			_lastTileSize   = TileSize;
			_lastMinDepth   = MinDepth;
			_lastMaxDepth   = MaxDepth;
			_lastSectorSize = SectorSize;
		}

		void ApplyToAllIsoObjects(Action<IsoObject> act) {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();
			foreach ( var iso_object in iso_objects ) {
				act(iso_object);
			}
		}

		int SectorIndex(Vector3 num_pos) {
			return Mathf.FloorToInt(
				num_pos.x + _sectorsNum.x * (num_pos.y + num_pos.z * _sectorsNum.y));
		}

		Vector3 SectorNumPos(int index) {
			var mz = _sectorsNum.x * _sectorsNum.y;
			var my = _sectorsNum.x;
			var vz = Mathf.FloorToInt(index / mz);
			var vy = Mathf.FloorToInt((index - vz * mz) / my);
			var vx = Mathf.FloorToInt(index - vz * mz - vy * my);
			return new Vector3(vx, vy, vz);
		}

		SectorInfo FindSector(Vector3 num_pos) {
			if ( num_pos.x < 0 || num_pos.y < 0 || num_pos.z < 0 ) {
				return null;
			}
			if ( num_pos.x >= _sectorsNum.x || num_pos.y >= _sectorsNum.y || num_pos.z >= _sectorsNum.z ) {
				return null;
			}
			return _sectors[SectorIndex(num_pos)];
		}

		void SetupSectors() {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();

			_objects.Clear();
			_objects.Capacity = iso_objects.Length;

			var min_num_pos = Vector3.zero;
			var max_num_pos = Vector3.one;
			foreach ( var obj in iso_objects ) {
				var renderer = obj.GetComponent<Renderer>();
				if ( renderer && renderer.isVisible ) {
					var obj_max_size = IsoUtils.Vec3Max(Vector3.one, obj.Size);
					var obj_min_num_pos = IsoUtils.Vec3DivFloor(obj.Position, SectorSize);
					var obj_max_num_pos = IsoUtils.Vec3DivCeil(obj.Position + obj_max_size, SectorSize);
					min_num_pos = IsoUtils.Vec3Min(min_num_pos, obj_min_num_pos);
					max_num_pos = IsoUtils.Vec3Max(max_num_pos, obj_max_num_pos);
					_objects.Add(new ObjectInfo(_objects.Count, obj, obj_min_num_pos, obj_max_num_pos));
				}
			}

			_sectorsNum = max_num_pos - min_num_pos;
			_sectorsMinNumPos = min_num_pos;

			_sectors.Clear();
			_sectors.Capacity = Mathf.FloorToInt(_sectorsNum.x * _sectorsNum.y * _sectorsNum.z);
			while ( _sectors.Count < _sectors.Capacity ) {
				_sectors.Add(new SectorInfo());
			}

			foreach ( var obj in _objects ) {
				obj.MinSector -= _sectorsMinNumPos;
				obj.MaxSector -= _sectorsMinNumPos;
				IsoUtils.LookUpCube(obj.MinSector, obj.MaxSector, p => {
					var sector = FindSector(p);
					if ( sector != null ) {
						sector.Objects.Add(obj.Index);
					}
				});
			}
		}

		void LookUpSectorDepends(Vector3 num_pos, Action<SectorInfo> act) {
			var ms = FindSector(num_pos);
			if ( ms != null ) {
				act(ms);

				var s1 = FindSector(num_pos + new Vector3(-1,  0, 0));
				var s2 = FindSector(num_pos + new Vector3( 0, -1, 0));
				var s3 = FindSector(num_pos + new Vector3(-1, -1, 0));
				if ( s1 != null ) act(s1);
				if ( s2 != null ) act(s2);
				if ( s3 != null ) act(s3);
				
				for ( var i = 1; i <= _sectorsNum.z ; ++i ) {
					var ss1 = FindSector(num_pos + new Vector3( 0 - i,  0 - i, i + 1));
					var ss2 = FindSector(num_pos + new Vector3(-1 - i,  0 - i, i + 1));
					var ss3 = FindSector(num_pos + new Vector3( 0 - i, -1 - i, i + 1));
					var ss4 = FindSector(num_pos + new Vector3(-1 - i, -1 - i, i + 1));
					if ( ss1 != null ) act(ss1);
					if ( ss2 != null ) act(ss2);
					if ( ss3 != null ) act(ss3);
					if ( ss4 != null ) act(ss4);
				}
			}
		}

		void SetupObjectDepends() {
			_depends.Clear();
			foreach ( var obj_a in _objects ) {
				obj_a.Init(_depends.Count);
				var obj_ao = obj_a.IsoObject;
				IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, p => {
					LookUpSectorDepends(p, sec => {
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

		bool IsDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			var a_max = a_min + a_size;
			var b_max = b_min + b_size;
			return a_max.x > b_min.x && a_max.y > b_min.y && b_max.z > a_min.z;
		}

		void SetupAllObjects() {
			var depth = MinDepth;
			foreach ( var info in _objects ) {
				depth = PlaceObject(info, depth);
			}
			_objects.Clear();
			_depends.Clear();
		}
		
		void PlaceObject(IsoObject obj, float depth) {
			var trans = obj.gameObject.transform;
			trans.position = new Vector3(trans.position.x, trans.position.y, depth);
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
			if ( _dirty ) {
				SetupSectors();
				SetupObjectDepends();
				SetupAllObjects();
				_dirty = false;
			}
		}

		void Start() {
			ChangeSortingProperty();
			StepSort();
		}

		void LateUpdate() {
			if ( Application.isEditor ) {
				if ( _lastTileType != _tileType )                         TileType   = _tileType;
				if ( !Mathf.Approximately(_lastTileSize,   _tileSize  ) ) TileSize   = _tileSize;
				if ( !Mathf.Approximately(_lastMinDepth,   _minDepth  ) ) MinDepth   = _minDepth;
				if ( !Mathf.Approximately(_lastMaxDepth,   _maxDepth  ) ) MaxDepth   = _maxDepth;
				if ( !Mathf.Approximately(_lastSectorSize, _sectorSize) ) SectorSize = _sectorSize;
			}
			StepSort();
		}

		void OnEnable() {
			MarkDirty();
		}

		void OnDisable() {
			ApplyToAllIsoObjects(obj => obj.ResetIsoWorld());
		}
	}
} // namespace IsoTools