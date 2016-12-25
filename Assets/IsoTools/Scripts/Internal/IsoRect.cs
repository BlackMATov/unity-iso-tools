using UnityEngine;

namespace IsoTools.Internal {
	public struct IsoRect {
		public IsoMinMax x;
		public IsoMinMax y;

		public IsoRect(float min_x, float min_y, float max_x, float max_y) : this() {
			x.Set(min_x, max_x);
			y.Set(min_y, max_y);
		}

		public IsoRect(Vector2 min, Vector2 max) : this() {
			x.Set(min.x, max.x);
			y.Set(min.y, max.y);
		}

		public IsoRect(IsoMinMax minmax_x, IsoMinMax minmax_y) : this() {
			x.Set(minmax_x);
			y.Set(minmax_y);
		}

		public IsoRect(IsoRect other) : this() {
			x.Set(other.x);
			y.Set(other.y);
		}

		public Vector2 size {
			get { return new Vector2(x.size, y.size); }
		}

		public Vector2 center {
			get { return new Vector2(x.center, y.center); }
		}

		public void Set(float min_x, float min_y, float max_x, float max_y) {
			x.Set(min_x, max_x);
			y.Set(min_y, max_y);
		}

		public void Set(Vector2 min, Vector2 max) {
			x.Set(min.x, max.x);
			y.Set(min.y, max.y);
		}

		public void Set(IsoMinMax minmax_x, IsoMinMax minmax_y) {
			x.Set(minmax_x);
			y.Set(minmax_y);
		}

		public void Set(IsoRect other) {
			x.Set(other.x);
			y.Set(other.y);
		}

		public void Resize(float size_x, float size_y) {
			x.Resize(size_x);
			y.Resize(size_y);
		}

		public void Resize(Vector2 size) {
			x.Resize(size.x);
			y.Resize(size.y);
		}

		public void Translate(float delta_x, float delta_y) {
			x.Translate(delta_x);
			y.Translate(delta_y);
		}

		public void Translate(Vector2 delta) {
			x.Translate(delta.x);
			y.Translate(delta.y);
		}

		public bool Contains(Vector2 other) {
			return
				x.Contains(other.x) &&
				y.Contains(other.y);
		}

		public bool Contains(IsoRect other) {
			return
				x.Contains(other.x) &&
				y.Contains(other.y);
		}

		public bool Overlaps(IsoRect other) {
			return
				x.Overlaps(other.x) &&
				y.Overlaps(other.y);
		}

		public bool Approximately(IsoRect other) {
			return
				x.Approximately(other.x) &&
				y.Approximately(other.y);
		}

		public static IsoRect zero {
			get { return new IsoRect(); }
		}

		public static IsoRect Merge(IsoRect a, IsoRect b) {
			a.x = IsoMinMax.Merge(a.x, b.x);
			a.y = IsoMinMax.Merge(a.y, b.y);
			return a;
		}
	}
}