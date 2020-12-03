using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodeObj
{
	public string name;
	public string parentName;
}

[CreateAssetMenu(fileName = "Hierarchy_", menuName = "Tools/Hierarchy", order = 0)]
public class HierarchyObj : ScriptableObject
{
	public List<NodeObj> nodes;
}