using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Internal {
	public class IsoPhysicHelperHolder : MonoBehaviour {

		static List<IsoPhysicHelperHolder> _tmpHolders = new List<IsoPhysicHelperHolder>(7);

		protected GameObject fakeObject {
			get { return physicHelper.isoFakeObject; }
		}

		protected IsoPhysicHelper physicHelper {
			get { return IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject); }
		}

		protected void DestroyUnnecessaryCheck() {
			var unnecessary = true;
			GetComponents<IsoPhysicHelperHolder>(_tmpHolders);
			for ( int i = 0, e = _tmpHolders.Count; i < e; ++i ) {
				if ( _tmpHolders[i] != this ) {
					unnecessary = false;
					break;
				}
			}
			if ( unnecessary ) {
				Destroy(physicHelper);
			}
			_tmpHolders.Clear();
		}
	}
}