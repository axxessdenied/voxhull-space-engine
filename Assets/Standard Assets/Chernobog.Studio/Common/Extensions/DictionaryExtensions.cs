﻿/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	public static class DictionaryExtensions
    {
	    /// <summary>
	    /// Finds a key (if there's one) that matches the value set in parameters
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <typeparam name="W"></typeparam>
	    /// <param name="dictionary"></param>
	    /// <param name="value"></param>
	    /// <returns></returns>
	    public static T KeyByValue<T, W>(this Dictionary<T, W> dictionary, T value)
	    {
		    T key = default;
		    foreach (KeyValuePair<T, W> pair in dictionary)
		    {
			    if (pair.Value.Equals(value))
			    {
				    key = pair.Key;
				    break;
			    }
		    }
		    return key;
	    }
    }
}