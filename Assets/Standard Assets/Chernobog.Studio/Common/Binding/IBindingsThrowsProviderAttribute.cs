/*
/ Created   : 4/7/2020 11:55:56 PM
/ Script    : IBindingsThrowsProviderAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    internal interface IBindingsThrowsProviderAttribute : IBindingsAttribute
    {
        bool ThrowsException { get; set; }
    }
}