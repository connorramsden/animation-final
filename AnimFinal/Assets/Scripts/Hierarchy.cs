using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/**
 * Effectively a Node pool
 * Nodes are sorted Tree-style, via Depth
 */
public class Hierarchy : MonoBehaviour
{
	[Tooltip("The prefab we use to describe a Node.")]
	public GameObject nodePrefab;

	[Tooltip("The list of nodes associated with this Hierarchy.")]
	public List<GameObject> nodes;

	[Tooltip("The number of nodes in this hierarchy.")]
	public int numNodes;

	[Tooltip("Allows us to check if this Hierarchy was properly initialized.")]
	public bool isInitialized;

	private void Awake()
	{
		if (nodePrefab == null)
		{
			nodePrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Node.prefab", typeof(GameObject));
		}

		nodes = new List<GameObject>();
		numNodes = 0;
		isInitialized = false;
	}

	// Initialize this Hierarchy
	public void Init(int nodesToCreate)
	{
		if (nodePrefab == null)
		{
			nodePrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Node.prefab", typeof(GameObject));
		}

		List<string> names = new List<string>();

		for (int i = 0; i < nodesToCreate; ++i)
		{
			names.Add("Node " + i);
		}

		if (numNodes <= 0)
			numNodes = nodesToCreate;

		if (names.Count > 0 || names.Count == nodesToCreate)
			Init(nodesToCreate, names);
		else
			Debug.LogError("Names Array improperly created");

		if (nodes.Count > 0 && isInitialized == false)
			isInitialized = true;
	}

	public void Init(int nodesToCreate, List<string> names)
	{
		for (int i = 0; i < nodesToCreate; ++i)
		{
			GameObject tempNode = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);
			tempNode.GetComponent<Node>().Init(names[i], i, i - 1);

			if (tempNode != null)
				nodes.Add(tempNode);
			else
				throw new NullReferenceException();
		}

		if (numNodes <= 0)
			numNodes = nodesToCreate;
		if (nodes.Count > 0 && isInitialized == false)
			isInitialized = true;
	}

	// Init this Hierarchy from a layout
	public void Init(LayoutObj layout)
	{
		int nodesToCreate = layout.GetSize();

		for (int i = 0; i < nodesToCreate; ++i)
		{
			// GameObject tempNode = Instantiate(nodePrefab, )
		}
	}

	// Set nodes[index] with the passed values
	public void SetNode(int index, int newPIndex, string newName)
	{
		Node tempNode = nodes[index].GetComponent<Node>();
		tempNode.parentIndex = newPIndex;
		tempNode.nodeName = newName;
	}
}