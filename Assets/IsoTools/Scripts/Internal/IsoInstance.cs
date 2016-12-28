using System.Collections.Generic;

namespace IsoTools.Internal {
	public abstract class IsoInstance<THold, TInst> : IsoBehaviour<TInst>
		where THold : IsoHolder   <THold, TInst>
		where TInst : IsoInstance <THold, TInst>
	{
		THold              _holder      = null;
		static List<THold> _tempHolders = new List<THold>();

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		THold FindFirstActiveHolder() {
			THold ret_value = null;
			GetComponentsInParent<THold>(false, _tempHolders);
			for ( int i = 0, e = _tempHolders.Count; i < e; ++i ) {
				var holder = _tempHolders[i];
				if ( holder.IsActive() ) {
					ret_value = holder;
					break;
				}
			}
			_tempHolders.Clear();
			return ret_value;
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void ResetHolder() {
			if ( _holder ) {
				_holder.RemoveInstance(this as TInst);
				_holder = null;
			}
		}

		public void RecacheHolder() {
			ResetHolder();
			if ( IsActive() ) {
				_holder = FindFirstActiveHolder();
				if ( _holder ) {
					_holder.AddInstance(this as TInst);
				}
			}
		}

		protected THold GetHolder() {
			if ( !_holder ) {
				RecacheHolder();
			}
			return _holder;
		}

		// ---------------------------------------------------------------------
		//
		// Virtual
		//
		// ---------------------------------------------------------------------

		protected override void OnEnable() {
			base.OnEnable();
			RecacheHolder();
		}

		protected override void OnDisable() {
			base.OnDisable();
			ResetHolder();
		}

		protected virtual void OnTransformParentChanged() {
			RecacheHolder();
		}
	}
}