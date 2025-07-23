using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BMW_Katalog.Model
{
    [XmlRoot("Cars")]
    public class Cars : INotifyPropertyChanged
    {
        public List<Cars> Car { get; set; } = new List<Cars>();
        public string Name {  get; set; }
        public int RealseDate {  get; set; }
        public string UrlImg {  get; set; }
        public string UrlRtf {  get; set; }
        public string Date { get; set; }

        private bool _checkBox;
        public bool CheckBox
        {
            get => _checkBox;
            set
            {
                if (_checkBox != value)
                {
                    _checkBox = value;
                    OnPropertyChanged(nameof(CheckBox));
                }
            }
        }

        public Cars() { }

        public Cars(string name, int realseDate, string urlImg, string urlRtf)
        {
            Name = name;
            RealseDate = realseDate;
            UrlImg = urlImg;
            UrlRtf = urlRtf;
            Date = DateTime.Now.ToString("dd.MM.yyyy");
            //CheckBox = checkBox;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
