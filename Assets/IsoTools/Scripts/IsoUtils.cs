using UnityEngine;
using System;

namespace IsoTools {
	public static class IsoUtils {

		// ---------------------------------------------------------------------
		//
		// Consts
		//
		// ---------------------------------------------------------------------

		public static Vector3 Vec2OneX  { get { return new Vector2(1.0f, 0.0f); } }
		public static Vector3 Vec2OneY  { get { return new Vector2(0.0f, 1.0f); } }
		public static Vector3 Vec2OneXY { get { return new Vector2(1.0f, 1.0f); } }

		public static Vector3 Vec3OneX  { get { return new Vector3(1.0f, 0.0f, 0.0f); } }
		public static Vector3 Vec3OneY  { get { return new Vector3(0.0f, 1.0f, 0.0f); } }
		public static Vector3 Vec3OneZ  { get { return new Vector3(0.0f, 0.0f, 1.0f); } }
		public static Vector3 Vec3OneXY { get { return new Vector3(1.0f, 1.0f, 0.0f); } }
		public static Vector3 Vec3OneYZ { get { return new Vector3(0.0f, 1.0f, 1.0f); } }
		public static Vector3 Vec3OneXZ { get { return new Vector3(1.0f, 0.0f, 1.0f); } }

		// ---------------------------------------------------------------------
		//
		// Abs/Min/Max
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Abs
		// -----------------------------

		public static Vector2 Vec2Abs(Vector2 v) {
			return new Vector2(
				Mathf.Abs(v.x),
				Mathf.Abs(v.y));
		}
		
		public static Vector3 Vec3Abs(Vector3 v) {
			return new Vector3(
				Mathf.Abs(v.x),
				Mathf.Abs(v.y),
				Mathf.Abs(v.z));
		}

		// -----------------------------
		// Min
		// -----------------------------

		public static float Vec2MinF(Vector2 v) {
			return Mathf.Min(v.x, v.y);
		}

		public static float Vec3MinF(Vector3 v) {
			return Mathf.Min(v.x, v.y, v.z);
		}
		
		public static Vector2 Vec2Min(Vector2 a, Vector2 b) {
			return new Vector2(
				Mathf.Min(a.x, b.x),
				Mathf.Min(a.y, b.y));
		}
		
		public static Vector3 Vec3Min(Vector3 a, Vector3 b) {
			return new Vector3(
				Mathf.Min(a.x, b.x),
				Mathf.Min(a.y, b.y),
				Mathf.Min(a.z, b.z));
		}

		// -----------------------------
		// Max
		// -----------------------------

		public static float Vec2MaxF(Vector2 v) {
			return Mathf.Max(v.x, v.y);
		}

		public static float Vec3MaxF(Vector3 v) {
			return Mathf.Max(v.x, v.y, v.z);
		}

		public static Vector2 Vec2Max(Vector2 a, Vector2 b) {
			return new Vector2(
				Mathf.Max(a.x, b.x),
				Mathf.Max(a.y, b.y));
		}

		public static Vector3 Vec3Max(Vector3 a, Vector3 b) {
			return new Vector3(
				Mathf.Max(a.x, b.x),
				Mathf.Max(a.y, b.y),
				Mathf.Max(a.z, b.z));
		}

		// ---------------------------------------------------------------------
		//
		// Ceil/Floor/Round
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Ceil
		// -----------------------------

		public static Vector2 Vec2Ceil(Vector2 v) {
			return new Vector2(
				Mathf.Ceil(v.x),
				Mathf.Ceil(v.y));
		}

		public static Vector3 Vec3Ceil(Vector3 v) {
			return new Vector3(
				Mathf.Ceil(v.x),
				Mathf.Ceil(v.y),
				Mathf.Ceil(v.z));
		}

		// -----------------------------
		// Floor
		// -----------------------------

		public static Vector2 Vec2Floor(Vector2 v) {
			return new Vector2(
				Mathf.Floor(v.x),
				Mathf.Floor(v.y));
		}

		public static Vector3 Vec3Floor(Vector3 v) {
			return new Vector3(
				Mathf.Floor(v.x),
				Mathf.Floor(v.y),
				Mathf.Floor(v.z));
		}

		// -----------------------------
		// Round
		// -----------------------------

		public static Vector2 Vec2Round(Vector2 v) {
			return new Vector2(
				Mathf.Round(v.x),
				Mathf.Round(v.y));
		}

		public static Vector3 Vec3Round(Vector3 v) {
			return new Vector3(
				Mathf.Round(v.x),
				Mathf.Round(v.y),
				Mathf.Round(v.z));
		}

		// ---------------------------------------------------------------------
		//
		// Div/DivCeil/DivFloor/DivRound
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Div
		// -----------------------------

		public static Vector3 Vec3Div(Vector3 a, float b) {
			return new Vector3(
				a.x / b,
				a.y / b,
				a.z / b);
		}
		
		public static Vector3 Vec3Div(Vector3 a, Vector3 b) {
			return new Vector3(
				a.x / b.x,
				a.y / b.y,
				a.z / b.z);
		}

		// -----------------------------
		// DivCeil
		// -----------------------------

