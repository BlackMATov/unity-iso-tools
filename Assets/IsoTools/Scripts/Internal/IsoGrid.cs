using UnityEngine;

namespace IsoTools.Internal {
	public class IsoGrid<T> {

		// ---------------------------------------------------------------------
		//
		// CellPool
		//
		// ---------------------------------------------------------------------

		class Cell {
			public IsoList<T> Items = new IsoList<T>();
		}

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

		// ---------------------------------------------------------------------
		//
		// Interfaces
		//
		// ---------------------------------------------------------------------

		public interface IItemAdapter {
			IsoRect GetBounds      (T item);
			void    SetMinMaxCells (T item, IsoPoint2 min, IsoPoint2 max);
			void    GetMinMaxCells (T item, ref IsoPoint2 min, ref IsoPoint2 max);
		}

		public interface ILookUpper {
			void LookUp(IsoList<T> items);
		}

		// ---------------------------------------------------------------------
		//
		// Members
		//
		// ---------------------------------------------------------------------

		IsoIPool<Cell> _cellPool        = null;
		IsoList<Cell>  _gridCells       = null;

		IsoList<T>     _gridItems       = null;
		IItemAdapter   _itemAdapter     = null;

		float          _gridCellSize    = 0.0f;
		IsoPoint2      _gridMinNumPos   = IsoPoint2.zero;
		IsoPoint2      _gridMaxNumPos   = IsoPoint2.zero;
		IsoPoint2      _gridNumPosCount = IsoPoint2.zero;

		// ---------------------------------------------------------------------
		//
		// Public
		//
		// ---------------------------------------------------------------------

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
			if ( min_cell_size < Mathf.Epsilon ) {
				throw new System.ArgumentOutOfRangeException(
					"min_cell_size", "min_cell_size must be >= Mathf.Epsilon");
			}
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
			_gridMinNumPos   = IsoPoint2.zero;
			_gridMaxNumPos   = IsoPoint2.zero;
			_gridNumPosCount = IsoPoint2.zero;
		}

		public void LookUpCells(
			IsoPoint2 min_cell, IsoPoint2 max_cell, ILookUpper look_upper)
		{
			if ( min_cell.x < 0 || min_cell.y < 0 ) {
				throw new System.ArgumentOutOfRangeException("min_cell");
			}
			if ( min_cell.y >= _gridNumPosCount.x || min_cell.y >= _gridNumPosCount.y ) {
				throw new System.ArgumentOutOfRangeException("max_cell");
			}
			if ( look_upper == null ) {
				throw new System.ArgumentNullException("look_upper");
			}
			for ( int y = min_cell.y, ye = max_cell.y; y < ye; ++y ) {
			for ( int x = min_cell.x, xe = max_cell.x; x < xe; ++x ) {
				var cell = GetCell(x, y);
				if ( cell.Items.Count > 0 ) {
					look_upper.LookUp(cell.Items);
				}
			}}
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void CalculateCellSize(float min_cell_size) {
			_gridCellSize = 0.0f;
			for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
				var bounds = _itemAdapter.GetBounds(_gridItems[i]);
				var size_x = bounds.x.max - bounds.x.min;
				var size_y = bounds.y.max - bounds.y.min;
				_gridCellSize += size_x > size_y ? size_x : size_y;
			}
			_gridCellSize = _gridItems.Count > 0
				? Mathf.Max(min_cell_size, _gridCellSize / _gridItems.Count)
				: min_cell_size;
		}

		void PrepareGridNumPos() {
			if ( _gridItems.Count > 0 ) {
				_gridMinNumPos.Set(int.MaxValue, int.MaxValue);
				_gridMaxNumPos.Set(int.MinValue, int.MinValue);
				for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
					var item    = _gridItems[i];
					var bounds  = _itemAdapter.GetBounds(item);
					var min_f_x = bounds.x.min / _gridCellSize;
					var min_f_y = bounds.y.min / _gridCellSize;
					var max_f_x = bounds.x.max / _gridCellSize;
					var max_f_y = bounds.y.max / _gridCellSize;
					var min_i_x = (int)(min_f_x >= 0.0f ? min_f_x : min_f_x - 1.0f);
					var min_i_y = (int)(min_f_y >= 0.0f ? min_f_y : min_f_y - 1.0f);
					var max_i_x = (int)(max_f_x >= 0.0f ? max_f_x + 1.0f : max_f_x);
					var max_i_y = (int)(max_f_y >= 0.0f ? max_f_y + 1.0f : max_f_y);
					if ( _gridMinNumPos.x > min_i_x ) {
						_gridMinNumPos.x = min_i_x;
					}
					if ( _gridMinNumPos.y > min_i_y ) {
						_gridMinNumPos.y = min_i_y;
					}
					if ( _gridMaxNumPos.x < max_i_x ) {
						_gridMaxNumPos.x = max_i_x;
					}
					if ( _gridMaxNumPos.y < max_i_y ) {
						_gridMaxNumPos.y = max_i_y;
					}
					_itemAdapter.SetMinMaxCells(
						item,
						new IsoPoint2(min_i_x, min_i_y),
						new IsoPoint2(max_i_x, max_i_y));
				}
			} else {
				_gridMinNumPos.Set(0, 0);
				_gridMaxNumPos.Set(1, 1);
			}
			_gridNumPosCount = _gridMaxNumPos - _gridMinNumPos;
		}

		void SetupGridCells() {
			var cell_count = _gridNumPosCount.x * _gridNumPosCount.y;
			if ( _gridCells.Capacity < cell_count ) {
				_gridCells.Capacity = cell_count * 2;
			}
			while ( _gridCells.Count < cell_count ) {
				_gridCells.Add(_cellPool.Take());
			}
		}

		void FillGridCells() {
			var min_cell = IsoPoint2.zero;
			var max_cell = IsoPoint2.zero;
			for ( int i = 0, e = _gridItems.Count; i < e; ++i ) {
				var item = _gridItems[i];
				_itemAdapter.GetMinMaxCells(item, ref min_cell, ref max_cell);
				min_cell -= _gridMinNumPos;
				max_cell -= _gridMinNumPos;
				_itemAdapter.SetMinMaxCells(item, min_cell, max_cell);
				for ( int y = min_cell.y, ye = max_cell.y; y < ye; ++y ) {
				for ( int x = min_cell.x, xe = max_cell.x; x < xe; ++x ) {
					var cell = GetCell(x, y);
					cell.Items.Add(item);
				}}
			}
		}

		Cell GetCell(int cell_x, int cell_y) {
			var cell_index = cell_x + _gridNumPosCount.x * cell_y;
			return _gridCells[cell_index];
		}
	}
}