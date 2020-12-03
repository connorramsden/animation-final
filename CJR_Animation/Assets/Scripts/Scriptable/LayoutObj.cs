using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct NodeInfo
{
	[Tooltip("Index of a Node.")]
	public int index;
	[Tooltip("The Index of a Node's Parent.")]
	public int parentIndex;
	[Tooltip("Name of a Node.")]
	public string name;
}

[CreateAssetMenu(fileName = "Layout_", menuName = "Tools/Hierarchy", order = 0)]
public class LayoutObj : ScriptableObject
{
	public List<NodeInfo> nodes;

	public int GetSize()
	{
		return nodes.Count - 1;
	}
}