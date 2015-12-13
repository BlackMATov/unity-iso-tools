// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Transfer a value from one array to another, basically a copy/cut paste action on steroids.")]
	public class ArrayTransferValue : FsmStateAction
	{
		public enum ArrayTransferType {Copy,Cut,nullify};
		public enum ArrayPasteType {AsFirstItem,AsLastItem,InsertAtSameIndex,ReplaceAtSameIndex};

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Array Variable source.")]
		public FsmArray arraySource;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Array Variable target.")]
		public FsmArray arrayTarget;

		[MatchFieldType("array")]
		[Tooltip("The index to transfer.")]
		public FsmInt indexToTransfer;

		[ActionSection("Transfer Options")]

		[ObjectType(typeof(ArrayTransferType))]
		public FsmEnum copyType;

		[ObjectType(typeof(ArrayPasteType))]
		public FsmEnum pasteType;

		[ActionSection("Result")]

		[Tooltip("Event sent if this array source does not contains that element (described below)")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent indexOutOfRange;

		public override void Reset ()
		{
			arraySource = null;
			arrayTarget = null;
			
			indexToTransfer = null;

			copyType = ArrayTransferType.Copy;
			pasteType = ArrayPasteType.AsLastItem;
		}
		
		// Code that runs on entering the state.
		public override void OnEnter ()
		{
			DoTransferValue ();
			Finish ();
		}
		
		private void DoTransferValue ()
		{
			if (arraySource.IsNone || arrayTarget.IsNone)
			{
				return;
			}
			int _index = indexToTransfer.Value;

			if (_index< 0 || _index>=arraySource.Length)
			{
				Fsm.Event(indexOutOfRange);
				return;
			}
			var _value = arraySource.Values[_index];



			if ((ArrayTransferType)copyType.Value ==  ArrayTransferType.Cut)
			{
				List<object> _list = new List<object>(arraySource.Values);
				_list.RemoveAt(_index);
				arraySource.Values = _list.ToArray();
			}else if ((ArrayTransferType)copyType.Value == ArrayTransferType.nullify)
			{
				arraySource.Values.SetValue(null,_index);
			}


			if ( (ArrayPasteType)pasteType.Value == ArrayPasteType.AsFirstItem)
			{
				List<object> _listTarget = new List<object>(arrayTarget.Values);
				_listTarget.Insert(0,_value);
				arrayTarget.Values = _listTarget.ToArray();

			}else if( (ArrayPasteType)pasteType.Value == ArrayPasteType.AsLastItem)
			{
				arrayTarget.Resize(arrayTarget.Length + 1);
				arrayTarget.Set(arrayTarget.Length - 1, _value);

			}else if( (ArrayPasteType)pasteType.Value == ArrayPasteType.InsertAtSameIndex)
			{
				if (_index>=arrayTarget.Length)
				{
					Fsm.Event(indexOutOfRange);
				}
				List<object> _listTarget = new List<object>(arrayTarget.Values);
				_listTarget.Insert(_index,_value);
				arrayTarget.Values = _listTarget.ToArray();

			}else if( (ArrayPasteType)pasteType.Value == ArrayPasteType.ReplaceAtSameIndex)
			{
				if (_index>=arrayTarget.Length)
				{
					Fsm.Event(indexOutOfRange);
				}else{
					arrayTarget.Set(_index, _value);
				}
			}
		}
		
	}
}
