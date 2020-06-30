using Microsoft.Win32;
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
            textBox2.IsReadOnly = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CUE文件(*.cue)|*.cue";
            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
                cue.ReadFile(FileName);
                newTitles.Clear();
                foreach (var c in cue.Items)
                {
                    newTitles.Add(c.Title);
                }
                if (textBox1 == null) return;
                string fstring = "";
                foreach (string line in newTitles)
                {
                    fstring = fstring + line + "\n";
                }
                textBox1.Text = fstring;
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (cue.lines.Count == 0)
            {
                MessageBox.Show("原文件数据错误！");
                return;
            }
            //string line;
            string l;
            int i = 0;
            int length;
            SaveFileDialog writeDialog = new SaveFileDialog();
            writeDialog.Filter = "CUE文件(*.cue)|*.cue";
            if (writeDialog.ShowDialog() == true)
            {
                if (System.IO.File.Exists(writeDialog.FileName))
                {
                    System.IO.File.Delete(writeDialog.FileName);
                }
                FileStream aFile = new FileStream(writeDialog.FileName, FileMode.CreateNew);
                StreamWriter file = new StreamWriter(aFile);
                //System.IO.StreamWriter file = new System.IO.StreamWriter(writeDialog.FileName,false);
                //StreamReader sr = new StreamReader(FileName, Encoding.Default);
                foreach (string line in cue.lines)
                {
                    l = line.Trim();
                    if (l.Substring(0, 5).ToUpper() == "TITLE")
                    {
                        if (i > 0)
                        {
                            length = line.Length - l.Length;
                            file.WriteLine(line.Substring(0, length + 6) + '\"' + newTitles[i - 1] + '\"');
                        }
                        else
                            file.WriteLine(line);
                        i++;
                    }
                    else
                    {
                        file.WriteLine(line);
                    }

                }
                file.Close();
                MessageBox.Show("完成！");
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
