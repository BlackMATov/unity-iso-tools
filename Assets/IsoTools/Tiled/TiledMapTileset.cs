using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Tiled {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class TiledMapTileset : MonoBehaviour {

		public TiledMapAsset      Asset      = null;
		public TiledMapProperties Properties = null;
	}
}
