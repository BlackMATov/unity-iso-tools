using System.Collections.Generic;

namespace IsoTools.Internal {
	public class IsoAssocList<T> {
		IsoList<T>         _list;
		Dictionary<T, int> _dict;

		public IsoAssocList() {
			_list = new IsoList<T>();
			_dict = new Dictionary<T, int>();
		}

		public IsoAssocList(int capacity) {
			_list = new IsoList<T>(capacity);
			_dict = new Dictionary<T, int>(capacity);
		}

		public T this[int index] {
			get {
				return _list[index];
			}
		}

		public int Count {
			get {
				return _list.Count;
			}
		}

		public bool Contains(T item) {
			return _dict.ContainsKey(item);
		}

		public bool Add(T item) {
			if ( _dict.ContainsKey(item) ) {
				return false;
			}
			_dict.Add(item, _list.Count);
			_list.Add(item);
			return true;
		}

		public bool Remove(T item) {
			int index;
			if ( _dict.TryGetValue(item, out index) ) {
				_dict.Remove(item);
				_list.UnorderedRemoveAt(index);
				if ( index != _list.Count ) {
					_dict[_list[index]] = index;
				}
				return true;
			}
			return false;
		}

		public T Pop() {
			var item = _list.Pop();
			_dict.Remove(item);
			return item;
		}

		public T Peek() {
			return _list.Peek();
		}

		public void Clear() {
			_list.Clear();
			_dict.Clear();
		}
	}
}