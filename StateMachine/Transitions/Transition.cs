using System;

namespace HJF.StateMachine
{
    public class Transition
    {
        public IState To { get; set; }
        public Func<bool> Condition { get; set; }
    }
}