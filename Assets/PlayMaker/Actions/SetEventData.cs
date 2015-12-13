// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sets Event Data before sending an event. Get the Event Data, along with sender information, using Get Event Info action.")]
	public class SetEventData : FsmStateAction
	{
		public FsmGameObject setGameObjectData;
		public FsmInt setIntData;
		public FsmFloat setFloatData;
		public FsmString setStringData;
		public FsmBool setBoolData;
		public FsmVector2 setVector2Data;
		public FsmVector3 setVector3Data;
		public FsmRect setRectData;
		public FsmQuaternion setQuaternionData;
		public FsmColor setColorData;
		public FsmMaterial setMaterialData;
		public FsmTexture setTextureData;
		public FsmObject setObjectData;

		public override void Reset()
		{
			setGameObjectData = new FsmGameObject{UseVariable = true};
			setIntData = new FsmInt { UseVariable = true };
			setFloatData = new FsmFloat { UseVariable = true };
			setStringData = new FsmString { UseVariable = true };
			setBoolData = new FsmBool { UseVariable = true };
			setVector2Data = new FsmVector2 { UseVariable = true };
			setVector3Data = new FsmVector3 { UseVariable = true };
			setRectData = new FsmRect { UseVariable = true };
			setQuaternionData = new FsmQuaternion { UseVariable = true };
			setColorData = new FsmColor { UseVariable = true };
			setMaterialData = new FsmMaterial { UseVariable = true };
			setTextureData = new FsmTexture { UseVariable = true };
			setObjectData = new FsmObject { UseVariable = true };
		}

		public override void OnEnter()
		{
			Fsm.EventData.BoolData = setBoolData.Value;
			Fsm.EventData.IntData = setIntData.Value;
			Fsm.EventData.FloatData = setFloatData.Value;
			Fsm.EventData.Vector2Data = setVector2Data.Value;
			Fsm.EventData.Vector3Data = setVector3Data.Value;
			Fsm.EventData.StringData = setStringData.Value;
			Fsm.EventData.GameObjectData = setGameObjectData.Value;
			Fsm.EventData.RectData = setRectData.Value;
			Fsm.EventData.QuaternionData = setQuaternionData.Value;
			Fsm.EventData.ColorData = setColorData.Value;
			Fsm.EventData.MaterialData = setMaterialData.Value;
			Fsm.EventData.TextureData = setTextureData.Value;
			Fsm.EventData.ObjectData = setObjectData.Value;
		
			Finish();
		}
	}
}