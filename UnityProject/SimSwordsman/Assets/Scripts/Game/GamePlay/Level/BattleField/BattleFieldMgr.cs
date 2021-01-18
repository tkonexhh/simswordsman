using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public class BattleFieldMgr : MonoBehaviour, IMgr
    {
        private BattleField m_BattleField = null;

        private List<CharacterController> m_OurCharacterList = new List<CharacterController>();
        private List<CharacterController> m_EnemyCharacterList = new List<CharacterController>();
        //private List<CharacterController> m_OurNotFightingCharacterList = new List<CharacterController>();
        //private List<CharacterController> m_EnemyNotFightingCharacterList = new List<CharacterController>();
        [SerializeField]private List<FightGroup> m_FightGroupList = new List<FightGroup>();

        private float m_TotalEnemyAtk = 0;
        private float m_TotalOurAtk = 0;
        private float m_TotalEnemyHp = 0;
        private float m_TotalOurHp = 0;
        private float m_EnemeyDamagePersecond = 1;
        private float m_OurDamagePersecond = 1;
        private int m_InitOurCharacterCount;
        private int m_InitEnemeyCharacterCount;
        private int m_Const = 15;

        private float m_ApplyDamageTime = 0f;
        private float m_ApplyDamageInterval = 1f;

        private bool m_IsBattleBegin = false;
        private bool m_IsBattleEnd = false;

        private Vector2 m_BattleAreaRightTop = new Vector2(44, 1.5f);
        private Vector2 m_BattleAreaLeftBottom = new Vector2(38, -1.1f);

        private int m_FightGroupId = 1;

        public Vector2 BattleAreaRightTop { get => m_BattleAreaRightTop;}
        public Vector2 BattleAreaLeftBottom { get => m_BattleAreaLeftBottom;}
        public List<CharacterController> OurCharacterList { get => m_OurCharacterList; }
        public List<CharacterController> EnemyCharacterList { get => m_EnemyCharacterList; }
        //public List<CharacterController> OurNotFightingCharacterList { get => m_OurNotFightingCharacterList; }
        //public List<CharacterController> EnemyNotFightingCharacterList { get => m_EnemyNotFightingCharacterList; }

        private int m_AllEnemyCount = 0;
        private int m_LoadedEnemyCount = 0;

        #region IMgr

        public void OnInit()
        {
            m_BattleField = FindObjectOfType<BattleField>();
            m_BattleField.Init();

            RegisterEvents();
        }

        public void OnUpdate()
        {
            if (!m_IsBattleBegin)
                return;

            if (m_IsBattleEnd)
                return;

            m_OurCharacterList.ForEach(i => 
            {
                i.RefreshBattleState();
                //Log.e("our character atk: " + i.CharacterModel.Atk);
            });

            m_EnemyCharacterList.ForEach( i => 
            {
                i.RefreshBattleState();
                //Log.e("Enemy character atk: " + i.CharacterModel.Atk);
            });

            m_ApplyDamageTime += Time.deltaTime;
            if (m_ApplyDamageTime >= m_ApplyDamageInterval)
            {
                m_ApplyDamageTime = 0f;

                ApplyDamage();
            }

            //RefreshFightGroup();
        }

        public void OnDestroyed()
        {
            UnregisterEvents();
        }

        #endregion

        #region Public Get

        public CharacterController GetNearestCharacterAlive(CharacterCamp camp, Vector2 pos)
        {
            List<CharacterController> list;
            if (camp == CharacterCamp.EnemyCamp)
            {
                list = m_OurCharacterList.Where(i => i.IsDead() == false).ToList();
            }
            else
            {
                list = m_EnemyCharacterList.Where(i => i.IsDead() == false).ToList();
            }

            CharacterController character = GetNearestCharacterAlive(list, pos);

            return character;
        }

        public CharacterController GetNearestCharacterAlive(List<CharacterController> characterList, Vector2 pos)
        {
            float minDistance = 100;
            CharacterController character = null;

            foreach (CharacterController item in characterList)
            {
                float distance = Vector2.Distance(item.GetPosition(), pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    character = item;
                }
            }

            return character;
        }

        #endregion

        #region Public Set

        public void OnFightGroupCharacterDead(FightGroup group)
        {
            group.Release();

            RemoveFightGroup(group);

            SpawnFightGroup(m_OurCharacterList, m_EnemyCharacterList);
        }

        #endregion

        #region Private

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnEnterBattle, HandleEvent);
            EventSystem.S.Register(EventID.OnExitBattle, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnEnterBattle, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnExitBattle, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnEnterBattle:
                    List<EnemyConfig> enemies = (List<EnemyConfig>)param[0];
                    List<CharacterController> ourSelectedCharacters = (List<CharacterController>)param[1];
                    OnEnterBattle(enemies, ourSelectedCharacters);
                    break;
                case (int)EventID.OnExitBattle:
                    OnExitBattle();
                    break;
            }
        }

        private void OnEnterBattle(List<EnemyConfig> enemies, List<CharacterController> ourSelectedCharacters)
        {
            m_IsBattleBegin = true;
            m_IsBattleEnd = false;

            m_AllEnemyCount = enemies.Count;
            m_LoadedEnemyCount = 0;

            SpawnOurCharacter(ourSelectedCharacters);

            enemies.ForEach(i =>
            {
                SpawnEnemyCharacter(i.ID, i.Atk);
            });

        }

        private void OnAllEnemyLoaded()
        {
            SpawnFightGroup(m_OurCharacterList, m_EnemyCharacterList);

            m_InitOurCharacterCount = m_OurCharacterList.Count;
            m_InitEnemeyCharacterCount = m_EnemyCharacterList.Count;

            m_OurCharacterList.Where(i => i.IsDead() == false && i.FightGroup == null).ToList().ForEach(i =>
            {
                i.GetBattleState().SetState(BattleStateID.Wait);
            });

            m_EnemyCharacterList.Where(i => i.IsDead() == false && i.FightGroup == null).ToList().ForEach(i =>
            {
                i.GetBattleState().SetState(BattleStateID.Wait);
            });

        }

        private void OnExitBattle()
        {
            m_IsBattleBegin = false;
            m_OurCharacterList.ForEach(i => i.OnExitBattleField());
            m_OurCharacterList.Clear();
            m_EnemyCharacterList.Clear();

            m_FightGroupList.ForEach(i => i.Release());
            m_FightGroupList.Clear();

            m_BattleField.OnBattleEnd();
        }

        private void SpawnOurCharacter(List<CharacterController> characters)
        {
            m_OurCharacterList.Clear();
            m_OurCharacterList.AddRange(characters);
            m_OurCharacterList.ForEach(i => 
            {
                Vector3 pos = m_BattleField.GetOurCharacterPos();
                i.OnEnterBattleField(pos);

                m_TotalOurAtk += i.CharacterModel.Atk;
                m_TotalOurHp += i.CharacterModel.Hp;
            });
        }

        private void SpawnEnemyCharacter(int id, int atk)
        {
            SpawnEnemyController(1, m_BattleField.GetEnemyCharacterPos(), CharacterCamp.EnemyCamp, (controller) => 
            {
                m_EnemyCharacterList.Add(controller);
                controller.CharacterModel.SetAtk(atk);

                m_TotalEnemyAtk += controller.CharacterModel.Atk;
                m_TotalEnemyHp += controller.CharacterModel.Hp;

                m_LoadedEnemyCount++;

                if (m_LoadedEnemyCount >= m_AllEnemyCount)
                {
                    OnAllEnemyLoaded();
                }
            });

        }

        private void SpawnFightGroup(List<CharacterController> ourList, List<CharacterController> enemyList)
        {
            ourList.ForEach(i =>
            {
                if (i.GetFightGroup() == null && i.IsDead() == false)
                {
                    List<CharacterController> enemiesThatHaveNoFightGroup = enemyList.Where(j => j.IsDead() == false && j.GetFightGroup() == null).ToList();
                    CharacterController target = GetNearestCharacterAlive(enemiesThatHaveNoFightGroup, i.GetPosition());

                    if (target != null) // Generate new fight group
                    {
                        FightGroup fightGroup = new FightGroup(m_FightGroupId, i, target);
                        fightGroup.StartFight();

                        m_FightGroupList.Add(fightGroup);

                        m_FightGroupId++;
                    }
                }
            });

            // Add enemy that has no group to exist group
            //List<CharacterController> enemiesThatHaveNoGroup = enemyList.Where(j => j.IsDead() == false && j.GetFightGroup() == null).ToList();
            //if (enemiesThatHaveNoGroup.Count > 0)
            //{
                //enemiesThatHaveNoGroup.ForEach(i => 
                //{
                //    JoinNearestEnemyGroup(i);
                //});
            //}
        }
        private void RemoveFightGroup(FightGroup group)
        {
            if (m_FightGroupList.Contains(group))
            {
                m_FightGroupList.Remove(group);
            }
        }

        private void SpawnEnemyController(int id, Vector3 pos, CharacterCamp camp, System.Action<CharacterController> onCharacterLoaded)
        {
            //GameObject prefab = Resources.Load("Prefabs/Enemy/Enemy1") as GameObject;
            //GameObject obj = GameObject.Instantiate(prefab);

            EnemyLoader.S.LoadEnemySync(id, (obj) => 
            {
                obj.name = "Character_" + camp;
                obj.transform.parent = m_BattleField.transform;

                CharacterView characterView = obj.GetComponent<CharacterView>();
                CharacterController controller = new CharacterController(id, characterView, CharacterStateID.Battle, CharacterCamp.EnemyCamp);
                controller.OnEnterBattleField(pos);

                onCharacterLoaded?.Invoke(controller);
            });
        }

        private void ApplyDamage()
        {
            m_OurDamagePersecond = m_TotalEnemyAtk / m_InitOurCharacterCount / m_Const * Random.Range(0.8f, 1.2f);
            m_EnemeyDamagePersecond = m_TotalOurAtk / m_InitEnemeyCharacterCount / m_Const * Random.Range(0.8f, 1.2f);

            m_OurCharacterList.ForEach(i => 
            {
                i.CacheDamage(m_OurDamagePersecond);
            });

            m_EnemyCharacterList.ForEach(i =>
            {
                i.CacheDamage(m_EnemeyDamagePersecond);
            });

            RefressProgress();
        }

        private void RefressProgress()
        {
            float curOurTotalHp = 0;
            m_OurCharacterList.ForEach(i =>
            {
                curOurTotalHp += i.CharacterModel.Hp;
            });

            float curEnemyTotalHp = 0;
            m_EnemyCharacterList.ForEach(i =>
            {
                curEnemyTotalHp += i.CharacterModel.Hp;
            });

            float ourProgress = curOurTotalHp / m_TotalOurHp;
            float enemyProgress = curEnemyTotalHp / m_TotalEnemyHp;

            EventSystem.S.Send(EventID.OnRefreshBattleProgress, ourProgress, enemyProgress);

            if (curOurTotalHp <= 0)
            {
                EventSystem.S.Send(EventID.OnBattleFailed);
                m_IsBattleEnd = true;
            }

            if (curEnemyTotalHp <= 0)
            {
                EventSystem.S.Send(EventID.OnBattleSuccessed);
                m_IsBattleEnd = true;
            }
        }

        List<CharacterController> m_OurCharacterThatHaveNoGroup = new List<CharacterController>();
        List<CharacterController> m_EnemyCharacterThatHaveNoGroup = new List<CharacterController>();

        //private void RefreshFightGroup()
        //{
        //    m_OurCharacterThatHaveNoGroup.Clear();
        //    m_EnemyCharacterThatHaveNoGroup.Clear();

        //    for (int i = m_FightGroupList.Count - 1; i >= 0; i--)
        //    {
        //        FightGroup group = m_FightGroupList[i];

        //        if (group.enemyMainCharacter == null || group.enemyMainCharacter.IsDead())
        //        {
        //            if (group.otherEnemyCharacterList.Count > 0) // Set main enemy form otherEnemyList
        //            {
        //                group.SetEnemyMainCharacter(group.otherEnemyCharacterList.FirstOrDefault());
        //            }
        //            else // There is no character is otherEnemyList
        //            {
        //                if (IsCharacterValid(group.ourMainCharacter))
        //                {
        //                    m_OurCharacterThatHaveNoGroup.Add(group.ourMainCharacter);
        //                    group.ourMainCharacter = null;
        //                }

        //                group.otherOurCharacterList.ForEach(m => 
        //                {
        //                    if (IsCharacterValid(m))
        //                    {
        //                        m_OurCharacterThatHaveNoGroup.Add(m);
        //                    }
        //                });
        //                group.otherOurCharacterList.Clear();
        //            }
        //        }

        //        if (group.ourMainCharacter == null || group.ourMainCharacter.IsDead())
        //        {
        //            if (group.otherOurCharacterList.Count > 0) // Set main our character form otherOurList
        //            {
        //                group.SetOurMainCharacter(group.otherOurCharacterList.FirstOrDefault());
        //            }
        //            else // There is no character is otherOurList
        //            {
        //                if (IsCharacterValid(group.enemyMainCharacter))
        //                {
        //                    m_EnemyCharacterThatHaveNoGroup.Add(group.enemyMainCharacter);
        //                    group.enemyMainCharacter = null;
        //                }

        //                group.otherEnemyCharacterList.ForEach(m =>
        //                {
        //                    if (IsCharacterValid(m))
        //                    {
        //                        m_EnemyCharacterThatHaveNoGroup.Add(m);
        //                    }
        //                });
        //                group.otherEnemyCharacterList.Clear();
        //            }
        //        }

        //        if (group.enemyMainCharacter == null && group.ourMainCharacter == null && group.otherEnemyCharacterList.Count == 0 &&
        //            group.otherOurCharacterList.Count == 0)
        //        {
        //            m_FightGroupList.Remove(group);
        //        }
        //    }

        //    // Character that has no group re-spawn group or join other group
        //    if (m_EnemyCharacterThatHaveNoGroup.Count > 0)
        //    {
        //        if (m_OurCharacterThatHaveNoGroup.Count > 0)
        //        {
        //            SpawnFightGroup(m_OurCharacterThatHaveNoGroup, m_EnemyCharacterThatHaveNoGroup);
        //        }
        //        else
        //        {
        //            m_EnemyCharacterThatHaveNoGroup.ForEach(i => 
        //            {
        //                JoinNearestEnemyGroup(i);
        //            });
        //        }
        //    }
        //    else
        //    {
        //        if (m_OurCharacterThatHaveNoGroup.Count > 0)
        //        {
        //            m_OurCharacterThatHaveNoGroup.ForEach(i =>
        //            {
        //                JoinNearestOurGroup(i);
        //            });
        //        }
        //    }

        //    m_FightGroupList.ForEach(i => 
        //    {

        //    });
        //}

        //private void JoinNearestOurGroup(CharacterController character)
        //{
        //    CharacterController randomTarget = GetNearestCharacterAlive(m_EnemyCharacterList, character.GetPosition());
        //    randomTarget.GetFightGroup().AddOtherOurCharacter(character);
        //}

        //private void JoinNearestEnemyGroup(CharacterController character)
        //{
        //    CharacterController randomTarget = GetNearestCharacterAlive(m_OurCharacterList, character.GetPosition());
        //    randomTarget.GetFightGroup().AddOtherEnemyCharacter(character);
        //}

        private bool IsCharacterValid(CharacterController character)
        {
            return character != null && character.IsDead() == false;
        }
        #endregion
    }


}