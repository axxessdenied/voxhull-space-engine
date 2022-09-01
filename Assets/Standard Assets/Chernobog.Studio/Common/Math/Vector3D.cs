/*
/ Created   : 4/7/2020 11:08:59 PM
/ Script    : Vector3D.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : SpaceRpg
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using UnityEngine.Internal;
    using UnityEngine;
    using Unity.Mathematics;
    using System.Text;

    public struct Vector3D : IEquatable<Vector3D>, IFormattable
    {
        private static readonly Vector3D zeroVector = new Vector3D(0.0, 0.0, 0.0);
        private static readonly Vector3D oneVector = new Vector3D(1, 1, 1);
        private static readonly Vector3D upVector = new Vector3D(0.0, 1, 0.0f);
        private static readonly Vector3D downVector = new Vector3D(0.0, -1, 0.0);
        private static readonly Vector3D leftVector = new Vector3D(-1f, 0.0f, 0.0);
        private static readonly Vector3D rightVector = new Vector3D(1f, 0.0f, 0.0);
        private static readonly Vector3D forwardVector = new Vector3D(0.0, 0.0, 1);
        private static readonly Vector3D backVector = new Vector3D(0.0, 0.0, -1);
        private static readonly Vector3D positiveInfinityVector = new Vector3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        private static readonly Vector3D negativeInfinityVector = new Vector3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        public const double kEpsilon = 1E-05f;
        public const double kEpsilonNormalSqrt = 1E-15f;
        /// <summary>
        ///   <para>X component of the vector.</para>
        /// </summary>
        public double x;
        /// <summary>
        ///   <para>Y component of the vector.</para>
        /// </summary>
        public double y;
        /// <summary>
        ///   <para>Z component of the vector.</para>
        /// </summary>
        public double z;
        
        public float xf => (float) x;

        public float yf => (float) y;

        public float zf => (float) z;

        public Vector3 double2float => new Vector3(xf, yf, zf);

        /// <summary>
        ///   <para>Spherically interpolates between two vectors.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [FreeFunction("VectorScripting::Slerp", IsThreadSafe = true)]
        public static Vector3D Slerp(Vector3D a, Vector3D b, double t)
        {
            Vector3D ret;
            Vector3D.Slerp_Injected(ref a, ref b, t, out ret);
            return ret;
        }

        /// <summary>
        ///   <para>Spherically interpolates between two vectors.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [FreeFunction("VectorScripting::SlerpUnclamped", IsThreadSafe = true)]
        public static Vector3D SlerpUnclamped(Vector3D a, Vector3D b, double t)
        {
            Vector3D ret;
            Vector3D.SlerpUnclamped_Injected(ref a, ref b, t, out ret);
            return ret;
        }

        [FreeFunction("VectorScripting::OrthoNormalize", IsThreadSafe = true)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void OrthoNormalize2(ref Vector3D a, ref Vector3D b);

        public static void OrthoNormalize(ref Vector3D normal, ref Vector3D tangent)
        {
            Vector3D.OrthoNormalize2(ref normal, ref tangent);
        }

        [FreeFunction("VectorScripting::OrthoNormalize", IsThreadSafe = true)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void OrthoNormalize3(ref Vector3D a, ref Vector3D b, ref Vector3D c);

        public static void OrthoNormalize(
            ref Vector3D normal,
            ref Vector3D tangent,
            ref Vector3D binormal)
        {
            Vector3D.OrthoNormalize3(ref normal, ref tangent, ref binormal);
        }

        /// <summary>
        ///   <para>Rotates a vector current towards target.</para>
        /// </summary>
        /// <param name="current">The vector being managed.</param>
        /// <param name="target">The vector.</param>
        /// <param name="maxRadiansDelta">The maximum angle in radians allowed for this rotation.</param>
        /// <param name="maxMagnitudeDelta">The maximum allowed change in vector magnitude for this rotation.</param>
        /// <returns>
        ///   <para>The location that RotateTowards generates.</para>
        /// </returns>
        [FreeFunction(IsThreadSafe = true)]
        public static Vector3D RotateTowards(
            Vector3D current,
            Vector3D target,
            double maxRadiansDelta,
            double maxMagnitudeDelta)
        {
            Vector3D ret;
            Vector3D.RotateTowards_Injected(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta, out ret);
            return ret;
        }

        /// <summary>
        ///   <para>Linearly interpolates between two points.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D Lerp(Vector3D a, Vector3D b, double t)
        {
            t = Mathd.Clamp01(t);
            return new Vector3D(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        /// <summary>
        ///   <para>Linearly interpolates between two vectors.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D LerpUnclamped(Vector3D a, Vector3D b, double t)
        {
            return new Vector3D(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        /// <summary>
        ///   <para>Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.</para>
        /// </summary>
        /// <param name="current">The position to move from.</param>
        /// <param name="target">The position to move towards.</param>
        /// <param name="maxDistanceDelta">Distance to move current per call.</param>
        /// <returns>
        ///   <para>The new position.</para>
        /// </returns>
        public static Vector3D MoveTowards(
            Vector3D current,
            Vector3D target,
            double maxDistanceDelta)
        {
            double num1 = target.x - current.x;
            double num2 = target.y - current.y;
            double num3 = target.z - current.z;
            double num4 = (num1 * num1 + num2 * num2 + num3 * num3);
            if (num4 == 0.0 || maxDistanceDelta >= 0.0 && num4 <= maxDistanceDelta * maxDistanceDelta)
                return target;
            double num5 = Math.Sqrt(num4);
            return new Vector3D(current.x + num1 / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
        }

        [ExcludeFromDocs]
        public static Vector3D SmoothDamp(
            Vector3D current,
            Vector3D target,
            ref Vector3D currentVelocity,
            double smoothTime,
            double maxSpeed)
        {
            double deltaTime = Time.deltaTime;
            return Vector3D.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        [ExcludeFromDocs]
        public static Vector3D SmoothDamp(
            Vector3D current,
            Vector3D target,
            ref Vector3D currentVelocity,
            double smoothTime)
        {
            double deltaTime = Time.deltaTime;
            double maxSpeed = double.PositiveInfinity;
            return Vector3D.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector3D SmoothDamp(
            Vector3D current,
            Vector3D target,
            ref Vector3D currentVelocity,
            double smoothTime,
            [DefaultValue("Mathf.Infinity")] double maxSpeed,
            [DefaultValue("Time.deltaTime")] double deltaTime)
        {
            smoothTime = Math.Max(0.0001, smoothTime);
            double num1 = 2f / smoothTime;
            double num2 = num1 * deltaTime;
            double num3 = (1.0 / (1.0 +  num2 + 0.479999989271164 *  num2 *  num2 + 0.234999999403954 * num2 * num2 * num2));
            double num4 = current.x - target.x;
            double num5 = current.y - target.y;
            double num6 = current.z - target.z;
            Vector3D Vector3D = target;
            double num7 = maxSpeed * smoothTime;
            double num8 = num7 * num7;
            double num9 = ( num4 * num4 + num5 * num5 + num6 * num6);
            if ( num9 > num8)
            {
                double num10 =  Math.Sqrt(num9);
                num4 = num4 / num10 * num7;
                num5 = num5 / num10 * num7;
                num6 = num6 / num10 * num7;
            }
            target.x = current.x - num4;
            target.y = current.y - num5;
            target.z = current.z - num6;
            double num11 = (currentVelocity.x + num1 * num4) * deltaTime;
            double num12 = (currentVelocity.y + num1 * num5) * deltaTime;
            double num13 = (currentVelocity.z + num1 * num6) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num1 * num11) * num3;
            currentVelocity.y = (currentVelocity.y - num1 * num12) * num3;
            currentVelocity.z = (currentVelocity.z - num1 * num13) * num3;
            double x = target.x + (num4 + num11) * num3;
            double y = target.y + (num5 + num12) * num3;
            double z = target.z + (num6 + num13) * num3;
            double num14 = Vector3D.x - current.x;
            double num15 = Vector3D.y - current.y;
            double num16 = Vector3D.z - current.z;
            double num17 = x - Vector3D.x;
            double num18 = y - Vector3D.y;
            double num19 = z - Vector3D.z;
            if (num14 * num17 + num15 * num18 + num16 * num19 > 0.0)
            {
                x = Vector3D.x;
                y = Vector3D.y;
                z = Vector3D.z;
                currentVelocity.x = (x - Vector3D.x) / deltaTime;
                currentVelocity.y = (y - Vector3D.y) / deltaTime;
                currentVelocity.z = (z - Vector3D.z) / deltaTime;
            }
            return new Vector3D(x, y, z);
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3D index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3D index!");
                }
            }
        }

        /// <summary>
        ///   <para>Creates a new vector with given x, y, z components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        ///   <para>Creates a new vector with given x, y components and sets z to zero.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public Vector3D(double x, double y)
        {
            this.x = x;
            this.y = y;
            z = 0.0f;
        }
        
        /// <summary>
        ///   <para>Creates a new vector from a float vector</para>
        /// </summary>
        /// <param name="copy"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public Vector3D(Vector3 copy)
        {
            x = copy.x;
            y = copy.y;
            z = copy.z;
        }

        /// <summary>
        ///   <para>Set x, y and z components of an existing Vector3D.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public void Set(double newX, double newY, double newZ)
        {
            this.x = newX;
            this.y = newY;
            this.z = newZ;
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D Scale(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public void Scale(Vector3D scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
            this.z *= scale.z;
        }

        /// <summary>
        ///   <para>Cross Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector3D Cross(Vector3D lhs, Vector3D rhs)
        {
            return new Vector3D((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y *  rhs.x));
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
        }

        /// <summary>
        ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            return other is Vector3D other1 && this.Equals(other1);
        }

        public bool Equals(Vector3D other)
        {
            return  this.x ==  other.x && this.y == other.y && this.z == other.z;
        }

        /// <summary>
        ///   <para>Reflects a vector off the plane defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static Vector3D Reflect(Vector3D inDirection, Vector3D inNormal)
        {
            double num = -2f * Vector3D.Dot(inNormal, inDirection);
            return new Vector3D(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z);
        }

        /// <summary>
        ///   <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        /// <param name="value"></param>
        public static Vector3D Normalize(Vector3D value)
        {
            double num = Vector3D.Magnitude(value);
            return num > 9.99999974737875E-06 ? value / num : Vector3D.zero;
        }

        public void Normalize()
        {
            double num = Vector3D.Magnitude(this);
            if (num > 9.99999974737875E-06)
                this = this / num;
            else
                this = Vector3D.zero;
        }

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Vector3D normalized
        {
            get
            {
                return Vector3D.Normalize(this);
            }
        }

        /// <summary>
        ///   <para>Dot Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static double Dot(Vector3D lhs, Vector3D rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
        }

        /// <summary>
        ///   <para>Projects a vector onto another vector.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        public static Vector3D Project(Vector3D vector, Vector3D onNormal)
        {
            double num1 = Vector3D.Dot(onNormal, onNormal);
            if (num1 < Mathf.Epsilon)
                return Vector3D.zero;
            double num2 = Vector3D.Dot(vector, onNormal);
            return new Vector3D(onNormal.x * num2 / num1, onNormal.y * num2 / num1, onNormal.z * num2 / num1);
        }

        /// <summary>
        ///   <para>Projects a vector onto a plane defined by a normal orthogonal to the plane.</para>
        /// </summary>
        /// <param name="planeNormal">The direction from the vector towards the plane.</param>
        /// <param name="vector">The location of the vector above the plane.</param>
        /// <returns>
        ///   <para>The location of the vector on the plane.</para>
        /// </returns>
        public static Vector3D ProjectOnPlane(Vector3D vector, Vector3D planeNormal)
        {
            double num1 = Vector3D.Dot(planeNormal, planeNormal);
            if (num1 < Mathf.Epsilon)
                return vector;
            double num2 = Vector3D.Dot(vector, planeNormal);
            return new Vector3D(vector.x - planeNormal.x * num2 / num1, vector.y - planeNormal.y * num2 / num1, vector.z - planeNormal.z * num2 / num1);
        }

        /// <summary>
        ///   <para>Returns the angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <returns>
        ///   <para>The angle in degrees between the two vectors.</para>
        /// </returns>
        public static double Angle(Vector3D from, Vector3D to)
        {
            double num = Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            return num < 1.00000000362749E-15 ? 0.0f : Math.Acos(math.clamp(Vector3D.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }

        /// <summary>
        ///   <para>Returns the signed angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <param name="axis">A vector around which the other vectors are rotated.</param>
        public static double SignedAngle(Vector3D from, Vector3D to, Vector3D axis)
        {
            double num1 = Vector3D.Angle(from, to);
            double num2 =  (from.y * to.z -from.z * to.y);
            double num3 = (from.z *  to.x - from.x * to.z);
            double num4 =  ( from.x * to.y - from.y * to.x);
            double num5 = Math.Sign(axis.x *  num2 + axis.y * num3 + axis.z * num4);
            return num1 * num5;
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static double Distance(Vector3D a, Vector3D b)
        {
            double num1 = a.x - b.x;
            double num2 = a.y - b.y;
            double num3 = a.z - b.z;
            return Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static Vector3D ClampMagnitude(Vector3D vector, double maxLength)
        {
            double sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude <= maxLength * maxLength)
                return vector;
            double num1 =  Math.Sqrt(sqrMagnitude);
            double num2 = vector.x / num1;
            double num3 = vector.y / num1;
            double num4 = vector.z / num1;
            return new Vector3D(num2 * maxLength, num3 * maxLength, num4 * maxLength);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static double Magnitude(Vector3D vector)
        {
            return Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public double magnitude
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static double SqrMagnitude(Vector3D vector)
        {
            return (vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public double sqrMagnitude
        {
            get
            {
                return (this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D Min(Vector3D lhs, Vector3D rhs)
        {
            return new Vector3D(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D Max(Vector3D lhs, Vector3D rhs)
        {
            return new Vector3D(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(0, 0, 0).</para>
        /// </summary>
        public static Vector3D zero
        {
            get
            {
                return Vector3D.zeroVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(1, 1, 1).</para>
        /// </summary>
        public static Vector3D one
        {
            get
            {
                return Vector3D.oneVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(0, 0, 1).</para>
        /// </summary>
        public static Vector3D forward
        {
            get
            {
                return Vector3D.forwardVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(0, 0, -1).</para>
        /// </summary>
        public static Vector3D back
        {
            get
            {
                return Vector3D.backVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(0, 1, 0).</para>
        /// </summary>
        public static Vector3D up
        {
            get
            {
                return Vector3D.upVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(0, -1, 0).</para>
        /// </summary>
        public static Vector3D down
        {
            get
            {
                return Vector3D.downVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(-1, 0, 0).</para>
        /// </summary>
        public static Vector3D left
        {
            get
            {
                return Vector3D.leftVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(1, 0, 0).</para>
        /// </summary>
        public static Vector3D right
        {
            get
            {
                return Vector3D.rightVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity).</para>
        /// </summary>
        public static Vector3D positiveInfinity
        {
            get
            {
                return Vector3D.positiveInfinityVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity).</para>
        /// </summary>
        public static Vector3D negativeInfinity
        {
            get
            {
                return Vector3D.negativeInfinityVector;
            }
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator -(Vector3D a)
        {
            return new Vector3D(-a.x, -a.y, -a.z);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator *(Vector3D a, double d)
        {
            return new Vector3D(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator *(double d, Vector3D a)
        {
            return new Vector3D(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector3D operator /(Vector3D a, double d)
        {
            return new Vector3D(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3D lhs, Vector3D rhs)
        {
            double num1 = lhs.x - rhs.x;
            double num2 = lhs.y - rhs.y;
            double num3 = lhs.z - rhs.z;
            return  num1 * num1 + num2 * num2 + num3 * num3 < 9.99999943962493E-11;
        }

        public static bool operator !=(Vector3D lhs, Vector3D rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///   <para>Returns a formatted string for this vector.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public override string ToString()
        {
            return this.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for this vector.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format)
        {
            return this.ToString(format, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        ///   <para>Returns a formatted string for this vector.</para>
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            var sb = new StringBuilder();
            return sb.AppendFormat("({0}, {1}, {2})", (object) this.x.ToString(format, formatProvider), (object) this.y.ToString(format, formatProvider), (object) this.z.ToString(format, formatProvider)).ToString();
        }

        [Obsolete("Use Vector3D.forward instead.")]
        public static Vector3D fwd
        {
            get
            {
                return new Vector3D(0.0f, 0.0f, 1f);
            }
        }

        [Obsolete("Use Vector3D.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
        public static double AngleBetween(Vector3D from, Vector3D to)
        {
            return Math.Acos(math.clamp(Vector3D.Dot(from.normalized, to.normalized), -1f, 1f));
        }

        [Obsolete("Use Vector3D.ProjectOnPlane instead.")]
        public static Vector3D Exclude(Vector3D excludeThis, Vector3D fromThat)
        {
            return Vector3D.ProjectOnPlane(fromThat, excludeThis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Slerp_Injected(
            ref Vector3D a,
            ref Vector3D b,
            double t,
            out Vector3D ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void SlerpUnclamped_Injected(
            ref Vector3D a,
            ref Vector3D b,
            double t,
            out Vector3D ret);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void RotateTowards_Injected(
            ref Vector3D current,
            ref Vector3D target,
            double maxRadiansDelta,
            double maxMagnitudeDelta,
            out Vector3D ret);
    }
}