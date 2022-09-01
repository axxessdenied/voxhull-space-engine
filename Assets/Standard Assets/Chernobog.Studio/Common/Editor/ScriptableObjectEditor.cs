/*
/ Created   : 4/15/2020 5:03:26 PM
/ Script    : ScriptableObjectEditor.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using UnityEngine;
    using UnityEditor;
 
    /// https://forum.unity.com/threads/better-scriptableobjects-inspector-editing-editor-tool.484392/
    ///see ScriptableObjectDrawer
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEditor : Editor
    {
    }
}