// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Rect)]
	[Tooltip("Tests if 2 Rects overlap.")]
	public class RectOverlaps : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Rectangle.")]
		public FsmRect rect1;

        [RequiredField]
		[Tooltip("Second Rectangle.")]
        public FsmRect rect2;

		[Tooltip("Event to send if the Rects overlap.")]
		public FsmEvent trueEvent;

        [Tooltip("Event to send if the Rects do not overlap.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		//[ActionSection("")]

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			rect1 = new FsmRect { UseVariable = true };
            rect2 = new FsmRect { UseVariable = true };
			storeResult = null;
			trueEvent = null;
			falseEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoRectOverlap();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoRectOverlap();
		}

		void DoRectOverlap()
		{
			if (rect1.IsNone || rect2.IsNone)
			{
				return;
			}

		    var overlapping = Intersect(rect1.Value, rect2.Value);
            storeResult.Value = overlapping;
            Fsm.Event(overlapping ? trueEvent : falseEvent);
		}

        public static bool Intersect(Rect a, Rect b)
        {
            FlipNegative(ref a);
            FlipNegative(ref b);
            bool c1 = a.xMin < b.xMax;
            bool c2 = a.xMax > b.xMin;
            bool c3 = a.yMin < b.yMax;
            bool c4 = a.yMax > b.yMin;
            return c1 && c2 && c3 && c4;
        }

        public static void FlipNegative(ref Rect r)
        {
            if (r.width < 0)
                r.x -= (r.width *= -1);
            if (r.height < 0)
                r.y -= (r.height *= -1);
        }
	}
}