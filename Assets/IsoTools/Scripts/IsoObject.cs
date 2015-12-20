using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class IsoObject : MonoBehaviour {

		// ------------------------------------------------------------------------
		//
		// Mode
		//
		// ------------------------------------------------------------------------

		public enum Mode {
			Mode2d,
			Mode3d
		}

		[SerializeField]
		Mode _mode = Mode.Mode2d;

		public Mode mode {
			get { return _mode; }
			set {
				_mode = value;
				FixTransform();
			}
		}

		// ------------------------------------------------------------------------
		//
		// Size
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _size = Vector3.one;

		public Vector3 size {
			get { return _size; }
			set {
				_size = IsoUtils.Vec3Max(value, Vector3.zero);
				FixTransform();
			}
		}

		public float sizeX {
			get { return size.x; }
			set { size = IsoUtils.Vec3ChangeX(size, value); }
		}

		public float sizeY {
			get { return size.y; }
			set { size = IsoUtils.Vec3ChangeY(size, value); }
		}

		public float sizeZ {
			get { return size.z; }
			set { size = IsoUtils.Vec3ChangeZ(size, value); }
		}

		public Vector2 sizeXY {
			get { return new Vector2(sizeX, sizeY); }
		}

		public Vector2 sizeYZ {
			get { return new Vector2(sizeY, sizeZ); }
		}

		public Vector2 sizeXZ {
			get { return new Vector2(sizeX, sizeZ); }
		}

		// ------------------------------------------------------------------------
		//
		// Position
		//
		// ------------------------------------------------------------------------

		[SerializeField]
		Vector3 _position = Vector3.zero;

		public Vector3 position {
			get { return _position; }
			set {
				_position = value;
				FixTransform();
			}
		}

		public float positionX {
			get { return position.x; }
			set { position = IsoUtils.Vec3ChangeX(position, value); }
		}

		public float positionY {
			get { return position.y; }
			set { position = IsoUtils.Vec3ChangeY(position, value); }
		}

		public float positionZ {
			get { return position.z; }
			set { position = IsoUtils.Vec3ChangeZ(position, value); }
		}

		public Vector2 positionXY {
			get { return new Vector2(positionX, positionY); }
		}

		public Vector2 positionYZ {
			get { return new Vector2(positionY, positionZ); }
		}

		public Vector2 positionXZ {
			get { return new Vector2(positionX, positionZ); }
		}

		// ------------------------------------------------------------------------
		//
		// TilePosition
		//
		// ------------------------------------------------------------------------

		public Vector3 tilePosition {
			get { return IsoUtils.Vec3Round(position); }
			set { position = value; }
		}

		public float tilePositionX {
			get { return tilePosition.x; }
			set { tilePosition = IsoUtils.Vec3ChangeX(tilePosition, value); }
		}

		public float tilePositionY {
			get { return tilePosition.y; }
			set { tilePosition = IsoUtils.Vec3ChangeY(tilePosition, value); }
		}

		public float tilePositionZ {
			get { return tilePosition.z; }
			set { tilePosition = IsoUtils.Vec3ChangeZ(tilePosition, value); }
		}

		public Vector2 tilePositionXY {
			get { return new Vector2(tilePositionX, tilePositionY); }
		}

		public Vector2 tilePositionYZ {
			get { return new Vector2(tilePositionY, tilePositionZ); }
		}

		public Vector2 tilePositionXZ {
			get { return new Vector2(tilePositionX, tilePositionZ); }
		}

		// ------------------------------------------------------------------------
		//
		// Internal
		//
		// ------------------------------------------------------------------------

		public class InternalState {
			public bool               Dirty        = false;
			public bool               Visited      = false;
			public Rect               ScreenRect   = new Rect();
			public Bounds             Bounds3d     = new Bounds();
			public float              Offset3d     = 0.0f;
			public Vector2            MinSector    = Vector2.zero;
			public Vector2            MaxSector    = Vector2.zero;
			public HashSet<IsoObject> SelfDepends  = new HashSet<IsoObject>();
			public HashSet<IsoObject> TheirDepends = new HashSet<IsoObject>();
		}

		public InternalState Internal = new InternalState();

		// ------------------------------------------------------------------------
		//
		// For editor
		//
		// ------------------------------------------------------------------------

		#if UNITY_EDITOR
		Vector3 _lastSize     = Vector3.zero;
		Vector3 _lastPosition = Vector3.zero;
		Vector2 _lastTransPos = Vector2.zero;

		[SerializeField] bool _isAlignment  = true;
		[SerializeField] bool _isShowBounds = false;

		public bool isAlignment {
			get { return _isAlignment; }
			set { _isAlignment = value; }
		}

		public bool isShowBounds {
			get { return _isShowBounds; }
			set { _isShowBounds = value; }
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
				if ( !_isoWorld && gameObject.activeInHierarchy ) {
					_isoWorld = GameObject.FindObjectOfType<IsoWorld>();
				}
				return _isoWorld;
			}
		}

		public void FixTransform() {
		#if UNITY_EDITOR
			if ( !Application.isPlaying && isAlignment ) {
				_position = tilePosition;
			}
		#endif
			if ( isoWorld ) {
				transform.position = IsoUtils.Vec3ChangeZ(
					isoWorld.IsoToScreen(position),
					transform.position.z);
				FixScreenRect();
			}
			FixLastProperties();
			MartDirtyIsoWorld();
		}

		public void FixIsoPosition() {
			if ( isoWorld ) {
				position = isoWorld.ScreenToIso(
					transform.position,
					positionZ);
			}
		}

		void FixScreenRect() {
			if ( isoWorld ) {
				var l = isoWorld.IsoToScreen(position + IsoUtils.Vec3FromY(size.y)).x;
				var r = isoWorld.IsoToScreen(position + IsoUtils.Vec3FromX(size.x)).x;
				var b = isoWorld.IsoToScreen(position).y;
				var t = isoWorld.IsoToScreen(position + size).y;
				Internal.ScreenRect = new Rect(l, b, r - l, t - b);
			}
		}

		void FixLastProperties() {
		#if UNITY_EDITOR
			_lastSize     = size;
			_lastPosition = position;
			_lastTransPos = transform.position;
		#endif
		}

		void MartDirtyIsoWorld() {
			if ( isoWorld ) {
				isoWorld.MarkDirty(this);
			}
		#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
		#endif
		}

		// ------------------------------------------------------------------------
		//
		// Messages
		//
		// ------------------------------------------------------------------------

		void Awake() {
			Internal.SelfDepends = new HashSet<IsoObject>(new IsoObject[47]);
			Internal.SelfDepends.Clear();
			Internal.TheirDepends = new HashSet<IsoObject>(new IsoObject[47]);
			Internal.TheirDepends.Clear();
			FixLastProperties();
			FixIsoPosition();
		}

		void OnEnable() {
			if ( isoWorld ) {
				isoWorld.AddIsoObject(this);
			}
			MartDirtyIsoWorld();
		}

		void OnDisable() {
			if ( isoWorld ) {
				isoWorld.RemoveIsoObject(this);
			}
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
			if ( !IsoUtils.Vec3Approximately(_lastSize, _size) ) {
				size = _size;
			}
			if ( !IsoUtils.Vec3Approximately(_lastPosition, _position) ) {
				position = _position;
			}
			if ( !IsoUtils.Vec2Approximately(_lastTransPos, transform.position) ) {
				FixIsoPosition();
			}
		}
		#endif
	}
} // namespace IsoTools