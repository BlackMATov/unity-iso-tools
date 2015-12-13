// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a GameObject Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
	public class GameObjectIsNull : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("The GameObject variable to test.")]
		public FsmGameObject gameObject;

        [Tooltip("Event to send if the GamObject is null.")]
		public FsmEvent isNull;

        [Tooltip("Event to send if the GamObject is NOT null.")]
		public FsmEvent isNotNull;

		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

        [Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			isNull = null;
			isNotNull = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsGameObjectNull();
			
			if (!everyFrame)
			{
			    Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsGameObjectNull();
		}
		
		void DoIsGameObjectNull()
		{
			var goIsNull = gameObject.Value == null;

			if (storeResult != null)
			{
			    storeResult.Value = goIsNull;
			}

			Fsm.Event(goIsNull ? isNull : isNotNull);
		}
	}
}