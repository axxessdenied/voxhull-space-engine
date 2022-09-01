/*
/ Created   : 4/7/2020 11:49:17 PM
/ Script    : NativeMethodAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    internal class NativeMethodAttribute : Attribute, IBindingsNameProviderAttribute, IBindingsAttribute, IBindingsIsThreadSafeProviderAttribute, IBindingsIsFreeFunctionProviderAttribute, IBindingsThrowsProviderAttribute
    {
        public string Name { get; set; }

        public bool IsThreadSafe { get; set; }

        public bool IsFreeFunction { get; set; }

        public bool ThrowsException { get; set; }

        public bool HasExplicitThis { get; set; }

        public bool WritableSelf { get; set; }

        public NativeMethodAttribute()
        {
        }

        public NativeMethodAttribute(string name)
        {
            switch (name)
            {
                case "":
                    throw new ArgumentException("name cannot be empty", nameof (name));
                case null:
                    throw new ArgumentNullException(nameof (name));
                default:
                    this.Name = name;
                    break;
            }
        }

        public NativeMethodAttribute(string name, bool isFreeFunction)
            : this(name)
        {
            this.IsFreeFunction = isFreeFunction;
        }

        public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe)
            : this(name, isFreeFunction)
        {
            this.IsThreadSafe = isThreadSafe;
        }

        public NativeMethodAttribute(string name, bool isFreeFunction, bool isThreadSafe, bool throws)
            : this(name, isFreeFunction, isThreadSafe)
        {
            this.ThrowsException = throws;
        }
    }
}