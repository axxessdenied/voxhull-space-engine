/*
/ Created   : 3/14/2020 4:39:27 AM
/ Script    : EndNameEdit.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using UnityEditor;
	using UnityEditor.ProjectWindowCallback;


	internal class EndNameEdit : EndNameEditAction
	{
		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
		}
	}
}