using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DefaultNamespace.VR;

namespace DefaultNamespace
{
	[Serializable]
	public class FrameData
	{
		public Vector3 head;
		public Vector3 handLeft;
		public Vector3 handRight;

		public void Init(Vector3 left, Vector3 right, Vector3 _head)
		{
			handLeft = left;
			handRight = right;
			head = _head;
		}
	}

	public class LiveDataCaptureInterface : MonoBehaviour
	{
		// Lists of positional and rotational data
		public List<FrameData> positionalData;
		public List<FrameData> rotationalData;

		// The handler associated with the OVR Input system
		public InputHandler inputHandler;

		public bool shouldCapture;

		public void captureData()
		{
			// Acquire positions from the Input Handler

			FrameData position = new FrameData();
			position.Init(inputHandler.leftControllerPosition, inputHandler.rightControllerPosition, inputHandler.headsetPosition);
			FrameData rotation = new FrameData();
			rotation.Init(inputHandler.leftControllerRotation, inputHandler.rightControllerRotation, inputHandler.headsetRotation);

			positionalData.Add(position);
			rotationalData.Add(rotation);
		}

		private void Awake()
		{
			shouldCapture = false;
		}

		private void FixedUpdate()
		{
			// If the user is holding a button down, perform data capture.
			if(shouldCapture)
				captureData();
		}
	}
}