using System;
using System.Collections.Generic;

namespace HJF.StateMachine
{
    public class StateMachine
    {
        protected IState CurrentState;
        protected Dictionary<Type, List<Transition>> Transitions = new Dictionary<Type, List<Transition>>();
        protected List<Transition> AnyTransitions = new List<Transition>();
        protected List<Transition> CurrentTransitions = new List<Transition>();
        protected readonly List<Transition> EmptyTransitions = new List<Transition>(0);
        
        public void ChangeState(IState nextState)
        {
            CurrentState?.Exit();
            CurrentState = nextState;
            
            Transitions.TryGetValue(nextState.GetType(), out CurrentTransitions);

            if (CurrentTransitions == null)
                CurrentTransitions = EmptyTransitions;
            
            CurrentState?.Enter();
        }

        public void Tick()
        {
            var transition = GetTransition();
            if (transition.To != null)
            {
                ChangeState(transition.To);
            }
            
            CurrentState?.Tick();
        }

        private Transition GetTransition()
        {
            foreach (var transition in AnyTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in CurrentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return new Transition();
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!Transitions.TryGetValue(from.GetType(), out var transitions))
            {
                transitions = new List<Transition>();
                Transitions[from.GetType()] = transitions;
            }
            
            transitions.Add(new Transition
            {
                To = to,
                Condition = condition
            });
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            AnyTransitions.Add(new Transition
            {
                To = to,
                Condition = condition
            });
        }
    }
}