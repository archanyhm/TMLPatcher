﻿namespace TML.Files.Generic.Files
{
    public readonly struct FileLengthData
    {
        public readonly int length;
        public readonly int lengthCompressed;

        public FileLengthData(int length, int lengthCompressed)
        {
            this.length = length;
            this.lengthCompressed = lengthCompressed;
        }
    }
}
