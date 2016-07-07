using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ranta.Gaea.Controls
{
    /// <summary>
    /// FolderPathTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class FolderPathTextBox : System.Windows.Controls.UserControl
    {
        public FolderPathTextBox()
        {
            InitializeComponent();
        }

        private Random random = new Random();

        #region Properties
        public static readonly DependencyProperty WordsProperty = DependencyProperty.Register("Words", typeof(string[]), typeof(FolderPathTextBox));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FolderPathTextBox));

        protected string[] Words
        {
            get { return (string[])GetValue(WordsProperty); }
            set { SetValue(WordsProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Directory.Exists(this.Text) && this.Text.EndsWith("\\"))
                {
                    this.Words = Directory.GetDirectories(this.Text);

                    if (this.Words != null && this.Words.Length > 0)
                    {
                        SuggestItemsListView.SelectedIndex = 0;

                        CompletePopup.IsOpen = true;
                    }
                    else
                    {
                        CompletePopup.IsOpen = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void InputTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                if (CompletePopup.IsOpen)
                {
                    SuggestItemsListView.Focus();
                    if (SuggestItemsListView.Items.Count > 1)
                    {
                        SuggestItemsListView.SelectedIndex = 1;
                    }
                }
            }
            else if (e.Key == Key.Enter)
            {
                CompletePopup.IsOpen = false;
            }
        }


        private void SuggestItemsListView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (listView != null)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        InputTextBox.Text = listView.SelectedItem as string;
                        CompletePopup.IsOpen = false;
                        InputTextBox.CaretIndex = InputTextBox.Text.Length;
                        InputTextBox.Focus();

                        break;
                    case Key.Up:
                        if (listView.SelectedIndex == 0)
                        {
                            InputTextBox.Focus();
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        private void SuggestItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BrowseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BrowseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select a folder to place the output files.";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                InputTextBox.Text = dialog.SelectedPath;
            }
        }
    }
}
