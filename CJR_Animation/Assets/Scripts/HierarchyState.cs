using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HierarchyPose
{
	public List<SpatialPose> pose;

	public bool isInitialized;

	private void Awake()
	{
		pose = new List<SpatialPose>();
		isInitialized = false;
	}

	public void Init(int numPoses)
	{
		// Create numPoses SpatialPoses and add them to the pose array.
		for (int i = 0; i < numPoses; ++i)
		{
			// Create a temporary spatial pose
			SpatialPose tempPose = new SpatialPose();
			pose.Add(tempPose);
		}

		// If poses were successfully created, we are initialized.
		isInitialized = pose.Count > 0;
	}

	public void Init(int numPoses, SpatialPose poseTemplate)
	{
		for (int i = 0; i < numPoses; ++i)
		{
			pose.Add(poseTemplate);
		}

		// If poses were successfully created, we are initialized.
		isInitialized = pose.Count > 0;
	}

	/// PER-POSE OPERATIONS ///
	public void PoseReset(int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount)
			return;

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i].ResetPose();
		}
	}

	public void PoseConvert(SpatialPoseChannel channel, SpatialPoseEulerOrder order, int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount)
			return;

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i].PoseConvert(channel, order);
		}
	}

	public void PoseRestore(SpatialPoseChannel channel, SpatialPoseEulerOrder order, int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount)
			return;

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i].PoseRestore(channel, order);
		}
	}

	public void PoseCopy(HierarchyPose other, int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount || other == null)
			throw new ArgumentNullException();

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i] = other.pose[i];
		}
	}

	public void PoseConcat(HierarchyPose other, int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount || other == null)
			throw new ArgumentNullException();

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i].PoseConcat(other.pose[i]);
		}
	}

	public void PoseLerp(HierarchyPose other, float u, int nodeCount, int firstIndex)
	{
		// Error Handling
		if (pose.Count <= 0 || nodeCount <= 0 || pose.Count < nodeCount || other == null)
			throw new ArgumentNullException();

		for (int i = firstIndex; i < nodeCount; ++i)
		{
			pose[i].PoseLerp(other.pose[i], u);
		}
	}
}

[Serializable]
public class HierarchyPoseGroup
{
	public Hierarchy hierarchy;
	public List<HierarchyPose> hierarchyPoses;
	public List<SpatialPose> spatialPoses;
	public List<SpatialPoseChannel> channels;
	public SpatialPoseEulerOrder order;

	public int hPoseCount;
	public int sPoseCount;

	public bool isInitialized;

	public void Init(Hierarchy newH, int newC, SpatialPoseEulerOrder newO)
	{
		hierarchy = newH;
		hierarchyPoses = new List<HierarchyPose>();
		spatialPoses = new List<SpatialPose>();
		channels = new List<SpatialPoseChannel>();
		order = newO;
		hPoseCount = newC;
		sPoseCount = hPoseCount * hierarchy.numNodes;
		isInitialized = true;
	}
}

[Serializable]
public class HierarchyState
{
	public Hierarchy hierarchy;
	public HierarchyPose localSpace;
	public HierarchyPose objectSpace;
	public HierarchyPose objectSpaceInv;
	public HierarchyPose objectSpaceBindToCurrent;

	public void Init()
	{
		// Init Hierarchy to null
		hierarchy = null;
		// Init poses to null
		localSpace = objectSpace = objectSpaceInv = objectSpaceBindToCurrent = null;
	}

	public void Init(Hierarchy newH, int poseCount = 4)
	{
		int nodeCount = newH.numNodes;
		int hPoseCount = poseCount;
		int sPoseCount = hPoseCount * nodeCount;

		hierarchy = newH;

		// Init all other poses from this hierarchy.
	}

	public void UpdateState()
	{
	}

	public void UpdateObjectInverse()
	{
		throw new NotImplementedException();
	}

	public void UpdateObjectBind()
	{
		throw new NotImplementedException();
	}
}