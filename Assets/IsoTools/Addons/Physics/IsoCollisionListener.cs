using UnityEngine;
using IsoTools.Physics.Internal;

namespace IsoTools.Physics {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoCollisionListener : IsoPhysicsHelperHolder {

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