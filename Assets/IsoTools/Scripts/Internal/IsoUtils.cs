using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Internal {
	public static class IsoUtils {

		// ---------------------------------------------------------------------
		//
		// Consts
		//
		// ---------------------------------------------------------------------

		public static readonly Vector2 vec2OneX  = new Vector2(1.0f, 0.0f);
		public static readonly Vector2 vec2OneY  = new Vector2(0.0f, 1.0f);

		public static readonly Vector3 vec3OneX  = new Vector3(1.0f, 0.0f, 0.0f);
		public static readonly Vector3 vec3OneY  = new Vector3(0.0f, 1.0f, 0.0f);
		public static readonly Vector3 vec3OneZ  = new Vector3(0.0f, 0.0f, 1.0f);

		public static readonly Vector3 vec3OneXY = new Vector3(1.0f, 1.0f, 0.0f);
		public static readonly Vector3 vec3OneYZ = new Vector3(0.0f, 1.0f, 1.0f);
		public static readonly Vector3 vec3OneXZ = new Vector3(1.0f, 0.0f, 1.0f);

		public static readonly int FloatBeautifierDigits = 4;

		// ---------------------------------------------------------------------
		//
		// Abs/Min/Max
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Abs
		// -----------------------------

		public static Vector2 Vec2Abs(Vector2 v) {
			if ( v.x < 0.0f ) {
				v.x = -v.x;
			}
			if ( v.y < 0.0f ) {
				v.y = -v.y;
			}
			return v;
		}
		
		public static Vector3 Vec3Abs(Vector3 v) {
			if ( v.x < 0.0f ) {
				v.x = -v.x;
			}
			if ( v.y < 0.0f ) {
				v.y = -v.y;
			}
			if ( v.z < 0.0f ) {
				v.z = -v.z;
			}
			return v;
		}

		// -----------------------------
		// Min
		// -----------------------------

		public static float Vec2MinF(Vector2 v) {
			return v.x < v.y ? v.x : v.y;
		}

		public static float Vec3MinF(Vector3 v) {
			var mxy = v.x < v.y ? v.x : v.y;
			return mxy < v.z ? mxy : v.z;
		}

		public static Vector2 Vec2Min(Vector2 a, float b) {
			if ( a.x > b ) {
				a.x = b;
			}
			if ( a.y > b ) {
				a.y = b;
			}
			return a;
		}
		
		public static Vector2 Vec2Min(Vector2 a, Vector2 b) {
			if ( a.x > b.x ) {
				a.x = b.x;
			}
			if ( a.y > b.y ) {
				a.y = b.y;
			}
			return a;
		}

		public static Vector3 Vec3Min(Vector3 a, float b) {
			if ( a.x > b ) {
				a.x = b;
			}
			if ( a.y > b ) {
				a.y = b;
			}
			if ( a.z > b ) {
				a.z = b;
			}
			return a;
		}

		public static Vector3 Vec3Min(Vector3 a, Vector3 b) {
			if ( a.x > b.x ) {
				a.x = b.x;
			}
			if ( a.y > b.y ) {
				a.y = b.y;
			}
			if ( a.z > b.z ) {
				a.z = b.z;
			}
			return a;
		}

		// -----------------------------
		// Max
		// -----------------------------

		public static float Vec2MaxF(Vector2 v) {
			return v.x > v.y ? v.x : v.y;
		}

		public static float Vec3MaxF(Vector3 v) {
			var mxy = v.x > v.y ? v.x : v.y;
			return mxy > v.z ? mxy : v.z;
		}

		public static Vector2 Vec2Max(Vector2 a, float b) {
			if ( a.x < b ) {
				a.x = b;
			}
			if ( a.y < b ) {
				a.y = b;
			}
			return a;
		}

		public static Vector2 Vec2Max(Vector2 a, Vector2 b) {
			if ( a.x < b.x ) {
				a.x = b.x;
			}
			if ( a.y < b.y ) {
				a.y = b.y;
			}
			return a;
		}

		public static Vector3 Vec3Max(Vector3 a, float b) {
			if ( a.x < b ) {
				a.x = b;
			}
			if ( a.y < b ) {
				a.y = b;
			}
			if ( a.z < b ) {
				a.z = b;
			}
			return a;
		}

		public static Vector3 Vec3Max(Vector3 a, Vector3 b) {
			if ( a.x < b.x ) {
				a.x = b.x;
			}
			if ( a.y < b.y ) {
				a.y = b.y;
			}
			if ( a.z < b.z ) {
				a.z = b.z;
			}
			return a;
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
			v.x = Mathf.Ceil(v.x);
			v.y = Mathf.Ceil(v.y);
			return v;
		}

		public static Vector3 Vec3Ceil(Vector3 v) {
			v.x = Mathf.Ceil(v.x);
			v.y = Mathf.Ceil(v.y);
			v.z = Mathf.Ceil(v.z);
			return v;
		}

		// -----------------------------
		// Floor
		// -----------------------------

		public static Vector2 Vec2Floor(Vector2 v) {
			v.x = Mathf.Floor(v.x);
			v.y = Mathf.Floor(v.y);
			return v;
		}

		public static Vector3 Vec3Floor(Vector3 v) {
			v.x = Mathf.Floor(v.x);
			v.y = Mathf.Floor(v.y);
			v.z = Mathf.Floor(v.z);
			return v;
		}

		// -----------------------------
		// Round
		// -----------------------------

		public static Vector2 Vec2Round(Vector2 v) {
			v.x = Mathf.Round(v.x);
			v.y = Mathf.Round(v.y);
			return v;
		}

		public static Vector3 Vec3Round(Vector3 v) {
			v.x = Mathf.Round(v.x);
			v.y = Mathf.Round(v.y);
			v.z = Mathf.Round(v.z);
			return v;
		}

		// ---------------------------------------------------------------------
		//
		// Vec2From/Vec3From
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Vec2From
		// -----------------------------

		public static Vector2 Vec2From(float v) {
			return new Vector2(v, v);
		}

		public static Vector2 Vec2FromX(float x) {
			return new Vector2(x, 0.0f);
		}
		
		public static Vector2 Vec2FromY(float y) {
			return new Vector2(0.0f, y);
		}
		
		public static Vector2 Vec2FromXY(float x, float y) {
			return new Vector2(x, y);
		}

		public static Vector2 Vec2FromVec3(Vector3 v) {
			return new Vector2(v.x, v.y);
		}

		// -----------------------------
		// Vec3From
		// -----------------------------

		public static Vector3 Vec3From(float v) {
			return new Vector3(v, v, v);
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
		
		public static Vector3 Vec3FromXY(float x, float y) {
			return new Vector3(x, y, 0.0f);
		}
		
		public static Vector3 Vec3FromYZ(float y, float z) {
			return new Vector3(0.0f, y, z);
		}
		
		public static Vector3 Vec3FromXZ(float x, float z) {
			return new Vector3(x, 0.0f, z);
		}

		public static Vector3 Vec3FromVec2(Vector2 v) {
			return new Vector3(v.x, v.y, 0.0f);
		}

		public static Vector3 Vec3FromVec2(Vector2 v, float z) {
			return new Vector3(v.x, v.y, z);
		}

		// ---------------------------------------------------------------------
		//
		// XChange
		//
		// ---------------------------------------------------------------------

		// -----------------------------
		// Vec2Change
		// -----------------------------

		public static Vector2 Vec2ChangeX(Vector2 v, float x) {
			v.x = x;
			return v;
		}
		
		public static Vector2 Vec2ChangeY(Vector2 v, float y) {
			v.y = y;
			return v;
		}

		public static Vector2 Vec2ChangeI(Vector2 v, int index, float n) {
			v[index] = n;
			return v;
		}

		// -----------------------------
		// Vec3Change
		// -----------------------------
		
		public static Vector3 Vec3ChangeX(Vector3 v, float x) {
			v.x = x;
			return v;
		}
		
		public static Vector3 Vec3ChangeY(Vector3 v, float y) {
			v.y = y;
			return v;
		}
		
		public static Vector3 Vec3ChangeZ(Vector3 v, float z) {
			v.z = z;
			return v;
		}
		
		public static Vector3 Vec3ChangeXY(Vector3 v, float x, float y) {
			v.x = x;
			v.y = y;
			return v;
		}
		
		public static Vector3 Vec3ChangeYZ(Vector3 v, float y, float z) {
			v.y = y;
			v.z = z;
			return v;
		}
		
		public static Vector3 Vec3ChangeXZ(Vector3 v, float x, float z) {
			v.x = x;
			v.z = z;
			return v;
		}

		public static Vector3 Vec3ChangeI(Vector3 v, int index, float n) {
			v[index] = n;
			return v;
		}

		// -----------------------------
		// ColorChange
		// -----------------------------

		public static Color ColorChangeA(Color c, float a) {
			c.a = a;
			return c;
		}

		public static Color ColorChangeR(Color c, float r) {
			c.r = r;
			return c;
		}

		public static Color ColorChangeG(Color c, float g) {
			c.g = g;
			return c;
		}

		public static Color ColorChangeB(Color c, float b) {
			c.b = b;
			return c;
		}

		// ---------------------------------------------------------------------
		//
		// Approximately
		//
		// ---------------------------------------------------------------------

		public static bool Vec2Approximately(Vector2 a, Vector2 b) {
			return a == b;
		}

		public static bool Vec2Approximately(Vector2 a, Vector2 b, float precision) {
			return
				Mathf.Abs(a.x - b.x) < precision &&
				Mathf.Abs(a.y - b.y) < precision;
		}

		public static bool Vec3Approximately(Vector3 a, Vector3 b) {
			return a == b;
		}

		public static bool Vec3Approximately(Vector3 a, Vector3 b, float precision) {
			return
				Mathf.Abs(a.x - b.x) < precision &&
				Mathf.Abs(a.y - b.y) < precision &&
				Mathf.Abs(a.z - b.z) < precision;
		}

		// ---------------------------------------------------------------------
		//
		// Beautifiers
		//
		// ---------------------------------------------------------------------

		public static float FloatBeautifier(float v) {
			return (float)System.Math.Round(v, FloatBeautifierDigits);
		}

		public static Vector2 VectorBeautifier(Vector2 v) {
			v.x = FloatBeautifier(v.x);
			v.y = FloatBeautifier(v.y);
			return v;
		}

		public static Vector3 VectorBeautifier(Vector3 v) {
			v.x = FloatBeautifier(v.x);
			v.y = FloatBeautifier(v.y);
			v.z = FloatBeautifier(v.z);
			return v;
		}

		// ---------------------------------------------------------------------
		//
		// Helpers
		//
		// ---------------------------------------------------------------------

		public static T GetOrCreateComponent<T>(GameObject obj) where T : Component {
			var comp = obj.GetComponent<T>();
			return comp != null
				? comp
				: obj.AddComponent<T>();
		}

		// ---------------------------------------------------------------------
		//
		// Debug draw
		//
		// ---------------------------------------------------------------------

	#if UNITY_EDITOR
		static void DrawIsoCubeTop(IsoWorld iso_world, Vector3 pos, Vector3 size) {
			if ( iso_world ) {
				var point0 = iso_world.IsoToScreen(pos);
				var point1 = iso_world.IsoToScreen(pos + IsoUtils.Vec3FromX (size.x));
				var point2 = iso_world.IsoToScreen(pos + IsoUtils.Vec3FromXY(size.x, size.y));
				var point3 = iso_world.IsoToScreen(pos + IsoUtils.Vec3FromY (size.y));
				Handles.DrawLine(point0, point1);
				Handles.DrawLine(point1, point2);
				Handles.DrawLine(point2, point3);
				Handles.DrawLine(point3, point0);
			}
		}

		static void DrawIsoCubeVert(IsoWorld iso_world, Vector3 pos, Vector3 size) {
			if ( iso_world ) {
				var point0 = iso_world.IsoToScreen(pos);
				var point1 = iso_world.IsoToScreen(pos + IsoUtils.Vec3FromZ(size.z));
				Handles.DrawLine(point0, point1);
			}
		}
		
		public static void DrawIsoCube(
			IsoWorld iso_world, Vector3 center, Vector3 size, Color color)
		{
			if ( iso_world ) {
				Handles.color = color;
				var pos = center - size * 0.5f;
				DrawIsoCubeTop (iso_world, pos, size);
				DrawIsoCubeTop (iso_world, pos + IsoUtils.Vec3FromZ (size.z), size);
				DrawIsoCubeVert(iso_world, pos, size);
				DrawIsoCubeVert(iso_world, pos + IsoUtils.Vec3FromX (size.x), size);
				DrawIsoCubeVert(iso_world, pos + IsoUtils.Vec3FromY (size.y), size);
				DrawIsoCubeVert(iso_world, pos + IsoUtils.Vec3FromXY(size.x, size.y), size);
			}
		}

		public static void DrawIsoSphere(
			IsoWorld iso_world, Vector3 pos, float radius, Color color)
		{
			if ( iso_world ) {
				Handles.color = color;
				Handles.RadiusHandle(
					Quaternion.Euler(45.0f, 45.0f, 0.0f),
					iso_world.IsoToScreen(pos),
					radius * iso_world.tileSize * 2.0f);
			}
		}

		public static void DrawIsoGrid(
			IsoWorld iso_world, Vector3 pos, Vector3 size, Color color)
		{
			if ( iso_world ) {
				Handles.color = color;
				var size_x = Mathf.RoundToInt(size.x);
				var size_y = Mathf.RoundToInt(size.y);
				for ( var i = 0; i <= size_x; ++i ) {
					Handles.DrawLine(
						iso_world.IsoToScreen(new Vector3(pos.x + i, pos.y + 0.0f  , pos.z)),
						iso_world.IsoToScreen(new Vector3(pos.x + i, pos.y + size_y, pos.z)));
				}
				for ( var i = 0; i <= size_y; ++i ) {
					Handles.DrawLine(
						iso_world.IsoToScreen(new Vector3(pos.x + 0.0f  , pos.y + i, pos.z)),
						iso_world.IsoToScreen(new Vector3(pos.x + size_x, pos.y + i, pos.z)));
				}
			}
		}

		public static void DrawLine(Vector2 begin, Vector2 end, Color color) {
			Handles.color = color;
			Handles.DrawLine(begin, end);
		}

		public static void DrawLine(Vector2 begin, Vector2 end, Color begin_color, Color end_color, float color_ratio) {
			var ratio_point = begin + (end - begin) * color_ratio;
			DrawLine(begin, ratio_point, begin_color);
			DrawLine(ratio_point, end, end_color);
		}

		public static void DrawRect(IsoRect rect, Color color) {
			Handles.color = color;
			var point0 = new Vector2(rect.x.min, rect.y.min);
			var point1 = new Vector2(rect.x.max, rect.y.min);
			var point2 = new Vector2(rect.x.max, rect.y.max);
			var point3 = new Vector2(rect.x.min, rect.y.max);
			Handles.DrawLine(point0, point1);
			Handles.DrawLine(point1, point2);
			Handles.DrawLine(point2, point3);
			Handles.DrawLine(point3, point0);
		}

		public static void DrawSolidRect(IsoRect rect, Color face_color, Color outline_color) {
			Handles.DrawSolidRectangleWithOutline(
				new Rect(rect.x.min, rect.y.min, rect.size.x, rect.size.y),
				face_color,
				outline_color);
		}
	#endif
	}
}