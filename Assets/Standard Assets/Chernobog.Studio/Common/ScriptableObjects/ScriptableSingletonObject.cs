/*
/ Created   : 3/13/2020 4:46:15 PM
/ Script    : ScriptableSingletonObject.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using UnityEngine;
    using System.Linq;

    public class ScriptableSingletonObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
	                var type = typeof(T);
	                var instances = Resources.LoadAll<T>(string.Empty);
	                _instance = instances.FirstOrDefault();
	                if (_instance == null)
	                {
		                Debug.LogErrorFormat("[ScriptableSingleton] No instance of {0} found!", type.ToString());
	                }
	                else if (instances.Multiple())
	                {
		                Debug.LogErrorFormat("[ScriptableSingleton] Multiple instances of {0} found!", type.ToString());
	                }
	                else
	                {
		                Debug.LogFormat("[ScriptableSingleton] An instance of {0} was found!", type.ToString());
	                }
                }
                return _instance;
            }
        }
    }
}