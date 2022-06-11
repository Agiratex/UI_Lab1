using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ClassLibrary
{
    public class VMBenchmark : INotifyPropertyChanged
    {
        // external function using MKL
        [DllImport("..\\..\\..\\..\\..\\x64\\Debug\\DynamicLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int calculate_d_function(int length, double[] vector, int function_code,
            double[] res_HA, double[] res_EP, double[] time);
        [DllImport("..\\..\\..\\..\\..\\x64\\Debug\\DynamicLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int calculate_f_function(int length, float[] vector, int function_code,
            float[] res_HA, float[] res_EP, double[] time);

        // Collections of time and accuracy information for functions on grids
        public ObservableCollection<VMTime> Time { get; set; }
        public ObservableCollection<VMAccuracy> Accuracy { get; set; }




        // Chosen elements in collection
        private VMTime chosen_time;
        public VMTime ChosenVMTime
        {
            get { return chosen_time; }
            set
            {
                chosen_time = value;
                OnPropertyChanged(nameof(ChosenVMTime));
            }
        }

        private VMAccuracy chosen_accuracy;
        public VMAccuracy ChosenAccuracy
        {
            get { return chosen_accuracy; }
            set
            {
                chosen_accuracy = value;
                OnPropertyChanged(nameof(ChosenAccuracy));
            }
        }



        //Return min and max time attention in benchmark
        public string MinMaxTimeAttention
        {
            get
            {
                if (Time.Count == 0) return "";
                double min_ep_ha = Time.Min(time => time.Att);
                double max_ep_ha = Time.Max(time => time.Att);
                return $"Min time attention: {min_ep_ha}, max time attention: {max_ep_ha}";
            }
        }


        public VMBenchmark()
        {
            Time = new ObservableCollection<VMTime>();
            Accuracy = new ObservableCollection<VMAccuracy>();
            Time.CollectionChanged += collection_changed;
        }

        //Add VMTime element to collection
        public void AddVMTime(VMGrid grid)
        {

            double[] time = new double[2];
            int status = 0;


            if (grid.F == VMf.vmdLGamma || grid.F == VMf.vmdLn)
            {
                // If function get double[]
                // Create double grid
                double[] vector = new double[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                {
                    vector[i] = grid.Ends.Item1 + i * grid.Step;
                }
                double[] res_HA = new double[grid.Length];
                double[] res_EP = new double[grid.Length];

                status = calculate_d_function(grid.Length, vector, (int)grid.F, res_HA, res_EP, time);
            } else
            {
                // If function get float[]
                // Create float grid
                float[] vector = new float[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                {
                    vector[i] = grid.Ends.Item1 + i * grid.Step;
                }
                float[] res_HA = new float[grid.Length];
                float[] res_EP = new float[grid.Length];

                status = calculate_f_function(grid.Length, vector, (int)grid.F, res_HA, res_EP, time);
            }

            if (status != 0)
            {
                throw new InvalidCastException($"MKL function exited with code: {status}");
            }
            
            //Create new VMTime elemet
            VMTime item = new VMTime
            {
                Grid = grid,
                Time_HA = time[0],
                Time_EP = time[1],
            };
            //Add to collection
            Time.Add(item);
            OnPropertyChanged(nameof(MinMaxTimeAttention));
        }


        public void AddVMAccuracy(VMGrid grid)
        {
            int status = 0;
            double[] time = new double[2];

            VMAccuracy item = new VMAccuracy();

            item.Grid = grid;
            item.Diff = 0;

            if (grid.F == VMf.vmdLGamma || grid.F == VMf.vmdLn)
            {
                //If function get double[]
                //Create double grid
                double[] vector = new double[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                {
                    vector[i] = grid.Ends.Item1 + i * grid.Step;
                }
                double[] res_HA = new double[grid.Length];
                double[] res_EP = new double[grid.Length];

                status = calculate_d_function(grid.Length, vector, (int)grid.F, res_HA, res_EP, time);
                if (status != 0)
                {
                    throw new InvalidCastException($"MKL function exited with code: {status}");
                }

                //Get results to VMAccuracy element
                for (int i = 0; i < grid.Length; ++i)
                { 
                    if (Math.Abs(res_HA[i] - res_EP[i]) > item.Diff)
                    {
                        item.Diff = Convert.ToSingle(Math.Abs(res_HA[i] - res_EP[i]));
                        item.Arg = Convert.ToSingle(vector[i]);
                        item.Values = (res_HA[i], res_EP[i]);
                    }
                }
            } else
            {
                //If function get float[]
                //Create float grid
                float[] vector = new float[grid.Length];
                for (int i = 0; i < grid.Length; i++)
                {
                    vector[i] = grid.Ends.Item1 + i * grid.Step;
                }
                float[] res_HA = new float[grid.Length];
                float[] res_EP = new float[grid.Length];
                status = calculate_f_function(grid.Length, vector, (int)grid.F, res_HA, res_EP, time);
                if (status != 0)
                {
                    throw new InvalidCastException($"MKL function exited with code: {status}");
                }

                //Set results to VMAccuracy element
                for (int i = 0; i < grid.Length; i++)
                {
                    if (Math.Abs(res_HA[i] - res_EP[i]) > item.Diff)
                    {
                        item.Diff = Math.Abs(res_HA[i] - res_EP[i]);
                        item.Arg = vector[i];
                        item.Values = (res_HA[i], res_EP[i]);
                    }
                }
            }
            //Add element
            Accuracy.Add(item);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Accuracy.Count; i++)
            {
                result += $"Accuracy item {i + 1}:\n";
                result += Accuracy[i].ToString();
            }
            for (int i = 0; i < Time.Count; i++)
            {
                result += $"Time item {i + 1}:\n";
                result += Time[i].ToString();
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        //Handler for recalculating min and max time attention when benchmark was changed
        private void collection_changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinMaxTimeAttention)));
        }
    }
}