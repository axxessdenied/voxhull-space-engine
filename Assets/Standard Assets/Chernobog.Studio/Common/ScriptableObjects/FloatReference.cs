/*
/ Created   : 3/13/2020 2:50:08 PM
/ Script    : FloatReference.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using System;
	
	[Serializable]
    public class FloatReference
	{
		#region Fields & Properties
        
		public bool UseConstant = true;
		public float ConstantValue;
		public FloatVariable Variable;
        
        #endregion

        #region Methods
        
		public FloatReference()
		{ }

		public FloatReference(float value)
		{
			UseConstant = true;
			ConstantValue = value;
		}

		public float Value
		{
			get { return UseConstant ? ConstantValue : Variable.Value; }
		}

		public static implicit operator float(FloatReference reference)
		{
			return reference.Value;
		}
        
        #endregion
    }
}