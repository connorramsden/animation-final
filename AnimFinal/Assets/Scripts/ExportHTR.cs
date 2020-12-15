using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportHTR : MonoBehaviour
{
    
    /* 
    # Maya handles Euler Angle Rotation Order as an enum, so this resolves that into string
    def handleRotOrder(num):
    	switcher = {
    		0: 'XYZ',
    		1: 'YZX',
    		2: 'ZXY',
    		3: 'XZY',
    		4: 'YXZ',
    		5: 'ZYX'
    	}
    
    	return switcher.get(num)
    
    # Selects and compiles information on the skeletal hierarchy
    def selectSkeleton():
    	print 'Beginning Skeleton Information Gathering\n'
    	# Gather all nodes from the Maya skeleton
    	maya_skel = cmds.ls('Character1_*')  # this could be more modular
    	# Create our output list
    	skeleton = []
    	# get rotation order from the hips (should be the same across all nodes,
    	#                                       but not best way to handle this)
    	rot_order = cmds.getAttr('Character1_Hips.rotateOrder')
    	# Iterate over each node to pack data
    	for elem in maya_skel:
    		# Packed data
    		if elem == 'Character1_ReferenceShape':  # ensure we do not pack blank data
    			continue
    		node = [elem]
    		# Collect each node's attribute names (no numerical info)
    		elem_attr = cmds.listAttr(elem, keyable=True)
    		# node.append(elem_attr)
    		# now collect numerical info per attribute
    		if elem_attr is not None:  # ensures that we will not have null type errors
    			for attr in elem_attr:
    				node_attr = cmds.getAttr(elem + '.' + attr)  # gets individual element data
    				if type(node_attr) is float:
    					node_attr = '%.6f' % node_attr
    				node.append(node_attr)
    		skeleton.append(node)
    	skeleton.append(cmds.getAttr('Character1_Hips.scaleX'))
    	skeleton.append(handleRotOrder(rot_order))  # rotation euler order is last thing packed
    	print '\nEnding Skeleton Information Gathering'
    	return skeleton
    
    # Organizes and packs Animation Header Data
    def collectAnimInfo(skeleton_data):
    	print 'Beginning Animation Information Gathering\n'
    	num_segments = len(skeleton_data)  # NumSegments
    	num_frames = 2  # NumFrames || Need to find out how to retrieve this from Maya
    	fps = int(mel.eval('float $fps = `currentTimeUnitToFPS`'))  # Calculate DataFrameRate
    	data = [num_segments, num_frames, fps]  # pack the data for return
    	print '\nEnding Animation Information Gathering'
    	return data
    
    # Returns a string for Segment Names & Hierarchy
    def formatSkeletonHierarchy(skeleton_data):
    	print(skeleton_data)
    	return ""
    
    # Writes out data to HTR file format
    def exportInfo(anim_data, skeleton_data):
    	print 'Beginning HTR exportation.\n'
    	# create a new file with output_file name
    	f = open(output_file, 'w')
    
    	''' Write disclaimers & user information'''
    	f.write('# Comment line ignore any data following # character\r\n')
    	f.write('# Hierarchical Translation and Rotation (.htr) file\r\n')
    
    	''' write header '''
    	f.write('[Header] \t\t# Header keywords are followed by a single value\r\n')
    	f.write('# KeyWord<space>Value<CR>\n')
    	f.write('FileType htr \r\nDataType HTRS \r\nFileVersion 1\r\n')
    	f.write('NumSegments %i \r\nNumFrames %i \r\nDataFrameRate %i\r\n' % (anim_data[0], anim_data[1], anim_data[2]))
    	f.write('EulerRotationOrder %s \r\nCalibrationUnits mm\r\n' % (skeleton_data[-1]))
    	# this information should be retrieved from the data, but I need HIK/MEL documentation (which doesn't exist)
    	f.write('GlobalAxisOfGravity Y \r\nRotationUnits Degrees \r\nBoneLengthAxis Y\r\n')
    	f.write('ScaleFactor %f\r\n' % skeleton_data[-2])
    
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
    
    
    # A class with a constructor and doIt(MPX Command thing) for MayaPy
    class ExportHtrClass(OpenMayaMPx.MPxCommand):
    
    	def __init__(self):
    		''' Constructor. '''
    		OpenMayaMPx.MPxCommand.__init__(self)
    
    	def doIt(self, args):
    		''' Command Execution (main()) '''
    		skel_data = selectSkeleton()
    		anim_data = collectAnimInfo(skel_data)
    		exportInfo(anim_data, skel_data)
    
    
    # Allows us to initialize the command in MayaPy
    def cmdCreator():
    	''' Create an instance of our command. '''
    	return OpenMayaMPx.asMPxPtr(ExportHtrClass())
    
    
    # Initializes the Plugin in Maya
    def initializePlugin(mobject):
    	''' Initialize the plug-in when Maya loads it. '''
    	mplugin = OpenMayaMPx.MFnPlugin(mobject)
    	try:
    		mplugin.registerCommand(commandName, cmdCreator)
    	except:
    		sys.stderr.write('Failed to register command: ' + commandName)
    
    
    # Uninitializes the plugin in Maya
    def uninitializePlugin(mobject):
    	''' Uninitialize the plug-in when Maya un-loads it. '''
    	mplugin = OpenMayaMPx.MFnPlugin(mobject)
    	try:
    		mplugin.deregisterCommand(commandName)
    	except:
    		sys.stderr.write('Failed to unregister command: ' + commandName)

    */
}
