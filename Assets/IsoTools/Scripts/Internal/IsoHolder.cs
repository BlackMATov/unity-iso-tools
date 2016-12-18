using System.Collections.Generic;

namespace IsoTools.Internal {
	public abstract class IsoHolder<THold, TInst> : IsoBehaviour<THold>
		where THold : IsoHolder   <THold, TInst>
		where TInst : IsoInstance <THold, TInst>
	{
		IsoAssocList<TInst> _instances     = new IsoAssocList<TInst>();
		static List<TInst>  _tempInstances = new List<TInst>();

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void RecacheChildrenHolders() {
			GetComponentsInChildren<TInst>(false, _tempInstances);
			for ( int i = 0, e = _tempInstances.Count; i < e; ++i ) {
				_tempInstances[i].RecacheHolder();
			}
			_tempInstances.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void AddInstance(TInst instance) {
			if ( instance && instance.IsActive() ) {
				_instances.Add(instance);
				OnAddInstanceToHolder(instance);
			}
		}

		public void RemoveInstance(TInst instance) {
			if ( instance ) {
				_instances.Remove(instance);
				OnRemoveInstanceFromHolder(instance);
			}
		}

		protected IsoAssocList<TInst> GetInstances() {
			return _instances;
		}

		// ---------------------------------------------------------------------
		//
		// Virtual
		//
		// ---------------------------------------------------------------------

		protected override void OnEnable() {
			base.OnEnable();
			RecacheChildrenHolders();
		}

		protected override void OnDisable() {
			base.OnDisable();
			RecacheChildrenHolders();
		}

		protected virtual void OnAddInstanceToHolder(TInst instance) {
		}

		protected virtual void OnRemoveInstanceFromHolder(TInst instance) {
		}
	}
}