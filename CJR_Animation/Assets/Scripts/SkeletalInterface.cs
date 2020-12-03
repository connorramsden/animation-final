using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkeletalInterface : MonoBehaviour
{
	private UI_Animation ui;

	// The Hierarchy associated with this Object
	private Hierarchy hierarchy;

	public LayoutObj hierarchyLayout;
	public int numStates;

	// Base pose, flip through all, and clip-controlled.
	public List<HierarchyState> states;

	private void Awake()
	{
		ui = GetComponent<UI_Animation>();
		hierarchy = gameObject.AddComponent<Hierarchy>();
		states = new List<HierarchyState>();
	}

	private void Start()
	{
		hierarchy.Init(hierarchyLayout);

		for (int i = 0; i < numStates; ++i)
		{
			HierarchyState tempState = new HierarchyState();

			tempState.Init(hierarchy);
			
			states.Add(tempState);
		}
	}

	private void Update()
	{
		// 1. Apply input from User
		// Acquire the value of the HierarchyState UI
		int stateController = ui.stateDrop.value;

		// 2. Update all Controllers

		// 3. Run the 4-step algorithm for each state
	}

	private void ApplyUserInput()
	{
		
	}

	private void UpdateControllers()
	{
		
	}

	private void KinematicAlgorithm()
	{
		for (int i = 0; i < states.Count; ++i)
		{
			states[i].UpdateState();
		}
	}
}