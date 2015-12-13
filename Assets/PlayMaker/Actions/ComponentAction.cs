// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    // Base class for actions that access a Component on a GameObject.
    // Caches the component for performance
    public abstract class ComponentAction<T> : FsmStateAction where T : Component
    {
        private GameObject cachedGameObject;
        private T component;

        protected Rigidbody rigidbody
        {
            get { return component as Rigidbody; }
        }

        protected Renderer renderer
        {
            get { return component as Renderer; }
        }

        protected Animation animation
        {
            get { return component as Animation; }
        }

        protected AudioSource audio
        {
            get { return component as AudioSource; }
        }

        protected Camera camera
        {
            get { return component as Camera; }
        }

        protected GUIText guiText
        {
            get { return component as GUIText; }
        }

        protected GUITexture guiTexture
        {
            get { return component as GUITexture; }
        }

        protected Light light
        {
            get { return component as Light; }
        }

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8 || UNITY_WIIU || UNITY_PSM || UNITY_WEBGL || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
        protected NetworkView networkView
        {
            get { return component as NetworkView; }
        }
#endif
        protected bool UpdateCache(GameObject go)
        {
            if (go == null)
            {
                return false;
            }

            if (component == null || cachedGameObject != go)
            {
                component = go.GetComponent<T>();
                cachedGameObject = go;

                if (component == null)
                {
                    LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name);
                }
            }

            return component != null;
        }
    }
}