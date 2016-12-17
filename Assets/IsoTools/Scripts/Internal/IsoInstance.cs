using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Internal {
	public abstract class IsoInstance<THold, TInst> : MonoBehaviour
		where THold : IsoHolder   <THold, TInst>
		where TInst : IsoInstance <THold, TInst>
	{
		THold              _holder      = null;
		static List<THold> _tempHolders = new List<THold>();

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public bool IsActive() {
			return isActiveAndEnabled && gameObject.activeInHierarchy;
		}

		public void ResetHolder() {
			if ( _holder ) {
				_holder.RemoveInstance(this as TInst);
				_holder = null;
			}
		}

		public void RecacheHolder() {
			ResetHolder();
			if ( IsActive() ) {
				GetComponentsInParent<THold>(false, _tempHolders);
				for ( int i = 0, e = _tempHolders.Count; i < e; ++i ) {
					var holder = _tempHolders[i];
					if ( holder.IsActive() ) {
						_holder = holder;
						break;
					}
				}
				_tempHolders.Clear();
			}
			if ( _holder ) {
				_holder.AddInstance(this as TInst);
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
		// Messages
		//
		// ---------------------------------------------------------------------

		protected virtual void OnEnable() {
			RecacheHolder();
		}

		protected virtual void OnDisable() {
			ResetHolder();
		}

		protected virtual void OnTransformParentChanged() {
			RecacheHolder();
		}
	}
}