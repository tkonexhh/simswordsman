using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class MusicMgr : TSingleton<MusicMgr>
    {
        private bool m_IsBgMusicPlaying = false;
        private string abTestBgMusicName = "test";

        public void PlayMenuMusic()
        {
            m_IsBgMusicPlaying = true;

            AudioUnitID.MUSIC_BGID = AudioMgr.S.PlayBg(Define.MUSIC_MENU);

            AudioMgr.S.SetVolume(AudioUnitID.MUSIC_BGID, 0.5f);

        }

        public void PlayBattleMusic()
        {
            m_IsBgMusicPlaying = true;

            AudioUnitID.MUSIC_BGID = AudioMgr.S.PlayBg(Define.MUSIC_BATTLE);

            AudioMgr.S.SetVolume(AudioUnitID.MUSIC_BGID, 1.0f);

        }

        public void StopBgMusic()
        {
            m_IsBgMusicPlaying = false;
            AudioMgr.S.Stop(AudioUnitID.MUSIC_BGID);
        }

        public void PauseBgMusic()
        {
            m_IsBgMusicPlaying = false;
            AudioMgr.S.Pause(AudioUnitID.MUSIC_BGID);
        }

        public void ResumeBgMusic()
        {
            m_IsBgMusicPlaying = true;
            AudioMgr.S.Resume(AudioUnitID.MUSIC_BGID);
        }
    }
}
