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

		public static Vector2 vec2OneX  { get { return new Vector2(1.0f, 0.0f); } }
		public static Vector2 vec2OneY  { get { return new Vector2(0.0f, 1.0f); } }
		public static Vector2 vec2OneXY { get { return new Vector2(1.0f, 1.0f); } }

		public static Vector3 vec3OneX  { get { return new Vector3(1.0f, 0.0f, 0.0f); } }
		public static Vector3 vec3OneY  { get { return new Vector3(0.0f, 1.0f, 0.0f); } }
		public static Vector3 vec3OneZ  { get { return new Vector3(0.0f, 0.0f, 1.0f); } }
		public static Vector3 vec3OneXY { get { return new Vector3(1.0f, 1.0f, 0.0f); } }
		public static Vector3 vec3OneYZ { get { return new Vector3(0.0f, 1.0f, 1.0f); } }
		public static Vector3 vec3OneXZ { get { return new Vector3(1.0f, 0.0f, 1.0f); } }

		static public readonly int FloatBeautifierDigits = 4;

		// ---------------------------------------------------------------------
		//
		// MinMax
		//
		// ---------------------------------------------------------------------

		public struct MinMax {
			public float min;
			public float max;

			public MinMax(float min, float max) {
				this.min = min;
				this.max = max;
			}
				
			public float size {
				get { return max - min; }
			}

			public float center {
				get { return min / 2.0f + max / 2.0f; }
			}

			public bool Approximately(MinMax minmax) {
				return
					Mathf.Approximately(min, minmax.min) &&
					Mathf.Approximately(max, minmax.max);
			}

			static public MinMax zero {
				get { return new MinMax(); }
			}
		}

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
			if ( b < a.x ) {
				a.x = b;
			}
			if ( b < a.y ) {
				a.y = b;
			}
			return a;
		}
		
		public static Vector2 Vec2Min(Vector2 a, Vector2 b) {
			if ( b.x < a.x ) {
				a.x = b.x;
			}
			if ( b.y < a.y ) {
				a.y = b.y;
			}
			return a;
		}

		public static Vector3 Vec3Min(Vector3 a, float b) {
			if ( b < a.x ) {
				a.x = b;
			}
			if ( b < a.y ) {
				a.y = b;
			}
			if ( b < a.z ) {
				a.z = b;
			}
			return a;
		}

		public static Vector3 Vec3Min(Vector3 a, Vector3 b) {
			if ( b.x < a.x ) {
				a.x = b.x;
			}
			if ( b.y < a.y ) {
				a.y = b.y;
			}
			if ( b.z < a.z ) {
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
			if ( b > a.x ) {
				a.x = b;
			}
			if ( b > a.y ) {
				a.y = b;
			}
			return a;
		}

		public static Vector2 Vec2Max(Vector2 a, Vector2 b) {
			if ( b.x > a.x ) {
				a.x = b.x;
			}
			if ( b.y > a.y ) {
				a.y = b.y;
			}
			return a;
		}

		public static Vector3 Vec3Max(Vector3 a, float b) {
			if ( b > a.x ) {
				a.x = b;
			}
			if ( b > a.y ) {
				a.y = b;
			}
			if ( b > a.z ) {
				a.z = b;
			}
			return a;
		}

		public static Vector3 Vec3Max(Vector3 a, Vector3 b) {
			if ( b.x > a.x ) {
				a.x = b.x;
			}
			if ( b.y > a.y ) {
				a.y = b.y;
			}
			if ( b.z > a.z ) {
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
			return new Vector2(x, v.y);
		}
		
		public static Vector2 Vec2ChangeY(Vector2 v, float y) {
			return new Vector2(v.x, y);
		}

		public static Vector2 Vec2ChangeI(Vector2 v, int index, float n) {
			var c = v;
			c[index] = n;
			return c;
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

		public static Vector3 Vec3ChangeI(Vector3 v, int index, float n) {
			var c = v;
			c[index] = n;
			return c;
		}

		// -----------------------------
		// ColorChange
		// -----------------------------

		public static Color ColorChangeA(Color color, float a) {
			var c = color;
			c.a = a;
			return c;
		}

		public static Color ColorChangeR(Color color, float r) {
			var c = color;
			c.r = r;
			return c;
		}

		public static Color ColorChangeG(Color color, float g) {
			var c = color;
			c.g = g;
			return c;
		}

		public static Color ColorChangeB(Color color, float b) {
			var c = color;
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

		public static bool Vec3Approximately(Vector3 a, Vector3 b) {
			return a == b;
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
			return new Vector2{
				x = FloatBeautifier(v.x),
				y = FloatBeautifier(v.y)};
		}

		public static Vector3 VectorBeautifier(Vector3 v) {
			return new Vector3{
				x = FloatBeautifier(v.x),
				y = FloatBeautifier(v.y),
				z = FloatBeautifier(v.z)};
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

		public static IsoCollider IsoConvertCollider(Collider collider) {
			var fake_collider = collider ? collider.GetComponent<IsoFakeCollider>() : null;
			return fake_collider ? fake_collider.isoCollider : null;
		}
		
		public static IsoRigidbody IsoConvertRigidbody(Rigidbody rigidbody) {
			var fake_rigidbody = rigidbody ? rigidbody.GetComponent<IsoFakeRigidbody>() : null;
			return fake_rigidbody ? fake_rigidbody.isoRigidbody : null;
		}
		
		public static GameObject IsoConvertGameObject(GameObject game_object) {
			var fake_object = game_object ? game_object.GetComponent<IsoFakeObject>() : null;
			var iso_object = fake_object ? fake_object.isoObject : null;
			return iso_object ? iso_object.gameObject : null;
		}
		
		public static IsoContactPoint[] IsoConvertContactPoints(ContactPoint[] points) {
			var iso_points = new IsoContactPoint[points.Length];
			for ( int i = 0, e = points.Length; i < e; ++i ) {
				iso_points[i] = new IsoContactPoint(points[i]);
			}
			return iso_points;
		}

		public static IsoRaycastHit[] IsoConvertRaycastHits(RaycastHit[] hits) {
			var iso_hits = new IsoRaycastHit[hits.Length];
			for ( int i = 0, e = hits.Length; i < e; ++i ) {
				iso_hits[i] = new IsoRaycastHit(hits[i]);
			}
			return iso_hits;
		}

		// ---------------------------------------------------------------------
		//
		// Debug draw
		//
		// ---------------------------------------------------------------------

		#if UNITY_EDITOR
		static void DrawTop(IsoWorld iso_world, Vector3 pos, Vector3 size) {
			if ( iso_world ) {
				var points = new Vector3[]{
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromX(size.x)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromXY(size.x, size.y)),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromY(size.y)),
					iso_world.IsoToScreen(pos)
				};
				Handles.DrawLine(points[0], points[1]);
				Handles.DrawLine(points[1], points[2]);
				Handles.DrawLine(points[2], points[3]);
				Handles.DrawLine(points[3], points[0]);
			}
		}

		static void DrawVert(IsoWorld iso_world, Vector3 pos, Vector3 size) {
			if ( iso_world ) {
				Handles.DrawLine(
					iso_world.IsoToScreen(pos),
					iso_world.IsoToScreen(pos + IsoUtils.Vec3FromZ(size.z)));
			}
		}
		
		public static void DrawCube(IsoWorld iso_world, Vector3 center, Vector3 size, Color color) {
			if ( iso_world ) {
				Handles.color = color;
				var pos = center - size * 0.5f;
				DrawTop (iso_world, pos, size);
				DrawTop (iso_world, pos + IsoUtils.Vec3FromZ(size.z), size);
				DrawVert(iso_world, pos, size);
				DrawVert(iso_world, pos + IsoUtils.Vec3FromX(size.x), size);
				DrawVert(iso_world, pos + IsoUtils.Vec3FromY(size.y), size);
				DrawVert(iso_world, pos + IsoUtils.Vec3FromXY(size.x, size.y), size);
			}
		}

		public static void DrawSphere(IsoWorld iso_world, Vector3 pos, float radius, Color color) {
			if ( iso_world ) {
				Handles.color = color;
				Handles.RadiusHandle(
					Quaternion.Euler(45.0f, 45.0f, 0.0f),
					iso_world.IsoToScreen(pos),
					radius * iso_world.tileSize * 2.0f);
			}
		}

		public static void DrawGrid(IsoWorld iso_world, Vector3 pos, Vector3 size, Color color) {
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
		#endif
	}
}
