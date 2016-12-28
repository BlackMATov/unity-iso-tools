namespace IsoTools.Internal {
	public interface IsoIPool<T> {
		T Take();
		void Release(T item);
	}

	public abstract class IsoPool<T> : IsoIPool<T> {
		IsoList<T> _items;

		public IsoPool(int capacity) {
			_items = new IsoList<T>(capacity);
		}

		public T Take() {
			return _items.Count > 0
				? _items.Pop()
				: CreateItem();
		}

		public void Release(T item) {
			CleanUpItem(item);
			_items.Add(item);
		}

		public abstract T CreateItem();
		public virtual void CleanUpItem(T item) {}
	}
}