using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalHandler : MonoBehaviour
{
	public List<GameObject> OVRSkeletonNodes;     //mJointList
	public Transform LeftOculus;
	public Transform RightOculus;
	public Transform HeadsetOculus;
	public int iterations = 3;

	private List<GameObject> LeftArm;
	private List<GameObject> RightArm;
	private List<GameObject> LeftLeg;
	private List<GameObject> RightLeg;
	private List<GameObject> Spine;
	private float HipToGroundHeight;

	List<float> LeftArmBoneLengths;
	List<float> RightArmBoneLengths;
	List<float> LeftLegBoneLengths;
	List<float> RightLegBoneLengths;
	List<float> SpineBoneLengths;

	float LeftArmChainLength = 0; //total length of all joints
	float RightArmChainLength = 0; //total length of all joints
	float LeftLegChainLength = 0; //total length of all joints
	float RightLegChainLength = 0; //total length of all joints
	float SpineChainLength = 0; //total length of all joints
	float LeftArmEffectorDistance = 0;    //distance from root to effector
	float RightArmEffectorDistance = 0;    //distance from root to effector
	float LeftLegEffectorDistance = 0;    //distance from root to effector
	float RightLegEffectorDistance = 0;    //distance from root to effector
	float SpineEffectorDistance = 0;    //distance from root to effector
	
	//ref GameObject mpBaseEffector;
	//ref GameObject mpEndEffector;
	//HierarchicalPosePool* mpSkeleton;


	void Start()
    {
		if (OVRSkeletonNodes.Count <= 0)
		{
			solveHipToGround();
			fillJointLists();
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		updateFullSkeletonPostion();

		SolveInverseKinematicsIter(ref LeftArm, LeftArmBoneLengths, LeftOculus.position, LeftArmEffectorDistance, LeftArmChainLength, iterations);
		SolveInverseKinematicsIter(ref RightArm, RightArmBoneLengths, RightOculus.position, RightArmEffectorDistance, RightArmChainLength, iterations);
		SolveInverseKinematicsIter(ref LeftLeg, LeftLegBoneLengths, LeftLegTarget(), LeftLegEffectorDistance, LeftLegChainLength, iterations);
		SolveInverseKinematicsIter(ref RightLeg, RightLegBoneLengths, RightLegTarget(), RightLegEffectorDistance, RightLegChainLength, iterations);
		SolveInverseKinematicsIter(ref Spine, SpineBoneLengths, HeadsetOculus.position, SpineEffectorDistance, SpineChainLength, iterations);
    }

	void solveHipToGround()
    {
		HipToGroundHeight = (HeadsetOculus.transform.position.y * 0.5f);	//this is assuming that your hips start at about halfway up your body
    }

	void fillJointLists()
	{
		//left arm
		LeftArm.Add(OVRSkeletonNodes[findInSkeleton("LeftShoulder")]);
		LeftArm.Add(OVRSkeletonNodes[findInSkeleton("LeftElbow")]);
		LeftArm.Add(OVRSkeletonNodes[findInSkeleton("LeftHand")]);
		

		//right arm
		RightArm.Add(OVRSkeletonNodes[findInSkeleton("RightShoulder")]);
		RightArm.Add(OVRSkeletonNodes[findInSkeleton("RightElbow")]);
		RightArm.Add(OVRSkeletonNodes[findInSkeleton("RightHand")]);
		

		//left leg
		LeftLeg.Add(OVRSkeletonNodes[findInSkeleton("LeftHip")]);
		LeftLeg.Add(OVRSkeletonNodes[findInSkeleton("LeftKnee")]);
		LeftLeg.Add(OVRSkeletonNodes[findInSkeleton("LeftFoot")]);


		//right leg
		RightLeg.Add(OVRSkeletonNodes[findInSkeleton("RightHip")]);
		RightLeg.Add(OVRSkeletonNodes[findInSkeleton("RightKnee")]);
		RightLeg.Add(OVRSkeletonNodes[findInSkeleton("RightFoot")]);


		//spine (has to go from hip up)
		Spine.Add(OVRSkeletonNodes[findInSkeleton("Hip")]);
		Spine.Add(OVRSkeletonNodes[findInSkeleton("Spine")]);
		Spine.Add(OVRSkeletonNodes[findInSkeleton("Neck")]);
		Spine.Add(OVRSkeletonNodes[findInSkeleton("Head")]);


		//calculate chain length after jointlists are filled
		fillJointListBoneAndChainLengths(LeftArm, ref LeftArmBoneLengths);
		fillJointListBoneAndChainLengths(RightArm, ref RightArmBoneLengths);
		fillJointListBoneAndChainLengths(LeftLeg, ref LeftLegBoneLengths);
		fillJointListBoneAndChainLengths(RightLeg, ref RightLegBoneLengths);
		fillJointListBoneAndChainLengths(Spine, ref SpineBoneLengths);

		//calculate effector distance
		LeftArmEffectorDistance = (LeftArm[0].transform.position - LeftArm[(LeftArm.Count - 1)].transform.position).magnitude;
		RightArmEffectorDistance = (RightArm[0].transform.position - RightArm[(RightArm.Count - 1)].transform.position).magnitude;
		LeftLegEffectorDistance = (LeftLeg[0].transform.position - LeftLeg[(LeftLeg.Count - 1)].transform.position).magnitude;
		RightLegEffectorDistance = (RightLeg[0].transform.position - RightLeg[(RightLeg.Count - 1)].transform.position).magnitude;
		SpineEffectorDistance = (Spine[0].transform.position - Spine[(Spine.Count - 1)].transform.position).magnitude;
		
	}

	int findInSkeleton(string jointName)
    {
		int result = 0;

		for(int i = 0; i < OVRSkeletonNodes.Count - 1; ++i)
        {
			if(OVRSkeletonNodes[i].gameObject.name == jointName)
            {
				result = i;
				break;
            }
        }

		return result;
    }

	void fillJointListBoneAndChainLengths(List<GameObject> targetNodes, ref List<float> distances)
    {
		if (targetNodes.Count != 0)
		{
			for (int i = 0; i < targetNodes.Count; i++)
			{
				Vector3 targetPos = targetNodes[i].transform.position;
				Debug.DrawLine(this.transform.position, targetPos, Color.red, 5f);

				//Debug.Log("Bone Length: " + (targetPos - this.transform.position).magnitude);
				distances.Add((targetPos - this.transform.position).magnitude);
			}
		}
		else
		{
			Debug.Log("Joint List is Empty");
		}
	}

	void updateFullSkeletonPostion()
    {
		Vector3 headsetXZPos = new Vector3(HeadsetOculus.position.x, 0.0f, HeadsetOculus.position.z);
		//based on the x-z position of the headset, move the full skeleton
		for(int i = 0; i < OVRSkeletonNodes.Count - 1; ++i)
        {
			OVRSkeletonNodes[i].transform.position += headsetXZPos;
        }
    }

	Vector3 LeftLegTarget()
    {
		return (LeftLeg[0].transform.position - new Vector3(0.0f, HipToGroundHeight, 0.0f));
	}

	Vector3 RightLegTarget()
	{
		return (RightLeg[0].transform.position - new Vector3(0.0f, HipToGroundHeight, 0.0f));
	}

	void SolveInverseKinematicsIter(ref List<GameObject> jointList, List<float> distances, Vector3 targetPos, float effectorDist, float chainLength, int iterations)
	{
		if (effectorDist >= chainLength)
		{
			return;
		}

		int iter = 0;
		if (iterations <= 0)
		{
			iter = 1;
		}

		Vector3 firstBasePos = jointList[0].transform.position;    //record original root location

		for (int i = 0; i < iter; ++i)
		{
			jointList[(jointList.Count - 1)].transform.position  = targetPos;    //move the endEffector to the target

			//go through the chain from end to beginning
			for (int j = (jointList.Count - 1); j > 1; --j)     //has to be this weird way for parents
			{
				//GameObject currJoint = jointList[j];
				//GameObject parentJoint = jointList[j - 1];

				Vector3 dir = (jointList[j - 1].transform.position - jointList[j].transform.position).normalized;    //set the direction vector to place the parent joint inline

				jointList[j - 1].transform.position = jointList[j].transform.position - (distances[j - 1] * dir); //set position of parent joint in the direction of the joint movement by the distance between
			}

			jointList[0].transform.position = firstBasePos;      //move the beginning joint back to its root pos

			for (int j = 0; j < jointList.Count - 1; ++j)
			{
				//GameObject currJoint = jointList[j];
				//GameObject childJoint = jointList[j + 1];

				Vector3 dir = (jointList[j + 1].transform.position - jointList[j].transform.position).normalized;
				jointList[j + 1].transform.position = jointList[j].transform.position + (distances[j] * dir);
			}
		}
	}

}
