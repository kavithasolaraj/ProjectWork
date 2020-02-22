using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using OCRReader;
using System.Threading.Tasks;
using log4net;
using System.Xml.Linq;

namespace ImageReader
{
    public partial class ImageReader : Form
    {
        public readonly List<string> ImageExtensions = new List<string> { ".JPG", ".TIFF", ".BMP", ".GIF", ".PNG" };
        public readonly List<string> PdfExtensions = new List<string> { ".PDF" };
        public OCRReaderEngine ocr = new OCRReaderEngine();        
        public string result = null;
        public static readonly ILog log = LogManager.GetLogger(typeof(ImageReader));
        public Dictionary<string, string> langDict = new Dictionary<string, string>();
        public ImageReader()
        {
            InitializeComponent();
            LanguageDict();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFile.FileName;
                log.Info(string.Format("Processing FileName:{0} started", fileName));
                log.Info(string.Format("File Language is:{0}", SrcLanguage));
                bool isIamgeFile = ValidateImageFile(fileName);
                bool isPdf = ValidatePdfFile(fileName);
                try
                {
                    if (isIamgeFile)
                    {
                        result = Task.Run(async () => await ocr.ImageReaderAsync(fileName, langDict[SrcLanguage] , true)).Result;
                    }
                    else if (isPdf)
                    {
                        result = Task.Run(async () => await ocr.PDFReader(fileName, langDict[SrcLanguage])).Result;
                    }
                    else
                    {
                        MessageBox.Show("Pls upload a valid image or Pdf file.", "InvalidFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                    printBox.Text = result.ToString();
                    log.Info(string.Format("Processing FileName:{0} completed", fileName));                    
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Processing FileName:{0} failed : {1} ", fileName, ex.Message);
                    printBox.Text = msg;
                }
            }
        }

        private void LanguageDict()
        {
            langDict.Add("English", "en-US");
            langDict.Add("Spanish", "es-ES");
        }

        public bool ValidateImageFile(string fileName)
        {
            if (ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant()))
            {
                return true;
            }
            return false;
        }

        public bool ValidatePdfFile(string fileName)
        {
            if (PdfExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant()))
            {
                return true;
            }
            return false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void srcLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            SrcLanguage = srcLang.SelectedItem.ToString();            
        }

        public string SrcLanguage { get; set; }        
    }
}
