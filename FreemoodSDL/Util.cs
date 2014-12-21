using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreemooSDL
{
    public class Util
    {
        public static T[] slice<T>(T[] src, int startindex, int size)
        {
            T[] dest = new T[size];
            Array.Copy(src, startindex, dest, 0, size);
            //List<int> t = new List<int>(); // what in the world was this??
            return dest;
        }

        public bool MemoryCmp(byte[] left, byte[] right)
        {
            // half assed implementation of memcmp, done the slow way for 
            // now because not really in an optimizing mood
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }
            // ...and apparently I never actually wrote this function
            return false;
        }

        public static int buildInt(int low, int high)
        {
            return low + (high << 8);
        }

        public static int buildInt(int a, int b, int c, int d)
        {
            return buildInt(a, b) + (buildInt(c, d) << 16);
        }

        private static int mFleetId = 0;
        public static int getNextFleetId()
        {
            // cause I'm an oracle guy and it's just easier for me to do it this way
            return mFleetId++;
        }

        public static string GetZString(byte[] bytes)
        {
            int idxZero = -1;
            for (int i = 0; i < bytes.Length && idxZero < 0; i++) if (bytes[i] == 0x00) idxZero = i;
            return Encoding.ASCII.GetString(bytes, 0, idxZero);
        }
    }
}
