using UnityEngine;
using System;
using System.Collections;

namespace IsoTools {
	public class IsoUtils {
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
	}
}