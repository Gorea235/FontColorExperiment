using FontColorExperiment.Models.Question;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FontColorSampleManager
{
    /// <summary>
    /// Interaction logic for QuestionsEditor.xaml
    /// </summary>
    public partial class QuestionsEditor : UserControl
    {
        private ObservableCollection<QuestionData> m_questions;
        /// <summary>
        /// The items in the question list.
        /// </summary>
        public ObservableCollection<QuestionData> Items
        {
            get { return m_questions; }
            set
            {
                m_questions.Clear();
                m_questions.AddRange(value);
            }
        }

        /// <summary>
        /// Whether the user can add new items to the list.
        /// </summary>
        public bool CanAddItems
        {
            set { m_btn_add_qu.IsEnabled = value; }
        }

        public QuestionsEditor()
        {
            InitializeComponent();
            m_questions = new ObservableCollection<QuestionData>();
            m_lstbx_questions.ItemsSource = m_questions;
            m_lstbx_questions.SelectedIndex = -1;
        }

        #region Button Events

        /// <summary>
        /// Adds a new items to the question list
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_add_qu_Click(object sender, RoutedEventArgs e)
        {
            int cid = -1;
            foreach (QuestionData question in m_questions)
                cid = Math.Max(cid, question.Id);
            cid++;
            m_questions.Add(new QuestionData()
            {
                Id = cid,
                QuestionString = "new question"
            });
        }

        /// <summary>
        /// Updates the current question.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_upd_qu_Click(object sender, RoutedEventArgs e)
        {
            if (!FullQuestion())
                MessageBox.Show("Please make sure that all the question data has something in", "Missing question data",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                QuestionData question = CurrentQuestion();
                question.Id = m_questions[m_lstbx_questions.SelectedIndex].Id;
                m_questions[m_lstbx_questions.SelectedIndex] = question;
            }
        }

        /// <summary>
        /// Removes the current question.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_dlt_qu_Click(object sender, RoutedEventArgs e)
        {
            m_questions.RemoveAt(m_lstbx_questions.SelectedIndex);
        }

        #endregion

        #region ListBox Events

        /// <summary>
        /// Enables or disables the editor buttons depending on whether
        /// there is a selection or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Event_lstbx_questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_lstbx_questions.SelectedIndex >= 0)
            {
                m_btn_upd_qu.IsEnabled = true;
                m_btn_dlt_qu.IsEnabled = true;
                ApplyQuestion(m_questions[m_lstbx_questions.SelectedIndex]);
            }
            else
            {
                m_btn_upd_qu.IsEnabled = false;
                m_btn_dlt_qu.IsEnabled = false;
                ApplyQuestion(new QuestionData());
            }
        }

        #endregion

        #region Control Handling

        /// <summary>
        /// Sets the controls to the data stored in the given object.
        /// </summary>
        /// <param name="data">The data to set.</param>
        private void ApplyQuestion(QuestionData data)
        {
            m_txt_question.Text = data.QuestionString ?? "";
            foreach (ComboBoxItem item in m_cbox_qtype.Items)
                if (((string)item.Tag) == data.Type.ToString())
                    m_cbox_qtype.SelectedItem = item;
            m_txtlst_answers.Items.Clear();
            if (data.QuestionItems != null)
                m_txtlst_answers.Items.AddRange(data.QuestionItems.Values);
            m_txtlst_questions.Items.Clear();
            if (data.QuestionStringSecondary != null)
                m_txtlst_questions.Items.AddRange(data.QuestionStringSecondary.Values);
        }

        /// <summary>
        /// Whether all of the relevant controls have values set.
        /// </summary>
        /// <returns>Whether all of the controls have a value.</returns>
        private bool FullQuestion()
        {
            return m_txt_question.Text != "" &
                m_cbox_qtype.SelectedIndex >= 0;
        }

        /// <summary>
        /// Returns a new data object from the current data in the controls.
        /// </summary>
        /// <returns>The new object containing the user set data.</returns>
        private QuestionData CurrentQuestion()
        {
            return new QuestionData()
            {
                QuestionString = m_txt_question.Text,
                Type = (QuestionType)Enum.Parse(typeof(QuestionType), (string)((ComboBoxItem)m_cbox_qtype.SelectedItem).Tag),
                QuestionItems = IndexDictionary(m_txtlst_answers.Items.ToList()),
                QuestionStringSecondary = IndexDictionary(m_txtlst_questions.Items.ToList())
            };
        }

        /// <summary>
        /// Converts a list of string into a dictionary of int, string using order.
        /// </summary>
        /// <param name="objs">The string list.</param>
        /// <returns>The dictionary of int, string.</returns>
        private Dictionary<int, string> IndexDictionary(List<string> objs)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < objs.Count; i++)
                dict.Add(i, objs[i]);
            return dict;
        }

        #endregion

    }
}
