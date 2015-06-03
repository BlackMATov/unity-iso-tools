using UnityEngine;
using UnityEditor;
using System.Linq;

namespace IsoTools {
	public class IsoEditorWindow : EditorWindow {

		public static bool Alignment  { get; private set; }
		public static bool ShowBounds { get; private set; }

		static string _alignmentPropName  = "IsoTools.IsoEditorWindow.Alignment";
		static string _showBoundsPropName = "IsoTools.IsoEditorWindow.ShowBounds";

		void AlignmentSelection() {
			var iso_objects = Selection.gameObjects
				.Where(p => p.GetComponent<IsoObject>())
				.Select(p => p.GetComponent<IsoObject>());
			foreach ( var iso_object in iso_objects ) {
				iso_object.Position = iso_object.TilePosition;
				iso_object.FixTransform();
			}
		}

		[MenuItem("IsoTools/IsoEditor")]
		static void Init() {
			var window = EditorWindow.GetWindow<IsoEditorWindow>();
			window.title = "IsoEditor";
			window.Show();
		}

		void OnGUI() {
			GUILayout.Space(5);
			ShowBounds = EditorGUILayout.Toggle("Show bounds", ShowBounds);
			Alignment = EditorGUILayout.Toggle("Auto alignment", Alignment);
			if ( GUILayout.Button("Alignment selection objects") || Alignment ) {
				AlignmentSelection();
			}
		}

		void OnFocus() {
			if ( EditorPrefs.HasKey(_alignmentPropName) ) {
				Alignment = EditorPrefs.GetBool(_alignmentPropName);
			}
			if ( EditorPrefs.HasKey(_showBoundsPropName) ) {
				ShowBounds = EditorPrefs.GetBool(_showBoundsPropName);
			}
		}

		void OnLostFocus() {
			EditorPrefs.SetBool(_alignmentPropName, Alignment);
			EditorPrefs.SetBool(_showBoundsPropName, ShowBounds);
		}

		void OnDestroy() {
			EditorPrefs.SetBool(_alignmentPropName, Alignment);
			EditorPrefs.SetBool(_showBoundsPropName, ShowBounds);
		}
	}
}