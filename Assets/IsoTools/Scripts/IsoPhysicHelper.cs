using UnityEngine;

namespace IsoTools {
	[RequireComponent(typeof(IsoObject))]
	public class IsoPhysicHelper : MonoBehaviour {

		IsoObject _isoObject = null;
		public IsoObject IsoObject {
			get {
				if ( !_isoObject ) {
					_isoObject = GetComponent<IsoObject>();
				}
				if ( !_isoObject ) {
					throw new UnityException("IsoPhysicHelper. IsoObject not found!");
				}
				return _isoObject;
			}
		}

		IsoFakeObject _isoFakeObject = null;
		public IsoFakeObject IsoFakeObject {
			get {
				if ( !_isoFakeObject ) {
					var go = new GameObject("_Fake" + gameObject.name);
					//go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
					_isoFakeObject = go.AddComponent<IsoFakeObject>();
					_isoFakeObject.Init(IsoObject);
				}
				return _isoFakeObject;
			}
		}
	}
} // namespace IsoTools