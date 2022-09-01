/*
/ Created   : 3/13/2020 5:58:08 PM
/ Script    : ColorExtensions.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using UnityEngine;

	public static class ColorExtensions
	{
		public static float[] ToArray(this Color color)
		{
			return new float[4] { color.r, color.g, color.b, color.a };
		}

		public static bool EqualsColor(this Color color, Color otherColor, bool ignoreAlpha = true)
		{
			if (ignoreAlpha)
			{
				return Mathf.Approximately(color.r, otherColor.r) && Mathf.Approximately(color.g, otherColor.g) && Mathf.Approximately(color.b, otherColor.b);
			}
			else
			{
				return Mathf.Approximately(color.r, otherColor.r) && Mathf.Approximately(color.g, otherColor.g) && Mathf.Approximately(color.b, otherColor.b) && Mathf.Approximately(color.a, otherColor.a);
			}
		}
	}
}