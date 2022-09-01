/*
/ Created   : 3/13/2020 5:56:27 PM
/ Script    : UnityEvents.cs
/ Author    : Nick Slusarczyk
/ Company   : Chernobog Studio
/ Project   : Common
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;

	[Serializable]
	public class ComponentEvent : UnityEvent<Component> { };

	[Serializable]
	public class ColliderEvent : UnityEvent<Collider> { };

	[Serializable]
	public class ColliderEvent2D : UnityEvent<Collider2D> { };
}