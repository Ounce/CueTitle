using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CueTitle
{
    public class Track
    {
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 艺术家
        /// </summary>
        public string Performer { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }

    public class Cue
    {
        /// <summary>
        /// 光盘名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 光盘镜像文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 艺术家
        /// </summary>
        public string Performer { get; set; }

        public ObservableCollection<Track> Items;

        public Cue()
        {
            Items = new ObservableCollection<Track>();
        }

        public void ReadFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            string line;
            string l;
            bool track = false;
            Items.Clear();
            Track t = new Track();
            int i = 1;
            while ((line = sr.ReadLine()) != null)
            {
                l = line.Trim();
                if (track)
                {
                    if (l.Substring(0, 9).ToUpper() == "PERFORMER")
                        t.Performer = l.Substring(11, l.Length - 12).Trim();
                    if (l.Substring(0, 5).ToUpper() == "TITLE")
                    {
                        t.Title = l.Substring(7, l.Length - 8).Trim();
                        t.id = i++;
                        Items.Add(t);
                        t = new Track();
                    }
                    if (l.Substring(0, 5).ToUpper() == "INDEX")
                    {
                    }
                }
                else
                {
                    if (l.Substring(0, 5).ToUpper() == "TRACK")
                    {
                        track = true;
                        //                    t = new Track();
                        continue;
                    }
                    if (l.Substring(0, 9).ToUpper() == "PERFORMER")
                        Performer = l.Substring(11, l.Length - 12).Trim();
                    if (l.Substring(0, 5).ToUpper() == "TITLE")
                        Title = l.Substring(7, l.Length - 8).Trim();
                    if (l.Substring(0, 4).ToUpper() == "FILE")
                        this.FileName = l.Substring(6, l.Length - 12);
                }
            }
            sr.Close();
        }
    }
}
