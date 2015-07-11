using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class IsoObject : MonoBehaviour {

		// ------------------------------------------------------------------------
		//
		// size
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _size = Vector3.one;
		
		/// <summary>Isometric object size.</summary>
		public Vector3 size {
			get { return _size; }
			set {
				_size = IsoUtils.Vec3Max(value, Vector3.zero);
				FixTransform();
			}
		}
		
		/// <summary>Isometric object size X.</summary>
		public float sizeX {
			get { return size.x; }
			set { size = IsoUtils.Vec3ChangeX(size, value); }
		}
		
		/// <summary>Isometric object size Y.</summary>
		public float sizeY {
			get { return size.y; }
			set { size = IsoUtils.Vec3ChangeY(size, value); }
		}
		
		/// <summary>Isometric object size Z.</summary>
		public float sizeZ {
			get { return size.z; }
			set { size = IsoUtils.Vec3ChangeZ(size, value); }
		}
		
		/// <summary>Isometric object size XY.</summary>
		public Vector2 sizeXY {
			get { return new Vector2(sizeX, sizeY); }
		}
		
		/// <summary>Isometric object size YZ.</summary>
		public Vector2 sizeYZ {
			get { return new Vector2(sizeY, sizeZ); }
		}
		
		/// <summary>Isometric object size XZ.</summary>
		public Vector2 sizeXZ {
			get { return new Vector2(sizeX, sizeZ); }
		}

		// ------------------------------------------------------------------------
		//
		// position
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _position = Vector3.zero;
		
		/// <summary>Isometric object position.</summary>
		public Vector3 position {
			get { return _position; }
			set {
				_position = value;
				FixTransform();
			}
		}
		
		/// <summary>Isometric object position X.</summary>
		public float positionX {
			get { return position.x; }
			set { position = IsoUtils.Vec3ChangeX(position, value); }
		}
		
		/// <summary>Isometric object position Y.</summary>
		public float positionY {
			get { return position.y; }
			set { position = IsoUtils.Vec3ChangeY(position, value); }
		}
		
		/// <summary>Isometric object position Z.</summary>
		public float positionZ {
			get { return position.z; }
			set { position = IsoUtils.Vec3ChangeZ(position, value); }
		}
		
		/// <summary>Isometric object position XY.</summary>
		public Vector2 positionXY {
			get { return new Vector2(positionX, positionY); }
		}
		
		/// <summary>Isometric object position YZ.</summary>
		public Vector2 positionYZ {
			get { return new Vector2(positionY, positionZ); }
		}
		
		/// <summary>Isometric object position XZ.</summary>
		public Vector2 positionXZ {
			get { return new Vector2(positionX, positionZ); }
		}

		// ------------------------------------------------------------------------
		//
		// tilePosition
		//
		// ------------------------------------------------------------------------

		/// <summary>Isometric object tile position.</summary>
		public Vector3 tilePosition {
			get { return IsoUtils.Vec3Round(position); }
			set { position = value; }
		}

		/// <summary>Isometric object tile position X.</summary>
		public float tilePositionX {
			get { return tilePosition.x; }
			set { tilePosition = IsoUtils.Vec3ChangeX(tilePosition, value); }
		}

		/// <summary>Isometric object tile position Y.</summary>
		public float tilePositionY {
			get { return tilePosition.y; }
			set { tilePosition = IsoUtils.Vec3ChangeY(tilePosition, value); }
		}

		/// <summary>Isometric object tile position Z.</summary>
		public float tilePositionZ {
			get { return tilePosition.z; }
			set { tilePosition = IsoUtils.Vec3ChangeZ(tilePosition, value); }
		}

		/// <summary>Isometric object tile position XY.</summary>
		public Vector2 tilePositionXY {
			get { return new Vector2(tilePositionX, tilePositionY); }
		}

		/// <summary>Isometric object tile position YZ.</summary>
		public Vector2 tilePositionYZ {
			get { return new Vector2(tilePositionY, tilePositionZ); }
		}

		/// <summary>Isometric object tile position XZ.</summary>
		public Vector2 tilePositionXZ {
			get { return new Vector2(tilePositionX, tilePositionZ); }
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

		[SerializeField] bool _isAlignment  = true;
		[SerializeField] bool _isShowBounds = false;

		public bool isAlignment {
			get { return _isAlignment; }
		}

		public bool isShowBounds {
			get { return _isShowBounds; }
		}
		#endif

		// ------------------------------------------------------------------------
		//
		// Functions
		//
		// ------------------------------------------------------------------------

		IsoWorld _isoWorld = null;
		public IsoWorld isoWorld {
			get {
				if ( (object)_isoWorld == null ) {
					_isoWorld = GameObject.FindObjectOfType<IsoWorld>();
				}
				if ( (object)_isoWorld == null ) {
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
			if ( Application.isEditor && !Application.isPlaying && isAlignment ) {
				_position = tilePosition;
			}
		#endif
			transform.position = IsoUtils.Vec3ChangeZ(
				isoWorld.IsoToScreen(position),
				transform.position.z);
			FixLastProperties();
			MartDirtyIsoWorld();
			MarkEditorObjectDirty();
		}

		public void FixIsoPosition() {
			position = isoWorld.ScreenToIso(
				transform.position,
				positionZ);
		}

		void FixLastProperties() {
		#if UNITY_EDITOR
			_lastSize      = size;
			_lastPosition  = position;
			_lastTransform = transform.position;
		#endif
		}

		void MartDirtyIsoWorld() {
			isoWorld.MarkDirty(this);
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
		void Reset() {
			size     = Vector3.one;
			position = Vector3.zero;
		}

		void OnValidate() {
			size     = _size;
			position = _position;
		}

		void OnDrawGizmos() {
			if ( isShowBounds && isoWorld ) {
				IsoUtils.DrawCube(isoWorld, position + size * 0.5f, size, Color.red);
			}
		}

		void Update() {
			if ( Application.isEditor ) {
				if ( !IsoUtils.Vec3Approximately(_lastSize, _size) ) {
					size = _size;
				}
				if ( !IsoUtils.Vec3Approximately(_lastPosition, _position) ) {
					position = _position;
				}
				if ( !IsoUtils.Vec2Approximately(_lastTransform, transform.position) ) {
					FixIsoPosition();
				}
			}
		}
		#endif
	}
} // namespace IsoTools