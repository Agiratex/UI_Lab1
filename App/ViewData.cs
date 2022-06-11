using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ClassLibrary;

namespace App
{
    public class ViewData : INotifyPropertyChanged
    {
        public VMBenchmark Benchmark { get; set; }
        private bool _VMBenchmarkChanged;
        public VMfConv Functions { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        //If benchmark was changed and not saved
        private void Collection_changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            VMBenchmarkChanged = true;
        }


        public ViewData()
        {
            Benchmark = new VMBenchmark();
            Benchmark.Time.CollectionChanged += Collection_changed;
            Benchmark.Accuracy.CollectionChanged += Collection_changed;
            Functions = new();
            VMBenchmarkChanged = false;
        }


        public bool VMBenchmarkChanged 
        { 
            get { return _VMBenchmarkChanged; }
            set
            {
                if (value != _VMBenchmarkChanged)
                {
                    _VMBenchmarkChanged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VMBenchmarkChanged)));
                }
            }    
        }

        

        //Add VMTime to benchmark
        public void AddVMTime(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMTime(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error occured: {error.Message}.", "Add error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }


        //Add VMAccuracy to benchmark
        public void AddVMAccuracy(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMAccuracy(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error occured: {error.Message}.", "Add error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        //Save benchmark to .txt file using stieamwriter
        public bool Save(string filename)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filename, false);

                try
                {
                    writer.WriteLine(Benchmark.Time.Count);
                    foreach (VMTime item in Benchmark.Time)
                    {
                        writer.WriteLine($"{item.Grid.Length}");
                        writer.WriteLine($"{item.Grid.Ends.Item1}");
                        writer.WriteLine($"{item.Grid.Ends.Item2}");
                        writer.WriteLine((int)item.Grid.F);
                        writer.WriteLine($"{item.Time_HA}");
                        writer.WriteLine($"{item.Time_EP}");
                    }
                    writer.WriteLine(Benchmark.Accuracy.Count);
                    foreach (VMAccuracy item in Benchmark.Accuracy)
                    {
                        writer.WriteLine($"{item.Grid.Length}");
                        writer.WriteLine($"{item.Grid.Ends.Item1}");
                        writer.WriteLine($"{item.Grid.Ends.Item2}");
                        writer.WriteLine((int)item.Grid.F);

                        writer.WriteLine($"{item.Diff}");
                        writer.WriteLine($"{item.Arg}");
                        writer.WriteLine($"{item.Values.Item1}");
                        writer.WriteLine($"{item.Values.Item2}");
                    }
                }
                catch (Exception e)
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();
                    MessageBox.Show($"Unable to save file: {e.Message}.", "Save error", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    writer.Close();
                    return false;
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception error)
            {
                Benchmark.Time.Clear();
                Benchmark.Accuracy.Clear();
                MessageBox.Show($"Unable to save file: {error.Message}.", "Save error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        //Load benchmark from .txt file using streamreader
        public bool Load(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);

                try
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();
                    int time_number = Convert.ToInt32(reader.ReadLine());
                    for (int i = 0; i < time_number; i++)
                    {
                        VMGrid grid = new VMGrid
                        {
                            Length = Convert.ToInt32(reader.ReadLine()),
                            Ends = (Convert.ToSingle(reader.ReadLine()), Convert.ToSingle(reader.ReadLine())),
                            F = (VMf)Convert.ToInt32(reader.ReadLine())
                        };

                        VMTime time = new VMTime
                        {
                            Grid = grid,
                            Time_HA = Convert.ToDouble(reader.ReadLine()),
                            Time_EP = Convert.ToDouble(reader.ReadLine()),
                        };

                        Benchmark.Time.Add(time);
                    }

                    int accur_number = Convert.ToInt32(reader.ReadLine());
                    for (int i = 0; i < accur_number; i++)
                    {
                        VMGrid grid = new VMGrid
                        {
                            Length = Convert.ToInt32(reader.ReadLine()),
                            Ends = (Convert.ToSingle(reader.ReadLine()), Convert.ToSingle(reader.ReadLine())),
                            F = (VMf)Convert.ToInt32(reader.ReadLine())
                        };

                        VMAccuracy accuracy = new VMAccuracy
                        {
                            Grid = grid,
                            Diff = Convert.ToSingle(reader.ReadLine()),
                            Arg = Convert.ToSingle(reader.ReadLine()),
                            Values = (Convert.ToDouble(reader.ReadLine()), Convert.ToDouble(reader.ReadLine()))
                        };

                        Benchmark.Accuracy.Add(accuracy);
                    }
                }
                catch (Exception e)
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();
                    MessageBox.Show($"Unable to load file: {e.Message}.", "Load error", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    reader.Close();
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            } 
            catch (Exception e)
            {
                Benchmark.Time.Clear();
                Benchmark.Accuracy.Clear();
                MessageBox.Show($"Unable to load file: {e.Message}.", "Load error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
