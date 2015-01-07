using UnityEngine;
using System.Collections;

namespace IsoTools { namespace Examples {
public class IsoAutoController : MonoBehaviour {
	public float StepTicks    = 0.5f;
	public float StepRndTicks = 0.5f;

	void Start() {
		StartCoroutine("Move");
	}

	WaitForSeconds RndWait() {
		return new WaitForSeconds(StepTicks + Random.Range(0.0f, StepRndTicks));
	}

	IEnumerator Move() {
		var iso_object = GetComponent<IsoObject>();
		if ( iso_object ) {
			for (;;) {
				yield return RndWait();
				iso_object.Position += new Vector3(1, 0, 0);
				yield return RndWait();
				iso_object.Position += new Vector3(0, 1, 0);
				yield return RndWait();
				iso_object.Position += new Vector3(-1, 0, 0);
				yield return RndWait();
				iso_object.Position += new Vector3(0, -1, 0);
			}
		}
	}
}
}} // namespace IsoTools::Examples