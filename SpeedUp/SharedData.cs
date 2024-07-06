using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedUp
{

    internal class ServerInfo
    {
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string ServerUser { get; set; }
        public string ServerPassword { get; set; }
        public string ServerPath { get; set; }
    }

    internal class SharedData : INotifyPropertyChanged
    {
        public List<string> FilePaths { get; set; }

        public void AddFilePathsItem(string path)
        {
            FilePaths.Add(path);
            OnPropertyChanged(nameof(FilePaths));
        }

        public void RemoveFilePathsItem(string path)
        {
            FilePaths.Remove(path);
            OnPropertyChanged(nameof(FilePaths));
        }

        public string RemoveFilePathsItem(int index)
        {
            string path = FilePaths[index];
            FilePaths.RemoveAt(index);
            OnPropertyChanged(nameof(FilePaths));
            return path;
        }


        public List<string> strings { get; set; }
        public string SharedData1
        {
            get { return SharedData1; }
            set
            {
                SharedData1 = value;
                OnPropertyChanged(nameof(SharedData1));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
