using UnityEngine;

namespace IsoTools.Internal {
	public struct IsoMinMax {
		public float min;
		public float max;

        public IsoMinMax(float minmax) : this() {
            this.min = minmax;
            this.max = minmax;
        }

        public IsoMinMax(float min, float max) : this() {
			this.min = min;
			this.max = max;
		}

		public IsoMinMax(IsoMinMax other) : this() {
			min = other.min;
			max = other.max;
		}

		public float size {
			get { return max - min; }
		}

		public float center {
			get { return min + (max - min) * 0.5f; }
		}

        public void Set(float minmax) {
            this.min = minmax;
            this.max = minmax;
        }

        public void Set(float min, float max) {
			this.min = min;
			this.max = max;
		}

		public void Set(IsoMinMax other) {
			min = other.min;
			max = other.max;
		}

		public void Resize(float size) {
			max = min + size;
		}

		public void Translate(float delta) {
			min += delta;
			max += delta;
		}

		public bool Contains(float other) {
			return other >= min && other <= max;
		}

		public bool Contains(IsoMinMax other) {
			return
				max >= other.max &&
				min <= other.min;
		}

		public bool Overlaps(IsoMinMax other) {
			return
				max > other.min &&
				min < other.max;
		}

		public bool Approximately(IsoMinMax other) {
			return
				Mathf.Approximately(min, other.min) &&
				Mathf.Approximately(max, other.max);
		}

		public static IsoMinMax zero {
			get { return new IsoMinMax(); }
		}

		public static IsoMinMax Merge(IsoMinMax a, IsoMinMax b) {
			if ( a.min > b.min ) {
				a.min = b.min;
			}
			if ( a.max < b.max ) {
				a.max = b.max;
			}
			return a;
		}
	}
}