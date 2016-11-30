using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoPhysicsHelperHolder : MonoBehaviour {

		static List<IsoPhysicsHelperHolder> _tmpHolders = new List<IsoPhysicsHelperHolder>(7);

		protected GameObject fakeObject {
			get { return physicsHelper.isoFakeObject; }
		}

		protected IsoPhysicsHelper physicsHelper {
			get { return IsoUtils.GetOrCreateComponent<IsoPhysicsHelper>(gameObject); }
		}

		protected void DestroyUnnecessaryCheck() {
			GetComponents<IsoPhysicsHelperHolder>(_tmpHolders);
			if ( _tmpHolders.Count == 1 && _tmpHolders[0] == this ) {
				Destroy(physicsHelper);
			}
			_tmpHolders.Clear();
		}
	}
}