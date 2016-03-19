using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMoO
{
    public class XMidData
    {
        public int NumTracks { get; set; }
        //public byte[,] Tracks { get; set; }
        public List<byte[]> Tracks { get; set; }

        public XMidData()
        {
            Tracks = new List<byte[]>();
        }
    }

    public class FreemooMusicAdapter
    {
        public const int MAX_TRACKS = 120;

        public FreemooMusicAdapter()
        {
        }

        public byte[] Convert(byte[] xmidFile)
        {
            byte[] actualFile = stripMooData(xmidFile);
            XMidData xMidData = readXmidData(actualFile);


            return null;
        }

        private int convertMidiTrack(byte[] xmidFile, int size, byte[] midiFile)
        {
            if (midiFile != null)
            {
                midiFile[0] = (byte)'M';
                midiFile[1] = (byte)'T';
                midiFile[2] = (byte)'r';
                midiFile[3] = (byte)'k';
            }

            XMidData xMidData = readXmidData(xmidFile);
            if (null == xMidData)
            {
                // failed to read the xmid header
                return 0;
            }

            int dataPosIdx = 0;
            int trackIdx = 0;


            return 0;
        }

        // adapted from https://github.com/Risca/xmidi_player who adapted it from ScummVM
        // who adapted it from Exult who probably had someone very smart that wrote it...or not
        // great programers steal :)
        // of course, since the .XMI format seemed so popular in games during the early 90s, you'd think someone
        // somewhere would have come up with a plugin for SDL that can handle them rather than multiple
        // OSS projects that just convert to MIDI on the fly
        private XMidData readXmidData(byte[] xmidFile)
        {
            XMidData data = new XMidData();
            List<byte[]> tmpTracks = new List<byte[]>();
            int posIdx = 0;
            if (!readByteToString(xmidFile, posIdx, 4).Equals("FORM"))
            {
                // invalid, return null
                return null; // 
            }

            posIdx += 4;
            int len = read4ByteBigEnd(xmidFile, posIdx);
            posIdx += 4;

            if (readByteToString(xmidFile, posIdx, 4).Equals("XMID"))
            {
                // no xdir chunk (probably not going to happen but I can check all the midi files in Music.lbx)
                posIdx += 4;
                data.NumTracks = 1;
            }
            else if (!readByteToString(xmidFile, posIdx, 4).Equals("XDIR"))
            {
                // not a valid midi file, bombs away
                return null;
            }
            else
            {
                posIdx += 4;
                data.NumTracks = 0;
                for (int i = 4; i < len; i++)
                {
                    string chunkId = readByteToString(xmidFile, posIdx, 4);
                    posIdx += 4;
                    int chunkLen = xmidFile[posIdx++] << 24;
                    chunkLen += xmidFile[posIdx++] << 16;
                    chunkLen += xmidFile[posIdx++] << 8;
                    chunkLen += xmidFile[posIdx++];

                    i += 8;

                    if (chunkId.Equals("INFO"))
                    {
                        if (chunkLen < 2)
                        {
                            // INFO block must be at least 2 bytes
                            return null;
                        }
                        data.NumTracks = Util.buildInt(xmidFile[posIdx], xmidFile[posIdx + 1]);
                        posIdx += 2;
                        if (chunkLen > 2)
                        {
                            // merits a warning i guess
                        }
                        break;
                    }

                    // align
                    posIdx += (chunkLen + 1) & ~1;
                    i += (chunkLen + 1) & ~1; // not sure why we're dropping the lowest bit here but ok
                }

                if (data.NumTracks <= 0)
                {
                    // failed to retrieve the number of tracks
                    return null;
                }

                posIdx = 8 + (len + 1) & ~1;
                if (!readByteToString(xmidFile, posIdx, 4).Equals("CAT "))
                {
                    // invalid xmid file
                    return null;
                }

                posIdx += 4;

                len = read4ByteBigEnd(xmidFile, posIdx);
                posIdx += 4;

                if (!readByteToString(xmidFile, posIdx, 4).Equals("XMID"))
                {
                    // expecting xmid
                    return null;
                }
                posIdx += 4;

            }

            if (data.NumTracks > MAX_TRACKS)
            {
                // too many tracks, bail
                return null;
            }

            int tracksRead = 0;
            //data.Tracks = new byte[data.NumTracks, 1];

            while (tracksRead < data.NumTracks)
            {
                string chunkId = readByteToString(xmidFile, posIdx, 4);
                if (chunkId.Equals("FORM"))
                {
                    // skip FORM + next 4 bytes
                    posIdx += 8;
                }
                else if (chunkId.Equals("XMID"))
                {
                    posIdx += 4;
                }
                else if (chunkId.Equals("TIMB"))
                {
                    // not sure MOO even has these, skip at any rate
                    posIdx += 4;
                    int skip = read4ByteBigEnd(xmidFile, posIdx);
                    posIdx += 4;
                    posIdx += (skip + 1) & ~1;
                }
                else if (chunkId.Equals("EVNT"))
                {
                    posIdx += 4;
                    int trackLen = read4ByteBigEnd(xmidFile, posIdx);
                    posIdx += 4;
                    byte[] currTrack = new byte[trackLen];
                    Array.Copy(xmidFile, posIdx, currTrack, 0, trackLen);
                    data.Tracks.Add(currTrack);
                    //data.Tracks[tracksRead] = new byte[trackLen];
                    //Array.Copy(xmidFile, posIdx, data.Tracks[tracksRead], 0, trackLen);
                    posIdx += (trackLen + 1) &  ~1;
                    tracksRead++;
                }
            }

            return data;
        }

        private byte[] stripMooData(byte[] rawData)
        {
            // remove the lbx header and what I can only assume is padding after the termination sig 
            // in the xmi file
            // xmi file starts with FORM and ends with 0xff 0x2f 0x00
            // 46 4f 52 4d
            int formIdx = -1;
            int currIdx = 0;
            
            while (formIdx < 0 && currIdx < rawData.Length)
            {
                if (rawData[currIdx] == 0x46 && currIdx < rawData.Length - 4)
                {
                    if (rawData[currIdx + 1] == 0x4f && rawData[currIdx + 2] == 0x52 && rawData[currIdx + 3] == 0x4d)
                    {
                        formIdx = currIdx;
                    }
                }
                currIdx++;
            }

            int termIdx = -1;
            currIdx = rawData.Length - 1;
            while (termIdx < 0 && currIdx >= 0)
            {
                if (rawData[currIdx] == 0x00 && currIdx >= 2)
                {
                    if (rawData[currIdx - 1] == 0x2f && rawData[currIdx - 2] == 0xff)
                    {
                        termIdx = currIdx + 1;
                    }
                }
                currIdx--;
            }

            if (termIdx >= 0 && formIdx >= 0)
            {
                return Util.slice(rawData, formIdx, termIdx - formIdx);
            }
            return null;
        }

        private string readByteToString(byte[] bArray, int idx, int len)
        {
            return Encoding.ASCII.GetString(Util.slice(bArray, idx, len));
        }

        private int read4ByteBigEnd(byte[] arr, int idx)
        {
            int retval = arr[idx] << 24;
            retval += arr[idx+1] << 16;
            retval += arr[idx+2] << 8;
            retval += arr[idx+3];
            return retval;
        }
    }
}
