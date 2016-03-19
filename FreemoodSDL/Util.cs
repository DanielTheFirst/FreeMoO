﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO
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
            return mFleetId++;
        }

        public static string GetZString(byte[] bytes)
        {
            int idxZero = -1;
            for (int i = 0; i < bytes.Length && idxZero < 0; i++) if (bytes[i] == 0x00) idxZero = i;
            if (idxZero < 0)
            {
                return string.Empty;
            }
            return Encoding.ASCII.GetString(bytes, 0, idxZero);
        }

        public static int CountDigits(int num)
        {
            int i = Math.Abs(num);
            int count = 0;
            while (i > 0)
            {
                i /= 10;
                count++;
            }
            return count;
        }

        public static string Fmt(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
    }
}
