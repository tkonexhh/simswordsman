using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class BattleFieldMgr : MonoBehaviour, IMgr
    {
        private BattleField m_BattleField = null;
        private const int BattleMaxTime = 30;

        private List<CharacterController> m_OurCharacterList = new List<CharacterController>();
        private List<CharacterController> m_EnemyCharacterList = new List<CharacterController>();
        //private List<CharacterController> m_OurNotFightingCharacterList = new List<CharacterController>();
        //private List<CharacterController> m_EnemyNotFightingCharacterList = new List<CharacterController>();
        [SerializeField] private List<FightGroup> m_FightGroupList = new List<FightGroup>();

        private double m_TotalEnemyAtk = 0;
        private double m_TotalOurAtk = 0;
        private double m_TotalEnemyHp = 0;
        private double m_TotalOurHp = 0;
        private double m_EnemeyDamagePersecond = 1;
        private double m_OurDamagePersecond = 1;
        private int m_InitOurCharacterCount;
        private int m_InitEnemeyCharacterCount;
        private int m_Const = 10;

        private float m_ApplyDamageTime = 0f;
        private float m_ApplyDamageInterval = 0.1f;

        private bool m_IsBattleBegin = false;
        private bool m_IsBattleEnd = false;

        //private Vector2 m_BattleAreaRightTop = new Vector2(44, 1.5f);
        //private Vector2 m_BattleAreaLeftBottom = new Vector2(38, -1.1f);

        private int m_FightGroupId = 1;

        private List<HerbType> m_SelectedHerbList = new List<HerbType>();

        public List<CharacterController> OurCharacterList { get => m_OurCharacterList; }
        public List<CharacterController> EnemyCharacterList { get => m_EnemyCharacterList; }
        public BattleField BattleField { get { return m_BattleField; } }
        //public List<CharacterController> OurNotFightingCharacterList { get => m_OurNotFightingCharacterList; }
        //public List<CharacterController> EnemyNotFightingCharacterList { get => m_EnemyNotFightingCharacterList; }

        public bool IsBattleing { get { return m_IsBattleBegin; } }

        private int m_AllEnemyCount = 0;
        private int m_LoadedEnemyCount = 0;
        public delegate void OnSpawnCharacterComplete(List<CharacterController> owrControllers, List<CharacterController> enemyControllers);
        public delegate void OnBattleExit(List<CharacterController> owrControllers, List<CharacterController> enemyControllers);
        public OnSpawnCharacterComplete onSpawnOwerCharacterComplete;
        public OnBattleExit onBattleExit;

        #region IMgr

        public void OnInit()
        {
            m_BattleField = FindObjectOfType<BattleField>();
            m_BattleField.Init();
            m_BattleField.CalculateBattleArea();

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

            m_EnemyCharacterList.ForEach(i =>
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

        public EnemyInfo GetEnemyInfo(int id)
        {
            return TDEnemyConfigTable.GetEnemyInfo(id);
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

        private void RegisterEventsWhenEnter()
        {
            EventSystem.S.Register(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void UnregisterEventsWhenExit()
        {
            EventSystem.S.UnRegister(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnEnterBattle:
                    List<EnemyConfig> enemies = (List<EnemyConfig>)param[0];
                    List<CharacterController> ourSelectedCharacters = (List<CharacterController>)param[1];
                    if (param.Length == 3)
                    {
                        m_SelectedHerbList = (List<HerbType>)param[2];
                    }
                    OnEnterBattle(enemies, ourSelectedCharacters);
                    StartCoroutine("BattleCountdown", (BattleMaxTime));
                    break;
                case (int)EventID.OnExitBattle:
                    OnExitBattle();
                    break;
                case (int)EventID.OnCharacterInFightGroupDead:
                    FightGroup fightGroup = (FightGroup)param[0];
                    Debug.Assert(fightGroup != null, "Fight group is null");
                    OnFightGroupCharacterDead(fightGroup);
                    break;
            }
        }

        private void OnEnterBattle(List<EnemyConfig> enemies, List<CharacterController> ourSelectedCharacters)
        {
            m_IsBattleBegin = true;
            m_IsBattleEnd = false;

            m_TotalEnemyAtk = 0;
            m_TotalOurAtk = 0;
            m_TotalEnemyHp = 0;
            m_TotalOurHp = 0;

            m_AllEnemyCount = enemies.Count;
            m_LoadedEnemyCount = 0;

            SpawnOurCharacter(ourSelectedCharacters);

            enemies.ForEach(i =>
            {
                if (i.ConfigId == ArenaDefine.ArenaEnemyID)
                {
                    var characterEnemyConfig = i as CharacterEnemyConfig;
                    SpawnEnemyCharacter(characterEnemyConfig.Quality, characterEnemyConfig.HeadID, characterEnemyConfig.BodyID, i.Atk);
                }
                else
                {
                    SpawnEnemyCharacter(i.ConfigId, i.Number, i.Atk);
                }

            });
            MusicMgr.S.PlayBattleMusic();

            if (onSpawnOwerCharacterComplete != null)
                onSpawnOwerCharacterComplete.Invoke(m_OurCharacterList, m_EnemyCharacterList);

            RegisterEventsWhenEnter();
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
            if (onBattleExit != null)
                onBattleExit.Invoke(m_OurCharacterList, m_EnemyCharacterList);

            m_IsBattleBegin = false;
            m_OurCharacterList.ForEach(i =>
            {
                Destroy(i.CharacterView.gameObject);
                //i.OnExitBattleField();
                //i.SetCurTask(null);
            });
            m_OurCharacterList.Clear();
            m_EnemyCharacterList.Clear();

            m_FightGroupList.ForEach(i => i.Release());
            m_FightGroupList.Clear();

            EnemyLoader.S.ReleaseAll();

            m_BattleField.OnBattleEnd();

            MusicMgr.S.PlayMenuMusic();

            UnregisterEventsWhenExit();
        }

        private void SpawnOurCharacter(List<CharacterController> characters)
        {
            m_OurCharacterList.Clear();
            characters.ForEach(i =>
            {
                CharacterItem characterItem = MainGameMgr.S.CharacterMgr.CharacterDataWrapper.GetCharacterItem(i.CharacterId);
                CharacterController controller = SpawnCharacterController(characterItem);
                if (controller != null)
                {
                    m_OurCharacterList.Add(controller);
                }
            });

            m_OurCharacterList.ForEach(i =>
            {
                i.ShowBody();

                Vector3 pos = m_BattleField.GetOurCharacterPos();
                i.OnEnterBattleField(pos);
                float baseAtk = i.CharacterModel.GetBaseAtkValue();
                float atkEnhance = m_SelectedHerbList.Any(j => j == HerbType.ChiDanZhuangQiWan) ? TDHerbConfigTable.GetEffectParam((int)HerbType.ChiDanZhuangQiWan) : 0;
                float hpEnhance = m_SelectedHerbList.Any(j => j == HerbType.LianHuaQingShenLu) ? TDHerbConfigTable.GetEffectParam((int)HerbType.LianHuaQingShenLu) : 0;
                i.CharacterModel.SetHp(baseAtk * (1 + hpEnhance));
                i.CharacterModel.SetMaxHp(baseAtk * (1 + hpEnhance));
                i.CharacterModel.SetAtk(baseAtk * (1 + atkEnhance));
                m_TotalOurAtk += i.CharacterModel.GetAtk();
                m_TotalOurHp += i.CharacterModel.GetHp();
            });
        }

        public CharacterController SpawnCharacterController(CharacterItem characterItem)
        {
            CharacterController controller = null;

            int id = characterItem.id;

            GameObject go = CharacterLoader.S.GetCharacterGo(id, characterItem.quality, characterItem.bodyId, characterItem.GetClanType());
            if (go != null)
            {
                CharacterView characterView = go.GetComponent<CharacterView>();
                controller = new CharacterController(id, characterView, CharacterStateID.Battle);
            }
            else
            {
                Log.e("SpawnCharacterController return null");
            }

            return controller;
        }

        private void SpawnEnemyCharacter(CharacterQuality quality, int headID, int bodyID, long atk)
        {
            CharacterItem characterItem = new CharacterItem(quality, "竞技场", "竞技场弟子", bodyID, headID);
            SpawnCharacterEnemyController(characterItem, m_BattleField.GetEnemyCharacterPos(), CharacterCamp.EnemyCamp, (controller) =>
            {

                m_EnemyCharacterList.Add(controller);

                float debuff = m_SelectedHerbList.Any(j => j == HerbType.JinZhenQingCheGao) ? TDHerbConfigTable.GetEffectParam((int)HerbType.JinZhenQingCheGao) : 0;

                controller.CharacterModel.SetHp(atk * (1 - debuff));
                controller.CharacterModel.SetMaxHp(atk * (1 - debuff));
                controller.CharacterModel.SetAtk(atk * (1 - debuff));

                m_TotalEnemyAtk += controller.CharacterModel.GetAtk();
                m_TotalEnemyHp += controller.CharacterModel.GetHp();
                m_LoadedEnemyCount++;

                if (m_LoadedEnemyCount >= m_AllEnemyCount)
                {
                    OnAllEnemyLoaded();
                }
            });
        }

        private void SpawnEnemyCharacter(int id, int count, long atk)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnEnemyController(id, m_BattleField.GetEnemyCharacterPos(), CharacterCamp.EnemyCamp, (controller) =>
                {
                    m_EnemyCharacterList.Add(controller);

                    float debuff = m_SelectedHerbList.Any(j => j == HerbType.JinZhenQingCheGao) ? TDHerbConfigTable.GetEffectParam((int)HerbType.JinZhenQingCheGao) : 0;

                    controller.CharacterModel.SetHp(atk * (1 - debuff));
                    controller.CharacterModel.SetMaxHp(atk * (1 - debuff));
                    controller.CharacterModel.SetAtk(atk * (1 - debuff));

                    m_TotalEnemyAtk += controller.CharacterModel.GetAtk();
                    m_TotalEnemyHp += controller.CharacterModel.GetHp();
                    m_LoadedEnemyCount++;

                    if (m_LoadedEnemyCount >= m_AllEnemyCount)
                    {
                        OnAllEnemyLoaded();
                    }
                });
            }
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

            ourList.Where(i => i.FightGroup == null && i.IsDead() == false).ToList().ForEach(i => i.CharacterView.PlayIdleAnim());

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

        private void SpawnCharacterEnemyController(CharacterItem characterItem, Vector3 pos, CharacterCamp camp, System.Action<CharacterController> onCharacterLoaded)
        {
            string prefabName = CharacterLoader.GetPrefabName(characterItem.quality, characterItem.bodyId, ClanType.None);
            EnemyLoader.S.LoadEnemySync(prefabName, (obj) =>
            {
                obj.name = "Character_" + camp;
                obj.transform.parent = m_BattleField.transform;

                CharacterView characterView = obj.GetComponent<CharacterView>();
                CharacterController controller = new CharacterController(ArenaDefine.ArenaEnemyID, characterView, CharacterStateID.Battle, CharacterCamp.EnemyCamp);
                controller.CharacterModel.SetCharacterItem(characterItem);//强制设置CharacterItem
                controller.OnEnterBattleField(pos);

                onCharacterLoaded?.Invoke(controller);

            });
        }

        private void ApplyDamage()
        {
            int ourLivingCharacterCount = m_OurCharacterList.Where(i => i.IsDead() == false).ToList().Count;
            ourLivingCharacterCount = Mathf.Max(ourLivingCharacterCount, 1);
            int enemyLivingCharacterCount = m_EnemyCharacterList.Where(i => i.IsDead() == false).ToList().Count;
            enemyLivingCharacterCount = Mathf.Max(enemyLivingCharacterCount, 1);

            m_OurDamagePersecond = m_TotalEnemyAtk / ourLivingCharacterCount / m_Const * UnityEngine.Random.Range(0.9f, 1.1f);
            m_EnemeyDamagePersecond = m_TotalOurAtk / enemyLivingCharacterCount / m_Const * UnityEngine.Random.Range(0.9f, 1.1f);

            m_OurCharacterList.ForEach(i =>
            {
                i.CacheDamage(m_OurDamagePersecond * m_ApplyDamageInterval);
            });

            m_EnemyCharacterList.ForEach(i =>
            {
                i.CacheDamage(m_EnemeyDamagePersecond * m_ApplyDamageInterval);
            });

            RefressProgress();
        }
        private IEnumerator BattleCountdown(int second)
        {
            while (second >= 0)
            {
                if (second <= 5)
                {
                    //TODO
                }
                EventSystem.S.Send(EventID.OnBattleSecondEvent, GameExtensions.SplicingTime(second));
                //m_CombatTime.text = SplicingTime(second);
                yield return new WaitForSeconds(1);
                second--;
                if (second == 0)
                {
                    if (m_TotalOurAtk > m_TotalEnemyAtk)
                    {
                        EventSystem.S.Send(EventID.OnBattleSuccessed);
                        m_IsBattleEnd = true;
                    }
                    else
                    {
                        m_IsBattleEnd = true;
                        EventSystem.S.Send(EventID.OnBattleFailed);
                    }

                    m_OurCharacterList.ForEach(i =>
                    {
                        i.GetBattleState().SetState(BattleStateID.Idle);
                    });

                    m_EnemyCharacterList.ForEach(i =>
                    {
                        i.GetBattleState().SetState(BattleStateID.Idle);
                    });
                }
            }
        }

        public void StopBattleCoroutine()
        {
            StopCoroutine("BattleCountdown");
        }

        private void RefressProgress()
        {
            double curOurTotalHp = 0;
            m_OurCharacterList.ForEach(i =>
            {
                curOurTotalHp += i.CharacterModel.GetHp();
            });

            double curEnemyTotalHp = 0;
            m_EnemyCharacterList.ForEach(i =>
            {
                curEnemyTotalHp += i.CharacterModel.GetHp();
            });

            double ourProgress = curOurTotalHp / m_TotalOurHp;
            double enemyProgress = curEnemyTotalHp / m_TotalEnemyHp;

            EventSystem.S.Send(EventID.OnRefreshBattleProgress, (float)ourProgress, (float)enemyProgress);

            if (curOurTotalHp <= 0)
            {
                EventSystem.S.Send(EventID.OnBattleFailed);
                m_IsBattleEnd = true;
                StopCoroutine("BattleCountdown");
            }

            if (curEnemyTotalHp <= 0)
            {
                EventSystem.S.Send(EventID.OnBattleSuccessed);
                m_IsBattleEnd = true;
                StopCoroutine("BattleCountdown");
            }
        }

        #endregion
    }


}