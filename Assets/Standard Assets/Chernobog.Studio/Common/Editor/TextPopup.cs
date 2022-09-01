/*
/ Created   : 3/14/2020 4:40:17 AM
/ Script    : TextPopup.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using UnityEngine;
	using UnityEditor;
	
	/// <summary>
	/// Display a popup window with a text and a "OK" button to close the window.
	/// </summary>
	public class TextPopup : EditorWindow
	{
		private static string text;

		public static void Display(string msg)
		{
			text = msg;
			TextPopup window = GetWindow<TextPopup>(true, "Notification");
			window.ShowAuxWindow();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField(text, EditorStyles.wordWrappedLabel);
			GUILayout.Space(70);
			if (GUILayout.Button("OK"))
			{
				Close();
			}
		}
	}
}