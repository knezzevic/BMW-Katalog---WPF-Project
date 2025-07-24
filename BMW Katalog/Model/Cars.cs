using System.ComponentModel;

public class Cars : INotifyPropertyChanged
{
    private string name;
    private int realseDate;
    private string urlImg;
    private string urlRtf;
    private bool checkBox;
    public string Date {  get; set; }

    public Cars() { }

    public Cars(string name, int realseDate, string urlImg, string urlRtf)
    {
        Name = name;
        RealseDate = realseDate;
        UrlImg = urlImg;
        UrlRtf = urlRtf;
        Date = DateTime.Now.ToString("dd.MM.yyyy");
        checkBox = false;
    }

    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(nameof(Name)); }
    }

    public int RealseDate
    {
        get => realseDate;
        set { realseDate = value; OnPropertyChanged(nameof(RealseDate)); }
    }

    public string UrlImg
    {
        get => urlImg;
        set { urlImg = value; OnPropertyChanged(nameof(UrlImg)); }
    }

    public string UrlRtf
    {
        get => urlRtf;
        set { urlRtf = value; OnPropertyChanged(nameof(UrlRtf)); }
    }

    public bool CheckBox
    {
        get => checkBox;
        set { checkBox = value; OnPropertyChanged(nameof(CheckBox)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
