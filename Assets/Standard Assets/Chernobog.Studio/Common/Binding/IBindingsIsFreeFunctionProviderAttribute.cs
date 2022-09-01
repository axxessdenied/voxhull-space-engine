/*
/ Created   : 4/7/2020 11:52:00 PM
/ Script    : IBindingsIsFreeFunctionProviderAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    internal interface IBindingsIsFreeFunctionProviderAttribute : IBindingsAttribute
    {
        bool IsFreeFunction { get; set; }

        bool HasExplicitThis { get; set; }
    }
}