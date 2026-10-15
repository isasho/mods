using System.Collections.Generic;

using Godot;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

using sts2mod.Cards;

namespace sts2mod.Characters
{
    public sealed class BloodbinderCardPool : CardPoolModel
    {
        public override string Title => "bloodbinder";
        public override string EnergyColorName => "ironclad";
        public override string CardFrameMaterialPath => "card_frame_red";
        public override Color DeckEntryCardColor => new Color("8B0000");
        public override Color EnergyOutlineColor => new Color("801212FF");
        public override bool IsColorless => false;

        protected override CardModel[] GenerateAllCards()
        {
            return new CardModel[]
            {
                ModelDb.Card<Blood_Attack>(),
                ModelDb.Card<Blood_Mark>(),
                ModelDb.Card<Blood_Rage_Slash>(),
                ModelDb.Card<Slaughter_Feast>(),
                ModelDb.Card<Blood_Desire>(),
                ModelDb.Card<Blood_Frenzy>(),
                ModelDb.Card<Blood_Sacrifice>(),
                ModelDb.Card<Blood_Strike>(),
                ModelDb.Card<Blood_Guard>(),
                ModelDb.Card<Blood_Debt>(),
                ModelDb.Card<Blood_End_Sacrifice>(),
                ModelDb.Card<Blood_God_Judgment>(),
                ModelDb.Card<Blood_Contract>(),
                ModelDb.Card<Blood_Ritual>(),
                ModelDb.Card<Blood_Bath>(),
                ModelDb.Card<Blood_Life_Exchange>(),
                ModelDb.Card<Blood_River>(),
                ModelDb.Card<Blood_God_Descends>(),
                ModelDb.Card<Blood_Throne>(),
                ModelDb.Card<Death_Wish>(),
                ModelDb.Card<Crimson_Eternity>(),
                ModelDb.Card<Blood_Slash>(),
                ModelDb.Card<Wrist_Cut>(),
                ModelDb.Card<Crimson_Lunge>(),
                ModelDb.Card<Lacerate>(),
                ModelDb.Card<Blood_Blade>(),
                ModelDb.Card<Hamstring>(),
                ModelDb.Card<Heavy_Wound>(),
                ModelDb.Card<Hunt>(),
                ModelDb.Card<Blood_Whirl>(),
                ModelDb.Card<Double_Slash>(),
                ModelDb.Card<Decapitate>(),
                ModelDb.Card<Brutality>(),
                ModelDb.Card<Blood_Storm>(),
                ModelDb.Card<Blood_Let>(),
                ModelDb.Card<Pain_Strike>(),
                ModelDb.Card<Sanguine_Strike>(),
                ModelDb.Card<Pursue>(),
                ModelDb.Card<Blood_Deal>(),
                ModelDb.Card<Coagulate>(),
                ModelDb.Card<Oppress>(),
                ModelDb.Card<Draw_Blood>(),
                ModelDb.Card<Blood_Transfusion>(),
                ModelDb.Card<Blood_Offering>(),
                ModelDb.Card<Life_Conversion>(),
                ModelDb.Card<Quick_Clot>(),
                ModelDb.Card<Drain_Blood>(),
                ModelDb.Card<Blood_Recycle>(),
                ModelDb.Card<Berserk_Stance>(),
                ModelDb.Card<Provoke>(),
                ModelDb.Card<Pain_Empower>(),
                ModelDb.Card<Blood_Resonance>(),
                ModelDb.Card<Blood_Circulation>(),
                ModelDb.Card<Life_Swap>(),
                ModelDb.Card<Pain_Surge>(),
                ModelDb.Card<Crimson_Recovery>(),
                ModelDb.Card<Wound_Worsen>(),
                ModelDb.Card<Loss_of_Control>(),
                ModelDb.Card<Blood_Instinct>(),
                ModelDb.Card<Blood_Will>(),
                ModelDb.Card<Fury_Awakening>(),
                ModelDb.Card<Endless_Rage>(),
                ModelDb.Card<Blood_Pact>(),
                ModelDb.Card<Blood_Burst>(),
                ModelDb.Card<Execution>(),
                ModelDb.Card<Crimson_Storm>(),
                ModelDb.Card<Condemn>(),
                ModelDb.Card<Body_Rend>(),
                ModelDb.Card<Berserk_Charge>(),
                ModelDb.Card<Blood_Loss_Strike>(),
                ModelDb.Card<Blood_Reap>(),
                ModelDb.Card<Garrote>(),
                ModelDb.Card<Blood_Judgment>(),
                ModelDb.Card<Rage_Blow>(),
                ModelDb.Card<Blood_Price>(),
                ModelDb.Card<Wound_Rip>(),
                ModelDb.Card<Crimson_Imprint>(),
                ModelDb.Card<Life_Drain>(),
                ModelDb.Card<Major_Bloodletting>(),
                ModelDb.Card<Blood_Pool>(),
                ModelDb.Card<Blood_Shield>(),
                ModelDb.Card<Fury_Infusion>(),
                ModelDb.Card<Blood_Burn>(),
                ModelDb.Card<Life_Siphon>(),
                ModelDb.Card<Blood_Moon_Descends>(),
                ModelDb.Card<Blood_Feast>(),
                ModelDb.Card<Combat_Frenzy>(),
                ModelDb.Card<Blood_for_Blood>(),
                ModelDb.Card<Blood_God_Acolyte>(),
                ModelDb.Card<Rage_Strike>(),
                ModelDb.Card<Bloodthirst>(),
                ModelDb.Card<Savage_Blow>(),
                ModelDb.Card<Crimson_Impulse>(),
                ModelDb.Card<Blood_Rush>(),
                ModelDb.Card<Critical_Blow>(),
                ModelDb.Card<Rage_Release>(),
                ModelDb.Card<Crimson_Aura>(),
                ModelDb.Card<Blood_Boil>(),
                ModelDb.Card<Blood_Rift>(),
            };
        }
    }
}
