using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchiveUtil
{
    /// <summary>
    /// Slice of file data.
    /// </summary>
    public class FileSlice
    {
        public FileSlice(int index, byte[] data)
        {
            Index = index;
            Data = data;
        }

        /// <summary>
        /// Serial number of slice of file in queue.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Byte array of slice of file.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
