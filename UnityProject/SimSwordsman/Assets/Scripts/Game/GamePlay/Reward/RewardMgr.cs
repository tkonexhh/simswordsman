using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum RewardItemType
    {
        Item,//��Ʒ
        Armor,//����
        Arms,//����
        Kongfu,//�书�ؼ�
        Medicine,//ҩ
        Food,//ʳ��
        Coin,//ͭǮ
        TowerCoin,//伏魔币
        Exp_Role,//���Ӿ���
        Exp_Kongfu,//������
    }
    public class RewardMgr : TSingleton<RewardMgr>
    {
        RewardBase CreateReward(RewardItemType type, int id, int count)
        {
            switch (type)
            {
                case RewardItemType.Item:
                    return new ItemReward(id, count);
                case RewardItemType.Armor:
                    return new ArmorReward(id, count);
                case RewardItemType.Arms:
                    return new ArmsReward(id, count);
                case RewardItemType.Kongfu:
                    return new KongfuReward(id, count);
                case RewardItemType.Medicine:
                    return new MedicineReward(id, count);
                case RewardItemType.Food:
                    return new FoodsReward(count);
                case RewardItemType.Coin:
                    return new CoinReward(count);
                case RewardItemType.Exp_Role:
                    return new Exp_RoleReward(id, count);
                case RewardItemType.Exp_Kongfu:
                    return new Exp_KongfuRweard(id, count);
                case RewardItemType.TowerCoin:
                    return new TowerCoinReward(count);
                default:

                    return null;
            }
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public RewardBase GetRewardBase(RewardItemType type, int id, int count = 1)
        {
            return CreateReward(type, id, count);
        }

        /// <summary>
        /// Param��ʽΪRewardItemType��string|id|����
        /// </summary>
        /// <param name="param">RewardItemType,id,count</param>
        /// <returns></returns>
        public RewardBase GetRewardBase(string param)
        {
            RewardItemType rewardItemType;
            string[] sp = param.Split('|');
            if (!Enum.TryParse(sp[0], out rewardItemType))
                return null;
            switch (rewardItemType)
            {
                case RewardItemType.Item:
                case RewardItemType.Armor:
                case RewardItemType.Arms:
                case RewardItemType.Kongfu:
                case RewardItemType.Medicine:
                    return CreateReward(rewardItemType, int.Parse(sp[1]), GetRewardCount(sp[2]));
                case RewardItemType.Food:
                case RewardItemType.Coin:
                case RewardItemType.TowerCoin:
                    return CreateReward(rewardItemType, 0, int.Parse(sp[1]));
                case RewardItemType.Exp_Role:
                case RewardItemType.Exp_Kongfu:
                    return CreateReward(rewardItemType, 0, GetRewardCount(sp[1]));
                default:
                    break;
            }
            return null;
        }
        string[] temp;
        int GetRewardCount(string param)
        {
            if (param.Contains("_"))
            {
                temp = param.Split('_');
                return RandomHelper.Range(int.Parse(temp[0]), int.Parse(temp[1]) + 1);
            }
            else
            {
                return int.Parse(param);
            }
        }

        // public RewardBase GetRewardBase(string type, int id, int count)
        // {
        //     RewardItemType rewardItemType;
        //     if (!Enum.TryParse(type, out rewardItemType))
        //         return null;
        //     return CreateReward(rewardItemType, id, count);
        // }


        //public RewardBase AcceptRewardByConfig(string type, int id, int count)
        //{
        //    RewardBase reward = GetRewardBase(type, id, count);
        //    reward.AcceptReward();
        //    return reward;
        //}


        public long GetOwnRewardCount(RewardBase reward)
        {
            switch (reward.RewardItem)
            {
                case RewardItemType.Coin:
                    return GameDataMgr.S.GetPlayerData().GetCoinNum();
                case RewardItemType.Food:
                    return GameDataMgr.S.GetPlayerData().GetFoodNum();
                case RewardItemType.Armor:
                    return MainGameMgr.S.InventoryMgr.GetItemCount(new ArmorItem((ArmorType)reward.KeyID.Value, Step.One));
                case RewardItemType.Arms:
                    return MainGameMgr.S.InventoryMgr.GetItemCount(new ArmsItem((ArmsType)reward.KeyID.Value, Step.One));
                case RewardItemType.Kongfu:
                    return MainGameMgr.S.InventoryMgr.GetItemCount(new KungfuItem((KungfuType)reward.KeyID.Value));
                case RewardItemType.Item:
                    return MainGameMgr.S.InventoryMgr.GetItemCount(new PropItem((RawMaterial)reward.KeyID.Value));
            }
            return 0;
        }

    }
}