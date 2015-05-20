﻿using System.Drawing;
using System.IO;
using APP.Model;

namespace APP.Helpers.FileHandling
{
    public interface IContourSaver
    {
        void SaveContour(string path, Bitmap bitmap);
    }

    internal class ContourSaver : IContourSaver
    {
        private readonly ITxtSaver _txtSaver;
        private readonly IBitmapSaver _bitmapSaver;
        private readonly IBitmapHandler _bitmapHandler;


        public ContourSaver(IBitmapSaver bitmapSaver, ITxtSaver txtSaver, IBitmapHandler bitmapHandler)
        {
            _bitmapSaver = bitmapSaver;
            _txtSaver = txtSaver;
            _bitmapHandler = bitmapHandler;
        }


        public void SaveContour(string path, Bitmap bitmap)
        {
            if (Path.GetExtension(path).Contains("txt"))
            {
                TextWriter writer = new StreamWriter(path);
                Contour tmp = _bitmapHandler.LoadBitmap(bitmap);
                _txtSaver.SaveTxt(tmp, writer);
            }
            else
            {
                _bitmapSaver.SaveBitmap(bitmap, path);
            }
        }
    }
}