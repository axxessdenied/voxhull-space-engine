/*
/ Created   : 4/7/2020 11:53:22 PM
/ Script    : IBindingsNameProviderAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    internal interface IBindingsNameProviderAttribute : IBindingsAttribute
    {
        string Name { get; set; }
    }
}