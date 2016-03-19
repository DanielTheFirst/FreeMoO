using System;
using System.Diagnostics;

namespace FreeMoO.Game
{
    public class Transport
        : SpaceObject
    {
        public int DestPlanetId { get; set; }
        public int OriginPlanetId { get; set; }
        public int PlayerId { get; set; }
        public int SizeInMillions { get; set; }
        public bool Launched { get; set; }

        // of the 18 bytes in a transport record, 8 are unknown to me at this time
        // wheras most are zeros because everything is stored in 16 bits regardless of size needed

        public Transport(int id)
            : base(id)
        {

        }
    }
}
