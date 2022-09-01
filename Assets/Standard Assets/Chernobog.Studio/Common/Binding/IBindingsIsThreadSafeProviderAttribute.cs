/*
/ Created   : 4/7/2020 11:54:11 PM
/ Script    : IBindingsIsThreadSafeProviderAttribute.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog.Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
    internal interface IBindingsIsThreadSafeProviderAttribute : IBindingsAttribute
    {
        bool IsThreadSafe { get; set; }
    }
}