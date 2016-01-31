using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Tiled {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class TiledMapLayer : MonoBehaviour {

		public TiledMapAsset Asset = null;
	}
} // namespace IsoTools.Tiled