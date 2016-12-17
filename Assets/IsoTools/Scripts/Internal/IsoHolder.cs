using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Internal {
	public abstract class IsoHolder<THold, TInst> : MonoBehaviour
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

		public bool IsActive() {
			return isActiveAndEnabled && gameObject.activeInHierarchy;
		}

		public void AddInstance(TInst instance) {
			if ( instance != null && instance.IsActive() ) {
				_instances.Add(instance);
				OnAddInstanceToHolder(instance);
			}
		}

		public void RemoveInstance(TInst instance) {
			if ( instance != null ) {
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

		protected virtual void OnEnable() {
			RecacheChildrenHolders();
		}

		protected virtual void OnDisable() {
			RecacheChildrenHolders();
		}

		protected virtual void OnAddInstanceToHolder(TInst instance) {
		}

		protected virtual void OnRemoveInstanceFromHolder(TInst instance) {
		}
	}
}