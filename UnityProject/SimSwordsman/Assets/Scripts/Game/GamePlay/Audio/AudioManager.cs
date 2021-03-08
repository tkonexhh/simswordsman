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
        #region imgr
        public void OnInit()
        {
            EventSystem.S.Register(EventID.OnCharacterUpLevel, OnCharacterUpLevelCallBack);
            EventSystem.S.Register(EventID.OnUpgradeFacility, OnUpgradeFacilityCallBack);
            EventSystem.S.Register(EventID.OnBattleSuccessed, OnBattleSuccessCallBack);
            EventSystem.S.Register(EventID.OnBattleFailed, OnBattleFaildCallBack);
            EventSystem.S.Register(EventID.OnAddCharacter, OnAddCharacterCallBack);
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
            AudioMgr.S.PlaySound(Define.SOUND_CHARACTER_LEVEL_UP);
        }
        private void OnUpgradeFacilityCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
        }
        private void OnBattleFaildCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_BATTLE_LOSE);
        }
        private void OnBattleSuccessCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_BATTLE_WIN);
        }
        private void OnAddCharacterCallBack(int key, object[] param)
        {
            AudioMgr.S.PlaySound(Define.SOUND_RECRUIT);
        }
        #endregion

        #region public
        /// <summary>
        /// 人物被攻击音效
        /// </summary>
        public void PlayCharacterAttackedSound() 
        {
            int index = UnityEngine.Random.Range(1, 5);

            string soundName = string.Format("Hit{0}", index);

            AudioMgr.S.PlaySound(soundName);
        }
        /// <summary>
        /// 人物死亡音效
        /// </summary>
        /// <param name="isWoman"></param>
        public void PlayCharacterDeadSound(bool isWoman) 
        {
            if (isWoman)
            {
                AudioMgr.S.PlaySound(Define.SOUND_DEATH_GIRL);
            }
            else {
                int index = Random.Range(1, 3);
                string soundName = string.Format("Death_{0}", index);
                AudioMgr.S.PlaySound(soundName);
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
        public void PlayForgeSound() {
            AudioMgr.S.PlaySound(Define.SOUND_FORGE);
        }
        /// <summary>
        /// 播放干活  扫地音效
        /// </summary>
        public void PlaySweepSound() {
            AudioMgr.S.PlaySound(Define.SOUND_SWEEP);
        }
        /// <summary>
        /// 播放干活  捣药音效
        /// </summary>
        public void PlayPoundSound() {
            AudioMgr.S.PlaySound(Define.SOUND_POUND);
        }
        public void PlayCollectWuwoodOrRockSound(CollectedObjType objType) 
        {
            switch (objType)
            {
                case CollectedObjType.WuWood:
                case CollectedObjType.SilverWood:
                    AudioMgr.S.PlaySound(Define.SOUND_LUMBER);
                    break;
                case CollectedObjType.CloudRock:
                case CollectedObjType.QingRock:
                    AudioMgr.S.PlaySound(Define.SOUND_MINE);
                    break;
            }
        }
        /// <summary>
        /// 敌人攻击完毕音效
        /// </summary>
        public void PlayEnemyAttackSound(string soundName) 
        {
            //string soundName = string.Empty;

            //switch (objType)
            //{
            //    case CollectedObjType.Boar:
            //        soundName = Define.SOUND_PIG;
            //        break;
            //    case CollectedObjType.Wolf:
            //        soundName = Define.SOUND_WOLF;
            //        break;
            //    case CollectedObjType.Bear:
            //        soundName = Define.SOUND_BEAR;
            //        break;
            //    case CollectedObjType.Snake:
            //        soundName = Define.SOUND_SNAKE;
            //        break;
            //    case CollectedObjType.Deer:
            //        soundName = Define.SOUND_DEER;
            //        break;
            //    case CollectedObjType.Chicken:
            //        soundName = Define.SOUND_CHICKEN;
            //        break;
            //}

            if (string.IsNullOrEmpty(soundName)) 
            {
                return;
            }

            AudioMgr.S.PlaySound(soundName);
        }
        #endregion
    }
}