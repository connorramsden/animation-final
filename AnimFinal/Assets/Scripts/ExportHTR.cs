using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportHTR : MonoBehaviour
{
	/*    
    # Writes out data to HTR file format
    def exportInfo(anim_data, skeleton_data):
    	print 'Beginning HTR exportation.\n'
    	# create a new file with output_file name
    	f = open(output_file, 'w')
            
    	''' write segment names and hierarchy info '''
    	f.write('[SegmentNames&Hierarchy] \r\n#CHILD\tPARENT\r\n')
    	segment_names_hierarchy = formatSkeletonHierarchy(skeleton_data)
    
    	''' write base position '''
    	f.write('[BasePosition] \r\n#SegmentName Tx, Ty, Tz, Rx, Ry, Rz, BoneLength\r\n')
    	for node in skeleton_data[:-2]:
    		for data in node:
    			f.write('%s\t' % str(data))
    		f.write('\r\n')
    
    	''' write per-node info per-keyframe'''
    	f.write('#Beginning of Data. Separated by tabs.\r\n')
    	for node in skeleton_data[:-2]:
    		f.write('[%s]\r\n' % str(node[0]))  # write the node's name
    		f.write('#Fr\tTx\tTy\tTz\tRx\tRy\tRz\tSF\r\n')  # write out comments
    		for frame in range(anim_data[1]):  # write per-frame data
    			f.write('%i\t' % frame)  # write frame number
    			for data in node[1:]:  # should eventually be PER-FRAME (but we only have one frame rn)
    				f.write('%s\t' % str(data))
    			f.write('\r\n')
    
    	''' write end of file '''
    	f.write('[EndOfFile]')
    
    	f.close()
    	print '\nEnding HTR exportation.'
        */

	const string outputPath = @"E:\projects\animation-final\mocap.htr";

	public bool ExportInfo(List<FrameData> posDat, List<FrameData> rotDat)
	{
		if (!File.Exists(outputPath))
		{
			using (StreamWriter sw = File.CreateText(outputPath))
			{
				// Write disclaimers & user info
				sw.Write("# Comment lines ignore any data following # character\r\n");
				sw.Write("# Hierarchical Translation and Rotation (.htr) file\r\n");

				// Write header
				sw.Write("[Header]\t\t# Header keywords are followed by a single value\r\n");
				sw.Write("# Keyword Value\r\n");
				sw.Write("FileType htr\r\nDataType HTRS\r\nFileVersion 1\r\n");
				sw.Write($"NumSegments {posDat.Count}\r\n");
				sw.Write($"EulerRotationOrder XYZ\r\nCalibrationUnits mm\r\n");
				sw.Write($"GlobalAxisOfGravity Y\r\nRotationUnits Degrees\r\nBoneLengthAxis Y\r\n");
				sw.Write($"ScaleFactor 1.0\r\n");

				// Write base position
				sw.Write("[BasePosition]\r\n# SegmentName\t Tx, Ty, Tz, Rx, Ry, Rz, BoneLength\r\n");
				sw.Write($"LeftHand {posDat[0].handLeft.x}, {posDat[0].handLeft.y}, {posDat[0].handLeft.z}, {rotDat[0].handLeft.x}, {rotDat[0].handLeft.y}, {rotDat[0].handLeft.z}, 1.0\r\n");
				sw.Write($"RightHand {posDat[0].handRight.x}, {posDat[0].handRight.y}, {posDat[0].handRight.z}, {rotDat[0].handRight.x}, {rotDat[0].handRight.y}, {rotDat[0].handRight.z}, 1.0\r\n");
				sw.Write($"Head {posDat[0].head.x}, {posDat[0].head.y}, {posDat[0].head.z}, {rotDat[0].head.x}, {rotDat[0].head.y}, {rotDat[0].head.z}, 1.0\r\n");

                // Write per-node info per-keyframe
                sw.Write("#Beginning of Data. Separated by tabs.\r\n");
                // NOTE: This is more manual than I'd like, but I'd have to change a lot of packing and formatting
               
                // Write out left hand
                sw.Write("[LeftHand]\r\n");
                for(int i = 1; i < posDat.Count - 1; ++ i)
				{
                    sw.Write($"{i}\t{posDat[i].handLeft.x}\t{posDat[i].handLeft.y}\t{posDat[i].handLeft.y}\t{rotDat[i].handLeft.x}\t{rotDat[i].handLeft.y}\t{rotDat[i].handLeft.z}\t1.00\r\n");
                }

                // Write out right hand
                sw.Write("[RightHand]\r\n");
                for(int i = 1; i < posDat.Count - 1; ++ i)
				{
                    sw.Write($"{i}\t{posDat[i].handRight.x}\t{posDat[i].handRight.y}\t{posDat[i].handRight.y}\t{rotDat[i].handRight.x}\t{rotDat[i].handRight.y}\t{rotDat[i].handRight.z}\t1.00\r\n");
                }

                // Write out head
                sw.Write("[Head]\r\n");
                for(int i = 1; i < posDat.Count - 1; ++ i)
				{
                    sw.Write($"{i}\t{posDat[i].head.x}\t{posDat[i].head.y}\t{posDat[i].head.y}\t{rotDat[i].head.x}\t{rotDat[i].head.y}\t{rotDat[i].head.z}\t1.00\r\n");
                }

                // Write end of file
                sw.Write("[EndOfFile]\r\n");
			}

			return true;
		}

		return false;
	}
}
