using System;
using Windows.Media.Ocr;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Globalization;

namespace OCRReader
{
    public class OCRReaderEngine
    {
        public OCRReaderEngine()
        {

        }
        
        public async Task<string> PDFReader(string fileName,string language)
        {
            var file = await StorageFile.GetFileFromPathAsync(fileName);
            PdfDocument document = await PdfDocument.LoadFromFileAsync(file);
            uint pageCount = document.PageCount;
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
                    result = result + ocrResult.Text;
                }
            }
            return (result);
        }
        public async Task<string> ImageReaderAsync(string fileName, string language)
        {
            //var check = OcrEngine.IsLanguageSupported(new Language("zh-Hans"));
            var check = OcrEngine.AvailableRecognizerLanguages.Count;
            var engine = OcrEngine.TryCreateFromLanguage(new Language(language));
            var file = await StorageFile.GetFileFromPathAsync(fileName);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            var ocrResult = await engine.RecognizeAsync(softwareBitmap);
            return (ocrResult.Text);            
        }
    }
}
