using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools {
	[ExecuteInEditMode]
	public class IsoObject : MonoBehaviour {

		Vector2   _lastTransform = Vector2.zero;
		Vector3   _lastPosition  = Vector3.zero;
		Vector3   _lastSize      = Vector3.zero;
		bool      _lastSorting   = false;
		bool      _lastAlignment = false;

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

		[SerializeField]
		Vector3 _size = Vector3.one;
		/// <summary>Isometric object size.</summary>
		public Vector3 Size {
			get { return _size; }
			set {
				_size = value;
				FixTransform();
			}
		}

		[SerializeField]
		bool _sorting = true;
		/// <summary>Auto sorting tile.</summary>
		public bool Sorting {
			get { return _sorting; }
			set {
				_sorting = value;
				FixTransform();
			}
		}

		[SerializeField]
		bool _alignment = true;
		/// <summary>Auto alignment position by isometric tile size.</summary>
		public bool Alignment {
			get { return _alignment; }
			set {
				_alignment = value;
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
			if ( Application.isEditor && Alignment ) {
				_position = TilePosition;
			}
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
			_lastTransform = transform.position;
			_lastPosition = Position;
			_lastSize = Size;
			_lastSorting = Sorting;
			_lastAlignment = Alignment;
		}

		void MartDirtyIsoWorld() {
			if ( Sorting ) {
				IsoWorld.MarkDirty(this);
			}
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
		
		void Update() {
			var trans_pos = transform.position;
			if ( !Mathf.Approximately(_lastTransform.x, trans_pos.x) ||
			     !Mathf.Approximately(_lastTransform.y, trans_pos.y) )
			{
				FixIsoPosition();
			}
			if ( Application.isEditor ) {
				if ( _lastPosition  != _position  ) Position  = _position;
				if ( _lastSize      != _size      ) Size      = _size;
				if ( _lastSorting   != _sorting   ) Sorting   = _sorting;
				if ( _lastAlignment != _alignment ) Alignment = _alignment;
			}
		}

		void OnEnable() {
			MartDirtyIsoWorld();
		}
	}
} // namespace IsoTools