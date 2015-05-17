using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoObject : MonoBehaviour {

		#if UNITY_EDITOR
		Vector2 _lastTransform = Vector2.zero;
		Vector3 _lastPosition  = Vector3.zero;
		Vector3 _lastSize      = Vector3.zero;
		#endif

		[SerializeField]
		Vector3 _position = Vector3.zero;

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

		/// <summary>Isometric object position.</summary>
		public Vector3 Position {
			get { return _position; }
			set {
				_position = value;
				FixTransform();
			}
		}

		[SerializeField]
		Vector3 _size = Vector3.one;

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

		/// <summary>Isometric object size.</summary>
		public Vector3 Size {
			get { return _size; }
			set {
				_size = IsoUtils.Vec3Max(value, Vector3.zero);
				FixTransform();
			}
		}

		[SerializeField]
		/// <summary>Isometric object tile position.</summary>
		public Vector3 TilePosition {
			get {
				return new Vector3(
					Mathf.Round(Position.x),
					Mathf.Round(Position.y),
					Mathf.Round(Position.z));
			}
			set { Position = value; }
		}

		IsoWorld _iso_world = null;
		public IsoWorld IsoWorld {
			get {
				if ( !_iso_world ) {
					_iso_world = GameObject.FindObjectOfType<IsoWorld>();
				}
				if ( !_iso_world ) {
					throw new UnityException("IsoObject. IsoWorld not found!");
				}
				return _iso_world;
			}
		}

		public void ResetIsoWorld() {
			_iso_world = null;
		}

		public void FixTransform() {
			Vector3 trans = IsoWorld.IsoToScreen(Position);
			trans.z = transform.position.z;
			transform.position = trans;
			FixLastProperties();
			MartDirtyIsoWorld();
			MarkEditorObjectDirty();
		}

		public void FixIsoPosition() {
			Vector2 trans = transform.position;
			Position = IsoWorld.ScreenToIso(trans, Position.z);
		}

		void FixLastProperties() {
		#if UNITY_EDITOR
			_lastTransform = transform.position;
			_lastPosition = Position;
			_lastSize = Size;
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

		void OnBecameVisible() {
			MartDirtyIsoWorld();
		}

		#if UNITY_EDITOR
		void Update() {
			if ( Application.isEditor ) {
				if ( !Mathf.Approximately(_lastTransform.x, transform.position.x) ||
				     !Mathf.Approximately(_lastTransform.y, transform.position.y))
				{
					FixIsoPosition();
				}
				if ( _lastPosition != _position ) Position = _position;
				if ( _lastSize     != _size     ) Size     = _size;
			}
		}
		#endif
	}
} // namespace IsoTools