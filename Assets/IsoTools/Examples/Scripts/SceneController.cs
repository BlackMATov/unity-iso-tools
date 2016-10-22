using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
			if ( !string.IsNullOrEmpty(NextSceneName) ) {
				SceneManager.LoadScene(NextSceneName);
			}
		}

		public void PrevScene() {
			if ( !string.IsNullOrEmpty(PrevSceneName) ) {
				SceneManager.LoadScene(PrevSceneName);
			}
		}
	}
}