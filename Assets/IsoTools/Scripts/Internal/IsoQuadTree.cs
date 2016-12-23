namespace IsoTools.Internal {
	public class IsoQuadTree<T> {

		const int MinChildCountPerNode = 3;

		//
		// IItem
		//

		public interface IItem {
		}

		//
		// Item
		//

		class Item : IItem {
			public Node    Owner   = null;
			public IsoRect Bounds  = IsoRect.zero;
			public T       Content = default(T);

			public Item Init(Node owner, IsoRect bounds, T content) {
				Owner   = owner;
				Bounds  = bounds;
				Content = content;
				return this;
			}

			public Item Clear() {
				return Init(null, IsoRect.zero, default(T));
			}
		}

		//
		// Node
		//

		class Node {
			public Node               Parent   = null;
			public IsoRect            Bounds   = IsoRect.zero;
			public Node[]             Children = new Node[4];
			public IsoAssocList<Item> Contents = new IsoAssocList<Item>(MinChildCountPerNode);

			public Node Init(Node parent, IsoRect bounds) {
				Parent = parent;
				Bounds = bounds;
				return this;
			}

			public Node Clear(IsoIPool<Node> node_pool, IsoIPool<Item> item_pool) {
				ClearChildren(node_pool, item_pool);
				ClearContents(item_pool);
				return Init(null, IsoRect.zero);
			}

			public bool Insert(
				IsoRect bounds, T content, out Item item,
				IsoIPool<Node> node_pool, IsoIPool<Item> item_pool)
			{
				if ( !Bounds.Contains(bounds) ) {
					item = null;
					return false;
				}
				bool has_any_children = false;
				for ( int i = 0, e = Children.Length; i < e; ++i ) {
					if ( Children[i] != null ) {
						has_any_children = true;
						if ( Children[i].Insert(bounds, content, out item, node_pool, item_pool) ) {
							return true;
						}
					}
				}
				if ( has_any_children || Contents.Count >= MinChildCountPerNode ) {
					for ( int i = 0, e = Children.Length; i < e; ++i ) {
						if ( Children[i] == null ) {
							var child_bounds = GetChildBounds(i);
							if ( child_bounds.Contains(bounds) ) {
								Children[i] = node_pool.Take().Init(this, child_bounds);
								if ( Children[i].Insert(bounds, content, out item, node_pool, item_pool) ) {
									return true;
								}
							}
						}
					}
				}
				item = item_pool.Take().Init(this, bounds, content);
				Contents.Add(item);
				return true;
			}

			public void VisitAllBounds(System.Action<IsoRect> act) {
				act(Bounds);
				for ( int i = 0, e = Children.Length; i < e; ++i ) {
					if ( Children[i] != null ) {
						Children[i].VisitAllBounds(act);
					}
				}
			}

			public void VisitItemsByBounds(IsoRect bounds, System.Action<T> act) {
				if ( Bounds.Overlaps(bounds) ) {
					for ( int i = 0, e = Contents.Count; i < e; ++i ) {
						act(Contents[i].Content);
					}
					for ( int i = 0, e = Children.Length; i < e; ++i ) {
						if ( Children[i] != null ) {
							Children[i].VisitItemsByBounds(bounds, act);
						}
					}
				}
			}

			//
			// Private
			//

			IsoRect GetChildBounds(int index) {
				var size   = Bounds.size * 0.5f;
				var center = Bounds.center;
				switch ( index ) {
				case 0: { // LT
						return new IsoRect(center - size, center);
					}
				case 1: { // RT
						var rect = new IsoRect(center - size, center);
						rect.Translate(size.x, 0.0f);
						return rect;
					}
				case 2: { // LB
						var rect = new IsoRect(center, center + size);
						rect.Translate(-size.x, 0.0f);
						return rect;
					}
				case 3: { // RB
						return new IsoRect(center, center + size);
					}
				default:
					return IsoRect.zero;
				}
			}

			void ClearChildren(IsoIPool<Node> node_pool, IsoIPool<Item> item_pool) {
				for ( int i = 0, e = Children.Length; i < e; ++i ) {
					if ( Children[i] != null ) {
						node_pool.Release(
							Children[i].Clear(node_pool, item_pool));
					}
				}
				System.Array.Clear(Children, 0, Children.Length);
			}

			void ClearContents(IsoIPool<Item> item_pool) {
				for ( int i = 0, e = Contents.Count; i < e; ++i ) {
					item_pool.Release(
						Contents[i].Clear());
				}
				Contents.Clear();
			}
		}

		//
		// ItemPool
		//

		class ItemPool : IsoPool<Item> {
			public ItemPool(int capacity) : base(capacity) {
			}

			public override Item CreateItem() {
				return new Item();
			}
		}

		//
		// NodePool
		//

		class NodePool : IsoPool<Node> {
			public NodePool(int capacity) : base(capacity) {
			}

			public override Node CreateItem() {
				return new Node();
			}
		}

		//
		// Members
		//

		Node           _root     = null;
		IsoIPool<Node> _nodePool = null;
		IsoIPool<Item> _itemPool = null;

		//
		// Public
		//

		public IsoQuadTree(int capacity) {
			_root     = null;
			_nodePool = new NodePool(capacity);
			_itemPool = new ItemPool(capacity);
		}

		public IItem Insert(IsoRect bounds, T content) {
			if ( _root == null ) {
				var initial_bounds = new IsoRect(
					bounds.center - bounds.size * 2.0f,
					bounds.center + bounds.size * 2.0f);
				_root = _nodePool.Take().Init(null, initial_bounds);
			}
			Item item;
			while ( !_root.Insert(bounds, content, out item, _nodePool, _itemPool) ) {
				GrowUp(
					bounds.center.x < _root.Bounds.center.x,
					bounds.center.y < _root.Bounds.center.y);
			}
			return item;
		}

		public void Clear() {
			if ( _root != null ) {
				_nodePool.Release(
					_root.Clear(_nodePool, _itemPool));
				_root = null;
			}
		}

		public void VisitAllBounds(System.Action<IsoRect> act) {
			if ( _root != null ) {
				_root.VisitAllBounds(act);
			}
		}

		public void VisitItemsByBounds(IsoRect bounds, System.Action<T> act) {
			if ( _root != null ) {
				_root.VisitItemsByBounds(bounds, act);
			}
		}

		//
		// Private
		//

		void GrowUp(bool left, bool top) {
			var new_root_bounds = _root.Bounds;
			new_root_bounds.Translate(
				left ? -_root.Bounds.size.x : 0.0f,
				top  ? -_root.Bounds.size.y : 0.0f);
			new_root_bounds.Resize(_root.Bounds.size * 2.0f);
			var new_root = _nodePool.Take().Init(null, new_root_bounds);
			if ( left ) {
				if ( top ) {
					new_root.Children[3] = _root;
				} else {
					new_root.Children[1] = _root;
				}
			} else {
				if ( top ) {
					new_root.Children[2] = _root;
				} else {
					new_root.Children[0] = _root;
				}
			}
			_root.Parent = new_root;
			_root = new_root;
		}
	}
}