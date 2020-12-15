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