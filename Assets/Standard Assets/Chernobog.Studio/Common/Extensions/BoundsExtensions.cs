namespace Chernobog.Studio.Common
{	
	using UnityEngine;
	using System.Collections;


	/// <summary>
	/// Bounds helpers
	/// </summary>
	public static class BoundsExtensions
	{
		/// <summary>
		/// Gets collider bounds for an object (from Collider2D)
		/// </summary>
		/// <param name="theObject"></param>
		/// <returns></returns>
		public static Bounds GetColliderBounds(GameObject theObject)
		{
			Bounds returnBounds;

			// if the object has a collider at root level, we base our calculations on that
			if (theObject.GetComponent<Collider>()!=null)
			{
				returnBounds = theObject.GetComponent<Collider>().bounds;
				return returnBounds;
			}

			// if the object has a collider2D at root level, we base our calculations on that
			if (theObject.GetComponent<Collider2D>()!=null) 
			{
				returnBounds = theObject.GetComponent<Collider2D>().bounds;
				return returnBounds;
			}

			// if the object contains at least one Collider we'll add all its children's Colliders bounds
			if (theObject.GetComponentInChildren<Collider>()!=null)
			{
				Bounds totalBounds = theObject.GetComponentInChildren<Collider>().bounds;
				Collider[] colliders = theObject.GetComponentsInChildren<Collider>();
				foreach (Collider col in colliders) 
				{
					totalBounds.Encapsulate(col.bounds);
				}
				returnBounds = totalBounds;
				return returnBounds;
			}

			// if the object contains at least one Collider2D we'll add all its children's Collider2Ds bounds
			if (theObject.GetComponentInChildren<Collider2D>()!=null)
			{
				Bounds totalBounds = theObject.GetComponentInChildren<Collider2D>().bounds;
				Collider2D[] colliders = theObject.GetComponentsInChildren<Collider2D>();
				foreach (Collider2D col in colliders) 
				{
					totalBounds.Encapsulate(col.bounds);
				}
				returnBounds = totalBounds;
				return returnBounds;
			}

			returnBounds = new Bounds(Vector3.zero, Vector3.zero);
			return returnBounds;
		}

		/// <summary>
		/// Gets bounds of a renderer
		/// </summary>
		/// <param name="theObject"></param>
		/// <returns></returns>
		public static Bounds GetRendererBounds(GameObject theObject)
		{
			Bounds returnBounds;

			// if the object has a renderer at root level, we base our calculations on that
			if (theObject.GetComponent<Renderer>()!=null)
			{
				returnBounds = theObject.GetComponent<Renderer>().bounds;
				return returnBounds;
			}

			// if the object contains at least one renderer we'll add all its children's renderer bounds
			if (theObject.GetComponentInChildren<Renderer>()!=null)
			{
				Bounds totalBounds = theObject.GetComponentInChildren<Renderer>().bounds;
				Renderer[] renderers = theObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in renderers) 
				{
					totalBounds.Encapsulate(renderer.bounds);
				}
				returnBounds = totalBounds;
				return returnBounds;
			}

			returnBounds = new Bounds(Vector3.zero, Vector3.zero);
			return returnBounds;
		}
		
		/// <summary>
		/// Calculates a screen space rectangle from world space bounds.
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="camera"></param>
		/// <returns></returns>
		public static Rect ToScreenSpace(this Bounds bounds, Camera camera)
		{
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			Vector2[] cornerPoints = new Vector2[8]
			{
				camera.WorldToScreenPoint(bounds.min),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z)),
				camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z)),
				camera.WorldToScreenPoint(bounds.max)
			};
			Vector2 min = cornerPoints[0];
			Vector2 max = cornerPoints[7];
			foreach (var corner in cornerPoints)
			{
				min = Vector2.Min(min, corner);
				max = Vector2.Max(max, corner);
			}
			return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
		}
	}
}
