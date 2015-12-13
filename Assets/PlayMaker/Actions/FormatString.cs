// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.String)]
    [Tooltip("Replaces each format item in a specified string with the text equivalent of variable's value. Stores the result in a string variable.")]
    public class FormatString : FsmStateAction
    {
        [RequiredField]
        [Tooltip("E.g. Hello {0} and {1}\nWith 2 variables that replace {0} and {1}\nSee C# string.Format docs.")]
        public FsmString format;

        [Tooltip("Variables to use for each formatting item.")]
        public FsmVar[] variables;

        [RequiredField]
        [UIHint(UIHint.Variable)] 
        [Tooltip("Store the formatted result in a string variable.")]
        public FsmString storeResult;

        [Tooltip("Repeat every frame. This is useful if the variables are changing.")]
        public bool everyFrame;

        private object[] objectArray;

        public override void Reset()
        {
            format = null;
            variables = null;
            storeResult = null;
            everyFrame = false;
        }

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            objectArray = new object[variables.Length];

            DoFormatString();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoFormatString();
        }

        void DoFormatString()
        {
            for (var i = 0; i < variables.Length; i++)
            {
				variables[i].UpdateValue();
                objectArray[i] = variables[i].GetValue();
            } 
            
            try
            {
                storeResult.Value = string.Format(format.Value, objectArray);
            }
            catch (System.FormatException e)
            {
                LogError(e.Message);
                Finish();
            }   
        }
    }
}
