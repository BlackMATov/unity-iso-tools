using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoObject : MonoBehaviour {

		// ------------------------------------------------------------------------
		//
		// Size
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _size = Vector3.one;
		
		/// <summary>Isometric object size.</summary>
		public Vector3 Size {
			get { return _size; }
			set {
				_size = IsoUtils.Vec3Max(value, Vector3.zero);
				FixTransform();
			}
		}
		
		/// <summary>Isometric object size X.</summary>
		public float SizeX {
			get { return Size.x; }
			set { Size = IsoUtils.Vec3ChangeX(Size, value); }
		}
		
		/// <summary>Isometric object size Y.</summary>
		public float SizeY {
			get { return Size.y; }
			set { Size = IsoUtils.Vec3ChangeY(Size, value); }
		}
		
		/// <summary>Isometric object size Z.</summary>
		public float SizeZ {
			get { return Size.z; }
			set { Size = IsoUtils.Vec3ChangeZ(Size, value); }
		}
		
		/// <summary>Isometric object size XY.</summary>
		public Vector2 SizeXY {
			get { return new Vector2(SizeX, SizeY); }
		}
		
		/// <summary>Isometric object size YZ.</summary>
		public Vector2 SizeYZ {
			get { return new Vector2(SizeY, SizeZ); }
		}
		
		/// <summary>Isometric object size XZ.</summary>
		public Vector2 SizeXZ {
			get { return new Vector2(SizeX, SizeZ); }
		}

		// ------------------------------------------------------------------------
		//
		// Position
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _position = Vector3.zero;
		
		/// <summary>Isometric object position.</summary>
		public Vector3 Position {
			get { return _position; }
			set {
				_position = value;
				FixTransform();
			}
		}
		
		/// <summary>Isometric object position X.</summary>
		public float PositionX {
			get { return Position.x; }
			set { Position = IsoUtils.Vec3ChangeX(Position, value); }
		}
		
		/// <summary>Isometric object position Y.</summary>
		public float PositionY {
			get { return Position.y; }
			set { Position = IsoUtils.Vec3ChangeY(Position, value); }
		}
		
		/// <summary>Isometric object position Z.</summary>
		public float PositionZ {
			get { return Position.z; }
			set { Position = IsoUtils.Vec3ChangeZ(Position, value); }
		}
		
		/// <summary>Isometric object position XY.</summary>
		public Vector2 PositionXY {
			get { return new Vector2(PositionX, PositionY); }
		}
		
		/// <summary>Isometric object position YZ.</summary>
		public Vector2 PositionYZ {
			get { return new Vector2(PositionY, PositionZ); }
		}
		
		/// <summary>Isometric object position XZ.</summary>
		public Vector2 PositionXZ {
			get { return new Vector2(PositionX, PositionZ); }
		}

		// ------------------------------------------------------------------------
		//
		// TilePosition
		//
		// ------------------------------------------------------------------------

		/// <summary>Isometric object tile position.</summary>
		public Vector3 TilePosition {
			get { return IsoUtils.Vec3Round(Position); }
			set { Position = value; }
		}

		/// <summary>Isometric object tile position X.</summary>
		public float TilePositionX {
			get { return TilePosition.x; }
			set { TilePosition = IsoUtils.Vec3ChangeX(TilePosition, value); }
		}

		/// <summary>Isometric object tile position Y.</summary>
		public float TilePositionY {
			get { return TilePosition.y; }
			set { TilePosition = IsoUtils.Vec3ChangeY(TilePosition, value); }
		}

		/// <summary>Isometric object tile position Z.</summary>
		public float TilePositionZ {
			get { return TilePosition.z; }
			set { TilePosition = IsoUtils.Vec3ChangeZ(TilePosition, value); }
		}

		/// <summary>Isometric object tile position XY.</summary>
		public Vector2 TilePositionXY {
			get { return new Vector2(TilePositionX, TilePositionY); }
		}

		/// <summary>Isometric object tile position YZ.</summary>
		public Vector2 TilePositionYZ {
			get { return new Vector2(TilePositionY, TilePositionZ); }
		}

		/// <summary>Isometric object tile position XZ.</summary>
		public Vector2 TilePositionXZ {
			get { return new Vector2(TilePositionX, TilePositionZ); }
		}

		// ------------------------------------------------------------------------
		//
		// For editor
		//
		// ------------------------------------------------------------------------

		#if UNITY_EDITOR
		Vector3 _lastSize      = Vector3.zero;
		Vector3 _lastPosition  = Vector3.zero;
		Vector2 _lastTransform = Vector2.zero;

		[SerializeField] bool _alignment  = true;
		[SerializeField] bool _showBounds = false;

		public bool Alignment {
			get { return _alignment; }
		}

		public bool ShowBounds {
			get { return _showBounds; }
		}
		#endif

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		IsoWorld _isoWorld = null;
		public IsoWorld IsoWorld {
			get {
				if ( !_isoWorld ) {
					_isoWorld = GameObject.FindObjectOfType<IsoWorld>();
				}
				if ( !_isoWorld ) {
					throw new UnityException("IsoObject. IsoWorld not found!");
				}
				return _isoWorld;
			}
		}

		public void ResetIsoWorld() {
			_isoWorld = null;
		}

		public void FixTransform() {
		#if UNITY_EDITOR
			if ( Application.isEditor && !Application.isPlaying && Alignment ) {
				_position = TilePosition;
			}
		#endif
			transform.position = IsoUtils.Vec3ChangeZ(
				IsoWorld.IsoToScreen(Position),
				transform.position.z);
			FixLastProperties();
			MartDirtyIsoWorld();
			MarkEditorObjectDirty();
		}

		public void FixIsoPosition() {
			Position = IsoWorld.ScreenToIso(
				transform.position,
				PositionZ);
		}

		void FixLastProperties() {
		#if UNITY_EDITOR
			_lastSize      = Size;
			_lastPosition  = Position;
			_lastTransform = transform.position;
		#endif
		}

		void MartDirtyIsoWorld() {
			IsoWorld.MarkDirty(this);
		}

		void MarkEditorObjectDirty() {
		#if UNITY_EDITOR
			if ( Application.isEditor ) {
				EditorUtility.SetDirty(this);
			}
		#endif
		}

		void Awake() {
			FixLastProperties();
			FixIsoPosition();
		}

		void OnEnable() {
			MartDirtyIsoWorld();
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			if ( ShowBounds ) {
				IsoUtils.DrawCube(Position, Size, Color.red);
			}
		}

		void Update() {
			if ( Application.isEditor ) {
				if ( !IsoUtils.Vec3Approximately(_lastSize, _size) ) {
					Size = _size;
				}
				if ( !IsoUtils.Vec3Approximately(_lastPosition, _position) ) {
					Position = _position;
				}
				if ( !IsoUtils.Vec2Approximately(_lastTransform, transform.position) ) {
					FixIsoPosition();
				}
			}
		}
		#endif
	}
} // namespace IsoTools