// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Mesh")]
	[Tooltip("Gets the position of a vertex in a GameObject's mesh. Hint: Use GetVertexCount to get the number of vertices in a mesh.")]
	public class GetVertexPosition : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(MeshFilter))]
		[Tooltip("The GameObject to check.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The index of the vertex.")]
		public FsmInt vertexIndex;

		[Tooltip("Coordinate system to use.")]
		public Space space;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the vertex position in a variable.")]
		public FsmVector3 storePosition;

		[Tooltip("Repeat every frame. Useful if the mesh is animated.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			space = Space.World;
			storePosition = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVertexPosition();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVertexPosition();
		}

		void DoGetVertexPosition()
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

				switch (space)
				{
					case Space.World:
						var position = meshFilter.mesh.vertices[vertexIndex.Value];
						storePosition.Value = go.transform.TransformPoint(position);
						break;
					
					case Space.Self:
						storePosition.Value = meshFilter.mesh.vertices[vertexIndex.Value];
						break;
				}
			}
		}
	}
}