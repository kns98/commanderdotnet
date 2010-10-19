﻿namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;

    public enum ItemPropId : uint
    {
        kpidAttributes = 9,
        kpidBigEndian = 0x2a,
        kpidBit64 = 0x29,
        kpidBlock = 0x1b,
        kpidCharacts = 0x2f,
        kpidChecksum = 0x2e,
        kpidClusterSize = 0x1102,
        kpidComment = 0x1c,
        kpidCommented = 14,
        kpidCpu = 0x2b,
        kpidCRC = 0x13,
        kpidCreationTime = 10,
        kpidCreatorApp = 0x33,
        kpidDictionarySize = 0x12,
        kpidEncrypted = 15,
        kpidExtension = 5,
        kpidFileSystem = 0x18,
        kpidFreeSpace = 0x1101,
        kpidGroup = 0x1a,
        kpidHandlerItemIndex = 2,
        kpidHeadersSize = 0x2d,
        kpidHostOS = 0x17,
        kpidId = 0x31,
        kpidIsAnti = 0x15,
        kpidIsFolder = 6,
        kpidIsVolume = 0x23,
        kpidLastAccessTime = 11,
        kpidLastWriteTime = 12,
        kpidLink = 0x36,
        kpidLinks = 0x25,
        kpidLocalName = 0x1200,
        kpidMethod = 0x16,
        kpidName = 4,
        kpidNoProperty = 0,
        kpidNumBlocks = 0x26,
        kpidNumSubFiles = 0x20,
        kpidNumSubFolders = 0x1f,
        kpidNumVolumes = 0x27,
        kpidOffset = 0x24,
        kpidPackedSize = 8,
        kpidPath = 3,
        kpidPhySize = 0x2c,
        kpidPosition = 0x1d,
        kpidPosixAttrib = 0x35,
        kpidPrefix = 30,
        kpidProvider = 0x1201,
        kpidSectorSize = 0x34,
        kpidShortName = 50,
        kpidSize = 7,
        kpidSolid = 13,
        kpidSplitAfter = 0x11,
        kpidSplitBefore = 0x10,
        kpidTimeType = 40,
        kpidTotalSize = 0x1100,
        kpidType = 20,
        kpidUnpackVer = 0x21,
        kpidUser = 0x19,
        kpidUserDefined = 0x10000,
        kpidVa = 0x30,
        kpidVolume = 0x22,
        kpidVolumeName = 0x1103
    }
}

