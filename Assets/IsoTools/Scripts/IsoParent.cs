using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[SelectionBase]
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoParent : IsoBehaviour {
		public IsoWorld isoWorld {
			get {
				return FindFirstActiveWorld();
			}
		}
	}
}