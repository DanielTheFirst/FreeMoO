using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace FreeMoO 
{
    public class FreemooSoundAdapter
    {
        struct DataBlock
        {
                public int sampleByte;
                public double sampleRate;
                public int blockSize;
                public int blockType;
                public byte[] data;
                public string typeDesc;
        }
        struct VocFile
        {
                public List<DataBlock> dataBlocks;
                public int initalOffset;
        }

        struct WavFile
        {
                public int totalChunkSize;
                public int fmtChunkSize;
                public int audioFmt;
                public int numChannels;
                public int sampleRate;
                public int byteRate;
                public int blockAlign;
                public int bitsPerSample;
                public int dataChunkSize;
                public byte[] chunkData;
        }

        private static string[] blockTypeDesc = { "Terminator", "Sound Data", "Sound continue", "Silence", "Marker", "ASCII", "Repeat", "End Repeat", "Extended" };
                
        public FreemooSoundAdapter()
        {
        }
                
        public byte[] convert(byte[] vocdata)
        {
                VocFile voc = readFile(vocdata, 0);
                WavFile wav = convertVocToWav(voc);
                return writeWaveFile(wav);
        }
                
        private WavFile convertVocToWav(VocFile input)
        {
            WavFile output = new WavFile();

            // luckily the voc files from moo are a single chunk of sound data with nothing fancy

            //if (input.dataBlocks[0].sampleRate == 0.0)
            //{
            //    input.dataBlocks[0].sampleRate = calcSampleRate(input.dataBlocks[0].sampleByte);
            //}
            output.sampleRate = (int)input.dataBlocks[0].sampleRate;
            output.dataChunkSize = input.dataBlocks[0].data.Length;
            //output.chunkData = input.dataBlocks[0].data;
            output.chunkData = new byte[output.dataChunkSize];
            input.dataBlocks[0].data.CopyTo(output.chunkData, 0);
            output.fmtChunkSize = 16;
            output.audioFmt = 1;
            output.numChannels = 1;
            output.bitsPerSample = 8;
            output.numChannels = 1;
            output.blockAlign = 1;
            output.byteRate = output.sampleRate;
            // 4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
            output.totalChunkSize = 4 + (8 + output.fmtChunkSize) + (8 + output.dataChunkSize);


            return output;
        }

        private byte [] writeWaveFile(WavFile wavData)
        {
            // this might be harder
            byte[] data = new byte[wavData.totalChunkSize + 8];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
            string tmp = "RIFF";
            byte[] tmp2 = strToByteArray(tmp);
            tmp2.CopyTo(data, 0);
            strToByteArray("WAVE").CopyTo(data, 8);
            strToByteArray("fmt ").CopyTo(data, 12);
            strToByteArray("data").CopyTo(data, 36);
            write32(wavData.fmtChunkSize).CopyTo(data, 16);
            write16(wavData.audioFmt).CopyTo(data, 20);
            write16(wavData.numChannels).CopyTo(data, 22);
            write32(wavData.sampleRate).CopyTo(data, 24);
            write32(wavData.byteRate).CopyTo(data, 28);
            write16(wavData.blockAlign).CopyTo(data, 32);
            write16(wavData.bitsPerSample).CopyTo(data, 34);
            write32(wavData.dataChunkSize).CopyTo(data, 40);
            wavData.chunkData.CopyTo(data, 44);

            return data;
        }

        private byte[] strToByteArray(string inStr)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(inStr);
        }
                
        private VocFile readFile(byte[] data, int offset)
        {
            VocFile file = new VocFile();
            //offset += 0x10;
            file.dataBlocks = new List<DataBlock>();
            // offset + 0x24 & 0x25 are the initial data block offset
            int initialOffset = data[0x24 + offset] + (data[0x25 + offset] << 8);
            file.initalOffset = initialOffset;
            int blockOffset = file.initalOffset;
            do
            {
                DataBlock db = readBlock(data, offset + 0x10, blockOffset);
                file.dataBlocks.Add(db);
                blockOffset += db.blockSize + 4; // 4 for the block header
                
            } while (file.dataBlocks[file.dataBlocks.Count - 1].blockType != 0);
            
            return file;
        }

        private DataBlock readBlock(byte[] data, int fileOffset, int blockOffset)
        {
            DataBlock block = new DataBlock();
            block.blockType = data[fileOffset + blockOffset];
            block.typeDesc = blockTypeDesc[block.blockType];
            switch (block.blockType)
            {
                case 0:
                    // terminator, return as is
                    //block.typeDesc = blockTypeDesc[block.blockType];
                    break;
                case 1:
                    // sound data, read
                    block.blockSize = data[fileOffset + blockOffset + 1] + (data[fileOffset + blockOffset + 2] << 8) + (data[fileOffset + blockOffset + 3] << 16);
                    block.sampleByte = data[fileOffset + blockOffset + 4];
                    block.sampleRate = calcSampleRate(block.sampleByte);
                    block.data = new byte[block.blockSize - 2];
                    for (int i = 0; i < block.blockSize - 2; i++)
                    {
                        block.data[i] = data[fileOffset + blockOffset + 6 + i];
                    }
                    break;
                default:
                    break;
            }

            return block;
        }
                
        private double calcSampleRate(int sampleByte)
        {
            double sampleRate = 0.0;
            double sampleInput = (double)sampleByte;
            sampleRate = -1000000.0 / (sampleInput - 256.0);
            return sampleRate;
        }

        private int read32(byte[] data, int offset)
        {
            int ret = 0;
            ret = read16(data, offset) + (read16(data, offset + 2) << 16);
            return ret;
        }

        private int read16(byte[] data, int offset)
        {
            int ret = 0;
            ret = data[offset] + (data[offset + 1] << 8);
            return ret;
        }

        private byte[] write16(int data)
        {
            byte[] ret = new byte[2];
            ret[0] = (byte)(0xff & data);
            ret[1] = (byte)(0xff & (data >> 8));
            return ret;
        }


        private byte[] write32(int data)
        {
            byte[] ret = write16(data);
            byte[] ret2 = write16(data >> 16);
            byte[] ret3 = new byte[4];
            ret.CopyTo(ret3, 0);
            ret2.CopyTo(ret3, 2);
            return ret3;
        }
    }
}