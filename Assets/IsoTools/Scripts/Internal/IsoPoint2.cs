namespace IsoTools.Internal {
	public struct IsoPoint2 {
		public int x;
		public int y;

		public IsoPoint2(int x, int y) : this() {
			this.x = x;
			this.y = y;
		}

		public IsoPoint2(IsoPoint2 other) : this() {
			x = other.x;
			y = other.y;
		}

		public void Set(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public void Set(IsoPoint2 other) {
			x = other.x;
			y = other.y;
		}

		public override bool Equals(object other) {
			return (other is IsoPoint2)
				? Equals((IsoPoint2)other)
				: false;
		}

		public bool Equals(IsoPoint2 other) {
			return x == other.x && y == other.y;
		}

		public override int GetHashCode() {
			return x ^ y;
		}

		public static IsoPoint2 operator+(IsoPoint2 a, IsoPoint2 b) {
			a.x += b.x;
			a.y += b.y;
			return a;
		}

		public static IsoPoint2 operator-(IsoPoint2 a, IsoPoint2 b) {
			a.x -= b.x;
			a.y -= b.y;
			return a;
		}

		public static IsoPoint2 operator-(IsoPoint2 a) {
			a.x = -a.x;
			a.y = -a.y;
			return a;
		}

		public static bool operator==(IsoPoint2 lhs, IsoPoint2 rhs) {
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		public static bool operator!=(IsoPoint2 lhs, IsoPoint2 rhs) {
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}

		public static IsoPoint2 one {
			get { return new IsoPoint2(1, 1); }
		}

		public static IsoPoint2 oneX {
			get { return new IsoPoint2(1, 0); }
		}

		public static IsoPoint2 oneY {
			get { return new IsoPoint2(0, 1); }
		}

		public static IsoPoint2 zero {
			get { return new IsoPoint2(0, 0); }
		}
	}
}