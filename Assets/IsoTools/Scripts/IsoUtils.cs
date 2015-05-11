using UnityEngine;
using System;

namespace IsoTools {
	public class IsoUtils {
		public static Vector3 Vec2OneX {
			get { return new Vector2(1.0f, 0.0f); }
		}

		public static Vector3 Vec2OneY {
			get { return new Vector2(0.0f, 1.0f); }
		}

		public static Vector3 Vec3OneX {
			get { return new Vector3(1.0f, 0.0f, 0.0f); }
		}

		public static Vector3 Vec3OneY {
			get { return new Vector3(0.0f, 1.0f, 0.0f); }
		}

		public static Vector3 Vec3OneZ {
			get { return new Vector3(0.0f, 0.0f, 1.0f); }
		}

		public static Vector2 Vec2FromX(float x) {
			return new Vector2(x, 0.0f);
		}
		
		public static Vector2 Vec2FromY(float y) {
			return new Vector2(0.0f, y);
		}

		public static Vector3 Vec3FromX(float x) {
			return new Vector3(x, 0.0f, 0.0f);
		}

		public static Vector3 Vec3FromY(float y) {
			return new Vector3(0.0f, y, 0.0f);
		}

		public static Vector3 Vec3FromZ(float z) {
			return new Vector3(0.0f, 0.0f, z);
		}

		public static int Sign<T>(float v) {
			return v > 0.0f ? 1 : (v == 0.0f ? 0 : -1);
		}

		public static void LookUpCube(Vector3 min, Vector3 max, Action<Vector3> act) {
			for ( var z = min.z; z < max.z; ++z ) {
				for ( var y = min.y; y < max.y; ++y ) {
					for ( var x = min.x; x < max.x; ++x ) {
						act(new Vector3(x, y, z));
					}
				}
			}
		}

		public static bool Vec3Approximately(Vector3 a, Vector3 b) {
			return
				Mathf.Approximately(a.x, b.x) &&
				Mathf.Approximately(a.y, b.y) &&
				Mathf.Approximately(a.z, b.z);
		}

		public static Vector3 Vec3Abs(Vector3 v) {
			return new Vector3(
				Mathf.Abs(v.x),
				Mathf.Abs(v.y),
				Mathf.Abs(v.z));
		}

		public static Vector3 Vec3Ceil(Vector3 v) {
			return new Vector3(
				Mathf.Ceil(v.x),
				Mathf.Ceil(v.y),
				Mathf.Ceil(v.z));
		}
		
		public static Vector3 Vec3Floor(Vector3 v) {
			return new Vector3(
				Mathf.Floor(v.x),
				Mathf.Floor(v.y),
				Mathf.Floor(v.z));
		}
		
		public static Vector3 Vec3Round(Vector3 v) {
			return new Vector3(
				Mathf.Round(v.x),
				Mathf.Round(v.y),
				Mathf.Round(v.z));
		}

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

		public static Vector3 Vec3DivCeil(Vector3 a, float b) {
			return Vec3Ceil(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivCeil(Vector3 a, Vector3 b) {
			return Vec3Ceil(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivFloor(Vector3 a, float b) {
			return Vec3Floor(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivFloor(Vector3 a, Vector3 b) {
			return Vec3Floor(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivRound(Vector3 a, float b) {
			return Vec3Round(Vec3Div(a, b));
		}

		public static Vector3 Vec3DivRound(Vector3 a, Vector3 b) {
			return Vec3Round(Vec3Div(a, b));
		}
		
		public static Vector3 Vec3Min(Vector3 a, Vector3 b) {
			return new Vector3(
				Mathf.Min(a.x, b.x),
				Mathf.Min(a.y, b.y),
				Mathf.Min(a.z, b.z));
		}
		
		public static Vector3 Vec3Max(Vector3 a, Vector3 b) {
			return new Vector3(
				Mathf.Max(a.x, b.x),
				Mathf.Max(a.y, b.y),
				Mathf.Max(a.z, b.z));
		}

		public static Vector3 Vec3ChangeX(Vector3 v, float x) {
			return new Vector3(x, v.y, v.z);
		}

		public static Vector3 Vec3ChangeY(Vector3 v, float y) {
			return new Vector3(v.x, y, v.z);
		}

		public static Vector3 Vec3ChangeZ(Vector3 v, float z) {
			return new Vector3(v.x, v.y, z);
		}

		public static Color ColorChangeR(Color c, float r) {
			return new Color(r, c.g, c.b, c.a);
		}

		public static Color ColorChangeG(Color c, float g) {
			return new Color(c.r, g, c.b, c.a);
		}

		public static Color ColorChangeB(Color c, float b) {
			return new Color(c.r, c.g, b, c.a);
		}

		public static Color ColorChangeA(Color c, float a) {
			return new Color(c.r, c.g, c.b, a);
		}
	}
}