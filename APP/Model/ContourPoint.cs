using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace APP.Model
{
    public class ContourPoint
    {
        public Pollen Type { get; set; }  // Typ Pyłku 

        public Point Location { get; set; }

        protected bool Equals(ContourPoint other)
        {
            return Location.Equals(other.Location) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ContourPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Location.GetHashCode()*397) ^ Type;
            }
        }
    }
}
