using System;
using System.Collections.Generic;

namespace Anthology.SimulationManager
{
    /**
     * Concrete example implementation of KnowledgeSim using the Lyra API 
     * Currently unimplemented until the Lyra API is complete
     */
    public class LyraKS : KnowledgeSim
    {
        /** TODO */
        public override void Init(string pathFile = "")
        {
            throw new NotImplementedException();
        }

        /** TODO */
        public override void LoadNpcs(Dictionary<string, NPC> npcs)
        {
            throw new NotImplementedException();
        }

        /** TODO */
        public override void PushUpdatedNpc(NPC npc)
        {
            throw new NotImplementedException();
        }

        /** TODO */
        public override void Run(int steps = 1)
        {
            throw new NotImplementedException();
        }

        /** TODO */
        public override void UpdateNpc(NPC npc)
        {
            throw new NotImplementedException();
        }
    }
}
