using UnityEngine;
using UnityEditor;

namespace IsoTools.Internal {
	static class IsoEditorUtils {
		public static void DoWithMixedValue(bool mixed, System.Action act) {
			var last_show_mixed_value = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = mixed;
			try {
				act();
			} finally {
				EditorGUI.showMixedValue = last_show_mixed_value;
			}
		}

		public static void DoWithEnabledGUI(bool enabled, System.Action act) {
			EditorGUI.BeginDisabledGroup(!enabled);
			try {
				act();
			} finally {
				EditorGUI.EndDisabledGroup();
			}
		}
	}
}