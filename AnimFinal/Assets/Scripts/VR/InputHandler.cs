using UnityEngine;

namespace DefaultNamespace.VR
{
	public class InputHandler : MonoBehaviour
	{
		// Positional data from the Headset / Controllers
		public Vector3 leftControllerPosition;
		public Vector3 rightControllerPosition;
		public Vector3 headsetPosition;
		
		// Rotational data from the Headset / Controllers
		public Vector3 leftControllerRotation;
		public Vector3 rightControllerRotation;
		public Vector3 headsetRotation;

		// Assign controller prefabs to left and right respectively
		// Set rotations to be Euler
		public void Init()
		{
		}

		// Acquire Pos & Rot data for both controllers & headset
		public void CheckInput()
		{
		}

		// Set the transform of the headset and controllers
		// in real world to be translated to gamespace using VR input
		public void AdjustControllerSpace()
		{
		}
	}
}