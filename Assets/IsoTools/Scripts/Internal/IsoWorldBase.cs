using System.Collections.Generic;

namespace IsoTools.Internal {
	public abstract class IsoWorldBase : IsoBehaviour<IsoWorld> {
		IsoAssocList<IsoObject> _isoObjects     = new IsoAssocList<IsoObject>();
		static List<IsoObject>  _tempIsoObjects = new List<IsoObject>();

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		void RecacheIsoObjectWorlds() {
			GetComponentsInChildren<IsoObject>(false, _tempIsoObjects);
			for ( int i = 0, e = _tempIsoObjects.Count; i < e; ++i ) {
				_tempIsoObjects[i].Internal_RecacheIsoWorld();
			}
			_tempIsoObjects.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		public void Internal_AddIsoObject(IsoObject iso_object) {
			if ( iso_object && iso_object.IsActive() ) {
				_isoObjects.Add(iso_object);
				OnAddIsoObjectToWorld(iso_object);
			}
		}

		public void Internal_RemoveIsoObject(IsoObject iso_object) {
			if ( iso_object ) {
				_isoObjects.Remove(iso_object);
				OnRemoveIsoObjectFromWorld(iso_object);
			}
		}

		protected IsoAssocList<IsoObject> GetIsoObjects() {
			return _isoObjects;
		}

		// ---------------------------------------------------------------------
		//
		// Virtual
		//
		// ---------------------------------------------------------------------

		protected override void OnEnable() {
			base.OnEnable();
			RecacheIsoObjectWorlds();
		}

		protected override void OnDisable() {
			base.OnDisable();
			RecacheIsoObjectWorlds();
		}

		protected virtual void OnAddIsoObjectToWorld(IsoObject iso_object) {
		}

		protected virtual void OnRemoveIsoObjectFromWorld(IsoObject iso_object) {
		}
	}
}