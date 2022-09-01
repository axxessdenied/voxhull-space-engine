/*
/ Created   : 3/13/2020 3:02:51 PM
/ Script    : FloatReferenceDrawer.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatReferenceDrawer : PropertyDrawer
	{
		
        #region Fields & Properties
        
		/// <summary>
		/// Options to display in the popup to select constant or variable.
		/// </summary>
		private readonly string[] popupOptions = 
		{ "Use Constant", "Use Variable" };

		/// <summary> Cached style to use to draw the popup button. </summary>
		private GUIStyle popupStyle;
        
        #endregion

        #region Update
        
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (popupStyle == null)
			{
				popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
				popupStyle.imagePosition = ImagePosition.ImageOnly;
			}

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);
            
			EditorGUI.BeginChangeCheck();

			// Get properties
			SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
			SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
			SerializedProperty variable = property.FindPropertyRelative("Variable");

			// Calculate rect for configuration button
			Rect buttonRect = new Rect(position);
			buttonRect.yMin += popupStyle.margin.top;
			buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
			position.xMin = buttonRect.xMax;

			// Store old indent level and set it to 0, the PrefixLabel takes care of it
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

			useConstant.boolValue = result == 0;

			EditorGUI.PropertyField(position, 
				useConstant.boolValue ? constantValue : variable, 
				GUIContent.none);

			if (EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
        
        #endregion
    }
}