namespace HJF.StateMachine
{
    public interface IState
    {
        public void Exit();
        public void Enter();
        public void Tick();

    }
}