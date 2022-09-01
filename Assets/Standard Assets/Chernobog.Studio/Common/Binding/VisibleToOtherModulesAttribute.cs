/*
/ Created   : 4/7/2020 11:58:19 PM
/ Script    : VisibleToOtherModulesAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    [VisibleToOtherModules]
    internal class VisibleToOtherModulesAttribute : Attribute
    {
        public VisibleToOtherModulesAttribute()
        {
        }

        public VisibleToOtherModulesAttribute(params string[] modules)
        {
        }
    }
}