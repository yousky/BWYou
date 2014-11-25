﻿using System;
using System.IO;

namespace BWYou.Cloud.Storage
{
    /// <summary>
    /// Cloud Storage manipulation
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Upload To Storage
        /// </summary>
        /// <param name="inputStream">inputStream</param>
        /// <param name="sourcefilename">Source File Name</param>
        /// <param name="containerName">container Name</param>
        /// <param name="destpath">Storage Destination path</param>
        /// <param name="useUUIDName">true : UUIDName use, false : sourcefilename use</param>
        /// <param name="overwrite">overwrite true, false</param>
        /// <param name="useSequencedName">overwrite false and same file exist then filename[1], filename[2], ... use</param>
        /// <returns>Saved File's Storage Uri</returns>
        string Upload(Stream inputStream, string sourcefilename, string containerName, string destpath = "", bool useUUIDName = true, bool overwrite = false, bool useSequencedName = true);

        /// <summary>
        /// Upload To Storage
        /// </summary>
        /// <param name="sourcefilepathname">upload source File Full Path</param>
        /// <param name="containerName">container Name</param>
        /// <param name="destpath">Storage Destination path</param>
        /// <param name="useUUIDName">true : UUIDName use, false : sourcefilename use</param>
        /// <param name="overwrite">overwrite true, false</param>
        /// <param name="useSequencedName">overwrite false and same file exist then filename[1], filename[2], ... use</param>
        /// <returns></returns>
        string Upload(string sourcefilepathname, string containerName, string destpath = "", bool useUUIDName = true, bool overwrite = false, bool useSequencedName = true);

        bool Download(Uri sourceUri, string destfilename);
        bool Download(Uri sourceUri, Stream deststream);
    }
}
