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
			GetComponents<IsoPhysicHelperHolder>(_tmpHolders);
			if ( _tmpHolders.Count == 1 && _tmpHolders[0] == this ) {
				Destroy(physicHelper);
			}
			_tmpHolders.Clear();
		}
	}
}