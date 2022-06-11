//Class for accuracy. Content information about working in different modes
namespace ClassLibrary
{
    public struct VMAccuracy
    {

        public VMGrid Grid { get; set; }
        public float Diff { get; set; } //Max difference between values in VML_HA and VML_EP modes
        public float Arg { get; set; } //The agrument on which the maximum value is reached
        public (double, double) Values { get; set; } //Values for Arg (VML_HA, VML_EP)


        public override string ToString()
        {
            string grid_output = Grid.ToString();
            string accuracy_output = $"Accuracy:\nMax Difference (HA and EP): {Diff}, " +
                $"Argument: {Arg}, Values (HA, EP): {Values}\n";
            return grid_output + accuracy_output;
        }
    }
}
