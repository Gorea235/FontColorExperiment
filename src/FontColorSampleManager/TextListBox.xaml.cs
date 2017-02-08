using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FontColorSampleManager
{
    /// <summary>
    /// Interaction logic for TextListBox.xaml
    /// </summary>
    public partial class TextListBox : UserControl
    {
        ObservableCollection<string> m_items;
        /// <summary>
        /// The items in the text box list.
        /// </summary>
        public ObservableCollection<string> Items
        {
            get
            {
                return m_items;
            }
            set
            {
                m_items.Clear();
                m_items.AddRange(value);
            }
        }

        public TextListBox()
        {
            InitializeComponent();
            m_items = new ObservableCollection<string>();
            m_lstbx_main.ItemsSource = m_items;
        }

        /// <summary>
        /// Enables or disables the editor buttons depending on whether
        /// there is a selection or not.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_lstbx_main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_btn_rmv.IsEnabled = m_lstbx_main.SelectedIndex >= 0;
        }

        /// <summary>
        /// Enables or disables the editor buttons depending on whether
        /// there is any text or not.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_txt_inp_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_btn_add.IsEnabled = m_txt_inp.Text.Length > 0;
        }

        /// <summary>
        /// Adds the current text in the textbox to the list, and then
        /// clears the textbox.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_add_Click(object sender, RoutedEventArgs e)
        {
            m_items.Add(m_txt_inp.Text);
            m_txt_inp.Text = "";
        }

        /// <summary>
        /// Removes the current item in the list.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Event_btn_rmv_Click(object sender, RoutedEventArgs e)
        {
            m_items.RemoveAt(m_lstbx_main.SelectedIndex);
        }
    }
}
