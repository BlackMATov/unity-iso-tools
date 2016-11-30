using UnityEngine;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoFakeRigidbody : MonoBehaviour {

		IsoRigidbody _isoRigidbody = null;
		
		public IsoFakeRigidbody Init(IsoRigidbody iso_rigidbody) {
			_isoRigidbody = iso_rigidbody;
			return this;
		}
		
		public IsoRigidbody isoRigidbody {
			get { return _isoRigidbody; }
		}
	}
}