		public static Vector3 Vec3DivCeil(Vector3 a, float b) {
			return Vec3Ceil(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivCeil(Vector3 a, Vector3 b) {
			return Vec3Ceil(Vec3Div(a, b));
		}

		// -----------------------------
		// DivFloor
		// -----------------------------
		
		public static Vector3 Vec3DivFloor(Vector3 a, float b) {
			return Vec3Floor(Vec3Div(a, b));
		}
		
		public static Vector3 Vec3DivFloor(Vector3 a, Vector3 b) {
			return Vec3Floor(Vec3Div(a, b));
		}

		// -----------------------------
		// DivRound
		// -----------------------------
		
		public static Vector3 Vec3DivRound(Vector3 a, float b) {
			return Vec3Round(Vec3Div(a, b));
		}
		
		public static Vector3 Vec3DivRound(Vector3 a, Vector3 b) {
			return Vec3Round(Vec3Div(a, b));
		}

		// ---------------------------------------------------------------------
		//
		// Vec2From/Vec3From
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Vec2From
		// -----------------------------

		public static Vector2 Vec2FromX(float x) {
			return new Vector2(x, 0.0f);
		}
		
		public static Vector2 Vec2FromY(float y) {
			return new Vector2(0.0f, y);
		}
		
		public static Vector2 Vec2FromXY(float x, float y) {
			return new Vector2(x, y);
		}

		// -----------------------------
		// Vec3From
		// -----------------------------
		
		public static Vector3 Vec3FromX(float x) {
			return new Vector3(x, 0.0f, 0.0f);
		}
		
		public static Vector3 Vec3FromY(float y) {
			return new Vector3(0.0f, y, 0.0f);
		}
		
		public static Vector3 Vec3FromZ(float z) {
			return new Vector3(0.0f, 0.0f, z);
		}
		
		public static Vector3 Vec3FromXY(float x, float y) {
			return new Vector3(x, y, 0.0f);
		}
		
		public static Vector3 Vec3FromYZ(float y, float z) {
			return new Vector3(0.0f, y, z);
		}
		
		public static Vector3 Vec3FromXZ(float x, float z) {
			return new Vector3(x, 0.0f, z);
		}

		// ---------------------------------------------------------------------
		//
		// ChangeX
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Vec2Change
		// -----------------------------

		public static Vector2 Vec2ChangeX(Vector2 v, float x) {
			return new Vector2(x, v.y);
		}
		
		public static Vector2 Vec2ChangeY(Vector2 v, float y) {
			return new Vector2(v.x, y);
		}

		// -----------------------------
		// Vec3Change
		// -----------------------------
		
		public static Vector3 Vec3ChangeX(Vector3 v, float x) {
			return new Vector3(x, v.y, v.z);
		}
		
		public static Vector3 Vec3ChangeY(Vector3 v, float y) {
			return new Vector3(v.x, y, v.z);
		}
		
		public static Vector3 Vec3ChangeZ(Vector3 v, float z) {
			return new Vector3(v.x, v.y, z);
		}
		
		public static Vector3 Vec3ChangeXY(Vector3 v, float x, float y) {
			return new Vector3(x, y, v.z);
		}
		
		public static Vector3 Vec3ChangeYZ(Vector3 v, float y, float z) {
			return new Vector3(v.x, y, z);
		}
		
		public static Vector3 Vec3ChangeXZ(Vector3 v, float x, float z) {
			return new Vector3(x, v.y, z);
		}

		// ---------------------------------------------------------------------
		//
		// Approximately
		//
		// ---------------------------------------------------------------------

		public static bool Vec2Approximately(Vector2 a, Vector2 b) {
			return
				Mathf.Approximately(a.x, b.x) &&
				Mathf.Approximately(a.y, b.y);
		}

		public static bool Vec3Approximately(Vector3 a, Vector3 b) {
			return
				Mathf.Approximately(a.x, b.x) &&
				Mathf.Approximately(a.y, b.y) &&
				Mathf.Approximately(a.z, b.z);
		}

		// ---------------------------------------------------------------------
		//
		// LookUpCube
		//
		// ---------------------------------------------------------------------

		public static void LookUpCube(Vector2 min, Vector2 max, Action<Vector2> act) {
			for ( var y = min.y; y < max.y; ++y ) {
			for ( var x = min.x; x < max.x; ++x ) {
				act(new Vector2(x, y));
			}}
		}

		public static void LookUpCube(Vector3 min, Vector3 max, Action<Vector3> act) {
			for ( var z = min.z; z < max.z; ++z ) {
			for ( var y = min.y; y < max.y; ++y ) {
			for ( var x = min.x; x < max.x; ++x ) {
				act(new Vector3(x, y, z));
			}}}
		}

		// ---------------------------------------------------------------------
		//
		// Debug draw
		//
		// ---------------------------------------------------------------------

		#if UNITY_EDITOR
		static void DrawTop(Vector3 pos, Vector3 size) {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				var points = new Vector3[]{
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromX(size.x)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromXY(size.x, size.y)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromY(size.y)),
					iso_world.IsoToScreen(pos)
				};
				Gizmos.DrawLine(points[0], points[1]);
				Gizmos.DrawLine(points[1], points[2]);
				Gizmos.DrawLine(points[2], points[3]);
				Gizmos.DrawLine(points[3], points[0]);
			}
		}

		static void DrawVert(Vector3 pos, Vector3 size) {
			var iso_world = GameObject.FindObjectOfType<IsoWorld>();
			if ( iso_world ) {
				Gizmos.DrawLine(
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromZ(size.z)));
			}
		}
		
		public static void DrawCube(Vector3 pos, Vector3 size, Color color) {
			Gizmos.color = color;
			DrawTop (pos - IsoUtils.Vec3FromZ(0.5f), size);
			DrawTop (pos + IsoUtils.Vec3FromZ(size.z - 0.5f), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f), size);
			DrawVert(pos + IsoUtils.Vec3FromZ(0.5f), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f) + IsoUtils.Vec3FromX(size.x), size);
			DrawVert(pos - IsoUtils.Vec3FromZ(0.5f) + IsoUtils.Vec3FromY(size.y), size);
		}
		#endif
	}
}