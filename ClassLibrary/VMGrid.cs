//Class for Grid information
namespace ClassLibrary
{
    public class VMGrid
    {

        public int Length { get; set; }
        public (float, float) Ends { get; set; }
        public float Step
        {
            get { return (Ends.Item2 - Ends.Item1) / Length; }
        }
        public VMf F { get; set; }

        public override string ToString() => $"Grid:\nLength: {Length}, ends: {Ends}, " +$"step: {Step}, function: {F}\n";
    }
}
