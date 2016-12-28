using UnityEngine;
using IsoTools.Internal;

namespace IsoTools {
	[SelectionBase]
	[ExecuteInEditMode, DisallowMultipleComponent]
	public sealed class IsoParent : IsoBehaviour<IsoParent> {
		public IsoWorld isoWorld {
			get {
				return FindFirstActiveWorld();
			}
		}
	}
}