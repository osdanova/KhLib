namespace KhLib
{
    public class CommonUtils
    {
        // Pads the given stream with 0s until the given padTo.
        // Eg: Pad stream with 11 bytes to 16 bytes length => adds 5 bytes
        public static void PadStreamToBytes(Stream stream, int padTo)
        {
            byte excess = (byte)(stream.Position % padTo);
            if (excess > 0)
            {
                for (int j = 0; j < padTo - excess; j++)
                {
                    stream.WriteByte(0);
                }
            }
        }

        // Moves the stream's position to align to a multiple of the given number
        public static void AlignStreamPositionToBytes(Stream stream, int padTo)
        {
            byte excess = (byte)(stream.Position % padTo);
            if (excess > 0)
            {
                stream.Position += (padTo - excess);
            }
        }

        // Returns how many bytes are left from the current byte count to align to a multiple of the given number
        public static int GetHowManyBytesToAlignTo(int currentByteCount, int alignTo)
        {
            int byteCountNeeded = 0;

            int excess = currentByteCount % alignTo;
            if (excess > 0)
            {
                byteCountNeeded  = alignTo - excess;
            }

            return byteCountNeeded;
        }
    }
}
