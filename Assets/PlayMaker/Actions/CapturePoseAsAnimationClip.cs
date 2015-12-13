// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Captures the current pose of a hierarchy as an animation clip.\n\nUseful to blend from an arbitrary pose (e.g. a ragdoll death) back to a known animation (e.g. idle).")]
	public class CapturePoseAsAnimationClip : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("The GameObject root of the hierarchy to capture.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Capture position keys.")]
		public FsmBool position;

		[Tooltip("Capture rotation keys.")]
		public FsmBool rotation;
		
		[Tooltip("Capture scale keys.")]
		public FsmBool scale;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(AnimationClip))]
		[Tooltip("Store the result in an Object variable of type AnimationClip.")]
		public FsmObject storeAnimationClip;

		public override void Reset()
		{
			gameObject = null;
			position = false;
			rotation = true;
			scale = false;
			storeAnimationClip = null;
		}

		public override void OnEnter()
		{
			DoCaptureAnimationClip();
			
			Finish();
		}

		void DoCaptureAnimationClip()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var animClip = new AnimationClip();

			foreach (Transform child in go.transform)
			{
				CaptureTransform(child, "", animClip);
			}

			storeAnimationClip.Value = animClip;
		}

		void CaptureTransform(Transform transform, string path, AnimationClip clip)
		{
			path += transform.name;

			//Debug.Log(path);

			if (position.Value)
			{
				CapturePosition(transform, path, clip);
			}

			if (rotation.Value)
			{
				CaptureRotation(transform, path, clip);
			}

			if (scale.Value)
			{
				CaptureScale(transform, path, clip);
			}

			foreach (Transform child in transform)
			{
				CaptureTransform(child, path + "/", clip);
			}
		}

		void CapturePosition(Transform transform, string path, AnimationClip clip)
		{
			SetConstantCurve(clip, path, "localPosition.x", transform.localPosition.x);
			SetConstantCurve(clip, path, "localPosition.y", transform.localPosition.y);
			SetConstantCurve(clip, path, "localPosition.z", transform.localPosition.z);
		}

		void CaptureRotation(Transform transform, string path, AnimationClip clip)
		{
			SetConstantCurve(clip, path, "localRotation.x", transform.localRotation.x);
			SetConstantCurve(clip, path, "localRotation.y", transform.localRotation.y);
			SetConstantCurve(clip, path, "localRotation.z", transform.localRotation.z);
			SetConstantCurve(clip, path, "localRotation.w", transform.localRotation.w);
		}

		void CaptureScale(Transform transform, string path, AnimationClip clip)
		{
			SetConstantCurve(clip, path, "localScale.x", transform.localScale.x);
			SetConstantCurve(clip, path, "localScale.y", transform.localScale.y);
			SetConstantCurve(clip, path, "localScale.z", transform.localScale.z);
		}

		void SetConstantCurve(AnimationClip clip, string childPath, string propertyPath, float value)
		{
			var curve = AnimationCurve.Linear(0, value, 100, value);
			curve.postWrapMode = WrapMode.Loop;
			
			clip.SetCurve(childPath, typeof(Transform), propertyPath, curve);
		}



	}
}