using UnityEngine;
using IsoTools.Internal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Tiled {
	[ExecuteInEditMode, DisallowMultipleComponent]
	[RequireComponent(typeof(IsoObject))]
	public class TiledMap : MonoBehaviour {

		public TiledMapAsset      Asset      = null;
		public TiledMapProperties Properties = null;

		// ---------------------------------------------------------------------
		//
		// For editor
		//
		// ---------------------------------------------------------------------

		#if UNITY_EDITOR
		[SerializeField] bool _isShowGrid = false;

		public bool isShowGrid {
			get { return _isShowGrid; }
			set { _isShowGrid = value; }
		}
		#endif

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------


		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Awake() {
		}

		void OnEnable() {
		}

		void OnDisable() {
		}

	#if UNITY_EDITOR
		void Reset() {
		}

		void OnValidate() {
		}

		void OnDrawGizmos() {
			var iso_object = GetComponent<IsoObject>();
			if ( isShowGrid && iso_object && iso_object.isoWorld ) {
				IsoUtils.DrawGrid(
					iso_object.isoWorld,
					iso_object.position, iso_object.size,
					IsoUtils.ColorChangeA(Color.green, 0.5f));
			}
		}

		void Update() {
		}
	#endif
	}
}
