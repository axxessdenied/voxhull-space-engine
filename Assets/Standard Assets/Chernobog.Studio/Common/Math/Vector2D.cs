/*
/ Created   : 4/7/2020 10:23:57 PM
/ Script    : Vector2D.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : SpaceRpg
/ Github    : https://github.com/axxessdenied
*/



namespace Chernobog.Studio.Common
{
    using UnityEngine;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using UnityEngine.Internal;
    using System.Text;
    using Unity.Mathematics;

    /// <summary>
    ///   <para>Representation of 2D vectors and points using doubles.</para>
    /// </summary>
    public struct Vector2D : IEquatable<Vector2D>, IFormattable
    {
        /// <summary>
        ///   <para>X component of the vector.</para>
        /// </summary>
        public double x;

        /// <summary>
        ///   <para>Y component of the vector.</para>
        /// </summary>
        public double y;
        
        public float xf
        {
            get => (float) x;
        }

        public float yf
        {
            get => (float) y;
        }

        private static readonly Vector2D zeroVector = new Vector2D(0.0, 0.0);
        private static readonly Vector2D oneVector = new Vector2D(1, 1);
        private static readonly Vector2D upVector = new Vector2D(0.0, 1);
        private static readonly Vector2D downVector = new Vector2D(0.0, -1);
        private static readonly Vector2D leftVector = new Vector2D(-1, 0.0);
        private static readonly Vector2D rightVector = new Vector2D(1, 0.0);

        private static readonly Vector2D positiveInfinityVector =
            new Vector2D(double.PositiveInfinity, double.PositiveInfinity);

        private static readonly Vector2D negativeInfinityVector =
            new Vector2D(double.NegativeInfinity, double.NegativeInfinity);

