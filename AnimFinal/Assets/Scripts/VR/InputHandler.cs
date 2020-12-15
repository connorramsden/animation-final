﻿using UnityEngine;

namespace DefaultNamespace.VR
{
	public class InputHandler : MonoBehaviour
	{
		// Positional & Rotational Data data from the Headset / Controllers
		[Header("Left Hand")]
		public Transform leftController;
		public Vector3 leftControllerPosition;
		public Vector3 leftControllerRotation;

		[Header("Right Hand")]
		public Transform rightController;
		public Vector3 rightControllerPosition;
		public Vector3 rightControllerRotation;

		[Header("Head")]
		public Transform headset;
		public Vector3 headsetPosition;
		public Vector3 headsetRotation;

		[Header("Sub-Managers")]
		public SkeletalHandler skeletalHandler;
		public LiveDataCaptureInterface captureInterface;

		// Assign controller prefabs to left and right respectively
		// Set rotations to be Euler
		public void Init()
		{
			// Init controller Transforms from SkeletalHandler
			if (leftController == null)
				leftController = skeletalHandler.LeftOculus;
			if (rightController == null)
				rightController = skeletalHandler.RightOculus;
			if (headset == null)
				headset = skeletalHandler.HeadsetOculus;

			// Set position and rotation for controllers and headset variables
			CheckInput();
		}

		// Acquire Pos & Rot data for both controllers & headset
		public void CheckInput()
		{
			// Init Left Controller pos and rot
			leftControllerPosition = leftController.position;
			leftControllerRotation = leftController.rotation.eulerAngles;
			// Init Right Controller pos and rot
			rightControllerPosition = rightController.position;
			rightControllerRotation = rightController.rotation.eulerAngles;
			// Init Headset pos and rot
			headsetPosition = headset.position;
			headsetRotation = headset.rotation.eulerAngles;
		}

		// Set the transform of the headset and controllers
		// in real world to be translated to gamespace using VR input
		public void AdjustControllerSpace()
		{
		}

		private void Awake()
		{
			Init();
		}

		public void Update()
		{
			CheckInput();
		}

		public void FixedUpdate()
		{
		}
	}
}