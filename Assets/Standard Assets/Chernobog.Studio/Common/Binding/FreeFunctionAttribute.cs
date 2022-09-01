/*
/ Created   : 4/7/2020 11:56:56 PM
/ Script    : FreeFunctionAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System;

    [VisibleToOtherModules]
    [AttributeUsage(AttributeTargets.Method)]
    internal class FreeFunctionAttribute : NativeMethodAttribute
    {
        public FreeFunctionAttribute()
        {
            this.IsFreeFunction = true;
        }

        public FreeFunctionAttribute(string name)
            : base(name, true)
        {
        }

        public FreeFunctionAttribute(string name, bool isThreadSafe)
            : base(name, true, isThreadSafe)
        {
        }
    }
}