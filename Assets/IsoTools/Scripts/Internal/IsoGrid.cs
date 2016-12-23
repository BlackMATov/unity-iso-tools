using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Internal {
	public class IsoGrid<T> {

		//
		// Cell
		//

		class Cell {
			public IsoList<T> Items = new IsoList<T>();
		}

		//
		// CellPool
		//

		class CellPool : IsoPool<Cell> {
			public CellPool(int capacity) : base(capacity) {
			}

			public override Cell CreateItem() {
				return new Cell();
			}

			public override void CleanUpItem(Cell item) {
				item.Items.Clear();
			}
		}

		//
		// IItemAdapter
		//

		public interface IItemAdapter {
			IsoRect GetBounds      (T item);
			void    SetMinMaxCells (T item, Vector2 min, Vector2 max);
			void    GetMinMaxCells (T item, ref Vector2 min, ref Vector2 max);
		}

		//
		// ILookUpper
		//

		public interface ILookUpper {
			void LookUp(IsoList<T> items);
		}

		//
		// Members
		//

		IsoIPool<Cell> _cellPool        = null;
		IsoList<Cell>  _gridCells       = null;

		IsoList<T>     _gridItems       = null;
		IItemAdapter   _itemAdapter     = null;

		float          _gridCellSize    = 0.0f;
		Vector2        _gridMinNumPos   = Vector2.zero;
		Vector2        _gridMaxNumPos   = Vector2.zero;
		Vector2        _gridNumPosCount = Vector2.zero;

		//
		// Public
		//

		public IsoGrid(IItemAdapter item_adapter, int capacity) {
			if ( item_adapter == null ) {
				throw new System.ArgumentNullException("item_adapter");
			}
			if ( capacity < 0 ) {
				throw new System.ArgumentOutOfRangeException(
					"capacity", "capacity must be >= 0");
			}
			_cellPool    = new CellPool(capacity);
			_gridCells   = new IsoList<Cell>(capacity);
			_gridItems   = new IsoList<T>(capacity);
			_itemAdapter = item_adapter;
		}

		public void AddItem(T content) {
			_gridItems.Add(content);
		}

		public void ClearItems() {
			_gridItems.Clear();
		}

		public void RebuildGrid(float min_cell_size) {
			ClearGrid();
			CalculateCellSize(min_cell_size);
			PrepareGridNumPos();
			SetupGridCells();
			FillGridCells();
		}

		public void ClearGrid() {
			while ( _gridCells.Count > 0 ) {
				_cellPool.Release(_gridCells.Pop());
			}
			_gridCellSize    = 0.0f;
			_gridMinNumPos   = Vector2.zero;
			_gridMaxNumPos   = Vector2.zero;
			_gridNumPosCount = Vector2.zero;
		}

		public void LookUpCells(Vector2 min, Vector2 max, ILookUpper look_upper) {
			for ( var y = min.y; y < max.y; ++y ) {
			for ( var x = min.x; x < max.x; ++x ) {
				var cell = FindCell(x, y);
				if ( cell != null ) {
					look_upper.LookUp(cell.Items);
				}
			}}
		}

		//
		// Private
		//

		void CalculateCellSize(float min_cell_size) {
			_gridCellSize = 0.0f;
			for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
				var bounds = _itemAdapter.GetBounds(_gridItems[i]);
				var size_x = bounds.x.max - bounds.x.min;
				var size_y = bounds.y.max - bounds.y.min;
				_gridCellSize += size_x > size_y ? size_x : size_y;
			}
			_gridCellSize = _gridItems.Count > 0
				? Mathf.Round(Mathf.Max(min_cell_size, _gridCellSize / _gridItems.Count))
				: min_cell_size;
		}

		void PrepareGridNumPos() {
			if ( _gridItems.Count > 0 ) {
				_gridMinNumPos.Set(float.MaxValue, float.MaxValue);
				_gridMaxNumPos.Set(float.MinValue, float.MinValue);
				for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
					var item       = _gridItems[i];
					var bounds     = _itemAdapter.GetBounds(item);
					var min_x      = bounds.x.min / _gridCellSize;
					var min_y      = bounds.y.min / _gridCellSize;
					var max_x      = bounds.x.max / _gridCellSize;
					var max_y      = bounds.y.max / _gridCellSize;
					var min_cell_x = (float)(int)(min_x >= 0.0f ? min_x : min_x - 1.0f);
					var min_cell_y = (float)(int)(min_y >= 0.0f ? min_y : min_y - 1.0f);
					var max_cell_x = (float)(int)(max_x >= 0.0f ? max_x + 1.0f : max_x);
					var max_cell_y = (float)(int)(max_y >= 0.0f ? max_y + 1.0f : max_y);
					if ( _gridMinNumPos.x > min_cell_x ) {
						_gridMinNumPos.x = min_cell_x;
					}
					if ( _gridMinNumPos.y > min_cell_y ) {
						_gridMinNumPos.y = min_cell_y;
					}
					if ( _gridMaxNumPos.x < max_cell_x ) {
						_gridMaxNumPos.x = max_cell_x;
					}
					if ( _gridMaxNumPos.y < max_cell_y ) {
						_gridMaxNumPos.y = max_cell_y;
					}
					_itemAdapter.SetMinMaxCells(
						item,
						new Vector2(min_cell_x, min_cell_y),
						new Vector2(max_cell_x, max_cell_y));
				}
			} else {
				_gridMinNumPos.Set(0.0f, 0.0f);
				_gridMaxNumPos.Set(_gridCellSize, _gridCellSize);
			}
			_gridNumPosCount = _gridMaxNumPos - _gridMinNumPos;
		}

		void SetupGridCells() {
			var cell_count = Mathf.RoundToInt(_gridNumPosCount.x * _gridNumPosCount.y);
			if ( _gridCells.Capacity < cell_count ) {
				_gridCells.Capacity = cell_count * 2;
			}
			while ( _gridCells.Count < cell_count ) {
				_gridCells.Add(_cellPool.Take());
			}
		}

		void FillGridCells() {
			var min_cell = Vector2.zero;
			var max_cell = Vector2.zero;
			for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
				var item = _gridItems[i];
				_itemAdapter.GetMinMaxCells(item, ref min_cell, ref max_cell);
				min_cell -= _gridMinNumPos;
				max_cell -= _gridMinNumPos;
				_itemAdapter.SetMinMaxCells(item, min_cell, max_cell);
				for ( var y = min_cell.y; y < max_cell.y; ++y ) {
				for ( var x = min_cell.x; x < max_cell.x; ++x ) {
					var cell = FindCell(x, y);
					if ( cell != null ) {
						cell.Items.Add(item);
					}
				}}
			}
		}

		Cell FindCell(float num_pos_x, float num_pos_y) {
			if ( num_pos_x < 0.0f || num_pos_y < 0.0f ) {
				return null;
			}
			if ( num_pos_x >= _gridNumPosCount.x || num_pos_y >= _gridNumPosCount.y ) {
				return null;
			}
			var cell_index = (int)(num_pos_x + _gridNumPosCount.x * num_pos_y);
			return _gridCells[cell_index];
		}
	}
}