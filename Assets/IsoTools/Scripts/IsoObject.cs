using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

namespace IsoTools {
	[SelectionBase]
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoObject : IsoObjectBase {

		// ---------------------------------------------------------------------
		//
		// Size
		//
		// ---------------------------------------------------------------------

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

		// ---------------------------------------------------------------------
		//
		// Position
		//
		// ---------------------------------------------------------------------

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
			set { position = IsoUtils.Vec3ChangeXY(position, value.x, value.y); }
		}

		public Vector2 positionYZ {
			get { return new Vector2(positionY, positionZ); }
			set { position = IsoUtils.Vec3ChangeYZ(position, value.x, value.y); }
		}

		public Vector2 positionXZ {
			get { return new Vector2(positionX, positionZ); }
			set { position = IsoUtils.Vec3ChangeXZ(position, value.x, value.y); }
		}

		// ---------------------------------------------------------------------
		//
		// TilePosition
		//
		// ---------------------------------------------------------------------

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

		// ---------------------------------------------------------------------
		//
		// Renderers mode
		//
		// ---------------------------------------------------------------------

		public enum RenderersMode {
			Mode2d,
			Mode3d
		}

		[SerializeField]
		RenderersMode _renderersMode = RenderersMode.Mode2d;

		public RenderersMode renderersMode {
			get { return _renderersMode; }
			set {
				_renderersMode = value;
				FixTransform();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Cache renderers
		//
		// ---------------------------------------------------------------------

		[SerializeField]
		bool _cachedRenderers = false;

		public bool isCachedRenderers {
			get { return _cachedRenderers; }
			set {
				_cachedRenderers = value;
				if ( value ) {
					UpdateCachedRenderers();
				} else {
					ClearCachedRenderers();
				}
				FixTransform();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public class InternalState {
			public bool                         Dirty        = true;
			public bool                         Placed       = true;
			public IsoQuadTree<IsoObject>.IItem QTItem       = null;
			public IsoRect                      QTBounds     = IsoRect.zero;
			public IsoMinMax                    MinMax3d     = IsoMinMax.zero;
			public float                        Offset3d     = 0.0f;
			public Transform                    Transform    = null;
			public List<Renderer>               Renderers    = new List<Renderer>();
			public IsoAssocList<IsoObject>      SelfDepends  = new IsoAssocList<IsoObject>(47);
			public IsoAssocList<IsoObject>      TheirDepends = new IsoAssocList<IsoObject>(47);
		}

		public InternalState Internal = new InternalState();

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public void FixTransform() {
			var iso_world = isoWorld;
			var cached_transform = FixCachedTransform();
			if ( iso_world && cached_transform ) {
				cached_transform.position = IsoUtils.Vec3ChangeZ(
					iso_world.IsoToScreen(position),
					cached_transform.position.z);
				FixScreenBounds();
				MartDirtyIsoWorld();
			}
		}

		public void FixIsoPosition() {
			var iso_world = isoWorld;
			var cached_transform = FixCachedTransform();
			if ( iso_world && cached_transform ) {
				position = iso_world.ScreenToIso(
					cached_transform.position,
					positionZ);
			}
		}

		public void UpdateCachedRenderers() {
			GetComponentsInChildren<Renderer>(Internal.Renderers);
		}

		public void ClearCachedRenderers() {
			Internal.Renderers.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void FixScreenBounds() {
			var iso_world = isoWorld;
			if ( iso_world ) {
				var l = iso_world.IsoToScreen(position + IsoUtils.Vec3FromY(size.y)).x;
				var r = iso_world.IsoToScreen(position + IsoUtils.Vec3FromX(size.x)).x;
				var b = iso_world.IsoToScreen(position).y;
				var t = iso_world.IsoToScreen(position + size).y;
				Internal.QTBounds.Set(l, b, r, t);
			} else {
				Internal.QTBounds.Set(0.0f, 0.0f, 0.0f, 0.0f);
			}
		}

		Transform FixCachedTransform() {
			var ret_value = Internal.Transform;
			if ( !ret_value ) {
				ret_value = Internal.Transform = transform;
			}
			return ret_value;
		}

		void MartDirtyIsoWorld() {
			var iso_world = isoWorld;
			if ( iso_world ) {
				iso_world.Internal_MarkDirty(this);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Awake() {
			FixCachedTransform();
			FixTransform();
		}

		protected override void OnEnable() {
			base.OnEnable();
			MartDirtyIsoWorld();
		}

		protected override void OnDisable() {
			base.OnDisable();
		}

		protected override void OnTransformParentChanged() {
			base.OnTransformParentChanged();
			FixCachedTransform();
			FixTransform();
		}

	#if UNITY_EDITOR
		void Reset() {
			size              = Vector3.one;
			position          = Vector3.zero;
			renderersMode     = RenderersMode.Mode2d;
			isCachedRenderers = false;
		}

		void OnValidate() {
			size              = _size;
			position          = _position;
			renderersMode     = _renderersMode;
			isCachedRenderers = _cachedRenderers;
		}

		void OnDrawGizmos() {
			var iso_world = isoWorld;
			if ( iso_world ) {
				if ( iso_world.isShowIsoBounds && iso_world.Internal_IsVisible(this) ) {
					IsoUtils.DrawIsoCube(
						iso_world,
						position + size * 0.5f,
						size,
						Color.red);
				}
				if ( iso_world.isShowScreenBounds && iso_world.Internal_IsVisible(this) ) {
					IsoUtils.DrawRect(
						Internal.QTBounds,
						Color.green);
				}
			}
		}

		void OnDrawGizmosSelected() {
			var iso_world = isoWorld;
			if ( iso_world && iso_world.isShowDepends && iso_world.Internal_IsVisible(this) ) {
				for ( int i = 0, e = Internal.SelfDepends.Count; i < e; ++i ) {
					IsoUtils.DrawLine(
						Internal.QTBounds.center,
						Internal.SelfDepends[i].Internal.QTBounds.center,
						Color.yellow,
						Color.cyan,
						0.25f);
				}
				for ( int i = 0, e = Internal.TheirDepends.Count; i < e; ++i ) {
					IsoUtils.DrawLine(
						Internal.QTBounds.center,
						Internal.TheirDepends[i].Internal.QTBounds.center,
						Color.yellow,
						Color.cyan,
						0.75f);
				}
			}
		}
	#endif
	}
}