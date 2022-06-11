//Class for Time information. Content information about time of work in different modes
namespace ClassLibrary
{
    public struct VMTime
    {
        public VMGrid Grid { get; set; }
        public double Time_HA { get; set; } //calculation time in VML_HA mode
        public double Time_EP { get; set; } //calculation time in VML_EP mode
        public double Att //Time attention
        {
            get
            {
                return Time_EP / Time_HA;
            }
        }

        public override string ToString()
        {
            string g = Grid.ToString();
            string t = $"Time:\nTime HA: {Time_HA}, Time EP: {Time_EP}, " + 
                $"Time attention: {Att}\n";
            return g + t;
        }
    }
}
