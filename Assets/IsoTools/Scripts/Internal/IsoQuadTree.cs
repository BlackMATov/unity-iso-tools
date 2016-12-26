using System.Collections.Generic;

namespace IsoTools.Internal {
	public class IsoQuadTree<T> {

		// ---------------------------------------------------------------------
		//
		// Interfaces
		//
		// ---------------------------------------------------------------------

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
				bool has_any_nodes = false;
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					var node = Nodes[i];
					if ( node != null ) {
						has_any_nodes = true;
						if ( node.AddItem(bounds, content, out item, node_pool, item_pool) ) {
							return true;
						}
					}
				}
				if ( has_any_nodes || Items.Count >= MinChildCountPerNode ) {
					for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
						var node = Nodes[i];
						if ( node == null ) {
							if ( NodeBounds[i].Contains(bounds) ) {
								Nodes[i] = node = node_pool.Take().Init(this, NodeBounds[i]);
								if ( node.AddItem(bounds, content, out item, node_pool, item_pool) ) {
									return true;
								}
							}
						}
					}
				}
				item = item_pool.Take().Init(this, bounds, content);
				Items.Add(item);
				return true;
			}

			public void RemoveItem(Item item, IsoIPool<Item> item_pool) {
				if ( Items.Remove(item) ) {
					item_pool.Release(item.Clear());
				}
			}

			public void VisitAllBounds(IBoundsLookUpper look_upper) {
				look_upper.LookUp(SelfBounds);
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					if ( Nodes[i] != null ) {
						Nodes[i].VisitAllBounds(look_upper);
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

		class Item {
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
			public ItemPool(int capacity) : base(capacity) {
			}

			public override Item CreateItem() {
				return new Item();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Members
		//
		// ---------------------------------------------------------------------

		Node                _rootNode = null;
		Dictionary<T, Item> _allItems = null;
		IsoIPool<Node>      _nodePool = null;
		IsoIPool<Item>      _itemPool = null;

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

		public IsoQuadTree(int capacity) {
			_rootNode = null;
			_allItems = new Dictionary<T, Item>(capacity);
			_nodePool = new NodePool(capacity);
			_itemPool = new ItemPool(capacity);
		}

		public void AddItem(IsoRect bounds, T content) {
			if ( _allItems.ContainsKey(content) ) {
				MoveItem(bounds, content);
			} else if ( bounds.x.size > 0.0f && bounds.y.size > 0.0f ) {
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
				_allItems.Add(content, item);
				_rootNode.CleanUpNodes(_nodePool, _itemPool);
			}
		}

		public void RemoveItem(T content) {
			Item item;
			if ( _allItems.TryGetValue(content, out item) ) {
				_allItems.Remove(content);
				var item_node = item.Owner;
				item_node.RemoveItem(item, _itemPool);
				if ( item_node.Items.Count == 0 ) {
					BackwardNodeCleanUp(item_node);
				}
			}
		}

		public void MoveItem(IsoRect bounds, T content) {
			Item item;
			if ( _allItems.TryGetValue(content, out item) ) {
				var item_node = item.Owner;
				if ( item_node.SelfBounds.Contains(bounds) && item_node.Items.Count <= MinChildCountPerNode ) {
					item.Bounds = bounds;
				} else {
					item_node.RemoveItem(item, _itemPool);
					if ( item_node.Items.Count == 0 ) {
						item_node = BackwardNodeCleanUp(item_node) ?? _rootNode;
					}
					while ( item_node != null ) {
						Item new_item;
						if ( item_node.SelfBounds.Contains(bounds) ) {
							if ( item_node.AddItem(bounds, content, out new_item, _nodePool, _itemPool) ) {
								_allItems[content] = new_item;
								return;
							}
						}
						item_node = item_node.Parent;
					}
					_allItems.Remove(content);
					AddItem(bounds, content);
				}
			} else {
				AddItem(bounds, content);
			}
		}

		public void Clear() {
			if ( _rootNode != null ) {
				_nodePool.Release(
					_rootNode.Clear(_nodePool, _itemPool));
				_rootNode = null;
			}
			_allItems.Clear();
		}

		public void VisitAllBounds(IBoundsLookUpper look_upper) {
			if ( look_upper == null ) {
				throw new System.ArgumentNullException("look_upper");
			}
			if ( _rootNode != null ) {
				_rootNode.VisitAllBounds(look_upper);
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

		public void VisitItemsByContent(T content, IContentLookUpper look_upper) {
			if ( content == null ) {
				throw new System.ArgumentNullException("content");
			}
			if ( look_upper == null ) {
				throw new System.ArgumentNullException("look_upper");
			}
			Item item;
			if ( _allItems.TryGetValue(content, out item) ) {
				item.Owner.VisitItemsByBounds(item.Bounds, look_upper);
				BackwardVisitNodes(item.Owner.Parent, item.Bounds, look_upper);
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
			_rootNode = new_root;
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