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
		// ItemPool
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

		class ItemPool : IsoPool<Item> {
			public ItemPool(int capacity) : base(capacity) {
			}

			public override Item CreateItem() {
				return new Item();
			}
		}

		// ---------------------------------------------------------------------
		//
		// NodePool
		//
		// ---------------------------------------------------------------------

		const int MinChildCountPerNode = 3;

		class Node {
			public Node[]             Nodes      = new Node[4];
			public IsoAssocList<Item> Items      = new IsoAssocList<Item>(MinChildCountPerNode);
			public Node               Parent     = null;
			public IsoRect            Bounds     = IsoRect.zero;
			public IsoRect[]          NodeBounds = new IsoRect[4];

			public Node Init(Node parent, IsoRect bounds) {
				Parent = parent;
				Bounds = bounds;
				return FillNodeBounds();
			}

			public Node Clear(IsoIPool<Node> node_pool, IsoIPool<Item> item_pool) {
				ClearNodes(node_pool, item_pool);
				ClearItems(item_pool);
				return Init(null, IsoRect.zero);
			}

			public bool AddItem(
				IsoRect bounds, T content, out Item item,
				IsoIPool<Node> node_pool, IsoIPool<Item> item_pool)
			{
				if ( !Bounds.Contains(bounds) ) {
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

			public void RemoveItem(Item item) {
				Items.Remove(item);
			}

			public void VisitAllBounds(IBoundsLookUpper look_upper) {
				look_upper.LookUp(Bounds);
				for ( int i = 0, e = Nodes.Length; i < e; ++i ) {
					if ( Nodes[i] != null ) {
						Nodes[i].VisitAllBounds(look_upper);
					}
				}
			}

			public void VisitItemsByBounds(IsoRect bounds, IContentLookUpper look_upper) {
				if ( Bounds.Overlaps(bounds) ) {
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
				var size   = Bounds.size * 0.5f;
				var center = Bounds.center;
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
						node_pool.Release(
							node.Clear(node_pool, item_pool));
					}
				}
				System.Array.Clear(Nodes, 0, Nodes.Length);
			}

			void ClearItems(IsoIPool<Item> item_pool) {
				for ( int i = 0, e = Items.Count; i < e; ++i ) {
					var item = Items[i];
					item_pool.Release(
						item.Clear());
				}
				Items.Clear();
			}
		}

		class NodePool : IsoPool<Node> {
			public NodePool(int capacity) : base(capacity) {
			}

			public override Node CreateItem() {
				return new Node();
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
			if ( bounds.x.size > 0.0f && bounds.y.size > 0.0f ) {
				if ( _allItems.ContainsKey(content) ) {
					MoveItem(bounds, content);
				} else {
					if ( _rootNode == null ) {
						var initial_bounds = new IsoRect(
							bounds.center - bounds.size * 2.0f,
							bounds.center + bounds.size * 2.0f);
						_rootNode = _nodePool.Take().Init(null, initial_bounds);
					}
					Item item;
					while ( !_rootNode.AddItem(bounds, content, out item, _nodePool, _itemPool) ) {
						GrowUp(
							bounds.center.x < _rootNode.Bounds.center.x,
							bounds.center.y < _rootNode.Bounds.center.y);
					}
					_allItems.Add(content, item);
				}
			}
		}

		public bool RemoveItem(T content) {
			Item item;
			if ( _allItems.TryGetValue(content, out item) ) {
				if ( item.Owner != null ) {
					item.Owner.RemoveItem(item);
				}
				_allItems.Remove(content);
				return true;
			} else {
				return false;
			}
		}

		public void MoveItem(IsoRect bounds, T content) {
			//TODO implme
			RemoveItem(content);
			AddItem(bounds, content);
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

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void GrowUp(bool left, bool top) {
			var new_root_bounds = _rootNode.Bounds;
			new_root_bounds.Translate(
				left ? -_rootNode.Bounds.size.x : 0.0f,
				top  ? -_rootNode.Bounds.size.y : 0.0f);
			new_root_bounds.Resize(_rootNode.Bounds.size * 2.0f);
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
	}
}