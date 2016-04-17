using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace IsoTools.Examples {
	public class SceneController : MonoBehaviour {
		public Button NextSceneBtn  = null;
		public Button PrevSceneBtn  = null;

		public string NextSceneName = string.Empty;
		public string PrevSceneName = string.Empty;

		public void OnValidate() {
			if ( !NextSceneBtn || !PrevSceneBtn ) {
				Debug.LogError("SceneController. Wrong description!");
			}
		}

		public void Start() {
			if ( NextSceneBtn ) {
				NextSceneBtn.interactable = !string.IsNullOrEmpty(NextSceneName);
			}
			if ( PrevSceneBtn ) {
				PrevSceneBtn.interactable = !string.IsNullOrEmpty(PrevSceneName);
			}
		}

		public void NextScene() {
		#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(NextSceneName);
		#else
			Application.LoadLevel(NextSceneName);
		#endif
		}

		public void PrevScene() {
		#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(PrevSceneName);
		#else
			Application.LoadLevel(PrevSceneName);
		#endif
		}
	}
}