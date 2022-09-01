/*
/ Created   : 4/15/2020 4:58:58 PM
/ Script    : MonoBehaviourEditor.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : SpaceRpg
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.SpaceRpg
{
    using UnityEngine;
    using UnityEditor;
 
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor
    {
    }
}