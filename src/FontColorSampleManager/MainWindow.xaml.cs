using FontColorExperiment.Models.Experiment;
using FontColorExperiment.Models.Question;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FontColorSampleManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<ExperimentData> m_data;
        ObservableCollection<ExperimentSample> m_samples;

        public MainWindow()
        {
            InitializeComponent();
            m_data = new ObservableCollection<ExperimentData>();
            m_lstbx_colors.ItemsSource = m_data;
            m_samples = new ObservableCollection<ExperimentSample>();
            m_lstbx_samples.ItemsSource = m_samples;
        }

        #region Button Events

        #region Color Data

        /// <summary>
        /// Adds a new item to the colour list.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_addcolor_Click(object sender, RoutedEventArgs e)
        {
            int cid = -1;
            foreach (ExperimentData sample in m_data)
                cid = Math.Max(cid, sample.Id); // get the next maximum id for the colours
            cid++;
            m_data.Add(new ExperimentData() // add new item
            {
                Id = cid
            });
        }

        /// <summary>
        /// Updates the current color with the sample data and colours.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_updcolor_Click(object sender, RoutedEventArgs e)
        {
            if (!FullExperimentData())
                MessageBox.Show("Please make sure every control has a value.", "Missing data",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                ExperimentData data = CurrentExperimentData();
                data.Id = m_data[m_lstbx_colors.SelectedIndex].Id;
                m_data[m_lstbx_colors.SelectedIndex] = data;
            }
        }

        /// <summary>
        /// Removes the currently selected color from the list.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_dltcolor_Click(object sender, RoutedEventArgs e)
        {
            m_data.RemoveAt(m_lstbx_colors.SelectedIndex);
        }

        #endregion

        #region Experiment

        /// <summary>
        /// Adds a new sample to the current color sample list.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_addsample_Click(object sender, RoutedEventArgs e)
        {
            int cid = -1;
            foreach (ExperimentSample sample in m_samples)
                cid = Math.Max(cid, sample.Id);
            cid++;
            m_samples.Add(new ExperimentSample()
            {
                Id = cid,
                Name = "new sample"
            });
        }

        /// <summary>
        /// Updates the current sample with the selected text and questions.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_updsample_Click(object sender, RoutedEventArgs e)
        {
            if (!FullExperimentSample())
                MessageBox.Show("Please make sure every control has a value.", "Missing sample data",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                ExperimentSample sample = CurrentExperimentSample();
                sample.Id = m_samples[m_lstbx_samples.SelectedIndex].Id;
                m_samples[m_lstbx_samples.SelectedIndex] = sample;
            }
        }

        /// <summary>
        /// Removes the current sample from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Event_btn_dltsample_Click(object sender, RoutedEventArgs e)
        {
            m_samples.RemoveAt(m_lstbx_samples.SelectedIndex);
        }

        #endregion

        #region Files

        /// <summary>
        /// Loads all of the data from a save file.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_load_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog jsonfinder = new System.Windows.Forms.OpenFileDialog()
            {
                DefaultExt = "json",
                Filter = "JSON Files (*.json)|*.json",
                CheckFileExists = true,
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Multiselect = false
            };
            if (jsonfinder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FullExperiment experiment = JsonConvert.DeserializeObject<FullExperiment>(
                    System.IO.File.ReadAllText(jsonfinder.FileName));
                m_data.Clear();
                m_data.AddRange(experiment.Experiments.Values);
                if (experiment.FinalQuestions != null)
                    m_qeditor_final.Items.AddRange(experiment.FinalQuestions.Values);
                m_txt_mainPage.Text = experiment.MainPageText;
            }
        }

        /// <summary>
        /// Saves all of the data to a save file ready to be loaded into the server.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_save_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog jsonfinder = new System.Windows.Forms.SaveFileDialog()
            {
                DefaultExt = "json",
                Filter = "JSON Files (*.json)|*.json",
                InitialDirectory = System.IO.Directory.GetCurrentDirectory()
            };
            if (jsonfinder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Dictionary<int, ExperimentData> dataitems = new Dictionary<int, ExperimentData>();
                foreach (ExperimentData data in m_data)
                    dataitems.Add(data.Id, data);
                Dictionary<int, QuestionData> finalquestions = new Dictionary<int, QuestionData>();
                foreach (QuestionData question in m_qeditor_final.Items)
                    finalquestions.Add(question.Id, question);
                System.IO.File.WriteAllText(jsonfinder.FileName, JsonConvert.SerializeObject(
                    new FullExperiment() {
                        Experiments = dataitems,
                        FinalQuestions = finalquestions,
                        MainPageText = m_txt_mainPage.Text
                    }, Formatting.Indented));
            }
        }

        #endregion

        #endregion

        #region ListBox Events

        /// <summary>
        /// Enables or disables the editor buttons depending on whether
        /// there is a selection.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_lstbx_colors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_lstbx_colors.SelectedIndex >= 0)
            {
                m_btn_updcolor.IsEnabled = true;
                m_btn_dltcolor.IsEnabled = true;
                ApplyExperimentData(m_data[m_lstbx_colors.SelectedIndex]);
                m_btn_addsample.IsEnabled = true;
            }
            else
            {
                m_btn_updcolor.IsEnabled = false;
                m_btn_dltcolor.IsEnabled = false;
                ApplyExperimentData(new ExperimentData());
                m_btn_addsample.IsEnabled = false;
            }
        }

        /// <summary>
        /// Enables or disables the editor button depending on whether
        /// there is a selection.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_lstbx_samples_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_lstbx_samples.SelectedIndex >= 0)
            {
                m_btn_updsample.IsEnabled = true;
                m_btn_dltsample.IsEnabled = true;
                ApplyExperimentSample(m_samples[m_lstbx_samples.SelectedIndex]);
                m_qeditor_sample.CanAddItems = true;
            }
            else
            {
                m_btn_updsample.IsEnabled = false;
                m_btn_dltsample.IsEnabled = false;
                ApplyExperimentSample(new ExperimentSample());
                m_qeditor_sample.CanAddItems = false;
            }
        }

        #endregion

        #region Control Handling

        #region Color Data

        /// <summary>
        /// Sets the controls to the data stored in the given object.
        /// </summary>
        /// <param name="data">The data to set.</param>
        private void ApplyExperimentData(ExperimentData data)
        {
            m_clr_fore.SelectedColor = data.Foreground;
            m_clr_back.SelectedColor = data.Background;
            m_samples.Clear();
            if (data.Samples != null)
                m_samples.AddRange(data.Samples.Values);
        }

        /// <summary>
        /// Whether all of the relevant controls have values set.
        /// </summary>
        /// <returns>Whether all of the controls have a value.</returns>
        private bool FullExperimentData()
        {
            return m_clr_fore.SelectedColor.HasValue &
                m_clr_back.SelectedColor.HasValue &
                m_samples.Count > 0;
        }

        /// <summary>
        /// Returns a new data object from the current data in the controls.
        /// </summary>
        /// <returns>The new object containing the user set data.</returns>
        private ExperimentData CurrentExperimentData()
        {
            Dictionary<int, ExperimentSample> sdict = new Dictionary<int, ExperimentSample>();
            foreach (ExperimentSample data in m_samples)
                sdict.Add(data.Id, data);
            Dictionary<int, QuestionData> qdict = new Dictionary<int, QuestionData>();
            foreach (QuestionData data in m_qeditor_final.Items)
                qdict.Add(data.Id, data);
            return new ExperimentData()
            {
                Foreground = m_clr_fore.SelectedColor.Value,
                Background = m_clr_back.SelectedColor.Value,
                Samples = sdict
            };
        }

        #endregion

        #region Samples

        /// <summary>
        /// Sets the controls to the data stored in the given object.
        /// </summary>
        /// <param name="data">The data to set.</param>
        private void ApplyExperimentSample(ExperimentSample sample)
        {
            m_txt_name.Text = sample.Name ?? "";
            m_txt_sample.Text = sample.SampleText ?? "";
            m_qeditor_sample.Items.Clear();
            if (sample.Questions != null)
                m_qeditor_sample.Items.AddRange(sample.Questions.Values);
        }

        /// <summary>
        /// Whether all of the relevant controls have values set.
        /// </summary>
        /// <returns>Whether all of the controls have a value.</returns>
        private bool FullExperimentSample()
        {
            return m_txt_name.Text != "" &
                m_txt_sample.Text != "" &
                m_qeditor_sample.Items.Count > 0;
        }

        /// <summary>
        /// Returns a new data object from the current data in the controls.
        /// </summary>
        /// <returns>The new object containing the user set data.</returns>
        private ExperimentSample CurrentExperimentSample()
        {
            Dictionary<int, QuestionData> qdict = new Dictionary<int, QuestionData>();
            foreach (QuestionData data in m_qeditor_sample.Items)
                qdict.Add(data.Id, data);
            return new ExperimentSample()
            {
                Name = m_txt_name.Text,
                SampleText = m_txt_sample.Text,
                Questions = qdict
            };
        }

        #endregion

        #endregion

    }

    /// <summary>
    /// A class adding an extension method that replaces the AddRange function from lists.
    /// </summary>
    public static class ExtMethods
    {
        public static void AddRange<T>(this ObservableCollection<T> first, IEnumerable<T> second)
        {
            foreach (T item in second)
                first.Add(item);
        }
    }
}
