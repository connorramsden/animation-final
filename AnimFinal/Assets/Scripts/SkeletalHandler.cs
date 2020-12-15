using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalHandler : MonoBehaviour
{
	public GameObject OVRSkeleton;
	public List<GameObject> OVRSkeletonNodes;     //mJointList
	public Transform LeftOculus;
	public Transform RightOculus;
	public Transform HeadsetOculus;
	public int iterations = 3;
	public float effectorTolerance = 0.2f;
	public float legHeightModifier = 1.0f;
	public float chainLengthModifier = 1.2f;//this is for when you are a bit bigger than the character and dont wanna deal with ik solving to stop
	public float personHeight = 20.0f;  //this is stock skeleton height
	public float walkSpeed = 1.0f;
	public float hieghtOfStep = 0.3f;	//0 to 1 range


	private List<int> LeftArm;
	private List<int> RightArm;
	private List<int> LeftLeg;
	private List<int> RightLeg;
	private List<int> Spine;
	private List<int> Look;
	private float HipToGroundHeight;

	List<float> LeftArmBoneLengths;
	List<float> RightArmBoneLengths;
	List<float> LeftLegBoneLengths;
	List<float> RightLegBoneLengths;
	List<float> SpineBoneLengths;
	List<float> LookBoneLengths;

	float LeftArmChainLength = 0; //total length of all joints
	float RightArmChainLength = 0; //total length of all joints
	float LeftLegChainLength = 0; //total length of all joints
	float RightLegChainLength = 0; //total length of all joints
	float SpineChainLength = 0; //total length of all joints
	float LookChainLength = 0; //total length of all joints
	float LeftArmEffectorDistance = 0;    //distance from root to effector
	float RightArmEffectorDistance = 0;    //distance from root to effector
	float LeftLegEffectorDistance = 0;    //distance from root to effector
	float RightLegEffectorDistance = 0;    //distance from root to effector
	float SpineEffectorDistance = 0;    //distance from root to effector
	float LookEffectorDistance = 0;    //distance from root to effector

	//ref GameObject mpBaseEffector;
	//ref GameObject mpEndEffector;
	//HierarchicalPosePool* mpSkeleton;


	void Start()
	{
		LeftArm = new List<int>();
		RightArm = new List<int>();
		LeftLeg = new List<int>();
		RightLeg = new List<int>();
		Spine = new List<int>();
		Look = new List<int>();
		LeftArmBoneLengths = new List<float>();
		RightArmBoneLengths = new List<float>();
		LeftLegBoneLengths = new List<float>();
		RightLegBoneLengths = new List<float>();
		SpineBoneLengths = new List<float>();
		LookBoneLengths = new List<float>();


		if (OVRSkeletonNodes.Count > 0)
		{
			fillJointLists();
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//updateFullSkeletonPostion();
		updateEffectorDistances();

		solveHipToGround();
		
		SolveInverseKinematicsIter(LeftArm, LeftArmBoneLengths, LeftOculus.position, LeftArmEffectorDistance, LeftArmChainLength, iterations);
		SolveInverseKinematicsIter(RightArm, RightArmBoneLengths, RightOculus.position, RightArmEffectorDistance, RightArmChainLength, iterations);
		//SolveInverseKinematicsIter(LeftLeg, LeftLegBoneLengths, LeftLegTarget(), LeftLegEffectorDistance, LeftLegChainLength, iterations);
		//SolveInverseKinematicsIter(RightLeg, RightLegBoneLengths, RightLegTarget(), RightLegEffectorDistance, RightLegChainLength, iterations);
		//SolveInverseKinematicsIter(Look, LookBoneLengths, HeadsetOculus.position, LookEffectorDistance, LookChainLength, iterations);
		SolveInverseKinematicsIter(Spine, SpineBoneLengths, (HeadsetOculus.position), SpineEffectorDistance, SpineChainLength, iterations);	//find new target for this

	}

	void solveHipToGround()
	{

		//HipToGroundHeight = 5f;
		//HipToGroundHeight = (OVRSkeletonNodes[Look[Look.Count - 1]].transform.position - OVRSkeletonNodes[RightLeg[RightLeg.Count - 1]].transform.position).magnitude - 
		//							(LookChainLength + SpineChainLength);

		//HipToGroundHeight = RightLegChainLength - (startHeight + HeadsetOculus.position.y);


		HipToGroundHeight = ((personHeight * 0.5f) - (Mathf.Abs(HeadsetOculus.position.y - OVRSkeletonNodes[Spine[0]].transform.position.y) * personHeight)) / (personHeight * 0.5f);
		Debug.Log("[OUR CODE] Hips To Ground: " + HipToGroundHeight.ToString());
	}


	void fillJointLists()
	{
		//left arm
		LeftArm.Add(findInSkeleton("LeftShoulder"));
		LeftArm.Add(findInSkeleton("LeftElbow"));
		LeftArm.Add(findInSkeleton("LeftHand"));

		/*
		for(int i = 0; i < LeftArm.Count; ++i)
        {
			Debug.Log("[OUR CODE] " + OVRSkeletonNodes[LeftArm[i]].name);
        }
		*/

		//right arm
		RightArm.Add(findInSkeleton("RightShoulder"));
		RightArm.Add(findInSkeleton("RightElbow"));
		RightArm.Add(findInSkeleton("RightHand"));


		//left leg
		LeftLeg.Add(findInSkeleton("LeftHip"));
		LeftLeg.Add(findInSkeleton("LeftKnee"));
		LeftLeg.Add(findInSkeleton("LeftFoot"));


		//right leg
		RightLeg.Add(findInSkeleton("RightHip"));
		RightLeg.Add(findInSkeleton("RightKnee"));
		RightLeg.Add(findInSkeleton("RightFoot"));


		//spine (has to go from hip up)
		Spine.Add(findInSkeleton("Hip"));
		Spine.Add(findInSkeleton("TorsoLower"));
		Spine.Add(findInSkeleton("TorsoUpper"));
		Spine.Add(findInSkeleton("Neck"));
		Spine.Add(findInSkeleton("Head"));

		//look neck
		Look.Add(findInSkeleton("TorsoUpper"));
		Look.Add(findInSkeleton("Neck"));
		Look.Add(findInSkeleton("Head"));


		//calculate chain length after jointlists are filled
		fillJointListBoneAndChainLengths(LeftArm, ref LeftArmBoneLengths, ref LeftArmChainLength);
		fillJointListBoneAndChainLengths(RightArm, ref RightArmBoneLengths, ref RightArmChainLength);
		fillJointListBoneAndChainLengths(LeftLeg, ref LeftLegBoneLengths, ref LeftLegChainLength);
		fillJointListBoneAndChainLengths(RightLeg, ref RightLegBoneLengths, ref RightLegChainLength);
		fillJointListBoneAndChainLengths(Spine, ref SpineBoneLengths, ref SpineChainLength);
		fillJointListBoneAndChainLengths(Look, ref LookBoneLengths, ref LookChainLength);

		/*
		for(int i = 0; i < LeftArmBoneLengths.Count; ++i)
        {
			Debug.Log("[OUR CODE] LeftArmBoneLengths: " + LeftArmBoneLengths[i].ToString());
        }
		Debug.Log("[OUR CODE] LeftArmChainLength: " + LeftArmChainLength);
		*/


		//calculate effector distance
		//updateEffectorDistances();

	}

	int findInSkeleton(string jointName)
	{
		int result = 0;

		for (int i = 0; i < OVRSkeletonNodes.Count; ++i)
		{
			if (OVRSkeletonNodes[i].gameObject.name == jointName)
			{
				result = i;
				//Debug.Log("[OUR CODE] findInSkeleton: " + OVRSkeletonNodes[i].gameObject.name + " at Position #" + i);
				return result;
			}
		}

		return result;
	}

	void fillJointListBoneAndChainLengths(List<int> targetNodes, ref List<float> boneLengths, ref float chainLength)
	{
		if (targetNodes.Count != 0)
		{
			for (int i = 0; i < targetNodes.Count - 1; i++)
			{
				if (i + 1 < targetNodes.Count)
				{
					Vector3 currNodePos = OVRSkeletonNodes[targetNodes[i]].transform.position;
					Vector3 nextNodePos = OVRSkeletonNodes[targetNodes[i + 1]].transform.position;

					float dif = (nextNodePos - currNodePos).magnitude;
					chainLength += dif;
					boneLengths.Add(dif);

				}
			}
		}
		else
		{
			Debug.Log("Joint List is Empty");
		}
	}

	void updateFullSkeletonPostion()
	{
		if (LookEffectorDistance >= LookChainLength)    //if we moved the headset far enough from the base move the whole body
		{
			Vector3 headsetXZPos = new Vector3(HeadsetOculus.position.x, 0.0f, HeadsetOculus.position.z);
			//based on the x-z position of the headset, move the full skeleton
			//for (int i = 0; i < OVRSkeletonNodes.Count - 1; ++i)
			//{
			//	OVRSkeletonNodes[i].transform.position = (headsetXZPos - OVRSkeletonNodes[i].transform.position);
			//}
			OVRSkeleton.transform.position = headsetXZPos - OVRSkeleton.transform.position;
		}
	}

	Vector3 LeftLegTarget()
	{
		float bottomFlatline = 0.2f;
		float topFlatline = 0.8f;
		float stepVal = Mathf.Clamp(Mathf.Sin(Time.deltaTime * walkSpeed), bottomFlatline, topFlatline) * (HipToGroundHeight * hieghtOfStep);
		return (OVRSkeletonNodes[LeftLeg[0]].transform.position - 
					(new Vector3(0.0f, Mathf.Abs(HipToGroundHeight - stepVal), 0.0f)));
	}

	Vector3 RightLegTarget()
	{
		float bottomFlatline = 0.2f;
		float topFlatline = 0.8f;
		float stepVal = Mathf.Clamp(Mathf.Cos(Time.deltaTime * walkSpeed), bottomFlatline, topFlatline) * (HipToGroundHeight * hieghtOfStep);
		return (OVRSkeletonNodes[RightLeg[0]].transform.position - 
					(new Vector3(0.0f, Mathf.Abs(HipToGroundHeight - stepVal), 0.0f)));
	}

	void updateEffectorDistances()
	{
		LeftArmEffectorDistance = (OVRSkeletonNodes[LeftArm[0]].transform.position - LeftOculus.position).magnitude;
		RightArmEffectorDistance = (OVRSkeletonNodes[RightArm[0]].transform.position - RightOculus.position).magnitude;
		LeftLegEffectorDistance = (OVRSkeletonNodes[LeftLeg[0]].transform.position - LeftLegTarget()).magnitude;
		RightLegEffectorDistance = (OVRSkeletonNodes[RightLeg[0]].transform.position - RightLegTarget()).magnitude;
		SpineEffectorDistance = (OVRSkeletonNodes[Spine[0]].transform.position - HeadsetOculus.position).magnitude;
		LookEffectorDistance = (OVRSkeletonNodes[Look[0]].transform.position - HeadsetOculus.position).magnitude;

		//Debug.Log("[OUR CODE] LeftArmEffectorDistance: " + LeftArmEffectorDistance.ToString());
	}

	void SolveInverseKinematicsIter(List<int> jointList, List<float> distances, Vector3 targetPos, float effectorDist, float chainLength, int iterations)
	{
		//Debug.Log("[OUR CODE] ED: " + effectorDist.ToString() + "  CL: " + chainLength.ToString());

		if (effectorDist < chainLength * chainLengthModifier)
		{

			int iter = 0;
			if (iterations <= 0)
			{
				iter = 1;
			}
			else
			{
				iter = iterations;
			}

			Vector3 firstBasePos = OVRSkeletonNodes[jointList[0]].transform.position;    //record original root location

			for (int i = 0; i < iter; ++i)
			{
				//Debug.Log("[OUR CODE] END JOINT NAME: " + OVRSkeletonNodes[jointList[(jointList.Count - 1)]].name);

				OVRSkeletonNodes[jointList[(jointList.Count - 1)]].transform.position = targetPos;    //move the endEffector to the target


				//go through the chain from end to beginning
				for (int j = (jointList.Count - 1); j > 0; --j)     //has to be this weird way for parents
				{
					//GameObject currJoint = jointList[j];
					//GameObject parentJoint = jointList[j - 1];
					//Debug.Log("[OUR CODE] Current Joint: " + OVRSkeletonNodes[jointList[j]].name + "    Parent Joint: " + OVRSkeletonNodes[jointList[j - 1]].name);


					Vector3 dir = (OVRSkeletonNodes[jointList[j - 1]].transform.position - OVRSkeletonNodes[jointList[j]].transform.position).normalized;    //set the direction vector to place the parent joint inline
					float dist = 1;
					if (j != 0)
					{
						dist = distances[j - 1];
						//Debug.Log("[OUR CODE] Iterations on distance: " + (j - 1));
					}

					OVRSkeletonNodes[jointList[j - 1]].transform.position = OVRSkeletonNodes[jointList[j]].transform.position + (dist * dir); //set position of parent joint in the direction of the joint movement by the distance between
				}

				OVRSkeletonNodes[jointList[0]].transform.position = firstBasePos;      //move the beginning joint back to its root pos

				//go from beginning to end
				for (int j = 0; j < jointList.Count - 1; ++j)
				{
					//GameObject currJoint = jointList[j];
					//GameObject childJoint = jointList[j + 1];
					//Debug.Log("[OUR CODE] Current Joint: " + OVRSkeletonNodes[jointList[j]].name + "    Child Joint: " + OVRSkeletonNodes[jointList[j + 1]].name);


					Vector3 dir = (OVRSkeletonNodes[jointList[j + 1]].transform.position - OVRSkeletonNodes[jointList[j]].transform.position).normalized;
					float dist = 1;
					if (j + 1 != jointList.Count)
					{
						dist = distances[j];
						//Debug.Log("[OUR CODE] Iterations on distance: " + j);
					}

					OVRSkeletonNodes[jointList[j + 1]].transform.position = OVRSkeletonNodes[jointList[j]].transform.position + (dist * dir);
				}

				//Solver for effector
				//then effector pos minus the pos of its child
				//Vector3 offset = OVRSkeletonNodes[jointList[(jointList.Count - 1)]].transform.position - OVRSkeletonNodes[jointList[(jointList.Count - 2)]].transform.position;
				//Vector3 offset = OVRSkeletonNodes[jointList[(jointList.Count - 1)]].transform.position - targetPos;
				//if(offset.magnitude < effectorTolerance)
				//            {
				//	break;
				//            }
			}
		}
	}
}
