using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using Random = UnityEngine.Random;

namespace GameWish.Game
{
	public class AudioManager : TSingleton<AudioManager>,IMgr
	{
        private float m_CharacterLevelUpSoundInterval = 3;
        private bool m_IsPlayingCharacterLevelUpSound = false;

        #region imgr
        public void OnInit()
        {
            EventSystem.S.Register(EventID.OnCharacterUpLevel, OnCharacterUpLevelCallBack);
            EventSystem.S.Register(EventID.OnUpgradeFacility, OnUpgradeFacilityCallBack);
            EventSystem.S.Register(EventID.OnBattleSuccessed, OnBattleSuccessCallBack);
            EventSystem.S.Register(EventID.OnBattleFailed, OnBattleFaildCallBack);
            EventSystem.S.Register(EventID.OnAddCharacter, OnAddCharacterCallBack);

            EventSystem.S.Register(EventID.OnStartUnlockFacility, OnStartUnlockFacilityCallBack);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, OnStartUpradeFacilityCallBack);
        }
        public void OnUpdate()
        {

        }
        public void OnDestroyed()
        {
            EventSystem.S.UnRegister(EventID.OnCharacterUpLevel, OnCharacterUpLevelCallBack);
        }
        #endregion

        #region call back
        private void OnCharacterUpLevelCallBack(int key, object[] param)
        {
            if (MainGameMgr.S.BattleFieldMgr.IsBattleing) 
            {
                return;
            }

            if (m_IsPlayingCharacterLevelUpSound) 
                return;

            m_IsPlayingCharacterLevelUpSound = true;
            Timer.S.Post2Really((x) => {
                m_IsPlayingCharacterLevelUpSound = false;
            }, m_CharacterLevelUpSoundInterval, 1);
            AudioMgr.S.PlaySound(Define.SOUND_CHARACTER_LEVEL_UP);
        }
        private void OnUpgradeFacilityCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
        }
        private void OnBattleFaildCallBack(int key, object[] param)
        {
            //AudioMgr.S.PlaySound(Define.SOUND_BATTLE_LOSE);
        }
        private void OnBattleSuccessCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_BATTLE_WIN);
        }
        private void OnAddCharacterCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_RECRUIT);
        }
        private void OnStartUpradeFacilityCallBack(int key, object[] param)
        {
            PlayFacilityBuildOrUpgradSound();
        }
        private void OnStartUnlockFacilityCallBack(int key, object[] param)
        {
            PlayFacilityBuildOrUpgradSound();
        }
        #endregion

        #region public
        /// <summary>
        /// 人物被攻击音效
        /// </summary>
        public void PlayCharacterAttackedSound(Vector3 characterPos) 
        {
            int index = UnityEngine.Random.Range(1, 5);

            string soundName = string.Format("Hit{0}", index);

            AudioMgr.S.PlaySound3D(soundName, characterPos);
        }
        /// <summary>
        /// 人物死亡音效
        /// </summary>
        /// <param name="isWoman"></param>
        public void PlayCharacterDeadSound(bool isWoman,Vector3 characterPos) 
        {
            if (isWoman)
            {
                AudioMgr.S.PlaySound3D(Define.SOUND_DEATH_GIRL, characterPos);
            }
            else {
                int index = Random.Range(1, 3);
                
                string soundName = string.Format("Death_{0}", index);

                AudioMgr.S.PlaySound3D(soundName, characterPos);
            }            
        }
        /// <summary>
        /// 收集系统音效
        /// </summary>
        /// <param name="keyID"></param>
        public void PlayCollectSound(int keyID) 
        {
            RawMaterial rm = (RawMaterial)keyID;
            switch (rm)
            {
                case RawMaterial.Vine:
                case RawMaterial.Ganoderma:
                case RawMaterial.LotusRoot:
                case RawMaterial.Lotus:
                case RawMaterial.LotusLeaf:
                    AudioMgr.S.PlaySound(Define.SOUND_COLLECT);
                    break;
            }
        }
        /// <summary>
        /// 播放干活  锻造音效
        /// </summary>
        public void PlayForgeSound(Vector3 pos) 
        {
            AudioMgr.S.PlaySound3D(Define.SOUND_FORGE, pos);
        }
        /// <summary>
        /// 播放干活  扫地音效
        /// </summary>
        public void PlaySweepSound(Vector3 characterPos) 
        {        
            AudioMgr.S.PlaySound3D(Define.SOUND_COLLECT, characterPos);
        }
        /// <summary>
        /// 播放干活  捣药音效
        /// </summary>
        public void PlayPoundSound(Vector3 pos) 
        {
            AudioMgr.S.PlaySound3D(Define.SOUND_POUND, pos);
        }
        public void PlayCollectWuwoodOrRockSound(CollectedObjType objType,Vector3 pos) 
        {
            switch (objType)
            {
                case CollectedObjType.WuWood:
                case CollectedObjType.SilverWood:
                    AudioMgr.S.PlaySound3D(Define.SOUND_LUMBER, pos);
                    break;
                case CollectedObjType.CloudRock:
                case CollectedObjType.QingRock:
                case CollectedObjType.Iron:
                    AudioMgr.S.PlaySound3D(Define.SOUND_MINE, pos);
                    break;
            }
        }
        /// <summary>
        /// 敌人攻击完毕音效
        /// </summary>
        public void PlayEnemyAttackSound(string soundName) 
        {
            if (string.IsNullOrEmpty(soundName)) 
            {
                return;
            }

            AudioMgr.S.PlaySound(soundName);
        }
        /// <summary>
        /// 建筑建造或者升级音效
        /// </summary>
        public void PlayFacilityBuildOrUpgradSound() 
        {
            AudioMgr.S.PlaySound(Define.SOUND_FACILITY_BUILD_UPGRAD);
        }
        #endregion
    }
}