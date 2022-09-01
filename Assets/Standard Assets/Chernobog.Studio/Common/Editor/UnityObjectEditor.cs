/*
/ Created   : 4/15/2020 5:18:18 PM
/ Script    : UnityObjectEditor.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using UnityEngine;
    using UnityEditor;
 
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class UnityObjectEditor : Editor
    {
    }
}