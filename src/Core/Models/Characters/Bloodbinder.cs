using System.Collections.Generic;

using Godot;

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

using sts2mod.Cards;
using sts2mod.Relics;
using sts2mod.PotionPools;
using sts2mod.RelicPools;

namespace sts2mod.Characters
{
    // 绯血者（Bloodbinder）— 初始70HP/99金币。以流血和痛楚为核心，用生命换取力量的近战角色。
    public sealed class Bloodbinder : CharacterModel
    {
        public override CharacterGender Gender => CharacterGender.Feminine;

        protected override CharacterModel? UnlocksAfterRunAs => null;

        public override Color NameColor => new Color("CC0000");

        public override int StartingHp => 70;

        public override int StartingGold => 99;

        public override CardPoolModel CardPool => ModelDb.CardPool<BloodbinderCardPool>();

        public override PotionPoolModel PotionPool => ModelDb.PotionPool<BloodbinderPotionPool>();

        public override RelicPoolModel RelicPool => ModelDb.RelicPool<BloodbinderRelicPool>();

        public override IEnumerable<CardModel> StartingDeck => new CardModel[]
        {
            ModelDb.Card<Blood_Strike>(),
            ModelDb.Card<Blood_Strike>(),
            ModelDb.Card<Blood_Strike>(),
            ModelDb.Card<Blood_Strike>(),
            ModelDb.Card<Blood_Guard>(),
            ModelDb.Card<Blood_Guard>(),
            ModelDb.Card<Blood_Guard>(),
            ModelDb.Card<Blood_Guard>(),
            ModelDb.Card<Blood_Mark>(),
            ModelDb.Card<Blood_Attack>(),
        };

        public override IReadOnlyList<RelicModel> StartingRelics =>
            new RelicModel[] { ModelDb.Relic<BloodPendant>() };

        public override float AttackAnimDelay => 0.15f;

        public override float CastAnimDelay => 0.25f;

        public override Color EnergyLabelOutlineColor => new Color("801212FF");

        // 角色选择界面资产 — 选择图标使用 mod 自带 PNG，其余暂时指向 Ironclad 占位。
        protected override string CharacterSelectIconPath =>
            ImageHelper.GetImagePath("packed/character_select/char_select_bloodbinder.png");

        protected override string CharacterSelectLockedIconPath =>
            ImageHelper.GetImagePath("packed/character_select/char_select_ironclad_locked.png");

        protected override string IconPath =>
            SceneHelper.GetScenePath("ui/character_icons/bloodbinder_icon");

        protected override string MapMarkerPath =>
            ImageHelper.GetImagePath("packed/map/icons/map_marker_ironclad.png");

        // 音频 — 暂时使用 Ironclad 的音效占位。
        public override string CharacterSelectSfx =>
            "event:/sfx/characters/ironclad/ironclad_select";

        public override string CharacterTransitionSfx =>
            "event:/sfx/ui/wipe_ironclad";

        public override Color DialogueColor => new Color("590000");

        public override VfxColor SpeechBubbleColor => VfxColor.Red;

        public override Color MapDrawingColor => new Color("CC0000");

        public override Color RemoteTargetingLineColor => new Color("E15847FF");

        public override Color RemoteTargetingLineOutline => new Color("801212FF");

        public override List<string> GetArchitectAttackVfx()
        {
            return new List<string>
            {
                "vfx/vfx_attack_slash",
                "vfx/vfx_bloody_impact",
                "vfx/vfx_attack_blunt",
                "vfx/vfx_heavy_blunt",
                "vfx/vfx_rock_shatter",
            };
        }

        // 角色动画状态机 — 复用铁血战士（Ironclad）的 spine 动画名，含 attack_heavy 重击。
        public override CreatureAnimator GenerateAnimator(MegaSprite controller)
        {
            AnimState idle = new AnimState("idle_loop", isLooping: true);
            AnimState cast = new AnimState("cast");
            AnimState attack = new AnimState("attack");
            AnimState heavyAttack = new AnimState("attack_heavy");
            AnimState hurt = new AnimState("hurt");
            AnimState die = new AnimState("die");
            AnimState relaxed = new AnimState("relaxed_loop", isLooping: true);

            cast.NextState = idle;
            attack.NextState = idle;
            heavyAttack.NextState = idle;
            hurt.NextState = idle;
            relaxed.AddBranch("Idle", idle);

            CreatureAnimator animator = new CreatureAnimator(idle, controller);
            animator.AddAnyState("Idle", idle);
            animator.AddAnyState("Dead", die);
            animator.AddAnyState("Hit", hurt);
            animator.AddAnyState("Attack", attack);
            animator.AddAnyState("Cast", cast);
            animator.AddAnyState("heavyAttack", heavyAttack);
            animator.AddAnyState("PowerUp", cast);
            animator.AddAnyState("Relaxed", relaxed);
            return animator;
        }
    }
}
