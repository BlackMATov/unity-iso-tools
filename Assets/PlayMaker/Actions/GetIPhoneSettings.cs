// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Device)]
    [Tooltip("Get various iPhone settings.")]
    public class GetIPhoneSettings : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [Tooltip("Allows device to fall into 'sleep' state with screen being dim if no touches occurred. Default value is true.")]
        public FsmBool getScreenCanDarken;

        [UIHint(UIHint.Variable)]
        [Tooltip("A unique device identifier string. It is guaranteed to be unique for every device (Read Only).")]
        public FsmString getUniqueIdentifier;

        [UIHint(UIHint.Variable)]
        [Tooltip("The user defined name of the device (Read Only).")]
        public FsmString getName;

        [UIHint(UIHint.Variable)]
        [Tooltip("The model of the device (Read Only).")]
        public FsmString getModel;

        [UIHint(UIHint.Variable)]
        [Tooltip("The name of the operating system running on the device (Read Only).")]
        public FsmString getSystemName;

        [UIHint(UIHint.Variable)]
        [Tooltip("The generation of the device (Read Only).")]
        public FsmString getGeneration;

        public override void Reset()
        {
            getScreenCanDarken = null;
            getUniqueIdentifier = null;
            getName = null;
            getModel = null;
            getSystemName = null;
            getGeneration = null;
        }

        public override void OnEnter()
        {
#if UNITY_IPHONE
			
			getScreenCanDarken.Value = Screen.sleepTimeout > 0f; //iPhoneSettings.screenCanDarken;
			getUniqueIdentifier.Value = SystemInfo.deviceUniqueIdentifier; //iPhoneSettings.uniqueIdentifier;
			getName.Value = SystemInfo.deviceName; //iPhoneSettings.name;
			getModel.Value = SystemInfo.deviceModel; //iPhoneSettings.model;
			getSystemName.Value = SystemInfo.operatingSystem; //iPhoneSettings.systemName;
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
            getGeneration.Value = iPhone.generation.ToString();
#else
            getGeneration.Value = UnityEngine.iOS.Device.generation.ToString();
#endif
#endif
            Finish();
        }
    }
}