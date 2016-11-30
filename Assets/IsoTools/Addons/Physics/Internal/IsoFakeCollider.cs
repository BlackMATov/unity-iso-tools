using UnityEngine;

namespace IsoTools.Physics.Internal {
	[AddComponentMenu("")]
	public class IsoFakeCollider : MonoBehaviour {

		IsoCollider _isoCollider = null;

		public IsoFakeCollider Init(IsoCollider iso_collider) {
			_isoCollider = iso_collider;
			return this;
		}
		
		public IsoCollider isoCollider {
			get { return _isoCollider; }
		}
	}
}