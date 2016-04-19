using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoCollisionListener : MonoBehaviour {

		IsoFakeCollisionListener _fakeListener;

		GameObject fakeObject {
			get { return physicHelper.isoFakeObject; }
		}

		IsoPhysicHelper physicHelper {
			get { return IsoUtils.GetOrCreateComponent<IsoPhysicHelper>(gameObject); }
		}

		void Awake() {
			_fakeListener = fakeObject.AddComponent<IsoFakeCollisionListener>().Init(this);
		}

		void OnDestroy() {
			if ( _fakeListener ) {
				Destroy(_fakeListener);
			}
			physicHelper.DestroyIfUnnecessary(this);
		}
	}
}