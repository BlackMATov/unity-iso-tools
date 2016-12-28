namespace IsoTools.Internal {
	public class IsoQuadTree<T> {

		// ---------------------------------------------------------------------
		//
		// Interfaces
		//
		// ---------------------------------------------------------------------

		public interface IItem {
		}

		public interface IBoundsLookUpper {
			void LookUp(IsoRect bounds);
		}

		public interface IContentLookUpper {
			void LookUp(T content);
		}

		// ---------------------------------------------------------------------
		//
		// Node
		//
		// ---------------------------------------------------------------------

		const int MinChildCountPerNode = 3;

		class Node {
			public Node[]             Nodes      = new Node[4];
			public IsoAssocList<Item> Items      = new IsoAssocList<Item>(MinChildCountPerNode);
			public Node               Parent     = null;
			public IsoRect            SelfBounds = IsoRect.zero;
			public IsoRect[]          NodeBounds = new IsoRect[4];

			public Node Init(Node parent, IsoRect bounds) {
				Parent     = parent;
				SelfBounds = bounds;
				return FillNodeBounds();
			}

			public Node Clear(
				IsoIPool<Node> node_pool, IsoIPool<Item> item_pool)
			{
				ClearNodes(node_pool, item_pool);
				ClearItems(item_pool);
				return Init(null, IsoRect.zero);
			}

			public bool CleanUpNodes(
				IsoIPool<Node> node_pool, IsoIPool<Item> item_pool)
			{
				var has_any_busy_nodes = false;
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null ) {
						if ( node.CleanUpNodes(node_pool, item_pool) ) {
							node_pool.Release(
								node.Clear(node_pool, item_pool));
							Nodes[i] = null;
						} else {
							has_any_busy_nodes = true;
						}
					}
				}
				return !has_any_busy_nodes && Items.Count == 0;
			}

			public bool AddItem(
				IsoRect bounds, T content, out Item item,
				IsoIPool<Node> node_pool, IsoIPool<Item> item_pool)
			{
				if ( !SelfBounds.Contains(bounds) ) {
					item = null;
					return false;
				}
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null ) {
						if ( node.AddItem(bounds, content, out item, node_pool, item_pool) ) {
							return true;
						}
					} else if ( Items.Count >= MinChildCountPerNode && NodeBounds[i].Contains(bounds) ) {
						Nodes[i] = node = node_pool.Take().Init(this, NodeBounds[i]);
						if ( node.AddItem(bounds, content, out item, node_pool, item_pool) ) {
							return true;
						}
					}
				}
				item = item_pool.Take().Init(this, bounds, content);
				Items.Add(item);
				return true;
			}

			public void RemoveItem(Item item, IsoIPool<Item> item_pool) {
				if ( item != null && item.Owner == this && Items.Remove(item) ) {
					item_pool.Release(item.Clear());
				} else {
					throw new System.ArgumentException("IsoQuadTree logic error", "item");
				}
			}

			public bool HasAnyItems() {
				if ( Items.Count > 0 ) {
					return true;
				}
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null && node.HasAnyItems() ) {
						return true;
					}
				}
				return false;
			}

			public void VisitAllBounds(IBoundsLookUpper look_upper) {
				look_upper.LookUp(SelfBounds);
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null ) {
						node.VisitAllBounds(look_upper);
					}
				}
			}

			public void VisitItemsByBounds(
				IsoRect bounds, IContentLookUpper look_upper)
			{
				if ( bounds.Overlaps(SelfBounds) ) {
					for ( int i = 0, e = Items.Count; i < e; ++i ) {
						var item = Items[i];
						if ( bounds.Overlaps(item.Bounds) ) {
							look_upper.LookUp(item.Content);
						}
					}
					for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
						var node = Nodes[i];
						if ( node != null ) {
							node.VisitItemsByBounds(bounds, look_upper);
						}
					}
				}
			}

			//
			// Private
			//

			Node FillNodeBounds() {
				var size   = SelfBounds.size * 0.5f;
				var center = SelfBounds.center;
				{ // LT
					var rect = new IsoRect(center - size, center);
					NodeBounds[0] = rect;
				}
				{ // RT
					var rect = new IsoRect(center - size, center);
					rect.Translate(size.x, 0.0f);
					NodeBounds[1] =  rect;
				}
				{ // LB
					var rect = new IsoRect(center, center + size);
					rect.Translate(-size.x, 0.0f);
					NodeBounds[2] = rect;
				}
				{ // RB
					var rect = new IsoRect(center, center + size);
					NodeBounds[3] = rect;
				}
				return this;
			}

			void ClearNodes(IsoIPool<Node> node_pool, IsoIPool<Item> item_pool) {
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null ) {
						node_pool.Release(node.Clear(node_pool, item_pool));
					}
				}
				System.Array.Clear(Nodes, 0, Nodes.Length);
			}

			void ClearItems(IsoIPool<Item> item_pool) {
				for ( int i = 0, e = Items.Count; i < e; ++i ) {
					var item = Items[i];
					item_pool.Release(item.Clear());
				}
				Items.Clear();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Item
		//
		// ---------------------------------------------------------------------

		class Item : IItem {
			public int     QTId    = 0;
			public Node    Owner   = null;
			public IsoRect Bounds  = IsoRect.zero;
			public T       Content = default(T);

			public Item(int qtId) {
				QTId = qtId;
			}

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

		// ---------------------------------------------------------------------
		//
		// Pools
		//
		// ---------------------------------------------------------------------

		class NodePool : IsoPool<Node> {
			public NodePool(int capacity) : base(capacity) {
			}

			public override Node CreateItem() {
				return new Node();
			}
		}

		class ItemPool : IsoPool<Item> {
			int _qtId = 0;

			public ItemPool(int qtId, int capacity) : base(capacity) {
				_qtId = qtId;
			}

			public override Item CreateItem() {
				return new Item(_qtId);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Members
		//
		// ---------------------------------------------------------------------

		int            _qtId     = 0;
		static int     _genQTId  = 0;

		Node           _rootNode = null;
		IsoIPool<Node> _nodePool = null;
		IsoIPool<Item> _itemPool = null;

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public IsoQuadTree(int capacity) {
			_qtId     = ++_genQTId;
			_rootNode = null;
			_nodePool = new NodePool(capacity);
			_itemPool = new ItemPool(_qtId, capacity);
		}

		public IItem AddItem(IsoRect bounds, T content) {
			if ( bounds.x.size > 0.0f && bounds.y.size > 0.0f ) {
				if ( _rootNode == null ) {
					var initial_side = IsoUtils.Vec2From(
						IsoUtils.Vec2MaxF(bounds.size));
					var initial_bounds = new IsoRect(
						bounds.center - initial_side * 2.0f,
						bounds.center + initial_side * 2.0f);
					_rootNode = _nodePool.Take().Init(null, initial_bounds);
				}
				Item item;
				while ( !_rootNode.AddItem(bounds, content, out item, _nodePool, _itemPool) ) {
					GrowUp(
						bounds.center.x < _rootNode.SelfBounds.center.x,
						bounds.center.y < _rootNode.SelfBounds.center.y);
				}
				return item;
			} else {
				return _itemPool.Take().Init(null, bounds, content);
			}
		}

		public void RemoveItem(IItem iitem) {
			var item = GetItemWithCast(iitem);
			var item_node = item.Owner;
			if ( item_node != null ) {
				item_node.RemoveItem(item, _itemPool);
				if ( item_node.Items.Count == 0 ) {
					BackwardNodeCleanUp(item_node);
				}
			} else {
				_itemPool.Release(item.Clear());
			}
		}

		public IItem MoveItem(IsoRect bounds, IItem iitem) {
			var item = GetItemWithCast(iitem);
			var item_node = item.Owner;
			if ( item_node != null ) {
				if ( item_node.SelfBounds.Contains(bounds) && item_node.Items.Count <= MinChildCountPerNode ) {
					item.Bounds = bounds;
					return item;
				} else {
					var content = item.Content;
					item_node.RemoveItem(item, _itemPool);
					if ( item_node.Items.Count == 0 ) {
						item_node = BackwardNodeCleanUp(item_node) ?? _rootNode;
					}
					while ( item_node != null ) {
						Item new_item;
						if ( item_node.SelfBounds.Contains(bounds) ) {
							if ( item_node.AddItem(bounds, content, out new_item, _nodePool, _itemPool) ) {
								return new_item;
							}
						}
						item_node = item_node.Parent;
					}
					return AddItem(bounds, content);
				}
			} else {
				var content = item.Content;
				_itemPool.Release(item.Clear());
				return AddItem(bounds, content);
			}
		}

		public void Clear() {
			if ( _rootNode != null ) {
				_nodePool.Release(
					_rootNode.Clear(_nodePool, _itemPool));
				_rootNode = null;
			}
		}

		public void VisitAllBounds(IBoundsLookUpper look_upper) {
			if ( look_upper == null ) {
				throw new System.ArgumentNullException("look_upper");
			}
			if ( _rootNode != null ) {
				_rootNode.VisitAllBounds(look_upper);
			}
		}

		public void VisitItemsByItem(IItem iitem, IContentLookUpper look_upper) {
			var item = GetItemWithCast(iitem);
			var item_node = item.Owner;
			if ( item_node != null ) {
				item_node.VisitItemsByBounds(item.Bounds, look_upper);
				BackwardVisitNodes(item_node.Parent, item.Bounds, look_upper);
			}
		}

		public void VisitItemsByBounds(IsoRect bounds, IContentLookUpper look_upper) {
			if ( look_upper == null ) {
				throw new System.ArgumentNullException("look_upper");
			}
			if ( _rootNode != null ) {
				_rootNode.VisitItemsByBounds(bounds, look_upper);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void GrowUp(bool left, bool top) {
			var new_root_bounds = _rootNode.SelfBounds;
			new_root_bounds.Translate(
				left ? -new_root_bounds.size.x : 0.0f,
				top  ? -new_root_bounds.size.y : 0.0f);
			new_root_bounds.Resize(new_root_bounds.size * 2.0f);
			var new_root = _nodePool.Take().Init(null, new_root_bounds);
			if ( _rootNode.HasAnyItems() ) {
				if ( left ) {
					if ( top ) {
						new_root.Nodes[3] = _rootNode;
					} else {
						new_root.Nodes[1] = _rootNode;
					}
				} else {
					if ( top ) {
						new_root.Nodes[2] = _rootNode;
					} else {
						new_root.Nodes[0] = _rootNode;
					}
				}
				_rootNode.Parent = new_root;
			} else {
				_nodePool.Release(
					_rootNode.Clear(_nodePool, _itemPool));
				_rootNode = null;
			}
			_rootNode = new_root;
		}

		Item GetItemWithCast(IItem iitem) {
			if ( iitem == null ) {
				throw new System.ArgumentNullException("iitem");
			}
			var item = iitem as Item;
			if ( item == null || item.QTId != _qtId ) {
				throw new System.ArgumentException("item from another IsoQuadTree", "iitem");
			}
			return item;
		}

		Node BackwardNodeCleanUp(Node node) {
			while ( node != null && node.CleanUpNodes(_nodePool, _itemPool) ) {
				node = node.Parent;
			}
			return node;
		}

		void BackwardVisitNodes(Node node, IsoRect bounds, IContentLookUpper loop_upper) {
			while ( node != null ) {
				for ( int i = 0, e = node.Items.Count; i < e; ++i ) {
					var item = node.Items[i];
					if ( bounds.Overlaps(item.Bounds) ) {
						loop_upper.LookUp(item.Content);
					}
				}
				node = node.Parent;
			}
		}
	}
}