        public const double kEpsilon = 1E-05f;
        public const double kEpsilonNormalSqrt = 1E-15f;

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
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2D index!");
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
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2D index!");
                }
            }
        }

        /// <summary>
        ///   <para>Constructs a new vector with given x, y components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///   <para>Set x and y components of an existing Vector2D.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public void Set(double newX, double newY)
        {
            this.x = newX;
            this.y = newY;
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D Lerp(Vector2D a, Vector2D b, double t)
        {
            t = Mathd.Clamp01(t);
            return new Vector2D(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D LerpUnclamped(Vector2D a, Vector2D b, double t)
        {
            return new Vector2D(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Moves a point current towards target.</para>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        public static Vector2D MoveTowards(
            Vector2D current,
            Vector2D target,
            double maxDistanceDelta)
        {
            double num1 = target.x - current.x;
            double num2 = target.y - current.y;
            double num3 =  ( num1 *  num1 +  num2 *  num2);
            if ( num3 == 0.0 ||  maxDistanceDelta >= 0.0 &&
                 num3 <=  maxDistanceDelta *  maxDistanceDelta)
                return target;
            double num4 =  Math.Sqrt( num3);
            return new Vector2D(current.x + num1 / num4 * maxDistanceDelta, current.y + num2 / num4 * maxDistanceDelta);
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D Scale(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public void Scale(Vector2D scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
        }

        /// <summary>
        ///   <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        public void Normalize()
        {
            double magnitude = this.magnitude;
            if ( magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vector2D.zero;
        }

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Vector2D normalized
        {
            get
            {
                Vector2D Vector2D = new Vector2D(this.x, this.y);
                Vector2D.Normalize();
                return Vector2D;
            }
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
            return sb.AppendFormat("({0}, {1})", this.x.ToString(format, formatProvider),
                this.y.ToString(format, formatProvider)).ToString();
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        /// <summary>
        ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            return other is Vector2D other1 && this.Equals(other1);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public bool Equals(Vector2D other)
        {
            return  this.x ==  other.x &&  this.y ==  other.y;
        }

        /// <summary>
        ///   <para>Reflects a vector off the vector defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static Vector2D Reflect(Vector2D inDirection, Vector2D inNormal)
        {
            double num = -2f * Vector2D.Dot(inNormal, inDirection);
            return new Vector2D(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        /// <summary>
        ///   <para>Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.</para>
        /// </summary>
        /// <param name="inDirection">The input direction.</param>
        /// <returns>
        ///   <para>The perpendicular direction.</para>
        /// </returns>
        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D Perpendicular(Vector2D inDirection)
        {
            return new Vector2D(-inDirection.y, inDirection.x);
        }

        /// <summary>
        ///   <para>Dot Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        [MethodImpl((MethodImplOptions) 256)]
        public static double Dot(Vector2D lhs, Vector2D rhs)
        {
            return lhs.x *  rhs.x +  lhs.y *  rhs.y;
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public double magnitude
        {
            get { return  Math.Sqrt( this.x *  this.x +  this.y *  this.y); }
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public double sqrMagnitude
        {
            get { return  ( this.x *  this.x +  this.y *  this.y); }
        }

        /// <summary>
        ///   <para>Returns the unsigned angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        [MethodImpl((MethodImplOptions) 256)]
        public static double Angle(Vector2D from, Vector2D to)
        {
            double num =  Math.Sqrt( from.sqrMagnitude *  to.sqrMagnitude);
            return num < 1.00000000362749E-15
                ? 0.0f
                :  Math.Acos( math.clamp(Vector2D.Dot(from, to) / num, -1, 1)) * 57.29578f;
        }

        /// <summary>
        ///   <para>Returns the signed angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        public static double SignedAngle(Vector2D from, Vector2D to)
        {
            return Vector2D.Angle(from, to) *
                   Math.Sign( from.x * to.y -  from.y *  to.x);
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static double Distance(Vector2D a, Vector2D b)
        {
            double num1 = a.x - b.x;
            double num2 = a.y - b.y;
            return  Math.Sqrt( num1 *  num1 +  num2 *  num2);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static Vector2D ClampMagnitude(Vector2D vector, double maxLength)
        {
            double sqrMagnitude = vector.sqrMagnitude;
            if ( sqrMagnitude <=  maxLength *  maxLength)
                return vector;
            double num1 =  Math.Sqrt( sqrMagnitude);
            double num2 = vector.x / num1;
            double num3 = vector.y / num1;
            return new Vector2D(num2 * maxLength, num3 * maxLength);
        }

        public static double SqrMagnitude(Vector2D a)
        {
            return  ( a.x *  a.x +  a.y *  a.y);
        }

        public double SqrMagnitude()
        {
            return  ( this.x *  this.x +  this.y *  this.y);
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector2D Min(Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector2D Max(Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }

        [ExcludeFromDocs]
        public static Vector2D SmoothDamp(
            Vector2D current,
            Vector2D target,
            ref Vector2D currentVelocity,
            double smoothTime,
            double maxSpeed)
        {
            double deltaTime = Time.deltaTime;
            return Vector2D.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        [ExcludeFromDocs]
        public static Vector2D SmoothDamp(
            Vector2D current,
            Vector2D target,
            ref Vector2D currentVelocity,
            double smoothTime)
        {
            double deltaTime = Time.deltaTime;
            double maxSpeed = double.PositiveInfinity;
            return Vector2D.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static Vector2D SmoothDamp(
            Vector2D current,
            Vector2D target,
            ref Vector2D currentVelocity,
            double smoothTime,
            [DefaultValue("Math.Infinity")] double maxSpeed,
            [DefaultValue("Time.deltaTime")] double deltaTime)
        {
            smoothTime = Math.Max(0.0001, smoothTime);
            double num1 = 2f / smoothTime;
            double num2 = num1 * deltaTime;
            double num3 =  (1.0 / (1.0 +  num2 + 0.479999989271164 *  num2 *  num2 +
                                           0.234999999403954 *  num2 *  num2 *  num2));
            double num4 = current.x - target.x;
            double num5 = current.y - target.y;
            Vector2D Vector2D = target;
            double num6 = maxSpeed * smoothTime;
            double num7 = num6 * num6;
            double num8 =  ( num4 *  num4 +  num5 *  num5);
            if ( num8 >  num7)
            {
                double num9 =  Math.Sqrt( num8);
                num4 = num4 / num9 * num6;
                num5 = num5 / num9 * num6;
            }

            target.x = current.x - num4;
            target.y = current.y - num5;
            double num10 = (currentVelocity.x + num1 * num4) * deltaTime;
            double num11 = (currentVelocity.y + num1 * num5) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num1 * num10) * num3;
            currentVelocity.y = (currentVelocity.y - num1 * num11) * num3;
            double x = target.x + (num4 + num10) * num3;
            double y = target.y + (num5 + num11) * num3;
            double num12 = Vector2D.x - current.x;
            double num13 = Vector2D.y - current.y;
            double num14 = x - Vector2D.x;
            double num15 = y - Vector2D.y;
            if ( num12 *  num14 +  num13 *  num15 > 0.0)
            {
                x = Vector2D.x;
                y = Vector2D.y;
                currentVelocity.x = (x - Vector2D.x) / deltaTime;
                currentVelocity.y = (y - Vector2D.y) / deltaTime;
            }

            return new Vector2D(x, y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator *(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x * b.x, a.y * b.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator /(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x / b.x, a.y / b.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator -(Vector2D a)
        {
            return new Vector2D(-a.x, -a.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator *(Vector2D a, double d)
        {
            return new Vector2D(a.x * d, a.y * d);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator *(double d, Vector2D a)
        {
            return new Vector2D(a.x * d, a.y * d);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vector2D operator /(Vector2D a, double d)
        {
            return new Vector2D(a.x / d, a.y / d);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static bool operator ==(Vector2D lhs, Vector2D rhs)
        {
            double num1 = lhs.x - rhs.x;
            double num2 = lhs.y - rhs.y;
            return  num1 *  num1 +  num2 *  num2 < 9.99999943962493E-11;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static bool operator !=(Vector2D lhs, Vector2D rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static implicit operator Vector2D(Vector3 v)
        {
            return new Vector2D(v.x, v.y);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static implicit operator Vector3D(Vector2D v)
        {
            return new Vector3D(v.x, v.y, 0.0f);
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(0, 0).</para>
        /// </summary>
        public static Vector2D zero
        {
            get { return Vector2D.zeroVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(1, 1).</para>
        /// </summary>
        public static Vector2D one
        {
            get { return Vector2D.oneVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(0, 1).</para>
        /// </summary>
        public static Vector2D up
        {
            get { return Vector2D.upVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(0, -1).</para>
        /// </summary>
        public static Vector2D down
        {
            get { return Vector2D.downVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(-1, 0).</para>
        /// </summary>
        public static Vector2D left
        {
            get { return Vector2D.leftVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(1, 0).</para>
        /// </summary>
        public static Vector2D right
        {
            get { return Vector2D.rightVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(double.PositiveInfinity, double.PositiveInfinity).</para>
        /// </summary>
        public static Vector2D positiveInfinity
        {
            get { return Vector2D.positiveInfinityVector; }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2D(double.NegativeInfinity, double.NegativeInfinity).</para>
        /// </summary>
        public static Vector2D negativeInfinity
        {
            get { return Vector2D.negativeInfinityVector; }
        }
    }
}