using UnityEngine;
using System.Collections;

namespace IsoTools.Internal {
	public class IsoPhysicHelperHolder : MonoBehaviour {
		protected GameObject fakeObject {
			get { return physicHelper.isoFakeObject; }
		}

		protected IsoPhysicHelper physicHelper {
			get { return IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject); }
		}

		protected void DestroyUnnecessaryCheck() {
			physicHelper.DestroyIfUnnecessary(this);
		}
	}
}