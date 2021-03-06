namespace GameWish.Game
{
    public class StateBattleDead : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        public StateBattleDead(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();
            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            m_Controller.CharacterView.PlayDeadAnim();

            //if (m_Controller.IsOurCharacterCamp())
            //{
            //    AudioManager.S.PlayCharacterDeadSound(m_Controller.CharacterModel.IsWoman(), m_Controller.GetPosition());
            //}

            AudioManager.S.PlayCharacterDeadSound(m_Controller);
        }

        public override void Exit(IBattleStateHander handler)
        {
        }

        public override void Execute(IBattleStateHander handler, float dt)
        {

        }

        public CharacterController GetCharacterController()
        {
            return m_Controller;
        }

        #region Private

        #endregion
    }
}
