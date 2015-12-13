// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds a Game Object by Name and/or Tag.")]
	public class FindGameObject : FsmStateAction
	{
        [Tooltip("The name of the GameObject to find. You can leave this empty if you specify a Tag.")]
		public FsmString objectName;

		[UIHint(UIHint.Tag)]
        [Tooltip("Find a GameObject with this tag. If Object Name is specified then both name and Tag must match.")]
		public FsmString withTag;

		[RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a GameObject variable.")]
		public FsmGameObject store;

		public override void Reset()
		{
			objectName = "";
			withTag = "Untagged";
			store = null;
		}

		public override void OnEnter()
		{
			Find();
			Finish();
		}

		void Find()
		{


			if (withTag.Value != "Untagged")
			{
				if (!string.IsNullOrEmpty(objectName.Value))
				{
					var possibleGameObjects = GameObject.FindGameObjectsWithTag(withTag.Value);

					foreach (var go in possibleGameObjects)
					{
						if (go.name == objectName.Value)
						{
							store.Value = go;
							return;
						}
					}

					store.Value = null;
					return;
				}

				store.Value = GameObject.FindGameObjectWithTag(withTag.Value);
				return;
			}

			store.Value = GameObject.Find(objectName.Value);



		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(objectName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
			    return "Specify Name, Tag, or both.";
			}

			return null;
		}

	}
}