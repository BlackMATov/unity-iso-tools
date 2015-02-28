using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

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

			public ObjectInfo(int index, IsoObject iso_object, Vector3 minSector, Vector3 maxSector) {
				Index     = index;
				IsoObject = iso_object;
				MinSector = minSector;
				MaxSector = maxSector;
			}

			public void Init(int first_depend) {
				Visited     = false;
				BeginDepend = first_depend;
				EndDepend   = first_depend;
			}
		}

		class SectorInfo {
			public Vector3   Position = Vector3.zero;
			public List<int> Objects  = new List<int>();
			public List<int> Depends  = new List<int>();
			public SectorInfo(Vector3 position) {
				Position = position;
			}
		}

		bool             _dirty          = true;
		List<SectorInfo> _sectors        = new List<SectorInfo>();
		Vector3          _sectorsNum     = Vector3.zero;
		List<ObjectInfo> _objects        = new List<ObjectInfo>();
		List<int>        _depends        = new List<int>();

		TileTypes        _lastTileType   = TileTypes.Isometric;
		float            _lastTileSize   = 0.0f;
		float            _lastMinDepth   = 0.0f;
		float            _lastMaxDepth   = 0.0f;
		float            _lastSectorSize = 0.0f;

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
		public float _sectorSize = 5.0f;
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
		}

		// ------------------------------------------------------------------------
		/// <summary>
		/// Marks world for resorting one object only
		/// </summary>
		/// <param name="obj">Isometric object for resorting.</param>
		// ------------------------------------------------------------------------ 
		public void MarkDirty(IsoObject obj) {
			if ( obj && obj.Sorting ) {
				_dirty = true;
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
			return Mathf.RoundToInt(
				num_pos.z * _sectorsNum.x * _sectorsNum.y +
				num_pos.y * _sectorsNum.x +
				num_pos.x);
		}

		Vector3 SectorNumPos(int index) {
			var z = Mathf.FloorToInt(index / (_sectorsNum.x * _sectorsNum.y));
			var y = Mathf.FloorToInt((index - (z * _sectorsNum.x * _sectorsNum.y)) / _sectorsNum.x);
			var x = index - (z * _sectorsNum.x * _sectorsNum.y) - (y * _sectorsNum.x);
			return new Vector3(x, y, z);
		}

		void SetupSectors() {
			var iso_objects = GameObject.FindObjectsOfType<IsoObject>();

			var min = Vector3.zero;
			var max = Vector3.one;
			foreach ( var iso_object in iso_objects ) {
				min = IsoUtils.Vec3Min(min, iso_object.Position);
				max = IsoUtils.Vec3Max(max, iso_object.Position + iso_object.Size);
			}

			_sectorsNum = IsoUtils.Vec3DivCeil(max - min, SectorSize);

			_sectors.Clear();
			_sectors.Capacity = Mathf.CeilToInt(_sectorsNum.x * _sectorsNum.y * _sectorsNum.z);
			while ( _sectors.Count < _sectors.Capacity ) {
				var num_pos = SectorNumPos(_sectors.Count);
				_sectors.Add(new SectorInfo(num_pos * SectorSize));
			}

			_objects.Clear();
			foreach ( var iso_object in iso_objects ) {
				var max_size   = IsoUtils.Vec3Max(iso_object.Size, Vector3.one);
				var min_sector = IsoUtils.Vec3DivFloor(iso_object.Position - min, SectorSize);
				var max_sector = IsoUtils.Vec3DivCeil(iso_object.Position + max_size - min, SectorSize);

				var obj_info = new ObjectInfo(_objects.Count, iso_object, min_sector, max_sector);
				_objects.Add(obj_info);

				IsoUtils.LookUpCube(min_sector, max_sector, p => {
					var index = SectorIndex(p);
					_sectors[index].Objects.Add(obj_info.Index);
					
				});
			}
		}

		void SetupSectorDepends() {
			foreach ( var sec_a in _sectors ) {
				for ( var i = 0; i < _sectors.Count; ++i ) {
					var sec_b = _sectors[i];
					if ( sec_a == sec_b || IsDepends(sec_a.Position, sec_b.Position, SectorSize) ) {
						sec_a.Depends.Add(i);
					}
				}
			}
		}

		void SetupObjectDepends() {
			_depends.Clear();
			foreach ( var obj_a in _objects ) {
				obj_a.Init(_depends.Count);
				var obj_ao = obj_a.IsoObject;
				IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, p => {
					var index = SectorIndex(p);
					foreach ( var sec_i in _sectors[index].Depends ) {
						var sec = _sectors[sec_i];
						foreach ( var obj_bi in sec.Objects ) {
							var obj_bo = _objects[obj_bi].IsoObject;
							if ( obj_ao != obj_bo && IsDepends(obj_ao.Position, obj_ao.Size, obj_bo.Position, obj_bo.Size) ) {
								_depends.Add(obj_bi);
								++obj_a.EndDepend;
							}
						}
					}
				});
			}
		}

		bool IsDepends(Vector3 a_pos, Vector3 b_pos, float size) {
			return IsDepends(
				a_pos, new Vector3(size, size, size),
				b_pos, new Vector3(size, size, size));
		}

		bool IsDepends(Vector3 a_min, Vector3 a_size, Vector3 b_min, Vector3 b_size) {
			var a_max = a_min + a_size;
			var b_max = b_min + b_size;
			return
				a_max.x > b_min.x && a_min.x < b_max.x + 1.0f &&
				a_max.y > b_min.y && a_min.y < b_max.y + 1.0f &&
				b_max.z > a_min.z && b_min.z < a_max.z + 1.0f;
		}

		void SetupAllObjects() {
			var depth = MinDepth;
			foreach ( var info in _objects ) {
				depth = PlaceObject(info, depth);
			}
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
				SetupSectorDepends();
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
				if ( _lastTileType != _tileType )                          TileType   = _tileType;
				if ( !Mathf.Approximately(_lastTileSize,   _tileSize   ) ) TileSize   = _tileSize;
				if ( !Mathf.Approximately(_lastMinDepth,   _minDepth   ) ) MinDepth   = _minDepth;
				if ( !Mathf.Approximately(_lastMaxDepth,   _maxDepth   ) ) MaxDepth   = _maxDepth;
				if ( !Mathf.Approximately(_lastSectorSize, _sectorSize ) ) SectorSize = _sectorSize;
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