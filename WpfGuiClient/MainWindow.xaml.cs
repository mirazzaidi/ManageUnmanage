using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Text;
using System.IO;

namespace XRYGuiClient
{
    // Managed FileInfo struct
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FileInfo
    {
        // Properties, not fields to serve data binding
        public string FileName { get; private set; }
        public string Content { get; private set; }
        public int WordCount { get; private set; }

        public FileInfo(string name, string content, int wordCount)
        {
            FileName = name;
            Content = content;
            WordCount = wordCount;
        }
    }

    // Callback for the GUI update event
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void GuiUpdateDelegate(int recordCount);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, INotifyPropertyChanged
    {
        // DLL methods
        [DllImport(@"Core.dll")]
        private static extern IntPtr CreateCore([MarshalAs(UnmanagedType.FunctionPtr)] GuiUpdateDelegate guiDelegate);

        [DllImport(@"Core.dll")]
        private static extern void DestroyCore(IntPtr instance);

        // Signals the Unmanaged DLL to load file data
        [DllImport(@"Core.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void LoadData(IntPtr instance, string s);

        // Queries file name
        [DllImport(@"Core.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetFilename(IntPtr instance, int recordNum, StringBuilder str);

        // Queries file content
        [DllImport(@"Core.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetFileContent(IntPtr instance, string key, StringBuilder  str, out int count);


        // Queries file word count
        [DllImport(@"Core.dll")]
        private static extern int GetWordCount(IntPtr instance, string key);

        public event PropertyChangedEventHandler PropertyChanged;


        private string fileContent;
        public string FileContent
        {
            get { return fileContent; }
            set
            {
                fileContent = value;
                OnPropertyChanged("FileContent");
            }
        }

        private string contentWordCount;
        public string ContentWordCount
        {
            get { return contentWordCount; }
            set
            {
                contentWordCount = "Word count: " + value;
                OnPropertyChanged("ContentWordCount");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private FileInfo[] fileInfoList;
        public void GetDataAndGuiUpdate(int recordCount)
        {
            if (fileInfoList == null || fileInfoList.Length != recordCount)
            {
                fileInfoList = new FileInfo[recordCount];
            }
            for (int i = 0; i < recordCount; i++)
            {
                StringBuilder kbuff = new StringBuilder(255);
                GetFilename(_this, i, kbuff);
                fileInfoList[i] = new FileInfo(kbuff.ToString(), "", 0);
            }
            FileList.ItemsSource = fileInfoList;

        }
        private GuiUpdateDelegate guiUpdateDelegate;
        private IntPtr _this = IntPtr.Zero;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            FolderPath.Text = "Enter folder path here";

            guiUpdateDelegate = new GuiUpdateDelegate(GetDataAndGuiUpdate);
            _this = CreateCore(guiUpdateDelegate);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileContent = String.Empty;

            string folderPath = FolderPath.Text;

            if (folderPath.Length > 0 && Directory.Exists(folderPath))
            {
                // Call DLL method to load data.
                LoadData(_this, folderPath);
            }
            else
            {
                MessageBox.Show("Please enter a valid folder path.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_this != IntPtr.Zero)
            {
                // delete the DLL object
                DestroyCore(_this);
                _this = IntPtr.Zero;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }
    }
}
