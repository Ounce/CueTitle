using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace CueTitle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FileName;
        private Cue cue;
        private List<string> newTitles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            cue = new Cue();
            listView.ItemsSource = cue.Items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CUE文件(*.cue)|*.cue";
            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
                cue.ReadFile(FileName);
            }
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] titles = textBox1.Text.Split("\n".ToCharArray());
   
            int n = 0;
            if (numberCheckBox == null) return;
            newTitles.Clear();
            foreach (var t in titles)
            {
                newTitles.Add(t);
                n++;
            }
            if (numberCheckBox.IsChecked == true)
                AddNumberHeader();
            else
                ClearNumberHeader();
            ShowNewTitles();
        }

        private void NumberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void NumberCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (numberCheckBox.IsChecked == true)
                AddNumberHeader();
            else
                ClearNumberHeader();
            ShowNewTitles();
        }

        private bool IsNumberHeader()
        {
            foreach (string t in newTitles)
            {
                char s = t.Trim()[0];
                if (s > '0' && s < '9') continue;
                return false;
            }
            return true;
        }

        private void ClearNumberHeader()
        {
            List<string> result = new List<string>();
            int k;
            for(int i = 0; i < newTitles.Count; i++)
            {
                string s = newTitles[i].Trim();
                k = 0;
                foreach (char c in s)
                {
                    if ((c < '0' || c > '9') && c != '.')
                    {
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                newTitles[i] = s.Substring(k, s.Length - k);
        }
        }

        private void AddNumberHeader()
        {
            if (IsNumberHeader()) return;
            for (int i = 0; i < newTitles.Count; i++)
            {
                newTitles[i] = (i + 1).ToString("d2") + "." + newTitles[i];
            }
        }

        private void ShowNewTitles()
        {
            if (textBox2 == null) return;
            string fstring = "";
            foreach (string line in newTitles)
            {
                fstring = fstring + line + "\n";
            }
            textBox2.Text = fstring;
        }
    }
}
