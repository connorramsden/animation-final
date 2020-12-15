using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
	public class FrameData
	{
		public Vector3 head;
		public Vector3 handLeft;
		public Vector3 handRight;
	}

	public class LiveDataCaptureInterface : MonoBehaviour
	{
		public List<FrameData> positionalData;
		public List<FrameData> rotationalData;

		public void PackPosition(Vector3 handLeft, Vector3 handRight, Vector3 head)
		{
		}

		public void PackRotation(Vector3 handLeft, Vector3 handRight, Vector3 head)
		{
		}
	}
}