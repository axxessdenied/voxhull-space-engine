/*
/ Created   : 3/13/2020 2:49:34 PM
/ Script    : FloatVariable.cs
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
			fileName = "FloatVariable",
			menuName = "Chernobog Studio/Create FloatVariable"
		)
	]
    public class FloatVariable : ScriptableObject
	{
    	#region Fields & Properties
    	
    	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
		#endif
		
		public float Value;
    	
        #endregion


        #region Methods
        
		public void SetValue(float value)
		{
			Value = value;
		}

		public void SetValue(FloatVariable value)
		{
			Value = value.Value;
		}

		public void ApplyChange(float amount)
		{
			Value += amount;
		}

		public void ApplyChange(FloatVariable amount)
		{
			Value += amount.Value;
		}
        
        #endregion
    }
}