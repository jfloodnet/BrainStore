using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsciousnessStream.Events
{
    public class AffectivEmoStateUpdated : EmoEvent
    {
        public float ExcitementShortTermScore;
        public float ExcitementLongTermScore;
        public float EngagementBoredomScore;
        public float FrustrationScore;
        public float MeditationScore;

        public AffectivEmoStateUpdated(
            uint userId,
            float excitementShortTermScore,
            float excitementLongTermScore,
            float engagementBoredomScore,
            float frustrationScore,
            float meditationScore)
            : base(userId)
        {
            this.ExcitementShortTermScore = excitementShortTermScore;
            this.ExcitementLongTermScore = excitementLongTermScore;
            this.EngagementBoredomScore = engagementBoredomScore;
            this.FrustrationScore = frustrationScore;
            this.MeditationScore = meditationScore;
        }
    }
}
