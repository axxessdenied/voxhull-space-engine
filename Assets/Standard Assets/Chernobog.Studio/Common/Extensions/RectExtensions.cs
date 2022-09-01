﻿/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	public static class RectExtensions 
    {
	    /// <summary>
	    /// Returns true if this rectangle intersects the other specified rectangle
	    /// </summary>
	    /// <param name="thisRectangle"></param>
	    /// <param name="otherRectangle"></param>
	    /// <returns></returns>
	    public static bool Intersects(this Rect thisRectangle, Rect otherRectangle)
	    {
		    return !((thisRectangle.x > otherRectangle.xMax) || (thisRectangle.xMax < otherRectangle.x) || (thisRectangle.y > otherRectangle.yMax) || (thisRectangle.yMax < otherRectangle.y));
	    }
    }
}