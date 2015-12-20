using UnityEngine;

namespace IsoTools.Internal {
	public class IsoFakeCollider : MonoBehaviour {
		
		IsoCollider _isoCollider = null;

		public void Init(IsoCollider iso_collider) {
			_isoCollider = iso_collider;
		}
		
		public IsoCollider isoCollider {
			get { return _isoCollider; }
		}
	}
} // namespace IsoTools.Internal