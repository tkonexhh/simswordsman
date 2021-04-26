using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class CharacterMgr : MonoBehaviour, IMgr
    {
        private CharacterDataWrapper m_CharacterDataWrapper = new CharacterDataWrapper();

        private List<CharacterController> m_CharacterControllerList = new List<CharacterController>();

        private Vector3 m_CharacterSpawnPos = new Vector3(-5f, -4.7f, 0);

        private RandomWayPoints m_RandomWayPoints = null;

        public List<CharacterController> CharacterControllerList { get => m_CharacterControllerList; }
        public CharacterDataWrapper CharacterDataWrapper { get => m_CharacterDataWrapper; }

        #region IMgr
        public void OnInit()
        {
            //InitPool();
        }

        public void OnUpdate()
        {
            CharacterControllerList.ForEach(i => i.Update());
        }

        public void OnDestroyed()
        {

        }

        #endregion

        #region Public Get      

        public int GetCharacterCount()
        {
            return m_CharacterDataWrapper.characterList.Count;
        }
        /// <summary>
        /// ��ȡװ���ļӳ�
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        //public float GetDiscipleEquipBonus(EquipmentItem equipment)
        //{
        //    return m_CharacterDataWrapper.GetDiscipleEquipBonus(equipment);
        //}

        /// <summary>
        /// ����������װ��
        /// </summary>
        /// <param name="_character"></param>
        /// <param name="_equipmentItem"></param>
        public void AddEquipment(int chracterID, CharaceterEquipment characeterEquipment, int delta = 1)
        {
            m_CharacterDataWrapper.AddEquipment(chracterID, characeterEquipment);
        }
        /// <summary>
        /// ����Ƿ����ظ�������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckForDuplicateNames(string name)
        {
            return m_CharacterDataWrapper.characterList.Any(i => i.name.Equals(name));
        }
        /// <summary>
        /// ��ȡ��ǰ���id
        /// </summary>
        /// <returns></returns>
        public int GetMaxCharacterId()
        {
            //FIXME 修改弟子的ID改为历史ID
            m_CharacterDataWrapper.characterList.Sort();
            CharacterItem item = m_CharacterDataWrapper.characterList.LastOrDefault();
            if (item != null)
            {
                int curId = item.id;
                return ++curId;
            }
            return 0;
        }
        /// <summary>
        /// ��ȡ��ǰ��ɫ��������
        /// </summary>
        /// <param name="character"></param>
        public int GetExpLevelUpNeed(CharacterItem character)
        {
            return TDCharacterStageConfigTable.GetExpLevelUpNeed(character);
        }


        /// <summary>
        /// ���ӵ���
        /// </summary>
        /// <param name="item"></param>
        public void AddCharacter(CharacterItem item)
        {
            m_CharacterDataWrapper.AddCharacter(item);
            EventSystem.S.Send(EventID.OnAddCharacter);
        }

        /// <summary>
        /// Get owned character info(name id desc ....)
        /// </summary>
        public CharacterItem GetOwnedCharacterInfo(int characterId)
        {
            CharacterItem item = m_CharacterDataWrapper.GetCharacterItem(characterId);
            return item;
        }

        /// <summary>
        /// Get all characters
        /// </summary>
        public List<CharacterItem> GetAllCharacterList()
        {
            return m_CharacterDataWrapper.characterList;
        }
        public List<CharacterItem> GetCharacterForQuality(CharacterQuality characterQuality)
        {
            return m_CharacterDataWrapper.GetCharacterForQuality(characterQuality);
        }

        public List<CharacterController> GetAllCharacterInTask(int taskId)
        {
            return m_CharacterControllerList.Where(i => i.CharacterModel.GetCurTaskId() == taskId).ToList();
        }
        /// <summary>
        /// 获取某一限定等级(>=)弟子的数量
        /// </summary>
        /// <returns></returns>
        public int GetLomitLevelDiscipleNumber(int level)
        {
            return m_CharacterDataWrapper.GetLomitLevelDiscipleNumber(level);
        }

        public CharacterItem GetCharacterItem(int id)
        {
            return m_CharacterDataWrapper.GetCharacterItem(id);
        }
        /// <summary>
        /// Get character controller
        /// </summary>
        public CharacterController GetCharacterController(int characterId)
        {
            CharacterController character = m_CharacterControllerList.Where(i => i.CharacterModel.Id == characterId).FirstOrDefault();
            return character;
        }

        /// <summary>
        /// ����ѧϰ����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kungfuItem"></param>
        public bool LearnKungfu(int id, int index, KungfuItem kungfuItem)
        {
            return m_CharacterDataWrapper.LearnKungfu(id, index, kungfuItem);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        public void AddCharacterLevel(int id, int level)
        {
            m_CharacterDataWrapper.AddCharacterLevel(id, level);
        }
        /// <summary>
        /// ��ȡ�����ȼ�(����)
        /// </summary>
        /// <param name="unlockContent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetUnlockConfigInfo(UnlockContent unlockContent, int index = 0)
        {
            return TDCharacterStageConfigTable.GetUnlockConfigInfo(unlockContent, index);
        }

        public GameObject SpawnTaskRewardBubble()
        {
            return GameObjectPoolMgr.S.Allocate(Define.CHARACTER_TASK_REWARD_BUBBLE);
        }

        public GameObject SpawnWorkProgressBar()
        {
            return GameObjectPoolMgr.S.Allocate(Define.CHARACTER_WORK_PROGRESS_BAR);
        }

        public GameObject SpawnWorkRewardPop()
        {
            return GameObjectPoolMgr.S.Allocate(Define.CHARACTER_WORK_REWARD_POP);
        }

        public GameObject SpawnWorkTip()
        {
            return GameObjectPoolMgr.S.Allocate(Define.CHARACTER_WORK_TIP);
        }
        #endregion

        #region Public Set

        /// <summary>
        /// Add a new character and save db data
        /// </summary>
        public void AddCharacter(int id, CharacterQuality quality)
        {
            m_CharacterDataWrapper.AddCharacter(id, quality);
        }

        //public void ExrInitData()
        //{
        //    InitData();
        //}

        /// <summary>
        /// Remove a character and save db data
        /// </summary>
        public void RemoveCharacter(int id)
        {
            EventSystem.S.Send(EventID.DeleteDisciple, id);
            m_CharacterDataWrapper.RemoveCharacter(id);

            CharacterController character = GetCharacterController(id);
            if (character != null && m_CharacterControllerList.Contains(character))
            {
                Destroy(character.CharacterView.gameObject);

                m_CharacterControllerList.Remove(character);
            }
        }

        public void SpawnCharacterController(CharacterItem characterItem)
        {
            int id = characterItem.id;
            CharacterStateID initState = characterItem.characterStateId;

            bool isSpawned = m_CharacterControllerList.Any(i => i.CharacterId == id);
            if (isSpawned)
                return;

            GameObject go = CharacterLoader.S.GetCharacterGo(id, characterItem.quality, characterItem.bodyId, characterItem.GetClanType());
            if (go != null)
            {
                OnCharacterLoaded(go, id, initState);
            }
            else
            {
                CharacterLoader.S.LoadCharactersync(id, characterItem.quality, characterItem.bodyId, characterItem.GetClanType());
                GameObject obj = CharacterLoader.S.GetCharacterGo(id, characterItem.quality, characterItem.bodyId, characterItem.GetClanType());
                OnCharacterLoaded(obj, id, initState);
            }
        }

        private void OnCharacterLoaded(GameObject obj, int id, CharacterStateID initState)
        {
            //obj.transform.parent = GameplayMgr.S.EntityRoot.transform; 

            CharacterView characterView = obj.GetComponent<CharacterView>();
            if (characterView == null)
            {
                Log.e("Characterview is null, id: " + id + " obj name: " + obj.name);
                return;
            }

            CharacterController controller = new CharacterController(id, characterView, initState);
            m_CharacterControllerList.Add(controller);

            if (initState == CharacterStateID.None)
            {
                controller.SetState(CharacterStateID.EnterClan);
            }

            Vector3 spawnPos = GetSpawnPos(controller.CurState);
            obj.transform.position = spawnPos;
        }


        public static int GetMaxLevel(CharacterQuality quality)
        {
            if (quality == CharacterQuality.Normal)
                return Define.CHARACTER_NORAML_MAX_LEVEL;
            else if (quality == CharacterQuality.Good)
                return Define.CHARACTER_GOOD_MAX_LEVEL;
            else
                return Define.CHARACTER_EXCELLENT_MAX_LEVEL;
        }
        #endregion

        #region Private Methods

        public void InitData()
        {
            InitCharacters();
        }

        public void InitCharacterDataWrapper()
        {
            m_CharacterDataWrapper.Wrap(GameDataMgr.S.GetClanData().ownedCharacterData);

        }

        private void InitCharacters()
        {
            foreach (var item in m_CharacterDataWrapper.characterList)
            {
                SpawnCharacterController(item);
            }
        }


        private bool IsCharacterOwned(int id)
        {
            bool isOwned = m_CharacterDataWrapper.characterList.Any(i => i.id == id);
            return isOwned;
        }


        private Vector3 GetSpawnPos(CharacterStateID initState)
        {
            Vector3 spawnPos;
            if (initState == CharacterStateID.EnterClan)
            {
                spawnPos = m_CharacterSpawnPos;
            }
            else if (initState == CharacterStateID.Deliver)
            {
                spawnPos = DeliverSystemMgr.S.GoOutSidePos;
            }
            else
            {
                if (m_RandomWayPoints == null)
                {
                    m_RandomWayPoints = GameObject.FindObjectOfType<RandomWayPoints>();
                }

                spawnPos = m_RandomWayPoints.GetRandomWayPointPos(Vector3.zero);
            }

            return spawnPos;
        }

        #endregion


        #region Static Method
        internal static string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        #endregion
    }
}