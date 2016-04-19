using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoTriggerListener : MonoBehaviour {

		IsoFakeTriggerListener _fakeListener;

		GameObject fakeObject {
			get { return physicHelper.isoFakeObject; }
		}

		IsoPhysicHelper physicHelper {
			get { return IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject); }
		}

		void Awake() {
			_fakeListener = fakeObject.AddComponent<IsoFakeTriggerListener>().Init(this);
		}

		void OnDestroy() {
			if ( _fakeListener ) {
				Destroy(_fakeListener);
			}
			physicHelper.DestroyIfUnnecessary(this);
		}
	}
}