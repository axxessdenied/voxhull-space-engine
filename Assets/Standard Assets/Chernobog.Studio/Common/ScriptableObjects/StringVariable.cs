/*
/ Created   : 3/13/2020 3:12:56 PM
/ Script    : StringVariable.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu
		(
			fileName = "StringVariable",
	menuName = "Chernobog Studio/Create StringVariable"
		)
	]
    public class StringVariable : ScriptableObject
	{
		[SerializeField]
		private string value = "";

		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
    }
}