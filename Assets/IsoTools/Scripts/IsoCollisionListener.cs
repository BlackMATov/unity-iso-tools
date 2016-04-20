using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoCollisionListener : IsoPhysicHelperHolder {

		IsoFakeCollisionListener _fakeListener;

		void Awake() {
			_fakeListener = fakeObject.AddComponent<IsoFakeCollisionListener>().Init(this);
		}

		void OnDestroy() {
			if ( _fakeListener ) {
				Destroy(_fakeListener);
			}
			DestroyUnnecessaryCheck();
		}
	}
}