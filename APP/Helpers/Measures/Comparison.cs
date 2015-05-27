using System;
using APP.Model;

namespace APP.Helpers.Measures
{
    public abstract class Comparison
    {
        public abstract Result GetResult(Contour a, Contour b);

        internal double _progres;
        public double Progres
        {
            get { return _progres; }
            internal set
            {
                _progres = value;
                ProgresChanged();
            }
        }

        public Action ProgresChanged { get; set; }

        public int Scale { get; protected set; }
    }
}