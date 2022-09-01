/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
	using UnityEngine;
	using System;

    public static class ArrayExtensions
    {
	    /// <summary>
	    /// Returns a random value inside the array
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="array"></param>
	    /// <returns></returns>
	    public static T RandomValue<T>(this T[] array)
	    {
		    int newIndex = UnityEngine.Random.Range(0, array.Length);
		    return array[newIndex];
	    }        
	    
	    public static Color ToRGBA(this float[] floatArray)
	    {
		    if (floatArray == null) { throw new Exception("[ArrayExtensions] The array is null."); }
		    if (floatArray.Length < 4) { throw new Exception("[ArrayExtensions] The array is too short to convert into a color."); }
		    return new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	    }
    }
}