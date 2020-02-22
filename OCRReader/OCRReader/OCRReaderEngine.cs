using System;
using Windows.Media.Ocr;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Globalization;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace OCRReader
{
    public class OCRReaderEngine
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(OCRReaderEngine));
        public OCRReaderEngine()
        {

        }

        public async Task<string> PDFReader(string fileName, string language)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(fileName);
                PdfDocument document = await PdfDocument.LoadFromFileAsync(file);
                uint pageCount = document.PageCount;
                log.Info(string.Format("Number of Pages in pdf are : {0}", pageCount));
                string result = null;
                for (int i = 0; i < pageCount; i++)
                {
                    uint pageIndex = Convert.ToUInt16(i);
                    using (PdfPage page = document.GetPage(pageIndex))
                    {
                        var stream = new InMemoryRandomAccessStream();
                        await page.RenderToStreamAsync(stream);
                        var decoder = await BitmapDecoder.CreateAsync(stream);
                        var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                        var engine = OcrEngine.TryCreateFromLanguage(new Language(language));
                        var ocrResult = await engine.RecognizeAsync(softwareBitmap);
                        log.Info("Pdf Processed Successfully");
                        result = result + ocrResult.Text;
                    }
                }
                return (result);
            }
            catch(Exception e)
            {
                log.Error(string.Format("Error while parsing pdf: {0}", e.Message));
                return null;
            }
        }

        public async Task<string> ImageReaderAsync(string fileName, string language, bool parseText)
        {
            try
            {
                //var check = OcrEngine.IsLanguageSupported(new Language("zh-Hans"));
                var check = OcrEngine.AvailableRecognizerLanguages.Count;
                var engine = OcrEngine.TryCreateFromLanguage(new Language(language));
                var file = await StorageFile.GetFileFromPathAsync(fileName);
                var stream = await file.OpenAsync(FileAccessMode.Read);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                var ocrResult = await engine.RecognizeAsync(softwareBitmap);
                if (parseText)
                {
                    var result = ParseText(ocrResult);
                    log.Info("Image Processed Successfully");
                    return result;
                }
                log.Info("Image Processed Successfully");
                return (ocrResult.Text);
            }
            catch(Exception e)
            {
                log.Error(string.Format("Error while parsing image: {0}", e.Message));
                return null;
            }
        }
        
        public static string ParseText(OcrResult ocrResult)
        {
            var lines = ocrResult.Lines;            
            bool headerChk = false;
            int cnt = 0;
            string header = null;
            string csvtext = null;
            var prevLsize = 0.0;
            bool fline = true;
            List<double> sizeArray = new List<double>();            
            bool headerfirst = false;
            //To find the header size 
            //as the header is always bigger than all the contents
            log.Info("Parsing the contents");
            foreach (var line in lines)
            {             
                var fSize = 0.0;               
                foreach (var word in line.Words)
                {                    
                    fSize = word.BoundingRect.Height;
                    sizeArray.Add(fSize);                    
                }
                
            }
            sizeArray.Sort();
            var maxSize = sizeArray.Last();
            log.Debug(string.Format("Header Size : {0}", maxSize));
            //Parse the lines
            //Header is present anywhere in the content
            foreach (var line in lines)
            {
                var fSize = 0.0;                
                var prevwordSize = 0.0;
                foreach (var word in line.Words)
                {                    
                    fSize = word.BoundingRect.Height;
                    if (fSize == maxSize && fline)
                    {
                       headerChk = true;
                       headerfirst = true; // sets id header is in the beginning of the content.
                       log.Debug("Header Fisrt Flag is set");
                    }
                    if (fSize == maxSize)
                    {
                        headerChk = true;
                        log.Debug("Header is present in between the content");
                    }
                    if (prevwordSize > fSize)
                    {
                        fSize = prevwordSize;
                    }
                    else 
                    {
                        prevwordSize = fSize;
                    }
                }
                if (prevLsize == 0 && !headerfirst)
                {
                    header = line.Text + "||";
                    fline = true;
                }
                if (headerChk == true)
                {
                    if (cnt == 0)
                    {
                        header = header + "Name||";
                        var tmp = line.Text;
                        csvtext = headerfirst ? (csvtext + tmp + "||") : (csvtext + "||" + tmp + "||")  ;
                        cnt++;
                        continue;
                    }
                    if (cnt == 1)
                    {
                        header = header + "HeaderDetails||";
                        cnt++;
                        if(headerfirst)
                        {
                            prevLsize = fSize; // Header is in teh beginning preserve the size of content.
                        }
                    }
                }
                if (headerfirst)
                {
                    if (fSize <= prevLsize)
                    {
                        csvtext = csvtext + line.Text;
                    }
                    else
                    {
                        headerfirst = false; //turned off as header content parsing is done.            
                        log.Debug("Header Fisrt Flag is off");
                        log.Debug("Header Processing is Done");
                    }
                }
                if (!headerfirst && prevLsize != 0 && ((fSize >= prevLsize) || (fSize < prevLsize && (Math.Abs(fSize - prevLsize)) <= 2)))
                {
                    headerChk = false;
                    headerfirst = false;
                    csvtext = csvtext + "||";
                    header = header + line.Text + "||";
                    prevLsize = fSize;
                }
                else if(prevLsize != 0 && fSize < prevLsize)
                {
                    csvtext = csvtext + line.Text;                    
                }                
                if(fline)
                {
                    prevLsize = fSize;
                    fline = false;
                }
            }
            return (header + Environment.NewLine + csvtext);
        }        
    }
}

