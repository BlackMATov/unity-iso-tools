using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoTriggerListener : IsoPhysicHelperHolder {

		IsoFakeTriggerListener _fakeListener;

		void Awake() {
			_fakeListener = fakeObject.AddComponent<IsoFakeTriggerListener>().Init(this);
		}

		void OnDestroy() {
			if ( _fakeListener ) {
				Destroy(_fakeListener);
			}
			DestroyUnnecessaryCheck();
		}
	}
}