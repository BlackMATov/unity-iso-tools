// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Mesh")]
	[Tooltip("Gets the number of vertices in a GameObject's mesh. Useful in conjunction with GetVertexPosition.")]
	public class GetVertexCount : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(MeshFilter))]
		[Tooltip("The GameObject to check.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the vertex count in a variable.")]
		public FsmInt storeCount;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVertexCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVertexCount();
		}

		void DoGetVertexCount()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go != null)
			{
				var meshFilter = go.GetComponent<MeshFilter>();

				if (meshFilter == null)
				{
					LogError("Missing MeshFilter!");
					return;
				}

				storeCount.Value = meshFilter.mesh.vertexCount;
			}
		}
	}
}