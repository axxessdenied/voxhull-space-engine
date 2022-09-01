﻿/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	public static class RendererExtensions
    {
	    /// <summary>
	    /// Returns true if a renderer is visible from a camera
	    /// </summary>
	    /// <param name="renderer"></param>
	    /// <param name="camera"></param>
	    /// <returns></returns>
	    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	    {
		    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
		    return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
	    }
    }
}