namespace IsoTools.Internal {
	public class IsoList<T> {
		T[] _data;
		int _size;

		static readonly T[] _emptyData       = new T[0];
		const int           _defaultCapacity = 4;

		public IsoList() {
			_data = _emptyData;
			_size = 0;
		}

		public IsoList(int capacity) {
			if ( capacity < 0 ) {
				throw new System.ArgumentOutOfRangeException(
					"capacity", "capacity must be >= 0");
			} else if ( capacity == 0 ) {
				_data = _emptyData;
				_size = 0;
			} else {
				_data = new T[capacity];
				_size = 0;
			}
		}

		public void Add(T value) {
			if ( _size == _data.Length ) {
				var new_capacity = _size == 0
					? _defaultCapacity : _size * 2;
				var new_data = new T[new_capacity];
				System.Array.Copy(_data, new_data, _size);
				_data = new_data;
			}
			_data[_size++] = value;
		}

		public T Pop() {
			if ( _size == 0 ) {
				throw new System.InvalidOperationException("empty list");
			}
			var last = _data[--_size];
			_data[_size] = default(T);
			return last;
		}

		public T Peek() {
			if ( _size == 0 ) {
				throw new System.InvalidOperationException("empty list");
			}
			return _data[_size - 1];
		}

		public void Clear() {
			System.Array.Clear(_data, 0, _size);
			_size = 0;
		}

		public void UnorderedRemoveAt(int index) {
			if ( (uint)index >= (uint)_size ) {
				throw new System.IndexOutOfRangeException();
			}
			_data[index] = _data[--_size];
			_data[_size] = default(T);
		}

		public T this[int index] {
			get {
				if ( (uint)index >= (uint)_size ) {
					throw new System.IndexOutOfRangeException();
				}
				return _data[index];
			}
			set {
				if ( (uint)index >= (uint)_size ) {
					throw new System.IndexOutOfRangeException();
				}
				_data[index] = value;
			}
		}

		public int Count {
			get { return _size; }
		}

		public int Capacity {
			get { return _data.Length; }
			set {
				if ( value < _size ) {
					throw new System.ArgumentOutOfRangeException("value");
				} else if ( value != _data.Length ) {
					if ( value > 0 ) {
						var new_data = new T[value];
						if ( _size > 0 ) {
							System.Array.Copy(_data, new_data, _size);
						}
						_data = new_data;
					} else {
						_data = _emptyData;
					}
				}
			}
		}
	}
}