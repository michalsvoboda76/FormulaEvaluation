namespace KiStudioCalculator
{
    public class DecodedScan
    {
        public DecodedScan()
        { }

        public DecodedScan(ulong epochTimestamp, float[] channelValues)
        {
            EpochTimestamp = epochTimestamp;
            ChannelValues = channelValues;
        }
        public ulong EpochTimestamp { get; set; }
        public float[] ChannelValues { get; set; }
    }
}