// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	/// <summary>
	/// Action version of Unity's builtin MouseLook behaviour.
	/// TODO: Expose invert Y option.
	/// </summary>
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
	public class MouseLook2 : ComponentAction<Rigidbody>
	{
		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The axes to rotate around.")]
		public RotationAxes axes = RotationAxes.MouseXAndY;

		[RequiredField]
		public FsmFloat sensitivityX;

		[RequiredField]
		public FsmFloat sensitivityY;

		[RequiredField]
		[HasFloatSlider(-360,360)]
		public FsmFloat minimumX;

		[RequiredField]
		[HasFloatSlider(-360, 360)]
		public FsmFloat maximumX;

		[RequiredField]
		[HasFloatSlider(-360, 360)]
		public FsmFloat minimumY;

		[RequiredField]
		[HasFloatSlider(-360, 360)]
		public FsmFloat maximumY;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		float rotationX;
		float rotationY;

		public override void Reset()
		{
			gameObject = null;
			axes = RotationAxes.MouseXAndY;
			sensitivityX = 15f;
			sensitivityY = 15f;
			minimumX = -360f;
			maximumX = 360f;
			minimumY = -60f;
			maximumY = 60f;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}

            // Make the rigid body not change rotation			
            // TODO: Original Unity script had this. Expose as option?
            if (!UpdateCache(go))
            {
                if (rigidbody)
                {
                    rigidbody.freezeRotation = true;
                }
            }

			DoMouseLook();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMouseLook();
		}

		void DoMouseLook()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			var transform = go.transform;

			switch (axes)
			{
				case RotationAxes.MouseXAndY:
					
					transform.localEulerAngles = new Vector3(GetYRotation(), GetXRotation(), 0);
					break;
				
				case RotationAxes.MouseX:

					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, GetXRotation(), 0);
					break;

				case RotationAxes.MouseY:

					transform.localEulerAngles = new Vector3(-GetYRotation(), transform.localEulerAngles.y, 0);
					break;
			}
		}

		float GetXRotation()
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX.Value;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			return rotationX;
		}

		float GetYRotation()
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY.Value;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			return rotationY;
		}

		// Clamp function that respects IsNone
		static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
		{
			if (!min.IsNone && angle < min.Value)
			{
				angle = min.Value;
			}

			if (!max.IsNone && angle > max.Value)
			{
				angle = max.Value;
			}
			
			return angle;
		}
	}
}