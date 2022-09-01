/*
/ Created   : 4/15/2020 5:21:20 PM
/ Script    : ExpandableAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using UnityEngine;

    /// <summary>
    /// Use this property on a ScriptableObject type to allow the editors drawing the field to draw an expandable
    /// area that allows for changing the values on the object without having to change editor.
    /// </summary>
    public class ExpandableAttribute : PropertyAttribute
    {
        public ExpandableAttribute ()
        {
 
        }
    }
 

}