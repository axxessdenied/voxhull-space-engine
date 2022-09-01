namespace Chernobog.Studio.Common
{
    using Unity.Mathematics;
    using UnityEngine;
    using System;

    public static class MeshUtils
    {
        /// <summary>
        /// Generates a simple quad of any size
        /// </summary>
        /// <param name="size">The size of the quad</param>
        /// <param name="pivot">Where the mesh pivots</param>
        /// <returns>The quad mesh</returns>
        public static Mesh GenerateQuad(float2 size, float2 pivot)
        {
            var mesh = new Mesh();
            var scaledPivot = size * pivot;
            var vertices = new[]
            {
                new Vector3(size.x - scaledPivot.x, size.y - scaledPivot.y, 0),
                new Vector3(size.x - scaledPivot.x, -scaledPivot.y, 0),
                new Vector3(-scaledPivot.x, -scaledPivot.y, 0),
                new Vector3(-scaledPivot.x, size.y - scaledPivot.y, 0),
            };
            mesh.vertices = vertices;

            var  uv = new[]
            {
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1)
            };
            mesh.uv = uv;
            
//            var normals = new[]
//            {
//                -Vector3.forward,
//                -Vector3.forward,
//                -Vector3.forward,
//                -Vector3.forward
//            };
//            mesh.normals = normals;

            var triangles = new[]
            {
                0, 1, 2,
                2, 3, 0
            };
            mesh.triangles = triangles;

            return mesh;
        }
        
        /// <summary>
        /// Generates a simple quad
        /// </summary>
        /// <returns>The quad mesh</returns>
        public static Mesh GenerateQuad() 
        {
            var mesh = new Mesh();
            
            var vertices = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0)
            };
            mesh.vertices = vertices;

            var tri = new[]
            {
                0, 2, 1,
                2, 3, 1
            };
            mesh.triangles = tri;

            var normals = new[]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };
            mesh.normals = normals;

            var uv = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.uv = uv;

            return mesh;
        }
        
        public static Mesh GetUnityPrimitiveMesh(PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.Sphere:
                    return GetCachedPrimitiveMesh(ref _unitySphereMesh, primitiveType);
                case PrimitiveType.Capsule:
                    return GetCachedPrimitiveMesh(ref _unityCapsuleMesh, primitiveType);
                case PrimitiveType.Cylinder:
                    return GetCachedPrimitiveMesh(ref _unityCylinderMesh, primitiveType);
                case PrimitiveType.Cube:
                    return GetCachedPrimitiveMesh(ref _unityCubeMesh, primitiveType);
                case PrimitiveType.Plane:
                    return GetCachedPrimitiveMesh(ref _unityPlaneMesh, primitiveType);
                case PrimitiveType.Quad:
                    return GetCachedPrimitiveMesh(ref _unityQuadMesh, primitiveType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
            }
        }
 
        private static Mesh GetCachedPrimitiveMesh(ref Mesh primMesh, PrimitiveType primitiveType)
        {
            if (primMesh == null)
            {
                Debug.Log("Getting Unity Primitive Mesh: " + primitiveType);
                primMesh = Resources.GetBuiltinResource<Mesh>(GetPrimitiveMeshPath(primitiveType));
 
                if (primMesh == null)
                {
                    Debug.LogError("Couldn't load Unity Primitive Mesh: " + primitiveType);
                }
            }
 
            return primMesh;
        }
 
        private static string GetPrimitiveMeshPath(PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.Sphere:
                    return "New-Sphere.fbx";
                case PrimitiveType.Capsule:
                    return "New-Capsule.fbx";
                case PrimitiveType.Cylinder:
                    return "New-Cylinder.fbx";
                case PrimitiveType.Cube:
                    return "Cube.fbx";
                case PrimitiveType.Plane:
                    return "New-Plane.fbx";
                case PrimitiveType.Quad:
                    return "Quad.fbx";
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
            }
        }
 
        private static Mesh _unityCapsuleMesh = null;
        private static Mesh _unityCubeMesh = null;
        private static Mesh _unityCylinderMesh = null;
        private static Mesh _unityPlaneMesh = null;
        private static Mesh _unitySphereMesh = null;
        private static Mesh _unityQuadMesh = null;
    }
}