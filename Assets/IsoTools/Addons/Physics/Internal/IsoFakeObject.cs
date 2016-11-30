using UnityEngine;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoFakeObject : MonoBehaviour {

		IsoObject _isoObject    = null;
		Vector3   _lastPosition = Vector3.zero;

		public void Init(IsoObject iso_object) {
			_isoObject         = iso_object;
			_lastPosition      = iso_object.position;
			transform.position = iso_object.position;
		}

		public IsoObject isoObject {
			get { return _isoObject; }
		}

		void CheckLayers() {
			var iso_object_layer = isoObject.gameObject.layer;
			if ( gameObject.layer != iso_object_layer ) {
				gameObject.layer = iso_object_layer;
			}
		}

		void FixedUpdate() {
			CheckLayers();
			if ( _lastPosition != isoObject.position ) {
				_lastPosition = transform.position = isoObject.position;
			} else if ( _lastPosition != transform.position ) {
				_lastPosition = isoObject.position = transform.position;
			}
		}
	}
}