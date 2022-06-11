using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClassLibrary;


namespace App
{
    public partial class MainWindow : Window
    {

        public ViewData Data { get; set; }
        //set default grid
        public VMGrid Grid { get; set; } = new VMGrid
        {
            Length = 3,
            Ends = (1f, 2f),
            F = VMf.vmdLn
        };

        public MainWindow()
        {
            InitializeComponent();
            Data = new ViewData();
            DataContext = this;
        }

        //Create new benchmark
        private void New(object sender, RoutedEventArgs args)
        {
            try
            {
                if (display_save_msg())
                {
                    Data.Benchmark.Time.Clear();
                    Data.Benchmark.Accuracy.Clear();
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Got an error when tried to create new object: {e.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Load benchmark from .txt file
        private void Open(object sender, RoutedEventArgs args)
        {
            try
            {
                if (display_save_msg())
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                    {
                        FileName = "",
                        DefaultExt = ".txt",
                        Filter = "Text documents (.txt)|*.txt"
                    };

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        string filename = dlg.FileName;
                        bool status = Data.Load(filename);
                        Data.VMBenchmarkChanged = false;
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while load data: {e.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Save benchmark to .txt file
        private void Save(object sender, RoutedEventArgs args)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "benchmark",
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt"
                };

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    bool status = Data.Save(filename);
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while saving: {e.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //add VMTime element to benchmark
        private void add_vmtime_cmd(object sender, RoutedEventArgs args)
        {
            try
            {
                Data.Benchmark.AddVMTime(Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Get error while adding VMTime element: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Add VMACCuracy element to benchmark
        private void add_vmaccuracy_cmd(object sender, RoutedEventArgs args)
        {
            try
            {
                Data.Benchmark.AddVMAccuracy(Grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Get error while adding VMAccuracy element: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //interaction with menu items
        private void click_menuitem(object sender, RoutedEventArgs args)
        {
            if (Data.Functions.SelectedFunc == null)
            {
                MessageBox.Show("Choose function");
                return;
            }

            MenuItem menu_item = (MenuItem)args.Source;
            try
            {
                Grid.F = Data.Functions.SelectedFunc.Function;
                switch (menu_item.Header.ToString())
                {
                    case "Add VMTime":
                        Data.Benchmark.AddVMTime(Grid);
                        break;
                    case "Add VMAccuracy":
                        Data.Benchmark.AddVMAccuracy(Grid);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Get error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        //Show message when create new load saved benchmark if current benckmark has not been saved
        public bool display_save_msg()
        {
            try
            {
                if (Data.VMBenchmarkChanged)
                {
                    MessageBoxResult choise = MessageBox.Show($"Save changes to file?", "task 4", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (choise)
                    {
                        case MessageBoxResult.Cancel:
                            return false;
                        case MessageBoxResult.Yes:
                            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                            {
                                FileName = "benchmark",
                                DefaultExt = ".txt",
                                Filter = "Text documents (.txt)|*.txt"

                            };
                            bool? result = dlg.ShowDialog();

                            if (result == true)
                            {
                                string filename = dlg.FileName;
                                bool status = Data.Save(filename);
                                return status;
                            }
                            else
                            {
                                return false;
                            }
                        case MessageBoxResult.No:
                            return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Get error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            if (display_save_msg())
                base.OnClosing(args);
        }


        //Checkers for changing text
        private void TextBoxLengthChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                Grid.Length = Convert.ToInt32(TextBoxLength.Text);
            }
            catch(Exception err)
            {
                MessageBox.Show($"Unexpected error: {err.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxLeftChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                Grid.Ends = (Convert.ToSingle(TextBoxLeft.Text), Grid.Ends.Item2);
            }
            catch (Exception err)
            {
                MessageBox.Show($"Unexpected error: {err.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxRightChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                Grid.Ends = (Grid.Ends.Item1, Convert.ToSingle(TextBoxRight.Text));
            }
            catch (Exception err)
            {
                MessageBox.Show($"Unexpected error: {err.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
