// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Reverse the order of items in an Array.")]
	public class ArrayReverse : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Array to reverse.")]
		public FsmArray array;
			
		public override void Reset()
		{
			array = null;
		}
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			var _list = new List<object>(array.Values);
			_list.Reverse();			
			array.Values = _list.ToArray();
			
			Finish();
		}
	}
}