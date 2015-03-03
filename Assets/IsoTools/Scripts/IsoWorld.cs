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
			public Vector3          Position = Vector3.zero;
			public List<int>        Objects  = new List<int>();
			public List<SectorInfo> Depends  = new List<SectorInfo>();
			public SectorInfo(Vector3 position) {
				Position = position;
			}
		}

		bool             _dirty          = true;
		Vector3          _sectorsNum     = Vector3.zero;
		List<SectorInfo> _sectors        = new List<SectorInfo>();
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

		SectorInfo FindSector(Vector3 pos) {
			if ( pos.x < 0 || pos.y < 0 || pos.z < 0 ) {
				return null;
			}
			if ( pos.x >= _sectorsNum.x || pos.y >= _sectorsNum.y || pos.z >= _sectorsNum.z ) {
				return null;
			}
			return _sectors[SectorIndex(pos)];
		}

		void SetupSectorDepends() {
			IsoUtils.LookUpCube(Vector3.zero, _sectorsNum, p => {
				var ms = FindSector(p);
				if ( ms != null ) {
					ms.Depends.Add(ms);

					var s1 = FindSector(p + new Vector3(-1,  0, 0));
					var s2 = FindSector(p + new Vector3( 0, -1, 0));
					var s3 = FindSector(p + new Vector3(-1, -1, 0));
					if ( s1 != null ) ms.Depends.Add(s1);
					if ( s2 != null ) ms.Depends.Add(s2);
					if ( s3 != null ) ms.Depends.Add(s3);

					for ( var i = 1; i < _sectorsNum.z; ++i ) {
						var ss1 = FindSector(p + new Vector3( 0 - i,  0 - i, i + 1));
						var ss2 = FindSector(p + new Vector3(-1 - i,  0 - i, i + 1));
						var ss3 = FindSector(p + new Vector3( 0 - i, -1 - i, i + 1));
						var ss4 = FindSector(p + new Vector3(-1 - i, -1 - i, i + 1));
						if ( ss1 != null ) ms.Depends.Add(ss1);
						if ( ss2 != null ) ms.Depends.Add(ss2);
						if ( ss3 != null ) ms.Depends.Add(ss3);
						if ( ss4 != null ) ms.Depends.Add(ss4);
					}
				}
			});
		}

		void SetupObjectDepends() {
			_depends.Clear();
			foreach ( var obj_a in _objects ) {
				obj_a.Init(_depends.Count);
				var obj_ao = obj_a.IsoObject;
				IsoUtils.LookUpCube(obj_a.MinSector, obj_a.MaxSector, p => {
					var index = SectorIndex(p);
					foreach ( var sec in _sectors[index].Depends ) {
						foreach ( var obj_bi in sec.Objects ) {
							var obj_bo = _objects[obj_bi].IsoObject;
							if ( obj_ao != obj_bo && IsDepends(obj_ao.Position, obj_ao.Size, obj_bo.Position, obj_bo.Size) ) {
								_depends.Add(obj_bi);
								++obj_a.EndDepend;
							}
						}
					}
				});
				//Debug.LogFormat("{0} --- {1}" , obj_a.IsoObject.Position, obj_a.EndDepend - obj_a.BeginDepend);
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
				//_dirty = false;
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