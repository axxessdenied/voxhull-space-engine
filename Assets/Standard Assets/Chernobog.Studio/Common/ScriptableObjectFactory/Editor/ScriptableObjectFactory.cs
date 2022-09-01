/*
/ Created   : 3/14/2020 4:34:10 AM
/ Script    : ScriptableObjectFactory.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using System;

	// Based on the implementation described here: http://www.tallior.com/unity-scriptableobject-factory/
	public static class ScriptableObjectFactory
	{
		[MenuItem("Chernobog Studio/Create/ScriptableObject")]
		public static void Create()
		{
			var window = EditorWindow.GetWindow<ScriptableObjectWindow>(true, "Create a new ScriptableObject", true);
			var assembly = Assembly.Load(new AssemblyName("Assembly-CSharp"));
			window.SetTypes(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ScriptableObject))).ToArray());
			window.ShowPopup();
		}

		public static ScriptableObject Create(Type t, string name)
		{
			var asset = ScriptableObject.CreateInstance(t);
			EditName(asset, name);
			return asset;
		}

		public static T Create<T>(string name) where T : ScriptableObject
		{
			var asset = ScriptableObject.CreateInstance<T>();
			EditName(asset, name);
			return asset;
		}

		private static void EditName(ScriptableObject asset, string name)
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(asset.GetInstanceID(),
				ScriptableObject.CreateInstance<EndNameEdit>(),
				string.Format("{0}.asset", name),
				AssetPreview.GetMiniThumbnail(asset),
				null);
		}
	}
}