using UnityEngine;

namespace IsoTools {
	public class IsoFakeRigidbody : MonoBehaviour {
		
		IsoRigidbody _isoRigidbody = null;
		
		public void Init(IsoRigidbody iso_rigidbody) {
			_isoRigidbody = iso_rigidbody;
		}
		
		public IsoRigidbody isoRigidbody {
			get { return _isoRigidbody; }
		}
	}
} // namespace IsoTools