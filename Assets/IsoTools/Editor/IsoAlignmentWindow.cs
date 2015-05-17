using UnityEngine;
using UnityEditor;
using System.Linq;

namespace IsoTools {
	public class IsoAlignmentWindow : EditorWindow {

		public static bool Alignment { get; private set; }

		void AlignmentSelection() {
			var iso_objects = Selection.gameObjects
				.Where(p => p.GetComponent<IsoObject>())
				.Select(p => p.GetComponent<IsoObject>());
			foreach ( var iso_object in iso_objects ) {
				iso_object.Position = iso_object.TilePosition;
				iso_object.FixTransform();
			}
		}

		[MenuItem("IsoTools/Alignment")]
		static void Init() {
			var window = EditorWindow.GetWindow<IsoAlignmentWindow>();
			window.title = "IsoAlignment";
			window.Show();
		}

		static IsoAlignmentWindow() {
			Alignment = true;
		}

		void OnGUI() {
			GUILayout.Space(5);
			Alignment = EditorGUILayout.Toggle("Auto alignment", Alignment);
			if ( GUILayout.Button("Alignment selection objects") || Alignment ) {
				AlignmentSelection();
			}
		}
	}
}