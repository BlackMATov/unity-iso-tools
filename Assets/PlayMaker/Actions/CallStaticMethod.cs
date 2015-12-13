// (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.

using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Call a static method in a class.")]
    public class CallStaticMethod : FsmStateAction
    {
        [Tooltip("Full path to the class that contains the static method.")]
        public FsmString className;

        [Tooltip("The static method to call.")]
        public FsmString methodName;

        [Tooltip("Method paramters. NOTE: these must match the method's signature!")]
        public FsmVar[] parameters;

        [ActionSection("Store Result")]

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result of the method call.")]
        public FsmVar storeResult;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private Type cachedType;
        private string cachedClassName;
        private string cachedMethodName;
        private MethodInfo cachedMethodInfo;
        private ParameterInfo[] cachedParameterInfo;
        private object[] parametersArray;
        private string errorString;
        
        public override void OnEnter()
        {
            parametersArray = new object[parameters.Length];

            DoMethodCall();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoMethodCall();
        }

        private void DoMethodCall()
        {
            if (className == null || string.IsNullOrEmpty(className.Value))
            {
                Finish();
                return;
            }

            if (cachedClassName != className.Value || cachedMethodName != methodName.Value)
            {
                errorString = string.Empty;
                if(!DoCache())
                {
                    Debug.LogError(errorString);
                    Finish();
                    return;
                }
            }

            object result = null;
            if (cachedParameterInfo.Length == 0)
            {
                result = cachedMethodInfo.Invoke(null, null);
            }
            else
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    parameter.UpdateValue();
                    parametersArray[i] = parameter.GetValue();
                }

                result = cachedMethodInfo.Invoke(null, parametersArray);
            }
            storeResult.SetValue(result);
        }

        private bool DoCache()
        {
            cachedType = ReflectionUtils.GetGlobalType(className.Value);
            if (cachedType == null)
            {
                errorString += "Class is invalid: " + className.Value + "\n";
                Finish();
                return false;
            }
            cachedClassName = className.Value;

#if NETFX_CORE
            cachedMethodInfo = cachedType.GetTypeInfo().GetDeclaredMethod(methodName.Value);
#else
			var types = new List<Type>( capacity: parameters.Length );
			foreach ( var each in parameters ) {
				types.Add( each.RealType );
			}

            cachedMethodInfo = cachedType.GetMethod(methodName.Value, types.ToArray());
#endif            
            if (cachedMethodInfo == null)
            {
                errorString += "Invalid Method Name or Parameters: " + methodName.Value +"\n";
                Finish();
                return false;
            }

            cachedMethodName = methodName.Value;
            cachedParameterInfo = cachedMethodInfo.GetParameters();
            return true;
        }

        public override string ErrorCheck()
        {
            errorString = string.Empty;
            DoCache();

            if (!string.IsNullOrEmpty(errorString))
            {
                return errorString;
            }

            if (parameters.Length != cachedParameterInfo.Length)
            {
                return "Parameter count does not match method.\nMethod has " + cachedParameterInfo.Length + " parameters.\nYou specified " +parameters.Length + " paramaters.";
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var paramType = p.RealType;
                var paramInfoType = cachedParameterInfo[i].ParameterType;
                if (!ReferenceEquals(paramType, paramInfoType ))
                {
                    return "Parameters do not match method signature.\nParameter " + (i + 1) + " (" + paramType + ") should be of type: " + paramInfoType;
                }
            }

            if (ReferenceEquals(cachedMethodInfo.ReturnType, typeof(void)))
            {
                if (!string.IsNullOrEmpty(storeResult.variableName))
                {
                    return "Method does not have return.\nSpecify 'none' in Store Result.";
                }
            }
            else if (!ReferenceEquals(cachedMethodInfo.ReturnType,storeResult.RealType))
            {
                return "Store Result is of the wrong type.\nIt should be of type: " + cachedMethodInfo.ReturnType;
            }

            return string.Empty;
        }
    }
}
