using System;

using FreeMoO.Collections;
using FreeMoO.Reverse;
using SdlDotNet.Audio;

namespace FreeMoO.Service
{
    public class SoundFXService
    {
        private SoundCollection mSoundFx;
        private FreemooGame mGame;

        public SoundFXService(FreemooGame pGame)
        {
            mGame = pGame;
            mSoundFx = new SoundCollection();
        }

        public void loadSoundFX()
        {
            //string fn = ((ConfigService)mGame.Services[ServiceEnum.ConfigService]).DataFolder + "\\LBX\\SOUNDFX.LBX";
            string fn = Config.DataFolder + "\\LBX\\SOUNDFX.LBX";
            Archive soundArc = new Archive(fn);

            Array soundEnumVals = Enum.GetValues(typeof(SoundFXEnum));
            foreach (SoundFXEnum sfx in soundEnumVals)
            {
                int soundIndex = (int)sfx;
                byte[] soundData = soundArc.readSoundFx(soundIndex);
                Sound snd = new Sound(soundData);
                mSoundFx.Add(sfx, snd);
            }
        }

        public Music GetMusic()
        {
            Music m = null;

            Archive musicArc = new Archive(Config.DataFolder + "\\LBX\\MUSIC.LBX");
            byte[] musicData = musicArc.readMusic(0);
            byte[] midi = (new FreemooMusicAdapter()).Convert(musicData);
            //byte[] fileData = Util.slice(musicData, 0x10, 0x36ac);
            //byte test = fileData[fileData.Length - 2];
            //m = new Music(fileData);
            //m = new Music(musicData);
            //return m;
            return null;
        }

        public void playSound(SoundFXEnum pSnd)
        {
            mSoundFx[pSnd].Play();
        }

    }

}
