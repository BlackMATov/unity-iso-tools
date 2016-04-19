using UnityEngine;
using System.Collections.Generic;

namespace IsoTools.Internal {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class IsoPhysicHelper : MonoBehaviour {

		static List<IsoCollider>          _tmpColliders          = new List<IsoCollider>(7);
		static List<IsoRigidbody>         _tmpRigidbodies        = new List<IsoRigidbody>(7);
		static List<IsoTriggerListener>   _tmpTriggerListeners   = new List<IsoTriggerListener>(7);
		static List<IsoCollisionListener> _tmpCollisionListeners = new List<IsoCollisionListener>(7);

		GameObject _isoFakeObject = null;
		public GameObject isoFakeObject {
			get { return _isoFakeObject; }
		}

		//TODO: fix copy-paste
		public void DestroyIfUnnecessary(Component except) {
			var unnecessary = true;
			GetComponents<IsoCollider>         (_tmpColliders);
			GetComponents<IsoRigidbody>        (_tmpRigidbodies);
			GetComponents<IsoTriggerListener>  (_tmpTriggerListeners);
			GetComponents<IsoCollisionListener>(_tmpCollisionListeners);
			if ( unnecessary ) {
				for ( int i = 0, e = _tmpColliders.Count; i < e; ++i ) {
					if ( _tmpColliders[i] != except ) {
						unnecessary = false;
						break;
					}
				}
			}
			if ( unnecessary ) {
				for ( int i = 0, e = _tmpRigidbodies.Count; i < e; ++i ) {
					if ( _tmpRigidbodies[i] != except ) {
						unnecessary = false;
						break;
					}
				}
			}
			if ( unnecessary ) {
				for ( int i = 0, e = _tmpTriggerListeners.Count; i < e; ++i ) {
					if ( _tmpTriggerListeners[i] != except ) {
						unnecessary = false;
						break;
					}
				}
			}
			if ( unnecessary ) {
				for ( int i = 0, e = _tmpCollisionListeners.Count; i < e; ++i ) {
					if ( _tmpCollisionListeners[i] != except ) {
						unnecessary = false;
						break;
					}
				}
			}
			_tmpColliders.Clear();
			_tmpRigidbodies.Clear();
			_tmpTriggerListeners.Clear();
			_tmpCollisionListeners.Clear();
			if ( unnecessary ) {
				Destroy(this);
			}
		}

		void Awake() {
			hideFlags = HideFlags.HideInInspector;
			var iso_object = GetComponent<IsoObject>();
			if ( iso_object ) {
				_isoFakeObject = new GameObject("_Fake" + gameObject.name);
				_isoFakeObject.AddComponent<IsoFakeObject>().Init(iso_object);
				_isoFakeObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
				GameObject.DontDestroyOnLoad(_isoFakeObject);
			}
		}

		void OnDestroy() {
			if ( _isoFakeObject ) {
				DestroyImmediate(_isoFakeObject);
				_isoFakeObject = null;
			}
		}
	}
}
