using UnityEngine;

namespace IsoTools {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoPhysicHelper : MonoBehaviour {

		GameObject _isoFakeObject = null;
		public GameObject isoFakeObject {
			get { return _isoFakeObject; }
		}

		void Awake() {
			hideFlags = HideFlags.HideInInspector;
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object ) {
				_isoFakeObject = new GameObject("_Fake" + gameObject.name);
				_isoFakeObject.AddComponent<IsoFakeObject>().Init(iso_object);
				_isoFakeObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
			}
		}

		void OnDestroy() {
			if ( _isoFakeObject ) {
				DestroyImmediate(_isoFakeObject);
				_isoFakeObject = null;
			}
		}
	}
} // namespace IsoTools