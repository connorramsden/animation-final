using UnityEngine;

/**
 * Core of all Hierarchical data
 * Contains no spatial or temporal information
 */
public class Node : MonoBehaviour
{
	[Tooltip("The name of this Node.")]
	public string nodeName;
	[Tooltip("The depth this Node has inside a tree.")]
	public int index;
	[Tooltip("The index of this Node's parent.")]
	public int parentIndex;

	// Default initialization
	public void Awake()
	{
		// Temporarily initialize all values
		nodeName = "Node";
		index = 0;
		parentIndex = -1;
	}

	// Custom initialization
	public void Init(string newName, int newIndex, int newPIndex)
	{
		// Initialize members with passed variables
		gameObject.name = nodeName = newName;
		index = newIndex;
		parentIndex = newPIndex;
	}
}
