using System;
using UnityEngine;

public enum SpatialPoseEulerOrder
{
	PoseEulerOrder_XYZ,
	PoseEulerOrder_YZX,
	PoseEulerOrder_ZXY,
	PoseEulerOrder_YXZ,
	PoseEulerOrder_XZY,
	PoseEulerOrder_ZYX
}

public enum SpatialPoseChannel
{
	// identity (no channels)
	PoseChannel_None,

	// orientation
	PoseChannel_Orient_X = 0x0001,
	PoseChannel_Orient_Y = 0x0002,
	PoseChannel_Orient_Z = 0x0004,
	PoseChannel_Orient_XY = PoseChannel_Orient_X | PoseChannel_Orient_Y,
	PoseChannel_Orient_YZ = PoseChannel_Orient_Y | PoseChannel_Orient_Z,
	PoseChannel_Orient_ZX = PoseChannel_Orient_Z | PoseChannel_Orient_X,
	PoseChannel_Orient_XYZ = PoseChannel_Orient_XY | PoseChannel_Orient_Z,

	// scale
	PoseChannel_Scale_X = 0x0010,
	PoseChannel_Scale_Y = 0x0020,
	PoseChannel_Scale_Z = 0x0040,
	PoseChannel_Scale_XY = PoseChannel_Scale_X | PoseChannel_Scale_Y,
	PoseChannel_Scale_YZ = PoseChannel_Scale_Y | PoseChannel_Scale_Z,
	PoseChannel_Scale_ZX = PoseChannel_Scale_Z | PoseChannel_Scale_X,
	PoseChannel_Scale_XYZ = PoseChannel_Scale_XY | PoseChannel_Scale_Z,

	// translation
	PoseChannel_Translate_X = 0x0100,
	PoseChannel_Translate_Y = 0x0200,
	PoseChannel_Translate_Z = 0x0400,
	PoseChannel_Translate_XY = PoseChannel_Translate_X | PoseChannel_Translate_Y,
	PoseChannel_Translate_YZ = PoseChannel_Translate_Y | PoseChannel_Translate_Z,
	PoseChannel_Translate_ZX = PoseChannel_Translate_Z | PoseChannel_Translate_X,
	PoseChannel_Translate_XYZ = PoseChannel_Translate_XY | PoseChannel_Translate_Z
}

public enum InterpMode
{
	Mode_Step = 0,
	Mode_Nearest,
	Mode_Linear,
	Mode_Smooth
}

[Serializable]
public class SpatialPose
{
	public Transform poseTransform;
	public Vector4 orientation;
	public Vector3 translation;
	public Vector3 rotation;
	public Vector3 scale;

	// Default constructor
	public SpatialPose()
	{
		ResetPose();
	}

	// Copy constructor
	public SpatialPose(SpatialPose other)
	{
		if (other == null)
			return;
		
		poseTransform = other.poseTransform;
		orientation = other.orientation;

		translation = other.translation;
		rotation = other.rotation;
		scale = other.scale;
	}

	// Initialize member variables for this pose with passed-in variables
	public void Init(Transform tMat, Vector4 o, Vector3 t, Vector3 r, Vector3 s)
	{
		poseTransform = tMat;
		orientation = o;
		translation = t;
		rotation = r;
		scale = s;
	}

	// Reset this Spatial Pose to its default values
	public void ResetPose()
	{
		// Should initialize an Identity transform.
		poseTransform = new RectTransform();
		// Initialize Orientation to V4 Identity
		orientation = new Vector4(0f, 0f, 0f, 1f);
		// Initialize translation to (0, 0, 0)
		translation = Vector3.zero;
		// Initialize rotation to (0, 0, 0)
		rotation = Vector3.zero;
		// Initialize Scale to (1, 1, 1,)
		scale = Vector3.one;
	}

	// Convert this Spatial Pose
	public void PoseConvert(SpatialPoseChannel channel, SpatialPoseEulerOrder order)
	{
		throw new NotImplementedException();
	}

	// Restore this Spatial Pose
	public void PoseRestore(SpatialPoseChannel channel, SpatialPoseEulerOrder order)
	{
		throw new NotImplementedException();
	}

	// Concatenate two Spatial Poses together.
	public void PoseConcat(SpatialPose other)
	{
		translation += other.translation;
		rotation += other.rotation;
		scale = Vector3.Cross(scale, other.scale);
	}

	// Lerp two Spatial Poses by float u. Defaults to regular LERP
	public void PoseLerp(SpatialPose other, float u, InterpMode mode = InterpMode.Mode_Linear)
	{
		switch (mode)
		{
			case InterpMode.Mode_Step:
				break;
			case InterpMode.Mode_Nearest:
				break;
			case InterpMode.Mode_Linear:
				translation = Vector3.Lerp(translation, other.translation, u);
				rotation = Vector3.Lerp(rotation, other.rotation, u);
				scale = Vector3.Lerp(scale, other.scale, u);
				break;
			case InterpMode.Mode_Smooth:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
		}
	}
}