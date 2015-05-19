using APP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Helpers.FileHandling
{
    public interface IContourSaver
    {
        void SaveContour(string path, Bitmap bitmap);
    }

    internal class ContourSaver : IContourSaver
    {
        private ITxtSaver _txtSaver;
        private IBitmapSaver _bitmapSaver;
        private IBitmapHandler _bitmapHandler;


        public ContourSaver(IBitmapSaver bitmapSaver, ITxtSaver txtSaver, IBitmapHandler bitmapHandler)
        {
            _bitmapSaver = bitmapSaver;
            _txtSaver = txtSaver;
            _bitmapHandler = bitmapHandler;
        }


        public void SaveContour(string path, Bitmap bitmap)
        {
            Contour WynikContour;

            if (path.EndsWith(".txt ")) //sciezka jaka.  tu ze spacja raczej ok.
            {
                TextWriter writer = new StreamWriter(path);
                Contour tmp = _bitmapHandler.LoadBitmap(bitmap);
                _txtSaver.SaveTxt(tmp, writer);
                //Contour tmpasa = new Contour(
                //StreamReader reader = new StreamReader(path);
                //WynikContour = _txtSaver.SaveTxt(WynikContour,  );
            }
            else
            {
                _bitmapSaver.SaveBitmap(bitmap, path);
            }
        }
    }
}