﻿using System.Drawing;

namespace APP.Model
{
    /// <summary>
    /// Klasa modelująca punkty konturów
    /// </summary>
    public class ContourPoint
    {
        //!gatunek pyłku
        public Pollen Type { get; set; } 

        //!współrzędne punktu
        public Point Location { get; set; }

        protected bool Equals(ContourPoint other)
        {
            return Location.Equals(other.Location) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
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