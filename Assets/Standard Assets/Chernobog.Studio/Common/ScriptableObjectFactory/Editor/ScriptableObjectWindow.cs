/*
/ Created   : 3/14/2020 4:35:33 AM
/ Script    : ScriptableObjectWindow.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	public class ScriptableObjectWindow : EditorWindow
	{
		private int selectedIndex;
		private string[] names;
		private Type[] types;

		public void SetTypes(Type[] types)
		{
			this.types = types;
			names = types.Select(t => t.FullName).ToArray();
		}

		public IEnumerable<Type> GetTypes() { return types; }

		private void OnGUI()
		{
			GUILayout.Label("ScriptableObject Class");
			selectedIndex = EditorGUILayout.Popup(selectedIndex, names);
			if (GUILayout.Button("Create"))
			{
				if (types.Length == 0) { return; }
				ScriptableObjectFactory.Create(types[selectedIndex], names[selectedIndex]);
				Close();
			}
		}
	}
}