using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoTriggerListener : IsoPhysicsHelperHolder {

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