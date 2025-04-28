using System;
using System.Collections.Generic;

namespace PaladinProject
{
    public class Spellbook
    {
        public Dictionary<string, string> Spells { get; private set; }

        public Spellbook()
        {
            Spells = new Dictionary<string, string>
            {
                // Core Abilities
                { "Word of Glory (WoG)", "Calls down the Light to heal you. Healing is increased the lower your health is. Costs 3 Holy Power." },
                { "Flash of Light (FoL)", "Quick-casting heal. Avoid using during active tanking as you cannot block, parry, or dodge while casting." },
                { "Shield of the Righteous (SotR)", "Slams enemies in front of you with Holy damage and significantly increases your armor. Costs 3 Holy Power." },
                { "Judgment (J)", "Deals moderate Holy damage and generates 1 Holy Power. Applies a debuff when talented into Greater Judgment." },
                { "Consecration (Cons)", "Blesses the ground beneath you, dealing Holy damage over time and granting damage reduction while standing in it." },
                { "Divine Shield (DS)", "Grants total immunity to all damage, spells, and crowd control effects for 8 seconds. Causes Forbearance." },
                { "Sense Undead", "Reveals Undead creatures on your minimap." },
                { "Hammer of Justice (HoJ)", "Stuns an enemy for 6 seconds. Useful for interrupting and controlling enemies." },
                { "Hand of Reckoning (Taunt)", "Forces a target to attack you and increases threat generation temporarily." },
                { "Redemption (Res)", "Out-of-combat resurrection spell to revive dead allies." },
                { "Intercession (BRes)", "In-combat resurrection that costs 3 Holy Power. Shares cooldowns with other Battle Res abilities." },

                // Passive Abilities
                { "Mastery: Divine Bulwark", "Increases your block chance and grants passive damage reduction while standing in Consecration." },
                { "Aegis of Light", "Passively increases your health and armor as a Protection Paladin." },
                { "Riposte", "Converts Critical Strike rating from gear into Parry chance." },
                { "Seasoned Warhorse", "Increases Divine Steed duration by 2 seconds." },

                // Hero Talents - Templar (Key Abilities)
                { "Light's Guidance (LG)", "After Eye of Tyr, allows casting Hammer of Light which deals AoE Holy damage and summons Empyrean Hammers." },
                { "Light's Deliverance (LD)", "Stacking buff that enables a free Hammer of Light after reaching 60 Empyrean Hammer stacks." },

                // Hero Talents - Templar (Choice Nodes)
                { "Zealous Vindication (ZV)", "Hammer of Light summons two additional Empyrean Hammers on your target." },
                { "For Whom the Bell Tolls (FWBT)", "Divine Toll boosts your next three Judgments' damage by up to 100%, reduced by number of targets hit." },
                { "Bonds of Fellowship", "Take 20% less damage from Blessing of Sacrifice; gain run speed whenever your ally takes damage." },
                { "Unrelenting Charger", "Divine Steed lasts 2 seconds longer and grants 30% bonus movement speed initially." },
                { "Endless Wrath (EW)", "Empyrean Hammer attacks have a 10% chance to reset Hammer of Wrath cooldown and allow its use on any target." },
                { "Sanctification", "Judgment increases Empyrean Hammer's damage by 10%. Buff stacks individually." },

                // Hero Talents - Templar (Passives)
                { "Shake the Heavens (StH)", "Hammer of Light periodically summons Empyrean Hammers during its duration." },
                { "Wrathful Descent (WD)", "Critical strikes from Empyrean Hammers deal splash damage and reduce enemy damage dealt." },
                { "Sacrosanct Crusade (SC)", "Eye of Tyr grants a shield; Hammer of Light heals and provides an absorb if overhealing." },
                { "Higher Calling (HC)", "Judgment, Hammer of Wrath, and fillers extend Shake the Heavens buff." },
                { "Hammerfall (HF)", "Shield of the Righteous and Word of Glory call down Empyrean Hammers." },
                { "Undisputed Ruling (UR)", "Hammer of Light grants Shield of the Righteous, Haste buff, and generates Holy Power via Eye of Tyr." },

                // Hero Talents - Lightsmith (Key Abilities)
                { "Holy Bulwark", "A shield spell that dynamically scales based on target's health." },
                { "Sacred Weapon", "A weapon buff that scales from your stats and all its damage counts as yours." },
                { "Blessing of the Forge", "Grants Sacred Weapon to yourself and an ally during Avenging Wrath, enabling echo effects on spenders." },

                // Hero Talents - Lightsmith (Choice Nodes)
                { "Rite of Sanctification", "Weapon enchant increasing armor and Strength. Preferred enchant while playing Lightsmith." },
                { "Rite of Adjuration", "Weapon enchant increasing stamina and providing burst healing on Holy Power spenders." },
                { "Divine Guidance (DG)", "Holy Power spenders empower Consecration, increasing its healing/damage." },
                { "Blessed Assurance (BA)", "Holy Power spenders empower Hammer of the Righteous or Blessed Hammer." },
                { "Divine Inspiration (DI)", "Your abilities have a chance to grant Holy Bulwark or Sacred Weapon to allies automatically." },
                { "Forewarning", "Reduces Holy Bulwark/Sacred Weapon cooldown by 20%." },
                { "Authoritative Rebuke (AR)", "Interrupting a spell reduces Rebuke cooldown, doubled if holding Holy Bulwark or Sacred Weapon." },
                { "Tempered in Battle (TB)", "Transfers 100% overhealing while Holy Bulwark is active; redistributes health on Sacred Weapon." },

                // Hero Talents - Lightsmith (Passives)
                { "Solidarity", "Casting Holy Bulwark/Sacred Weapon on an ally also benefits you and vice versa." },
                { "Laying Down Arms", "When buffs expire, reduce Lay on Hands cooldown and gain Shining Light." },
                { "Shared Resolve", "Your active aura becomes 33% more effective on allies affected by Holy Bulwark/Sacred Weapon." },
                { "Valiance", "Consuming Shining Light reduces recharge time of Holy Bulwark/Sacred Weapon." },
                { "Hammer and Anvil", "Judgment critical strikes deal bonus Holy splash damage around the target." },

                // Class Talents (Selected Highlights)
                { "Lay on Hands (LoH)", "Instantly heals a friendly target for your maximum health. Causes Forbearance." },
                { "Devotion Aura (Devo)", "Provides passive 3% damage reduction to all allies within 40 yards." },
                { "Concentration Aura", "Reduces silence and interrupt effects on party members." },
                { "Hammer of Wrath (HoW)", "Deals Holy damage to targets below 20% HP or usable during Avenging Wrath." },
                { "Cleanse Toxins", "Removes all poison and disease effects from a friendly target." },
                { "Rebuke", "Interrupts spellcasting on enemies for 3 seconds." },
                { "Divine Steed", "Summons a holy steed, increasing your movement speed for 5 seconds." },
                { "Divine Toll (DT)", "Instantly throws Avenger's Shield at multiple enemies and generates Holy Power." },
                { "Blessing of Protection (BoP)", "Grants immunity to physical damage for 8 seconds. Causes Forbearance." },
                { "Blessing of Sacrifice (Sac)", "Redirects 30% of an ally’s damage to you for 12 seconds." },
                { "Blessing of Freedom (BoF)", "Removes movement impairing effects and grants immunity to them." },

                // Protection Spec Talents (Selected Highlights)
                { "Avenger's Shield (AS)", "Hurls your shield to deal Holy damage, silence, and interrupt a target." },
                { "Holy Shield (HS)", "Allows you to block spells and increases block chance by 20%." },
                { "Grand Crusader (GC)", "Avoiding attacks or using fillers gives a chance to reset Avenger's Shield cooldown." },
                { "Shining Light (SL)", "Every three Shield of the Righteous casts give a free Word of Glory." },
                { "Ardent Defender (AD)", "Reduces all damage by 20% and can prevent death once while active." },
                { "Guardian of Ancient Kings (GoAK)", "Reduces all damage taken by 50% for 8 seconds." },
                { "Eye of Tyr (EoT)", "AoE that reduces damage enemies deal by 25%." },
                { "Moment of Glory (MoG)", "Empowers Avenger's Shield for major AoE damage and shield generation." },
                { "Final Stand (FS)", "Turns Divine Shield into an AoE taunt while immune." },
                { "Bulwark of Righteous Fury (BoRF)", "Each target hit by Avenger's Shield buffs your Shield of the Righteous damage." },
                { "Righteous Protector (RP)", "Spending Holy Power reduces Guardian and Avenging Wrath cooldowns." }
            };
        }
    }
}
