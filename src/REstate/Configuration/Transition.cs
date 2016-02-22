﻿namespace REstate.Configuration
{
    public class Transition : ITransition
    {
        public Transition()
        {
            IsActive = true;
        }

        public string GuardName { get; set; }
        public int MachineDefinitionId { get; set; }

        public string StateName { get; set; }

        public string TriggerName { get; set; }

        public string ResultantStateName { get; set; }

        public bool IsActive { get; set; }
    }
}