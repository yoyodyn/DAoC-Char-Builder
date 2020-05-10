using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ConsoleApp1
{
    public class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }

            if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            string value;
            if (reader.TokenType == JsonTokenType.Number)
            {
                value = string.Format("{0}", reader.GetInt32());
            }
            else
            {
                value = reader.GetString();
            }

            string chkValue = value.ToLower();
            if (chkValue.Equals("true") || chkValue.Equals("yes") || chkValue.Equals("1"))
            {
                return true;
            }
            if (value.ToLower().Equals("false") || chkValue.Equals("no") || chkValue.Equals("0"))
            {
                return false;
            }
            throw new JsonException();
        }
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case true:
                    writer.WriteStringValue("true");
                    break;
                case false:
                    writer.WriteStringValue("false");
                    break;
            }
        }
    }
    public class ResistScoredAttribute : Attribute
    {
        internal ResistScoredAttribute(bool scored)
        {
            this.Scored = scored;
        }
        public bool Scored { get; private set; }
    }

    public class SlotCount : Attribute
    {
        internal SlotCount(int count)
        {
            this.Count = count;
        }
        public int Count { get; private set; }
    }

    public class BonusScored : Attribute
    {
        internal BonusScored(bool scored = false)
        {
            this.Scored = scored;
        }
        public bool Scored { get; private set; }
    }

    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name).GetCustomAttribute<TAttribute>();
        }
    }

    public enum Races
    {
        Avalonian = 1,
        Briton = 2,
        Highlander = 3,
        Saracen = 4,
        Inconnu = 5,
        HalfOgre = 6,

        Celt = 13,
        Elf = 14,
        Firbolg = 15,
        Lurikeen = 16,
        Sylvan = 17,
        Shar = 18,

        Dwarf = 25,
        Kobold = 26,
        Norseman = 27,
        Troll = 28,
        Valkyn = 29,
        Frostalf = 30,

        Minotaur = 37,
    }

    /// <summary>
    /// Class to represent the race of a character and include the default bonuses to stats and resists
    /// </summary>
    public class Race : ICloneable
    {
        public Races race { get; set; }
        public List<StatType> statBonuses { get; set; }
        public List<ResistType> resistBonuses { get; set; }

        public Race(Races newRace)
        {
            race = newRace;
            statBonuses = new List<StatType>();
            resistBonuses = new List<ResistType>();

            switch (newRace)
            {
                case Races.Celt:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 2 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Slash, value = 3 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Spirit, value = 5 });
                    break;
                case Races.Elf:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Slash, value = 2 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Thrust, value = 3 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Spirit, value = 5 });
                    break;
                case Races.Firbolg:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 3 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Slash, value = 2 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Heat, value = 5 });
                    break;
                case Races.Lurikeen:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 5 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Energy, value = 5 });
                    break;
                case Races.Shar:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 5 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Energy, value = 5 });
                    break;
                case Races.Sylvan:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 3 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Thrust, value = 2 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Matter, value = 5 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Energy, value = 5 });
                    break;
                case Races.Minotaur:
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Crush, value = 4 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Heat, value = 3 });
                    resistBonuses.Add(new ResistType() { resist = ResistTypes.Cold, value = 3 });
                    break;
            }
        }

        public Race(Race toCopy)
        {
            race = toCopy.race;
            statBonuses = toCopy.statBonuses?.Select(x => (StatType)x.Clone()).ToList();
            resistBonuses = toCopy.resistBonuses?.Select(x => (ResistType)x.Clone()).ToList();
        }

        public object Clone()
        {
            return new Race(this);
        }
    }
    public enum Classes
    {
        Paladin = 1,
        Armsman = 2,
        Scout = 3,
        Minstrel = 4,
        Theurgist = 5,
        Cleric = 6,
        Wizard = 7,
        Sorcerer = 8,
        Infiltrator = 9,
        Friar = 10,
        Mercenary = 11,
        Necromancer = 12,
        Cabalist = 13,
        Reaver = 19,
        Thane = 21,
        Warrior = 22,
        Shadowblade = 23,
        Skald = 24,
        Hunter = 25,
        Healer = 26,
        Spiritmaster = 27,
        Shaman = 28,
        Runemaster = 29,
        Bonedancer = 30,
        Berserker = 31,
        Savage = 32,
        Heretic = 33,
        Valkyrie = 34,
        Bainshee = 39,
        Eldritch = 40,
        Enchanter = 41,
        Mentalist = 42,
        Blademaster = 43,
        Hero = 44,
        Champion = 45,
        Warden = 46,
        Druid = 47,
        Bard = 48,
        Nightshade = 49,
        Ranger = 50,
        Animist = 55,
        Valewalker = 56,
        Vampiir = 58,
        Warlock = 59,
        MaulerAlb = 60,
        MaulerMid = 61,
        MaulerHib = 62
    }

    public class DaocClass : ICloneable
    {
        public Classes characterClass;
        
        /// <summary>
        /// using the stat type to hold an scoring factor for each stat for the class.
        /// </summary>
        public List<StatType> scoringStats;

        /// <summary>
        /// using the BonusType to hold a scoring factor for each bonus that items could have.
        /// Putting this at the class level right now, but could move to the character level (or both) if feel it needs to be more specialize for each person.
        /// </summary>
        public List<BonusType> scoringBonuses;

        public DaocClass(Classes newClass)
        {
            characterClass = newClass;

            scoringStats = new List<StatType>();
            scoringBonuses = new List<BonusType>();

            switch (characterClass)
            {
                case Classes.Warden:
                    scoringStats.Add(new StatType { stat = StatTypes.Empathy, value = 13 });
                    scoringStats.Add(new StatType { stat = StatTypes.Strength, value = 12 });
                    scoringStats.Add(new StatType { stat = StatTypes.Constitution, value = 11 });
                    scoringStats.Add(new StatType { stat = StatTypes.Dexterity, value = 10 });
                    scoringStats.Add(new StatType { stat = StatTypes.Quickness, value = 10 });

                    scoringBonuses.Add(new BonusType { type = BonusTypes.TOACastSpeed, value = 10 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.TOASpellRange, value = 10 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.TOAMeleeSpeed, value = 10 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.NegativeEffectDurationReduction, value = 10 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.MythicalEnduranceRegen, value = 5 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.BladeturnReinforcement, value = 5 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.Block, value = 5 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.Evade, value = 5 });
                    scoringBonuses.Add(new BonusType { type = BonusTypes.Parry, value = 5 });
                    break;
            }

            foreach (StatTypes s in (StatTypes[])Enum.GetValues(typeof(StatTypes)))
            {
                // default scoring weight is 1 for all stats.
                if (!scoringStats.Any(x => x.stat == s))
                {
                    scoringStats.Add(new StatType { stat = s, value = 0 });
                }
            }

            foreach (BonusTypes b in (BonusTypes[])Enum.GetValues(typeof(BonusTypes)))
            {
                if ((b.GetAttribute<BonusScored>()?.Scored ?? false) &&
                    !scoringBonuses.Any(x => x.type == b))
                {
                    scoringBonuses.Add(new BonusType { type = b, value = 0 });
                }
            }
        }

        public DaocClass(DaocClass toCopy)
        {
            characterClass = toCopy.characterClass;

            scoringStats = toCopy.scoringStats?.Select(x => (StatType)x.Clone()).ToList();
            scoringBonuses = toCopy.scoringBonuses?.Select(x => (BonusType)x.Clone()).ToList();
        }
        public object Clone()
        {
            return new DaocClass(this);
        }
    }

    public class FlagsType : ICloneable
    {
        [JsonConverter(typeof(BooleanConverter))]
        public bool emblemizable { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool dyable { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool no_sell { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool no_trade { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool no_drop { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool no_destroy { get; set; }

        public object Clone()
        {
            return new FlagsType() { dyable = this.dyable, emblemizable = this.emblemizable, no_destroy = this.no_destroy, no_drop = this.no_drop, no_sell = this.no_sell, no_trade = this.no_trade };
        }
    }

    public enum BonusTypes
    {
        [BonusScored(false)]        // scored separately, not individually
        Stats = 1,
        [BonusScored(false)]        // scored separately, not individually
        Skills = 2,
        [BonusScored(true)]
        HitPoints = 4,
        [BonusScored(false)]        // scored separately, not individually
        Resistance = 5,
        [BonusScored(false)]        // scored separately, not individually
        Focus = 6,
        [BonusScored(true)]
        TOAMeleeDamage = 8,
        [BonusScored(true)]
        TOAMagicDamage = 9,
        [BonusScored(true)]
        TOAStyleDamage = 10,
        [BonusScored(true)]
        TOAArcheryRange = 11,
        [BonusScored(true)]
        TOASpellRange = 12,
        [BonusScored(true)]
        TOASpellDuration = 13,
        [BonusScored(true)]
        TOABuffBonus = 14,
        [BonusScored(true)]
        TOADebuffBonus = 15,
        [BonusScored(true)]
        TOAHealBonus = 16,
        [BonusScored(true)]
        TOAFatigue = 17,
        [BonusScored(true)]
        TOAMeleeSpeed = 19,
        [BonusScored(true)]
        TOAArcherySpeed = 20,
        [BonusScored(true)]
        TOACastSpeed = 21,
        [BonusScored(true)]
        ArmorFactor = 22,
        [BonusScored(false)]
        CraftingMinQuality = 23,
        CraftingQuality = 24,
        CraftingSpeed = 25,
        CraftingSkillGain = 26,
        [BonusScored(true)]
        TOAArcheryDamage = 27,
        TOAOvercapp = 28,
        TOAHitPointsCap = 29,
        TOAPowerPoolCap = 30,
        TOAFatigueCap = 31,
        TOAResistancePiece = 32,
        TOAPowerPool = 34,
        TOAArtifact = 35,
        [BonusScored(true)]
        ArrowRecovery = 36,
        [BonusScored(true)]
        SpellPowerCastReduction = 37,
        [BonusScored(true)]
        Concentration = 38,
        [BonusScored(true)]
        SafeFall = 39,
        [BonusScored(true)]
        HealthRegeneration = 40,
        [BonusScored(true)]
        ManaRegeneration = 41,
        PieceAblative = 42,
        DeathExperienceLossReduction = 44,
        [BonusScored(true)]
        NegativeEffectDurationReduction = 46,
        [BonusScored(true)]
        StyleCostReduction = 47,
        [BonusScored(true)]
        ToHitBonus = 48,
        [BonusScored(true)]
        DefensiveBonus = 49,
        [BonusScored(true)]
        BladeturnReinforcement = 50,
        [BonusScored(true)]
        Parry = 51,
        [BonusScored(true)]
        Block = 52,
        [BonusScored(true)]
        Evade = 53,
        ReactionaryStyleDamageBonus = 54,
        [BonusScored(true)]
        MythicalEncumberance = 55,
        MythicalResistanceCap = 57,
        MythicalSeigeSpeed = 58,
        [BonusScored(true)]
        MythicalParry = 60,
        [BonusScored(true)]
        MythicalEvade = 61,
        [BonusScored(true)]
        MythicalBlock = 62,
        [BonusScored(true)]
        MythicalCoin = 63,
        MythicalCapIncrease = 64,
        [BonusScored(true)]
        MythicalCrowdControlDuractionDecrease = 66,
        [BonusScored(true)]
        MythiclEssenceResist = 67,
        MythicalResistAndCap = 68,
        MythicalSeigeDamageAblative = 69,
        [BonusScored(true)]
        MythicalDPS = 71,
        [BonusScored(true)]
        MythicalRealmPoints = 72,
        [BonusScored(true)]
        MythicalSpellFocus = 73,
        [BonusScored(true)]
        MythicalResurectionSicknessReduction = 74,
        MythicalStatAndCapIncrease = 75,
        [BonusScored(true)]
        MythicalHealthRegen = 76,
        [BonusScored(true)]
        MythicalPowerRegen = 77,
        [BonusScored(true)]
        MythicalEnduranceRegen = 78,
        [BonusScored(true)]
        MythicalPhysicalDefence = 80
    }
    public class BonusType : ICloneable
    {
        public BonusTypes type { get; set; }
        public int value { get; set; }
        public int id { get; set; }
        public double utility { get; set; }     // utility is going to be how usefull this bonus is for the sake of scoring a configuration

        public object Clone()
        {
            return new BonusType() { id = this.id, type = this.type, value = this.value };
        }
    }

    public class TypeDataType : ICloneable
    {
        public int armor_factor { get; set; }
        public int clamped_armor_factor { get; set; }
        public int absorption { get; set; }
        public int base_quality { get; set; }
        public float dps { get; set; }
        public float clamped_dps { get; set; }
        public float speed { get; set; }
        public int damage_type { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool two_handed { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool left_handed { get; set; }
        public int skill_used { get; set; }
        public int shield_size { get; set; }
        public int range { get; set; }

        public object Clone()
        {
            return new TypeDataType() 
            { 
                absorption = this.absorption, 
                armor_factor = this.armor_factor, 
                base_quality = this.base_quality, 
                clamped_armor_factor = this.clamped_armor_factor, 
                clamped_dps = this.clamped_dps, 
                damage_type = this.damage_type, 
                dps = this.dps, 
                left_handed = this.left_handed, 
                range = this.range, 
                shield_size = this.shield_size, 
                skill_used = this.skill_used, 
                speed = this.speed, 
                two_handed = this.two_handed 
            };
        }
    }

    public class RequirementsType : ICloneable
    {
        public int level_required { get; set; }
        public List<Classes> usable_by { get; set; }
        public int skill_trains_required { get; set; }
        public int champion_level_required { get; set; }

        public object Clone()
        {
            return new RequirementsType() 
            { 
                level_required = this.level_required, 
                champion_level_required = this.champion_level_required, 
                skill_trains_required = this.skill_trains_required, 
                usable_by = this.usable_by 
            };
        }
    }

    public class AbilitiesType : ICloneable
    {
        public int spell { get; set; }
        public int position { get; set; }
        public int power_level { get; set; }
        public int magic_type { get; set; }
        public int max_charges { get; set; }
        public int requirement_id { get; set; }
        public int level_required { get; set; }

        public object Clone()
        {
            return new AbilitiesType() 
            { 
                level_required = this.level_required, 
                magic_type = this.magic_type, 
                max_charges = this.max_charges, 
                position = this.position, 
                power_level = this.power_level, 
                requirement_id = this.requirement_id, 
                spell = this.spell 
            };
        }
    }
    public class Item : ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category { get; set; }
        public int realm { get; set; }
        public Slots slot { get; set; }
        public int icon { get; set; }
        public int material { get; set; }

        [JsonConverter(typeof(BooleanConverter))]
        public bool artifact { get; set; }
        public FlagsType flags { get; set; }
        public List<BonusType> bonuses { get; set; }

        public int bonus_level { get; set; }
        public string delve_text { get; set; }

        public int sell_value { get; set; }
        public int salvage_amount { get; set; }

        public TypeDataType type_data { get; set; }
        public int dye_type { get; set; }
        public int use_duration { get; set; }
        [JsonConverter(typeof(BooleanConverter))]
        public bool artifcat_pre_leveled { get; set; }

        public RequirementsType requirements { get; set; }

        //    sources { get; set; }
        public List<AbilitiesType> abilities { get; set; }

        public int level
        {
            get
            {
                if ((type_data?.armor_factor ?? 0) > 0)
                {
                    return type_data.armor_factor / 2;
                }
                else
                {
                    return bonus_level;
                }
            }
        }

        public object Clone()
        {
            return new Item()
            {
                abilities = this.abilities?.Select(x => (AbilitiesType)x.Clone()).ToList(),
                artifact = this.artifact,
                artifcat_pre_leveled = this.artifcat_pre_leveled,
                bonuses = this.bonuses?.Select(x => (BonusType)x.Clone()).ToList(),
                bonus_level = this.bonus_level,
                category = this.category,
                delve_text = this.delve_text,
                dye_type = this.dye_type,
                flags = (FlagsType)this.flags?.Clone(),
                icon = this.icon,
                id = this.id,
                material = this.material,
                name = this.name,
                realm = this.realm,
                requirements = (RequirementsType)this.requirements?.Clone(),
                salvage_amount = this.salvage_amount,
                sell_value = this.sell_value,
                slot = this.slot,
                type_data = (TypeDataType)this.type_data?.Clone(),
                use_duration = this.use_duration
            };
        }
    }

    public class Items
    {
        public List<Item> items { get; set; }
        public int count { get; set; }
    }

    public enum Slots
    {
        [SlotCount(1)]
        Helm = 1,
        [SlotCount(1)]
        Hands = 2,
        [SlotCount(1)]
        Feet = 3,
        [SlotCount(1)]
        Jewel = 4,
        [SlotCount(1)]
        Torso = 5,
        [SlotCount(1)]
        Cloak = 6,
        [SlotCount(1)]
        Legs = 7,
        [SlotCount(1)]
        Arms = 8,
        [SlotCount(1)]
        Necklace = 9,
        [SlotCount(1)]
        Belt = 12,
        [SlotCount(2)]
        Bracer = 13,
        [SlotCount(2)]
        Ring = 15,
        [SlotCount(1)]
        Mythrian = 17
    }
    public class SlotType : ICloneable
    {
        public Slots slot { get; set; }
        public Item item { get; set; }
        public bool locked { get; set; }

        public object Clone()
        {
            return new SlotType() { slot = this.slot, item = (Item)this.item?.Clone(), locked = this.locked };
        }
    }

    public enum StatTypes
    {
        Strength = 0,
        Dexterity = 1,
        Constitution = 2,
        Quickness = 3,
        Intellegence = 4,
        Piety = 5,
        Empathy = 6,
        Charisma = 7,
        Acuity = 10
    }

    public class StatType :ICloneable
    {
        public StatTypes stat { get; set; }
        public int statLimit { get; set; }
        public int value { get; set; }
        public int hardCap { get; set; }

        public StatType()
        {
            hardCap = 127;
        }

        public object Clone()
        {
            return new StatType() { stat = this.stat, value = this.value, statLimit = this.statLimit, hardCap = this.hardCap };
        }
        public override string ToString()
        {
            return $"{stat.ToString()}:{value}:{statLimit}";
        }
    }

    public enum ResistTypes
    {
        [ResistScoredAttribute(true)]
        Crush = 1,
        [ResistScoredAttribute(true)]
        Slash = 2,
        [ResistScoredAttribute(true)]
        Thrust = 3,
        [ResistScoredAttribute(false)]
        Siege = 4,
        [ResistScoredAttribute(true)]
        Heat = 10,
        [ResistScoredAttribute(false)]
        Spirit2 = 11,       // Didn't find any 11
        [ResistScoredAttribute(true)]
        Cold = 12,
        [ResistScoredAttribute(false)]
        Matter2 = 13,       // Didn't find any 13
        [ResistScoredAttribute(false)]
        Heat2 = 14,         // Didn't find any 14
        [ResistScoredAttribute(true)]
        Matter = 15,
        [ResistScoredAttribute(true)]
        Body = 16,
        [ResistScoredAttribute(true)]
        Spirit = 17,
        [ResistScoredAttribute(false)]
        Spirit3 = 18,       // Didn't find any 18
        [ResistScoredAttribute(false)]
        Cold2 = 19,         // Didn't find any 19
        [ResistScoredAttribute(false)]
        Energy2 = 20,       // Didn't find any 20
        [ResistScoredAttribute(false)]
        Essence = 21,       // not sure this is used
        [ResistScoredAttribute(true)]
        Energy = 22,
        [ResistScoredAttribute(false)]
        Cold3 = 23,         // Didn't find any 23
        [ResistScoredAttribute(false)]
        Body2 = 25,         // Didn't find any 25
        [ResistScoredAttribute(false)]
        Body3 = 26,         // Didn't find any 26
        [ResistScoredAttribute(false)]
        Body4 = 27          // Didn't find any 27
    }

    public class ResistType : ICloneable
    {
        public ResistTypes resist { get; set; }
        public int value { get; set; }

        public int cap { get; set; }

        public int hardCap { get; set; }

        public object Clone()
        {
            return new ResistType() { cap = this.cap, resist = this.resist, value = this.value, hardCap = this.hardCap };
        }

        public override string ToString()
        {
            return $"{resist.ToString()}:{value}:{cap}";
        }
    }
    /// <summary>
    /// class for character creation.  Should have the stats, skills, attributes, inventory slots, etc.
    /// </summary>
    public class Character : ICloneable
    {
        // going to need to add all the bonues here with values and a utility metric to be used in the scoring somehow.  Might need to change the bonus type to include the utlity score.
        //  At some point these utility values could be class specific, at least for the starting values with the user able to modify them.

        public string characterName { get; set; }
        public DaocClass characterClass { get; set; }
        public Race characterRace { get; set; }
        public List<SlotType> itemSlots { get; set; }
        public List<StatType> stats { get; set; }
        public List<ResistType> resists { get; set; }

        public int hitpoints { get; set; }
        public int hitcap { get; set; }
        public int power { get; set; }
        public int powercap { get; set; }
        public int powerpool { get; set; }
        public int powerpoolcap { get; set; }

        public int level { get; set; }

        public Character(string newName, Classes newClass = Classes.Warden, Races newRace = Races.Celt, int newLevel = 50)
        {
            characterName = newName;
            characterClass = new DaocClass(newClass);
            characterRace = new Race(newRace);
            level = newLevel;

            itemSlots = new List<SlotType>();
            foreach (Slots s in (Slots[])Enum.GetValues(typeof(Slots)))
            {
                int count = s.GetAttribute<SlotCount>()?.Count ?? 1;

                for (int i = 0; i < count; i++)
                {
                    itemSlots.Add(new SlotType { slot = s });
                }
            }

            int statCap = (int)(level * 1.5);
            stats = new List<StatType>();
            foreach (StatTypes s in (StatTypes[])Enum.GetValues(typeof(StatTypes)))
            {
                stats.Add(new StatType { stat = s, statLimit = statCap });
                // Need to add a weight to these for scoring the different classes
            }

            resists = new List<ResistType>();

            // y = 0.816327x + 0.199        // This is not the exact formula.  it will be +/-1 in some levels, but it's correct for level 50 (41)	
            int baseHardCap = (int)(0.816327 * level + 0.199);
            int baseSoftCap = (int)(level / 2 + 1);

            foreach (ResistTypes r in (ResistTypes[])Enum.GetValues(typeof(ResistTypes)))
            {
                if (r.GetAttribute<ResistScoredAttribute>().Scored)
                {
                    ResistType rb = characterRace.resistBonuses.FirstOrDefault(x => x.resist == r);
                    if (rb != null)
                    {
                        resists.Add(new ResistType { resist = r, value = rb.value, cap = baseSoftCap + rb.value, hardCap = baseHardCap + rb.value });
                    }
                    else
                    {
                        resists.Add(new ResistType { resist = r, value = 0, cap = baseSoftCap, hardCap = baseHardCap });
                    }
                }
            }

            hitpoints = 0;
            power = 0;
            powerpool = 0;
            hitcap = (int)(level * 4);
            powercap = (int)(level / 2 + 1);
            powerpoolcap = 0;
        }

        public Character(Character toCopy)
        {
            characterName = toCopy.characterName;
            hitpoints = toCopy.hitpoints;
            power = toCopy.power;
            powerpool = toCopy.powerpool;
            hitcap = toCopy.hitcap;
            powercap = toCopy.powercap;
            powerpoolcap = toCopy.powerpoolcap;

            characterRace = (Race)toCopy.characterRace.Clone();
            characterClass = (DaocClass)toCopy.characterClass.Clone();

            resists = toCopy.resists.Select(x => (ResistType)x.Clone()).ToList();
            itemSlots = toCopy.itemSlots.Select(x => (SlotType)x.Clone()).ToList();
            stats = toCopy.stats.Select(x => (StatType)x.Clone()).ToList();
        }

        public List<ResistType> Resists
        {
            // Has to be something wrong with the math here.  Or the general idea.  Should we include the difference between the cap and max cap somehow?
            get
            {
                var totals = resists.Select(x => (ResistType)x.Clone()).ToList();
                foreach(ResistType r in totals)
                {
                    foreach(SlotType s in itemSlots)
                    {
                        if (s.item == null || s.item.bonuses == null)
                        {
                            continue;
                        }
                        foreach (BonusType b in s.item.bonuses)
                        {
                            if (b.type == BonusTypes.Resistance &&
                                b.id == (int)r.resist)
                            {
                                r.value += b.value;
                            }
                            if (b.type == BonusTypes.MythicalResistanceCap &&
                                b.id == (int)r.resist)
                            {
                                r.cap += b.value;
                            }
                            if (b.type == BonusTypes.MythicalResistAndCap &&
                                b.id == (int)r.resist)
                            {
                                r.cap += b.value;
                                r.value += b.value;
                            }
                        }
                    }

                    if (r.cap > r.hardCap)
                    {
                        r.cap = r.hardCap;
                    }

                    if (r.value > r.cap)
                    {
                        r.value = r.cap;
                    }
                }

                return totals;
            }
        }

        public List<StatType> Stats
        {
            get
            {
                var totals = stats.Select(x => (StatType)x.Clone()).ToList();
                foreach (StatType s in totals)
                {
                    foreach (SlotType i in itemSlots)
                    {
                        if (i.item == null || i.item.bonuses == null)
                        {
                            continue;
                        }
                        foreach (BonusType b in i.item.bonuses)
                        {
                            if (b.type == BonusTypes.Stats &&
                                b.id == (int)s.stat)
                            {
                                s.value += b.value;
                            }
                            if (b.type == BonusTypes.TOAOvercapp &&
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                            }
                            if (b.type == BonusTypes.MythicalCapIncrease &&        // online char planner has this bonus adding to both stat and limit.  Will need to verify online.
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                            }
                            if (b.type == BonusTypes.MythicalStatAndCapIncrease &&
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                                s.value += b.value;
                            }
                        }
                    }

                    if (s.statLimit > s.hardCap)
                    {
                        s.statLimit = s.hardCap;
                    }

                    if (s.value > s.statLimit)
                    {
                        s.value = s.statLimit;
                    }
                }

                return totals;
            }
        }

        public object Clone()
        {
            return new Character(this);
        }

        public double Evaluate()
        {
            List<StatType> eStat = Stats;
            List<ResistType> eResist = Resists;
            #region Rev1
            // Scoring method Rev 1
            // This method tries to level the stat bonuses and the resist bonuses by getting a ratio of the respective values by comparing the avg of the hard limits.
            // Get avg of stat hard caps
            // Get avg of resist hard caps
            // get a ratio of the avgerages
            // sum the difference of the stat hard cap and the current stat total, divide by the ratio
            // sum the difference of the resist hard cap and the current resist total
            // add these two sums.  lower scores are better

            //double A1 = eStat.Where(x => x.statLimit > 0).Average(x => x.hardCap);      // get the average of the stat caps
            //double A2 = eResist.Average(x => x.hardCap);    // get the average of the resist caps
            //double ratio = A1 / A2;                         // get the ratio

            //double S1 = eStat.Where(x => x.statLimit > 0).Sum(x => (x.hardCap - x.value) / ratio);      // sum the stat differences and apply the ratio
            //double S2 = eResist.Sum(x => (x.hardCap - x.value));            // sum the resist differences

            //return S1 + S2;
            #endregion

            #region Rev2
            // Scoring method Rev 2
            // different paradigm.  
            // (item.bonus + char.bonus - char.bonusLimit)^2 summed for all bonuses.  lowest score is best
            // challenge is get the char.bonus values without the piece being considered.
            // could create another eval method that takes an item param.  if the calling routine already set the slot item to null
            // ok, don't need to consider item by item.  Can still do this just without the item bonus getting added in, since it already is.
            //double S3 = eStat.Where(x => x.statLimit > 0).Sum(x => Math.Pow(x.value - x.hardCap, 2));
            //double S4 = eResist.Sum(x => 9 * Math.Pow(x.value - x.hardCap, 2));

            //return S3 + S4;
            #endregion

            #region Rev3
            // Enhancement to Rev2 by using emun attributes and character race and class to give priority to other bonuses besides the stat and resist.  But also weighting the stat bonuses
            var allBonuses = itemSlots.Where(i => i.item.bonuses != null).SelectMany(i => i.item.bonuses).ToList();
            var totalStats = stats.Select(x => (StatType)x.Clone()).ToList();
            var totalResists = resists.Select(x => (ResistType)x.Clone()).ToList();

            int scoreHitpoints = hitpoints;
            int scoreHitpointCap = hitcap;
            int scorePowerPool = powerpool;
            int scorePowerPoolCap = powerpoolcap;

            double otherBonuses = 0;

            foreach (BonusType b in allBonuses)
            {
                var classBonus = characterClass.scoringBonuses.FirstOrDefault(x => x.type == b.type);

                switch (b.type)
                {
                    case BonusTypes.Stats:
                    case BonusTypes.TOAOvercapp:
                    case BonusTypes.MythicalCapIncrease:
                    case BonusTypes.MythicalStatAndCapIncrease:
                        var s = totalStats.FirstOrDefault(x => (int)x.stat == b.id);
                        if (s != null)
                        {
                            if (b.type == BonusTypes.Stats ||
                                b.type == BonusTypes.MythicalStatAndCapIncrease)
                            {
                                s.value += b.value;
                            }

                            if (b.type == BonusTypes.TOAOvercapp ||
                                b.type == BonusTypes.MythicalCapIncrease ||
                                b.type == BonusTypes.MythicalStatAndCapIncrease)
                            {
                                s.statLimit += b.value;
                            }
                        }
                        break;
                    case BonusTypes.Resistance:
                    case BonusTypes.MythicalResistanceCap:
                    case BonusTypes.MythicalResistAndCap:
                        var r = totalResists.FirstOrDefault(x => (int)x.resist == b.id);
                        if (r != null)
                        {
                            if (b.type == BonusTypes.Resistance ||
                                b.type == BonusTypes.MythicalResistAndCap)
                            {
                                r.value += b.value;
                            }
                            if (b.type == BonusTypes.MythicalResistanceCap ||
                                b.type == BonusTypes.MythicalResistAndCap)
                            {
                                r.cap += b.value;
                            }
                        }
                        break;
                    case BonusTypes.HitPoints:
                        scoreHitpoints += b.value;
                        break;
                    case BonusTypes.TOAHitPointsCap:
                        scoreHitpointCap += b.value;
                        break;
                    case BonusTypes.TOAPowerPool:
                        scorePowerPool += b.value;
                        break;
                    case BonusTypes.TOAPowerPoolCap:
                        scorePowerPoolCap += b.value;
                        break;
                    default:
                        if (classBonus != null)
                        {
                            otherBonuses += ((classBonus.value / 10) * b.value);
                        }
                        break;
                }
            }

            foreach (StatType s in totalStats.ToList())
            {
                if (s.statLimit > s.hardCap)
                {
                    s.statLimit = s.hardCap;
                }
                if (s.value > s.statLimit)
                {
                    s.value = s.statLimit;
                }
            }
            foreach(ResistType r in totalResists.ToList())
            {
                if (r.cap > r.hardCap)
                    r.cap = r.hardCap;
                if (r.value > r.cap)
                    r.value = r.cap;
            }

            // trying to get the stat weights from the class.  will need to reintroduce the caps in the final calculation as well.
            var joinStats = (from s1 in characterClass.scoringStats
                             join s2 in totalStats on s1.stat equals s2.stat
                             select new { stat = s2.stat, statLimit = s2.statLimit, hardCap = s2.hardCap, value = s2.value, weight = (float)s1.value/10 }).ToList();

            // Do we need to know how much of the score each section contributed?
            // stats are scored by subracting the hard cap from the stat value adjusted for caps and squaring it.  This makes each point for a stat bonus have a lot of weight.
            // the result is multiplied by the class weight for that stat.  e.g. stat of STR value 100, cap 127 -27 squared 729 multiplied by 1.2, 874.8
            double S5 = joinStats.Where(x => x.weight > 0).Sum(x => (Math.Pow(x.value - x.hardCap, 2) * x.weight));

            double S6 = totalResists.Sum(x => 9 * Math.Pow(x.value - x.hardCap, 2));
            double S7 = Math.Pow(scoreHitpoints - scoreHitpointCap, 2);
            double S8 = Math.Pow(scorePowerPool - scorePowerPoolCap, 2);

            return S5 + S6 + S7 + S8 - Math.Pow(otherBonuses, 2);
            #endregion
        }
    }

    public class SlotSearchType
    {
        public int currentIndex { get; set; }
        public List<Item> items { get; set; }
        public SlotType charSlot { get; set; }
    }

    public class SearchThreadParameter
    {
        public int threadID;
        public Character c;
        public List<Item> items;
    }
    class Program
    {
        public static ConcurrentQueue<Character> finalists;
        static void Main(string[] args)
        {
            string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\static_objects.json";
            //string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\oneObject.json";
            string json = File.ReadAllText(jsonFilePath);

            //var obj = JsonDocument.Parse(json);
            //var items = obj.RootElement.GetProperty("items").EnumerateArray();

            var daocItems = JsonSerializer.Deserialize<Items>(json);

            finalists = new ConcurrentQueue<Character>();

            int thisRealm = 3;                              // Hibernia = 3  Should make this based on the class selected
            Character one = new Character("Tronerth");

            var realmItems = daocItems.items.Where(x => (x.realm == 0 || x.realm == thisRealm)).ToList();

            // eval the base pieces of existing template
            List<int> tempPieces = new List<int>();
            
            // lock in the base pieces
            List<int> lockedPieces = new List<int>();
            lockedPieces.Add(30074);        // 30074 Torso
            lockedPieces.Add(30100);        // 30100 Legs
            lockedPieces.Add(30176);        // 30176 Arms

            tempPieces.Add(30074);   // 30074 Torso
            tempPieces.Add(30100);   // 30100 Legs
            tempPieces.Add(30176);   // 30176 Arms
            tempPieces.Add(17077);   // helm  
            tempPieces.Add(30474);   // hands
            tempPieces.Add(25771);   // boots
            tempPieces.Add(16659);   // neck
            tempPieces.Add(17096);   // cloak
            tempPieces.Add(16176);   // jewel
            tempPieces.Add(38054);   // belt
            tempPieces.Add(31394);   // ringr
            tempPieces.Add(52215);   // ringl
            tempPieces.Add(12301);   // bracerr
            tempPieces.Add(26279);   // bracerl
            tempPieces.Add(12566);   // myth
            foreach (int id in tempPieces)
            {
                Item i = realmItems.FirstOrDefault(x => x.id == id);
                if (i == null)
                {
                    Console.WriteLine($"Could not find {id}");
                    continue;
                }
                SlotType s = one.itemSlots.FirstOrDefault(x => x.slot == i.slot && x.item == null);
                if (s == null)
                {
                    Console.WriteLine($"Could not find empty slot {i.slot} for {id}");
                    continue;
                }
                s.item = i;

                if (lockedPieces.Contains(id))
                {
                    s.locked = true;
                }
            }
             
            Console.WriteLine($"Starting:\n{string.Join("\n", one.itemSlots.Select(x => x.item.name).ToList())}\nStats:\n{string.Join("\n", one.Stats)}\nResists:\n{string.Join("\n", one.Resists)}");
            Console.WriteLine($"Template Score: ${one.Evaluate()}");
            Console.WriteLine($"Template Score: ${one.Evaluate()}");

            // Search Rev1
            // Rev 1 was a brute force exhaustive search of all possible combinations.  It would have taken forever given the number of combinations involved.
            //  Tried to limit the number of items for each slot by material or level, but the count for the remaining slots was still too high

            // Search Rev2
            // Rev 2 uses threads.  Each thread generates a random character, by choosing a random item for each unlocked slot.  
            //  Then it cycles through all the items for one slot and keeps the best item for that slot for this character.  Then it does the same thing for every slot.
            //  This method doesn't try every possible combination, and it will not get a perfect result.  But should get some good results. 
            //  Testing of Rev 2 is less than ideal results.  Trying different scoring/evaluation methods.  Since this will be key no matter what search method is used.

            // Search Rev3
            // Rev 3 will be a genetic algorithm.  Create 1000 random characters by putting a random item in each slot.  Evaluate each character.  Pick the top % to reproduce.  
            //  Crossover can be handled by simply swapping the item in the same slot on the two parents
            //  Mutation can be handled by selecting a new random item for the specified slot.
            //  This method requires a good evaluation method.

            int numThreads = 100;
            List<Thread> workers = new List<Thread>();

            for (int i = 0; i < numThreads; i++)
            {
                SearchThreadParameter s = new SearchThreadParameter { threadID = i + 1, c = one, items = realmItems };
                workers.Add(new Thread(() => RoundRobinMethod(s)));
            }

            foreach (var t in workers)
            {
                t.Start();
            }

            Console.WriteLine($"Template Score: ${one.Evaluate()}");
            //Console.WriteLine($"Starting:\n{string.Join("\n", chosenOne.itemSlots.Where(x => !x.locked).Select(x => x.item.name).ToList())}\nStats:\n{string.Join("\n", chosenOne.Stats)}\nResists:\n{string.Join("\n", chosenOne.Resists)}");

            // getting good results now.  might not need to go to rev3 search.
            //  Next step is to return the best combination from each thread and rank those for the output.  Also need to output to a file.  
            // !!! ???

            foreach (var t in workers)
            {
                t.Join();
            }

            foreach(Character c in finalists.ToList().OrderByDescending(x => x.Evaluate()))
            {
                Console.WriteLine($"        : {c.Evaluate():000.0000} : {string.Join(":", c.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.item.id,6}").ToList())}");
                //Console.WriteLine($"         \n{string.Join("\n", c.itemSlots.Select(x => x.item.name).ToList())}\nStats:\n{string.Join("\n", c.Stats)}\nResists:\n{string.Join("\n", c.Resists)}");
            }
            //List<string> useFlags = new List<string>();
            //Dictionary<int, int> slotCounts = new Dictionary<int, int>();

            //int itemCount = 0;
            //foreach(var item in items)
            //{
            //    if (item.TryGetProperty("realm", out var realm))
            //    {
            //        int realmValue;
            //        realm.TryGetInt32(out realmValue);

            //        if (realmValue != 3 && realmValue != 0)
            //            continue;
            //    }
            //    if (item.TryGetProperty("slot", out var slot))
            //    {
            //        int slotPosition = 0;
            //        slot.TryGetInt32(out slotPosition);
            //        if (slotCounts.ContainsKey(slotPosition))
            //        {
            //            slotCounts[slotPosition]++;
            //        }
            //        else
            //        {
            //            slotCounts.Add(slotPosition, 1);
            //        }
            //    }
            //    if (item.TryGetProperty("name", out var name))
            //    {
            //        Console.WriteLine($"{++itemCount} {name}");
            //    }
            //    //if (item.TryGetProperty("flags", out var flags))
            //    if (item.TryGetProperty("abilities", out var flags))
            //    {
            //        var theFlags = flags.EnumerateArray();
            //        //var theFlags = flags.EnumerateObject();
            //        //var theFlags = item.EnumerateObject();
            //        foreach (var flag in theFlags)
            //        {
            //            var theElements = flag.EnumerateObject();
            //            foreach(var element in theElements)
            //            {
            //                if (!useFlags.Contains(element.Name))
            //                {
            //                    useFlags.Add(element.Name);
            //                }
            //            }
            //        }
            //    }
            //}

            //dynamic array = JsonSerializer..Deserialize(json);
            //foreach (var item in array)
            //{
            //    Console.WriteLine("{0} {1}", item.temp, item.vcc);
            //}

            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// Thread routine to do a round robin type of search.
        /// We're going to pick a random item for each unlocked slot. 
        /// Select a slot to start, and go through each item for that slot, keeping only the best one. 
        /// Repeat for the next slot.  
        /// If we go through all the slots without changing an item we are done.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="items"></param>
        private static void RoundRobinMethod(SearchThreadParameter param)
        {
            int thread = param.threadID;
            Character c = param.c;
            List<Item> items = param.items;

            Random rnd = new Random((int)DateTime.Now.Ticks);
            Character me = new Character(c);       // get a deep copy of the character passed in.

            // get a list of all the items for each slot.
            Dictionary<Tuple<Slots, int>, SlotSearchType> slotItems = new Dictionary<Tuple<Slots, int>, SlotSearchType>();
            foreach (var s in me.itemSlots)
            {
                if (s.locked)
                {
                    continue;
                }
                int slotNumber = 1;
                Tuple<Slots, int> key = new Tuple<Slots, int>(s.slot, slotNumber);
                while (slotItems.ContainsKey(key))
                {
                    key = new Tuple<Slots, int>(s.slot, ++slotNumber);
                }

                var titems = items.Where(x => x.slot == s.slot &&
                    //(x.slot == (int)Slots.Mythrian ||
                    //(x.requirements != null &&
                    //x.requirements.level_required > 48)) &&
                    (x.requirements == null || x.requirements.usable_by == null || x.requirements.usable_by.Contains(me.characterClass.characterClass))).ToList();

                // remove items that are not the highest level (hard coding these, would need some kind of look up to do better)
                //if (s.slot == Slots.Arms || s.slot == Slots.Feet || s.slot == Slots.Hands || s.slot == Slots.Legs || s.slot == Slots.Torso || s.slot == Slots.Helm)
                //{
                //    titems.RemoveAll(x => x.material != 67);
                //}

                slotItems.Add(key, new SlotSearchType() { currentIndex = 0, items = titems, charSlot = s });

                if (thread != 1)      // keeping thread #1 working on the original template
                {
                    int r = rnd.Next(titems.Count);     // Get a random index from the list just added
                    s.item = titems[r];                 // Assign a random item to the slot
                }
            }

            double min = double.MaxValue;
            double score = 0;
            Character chosenOne = new Character(me);
            Int64 counter = 0;
            double chosenScore = min;

            //Console.WriteLine($"                     {string.Join(":", me.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.slot,6}").ToList())}");
            //Console.WriteLine($"                     {string.Join(":", slotItems.Select(x => $"{x.Value.items.Count,6}").ToList())}");

            bool changed = true;

            while (changed)
            {
                Console.Write($"{++counter}\r");
                changed = false;
                score = me.Evaluate();
                if (score < min)
                {
                    chosenOne = new Character(me);
                    Console.WriteLine($"{thread:000} Best: {score:000.0000} : {string.Join(":", chosenOne.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.item.id,6}").ToList())}");
                    min = score;
                }

                foreach (var s in slotItems)            // if we can complete this foreach, then it's cycled through all the items on the last slot and we are done.
                {
                    SlotSearchType current = s.Value;
                    Item chosenItem = current.charSlot.item;        // remember the item we have, in case we don't find anything better.
                    double partMin = min;                           // keep a local min value for this slot
                    foreach(Item i in current.items)
                    {
                        current.charSlot.item = i;
                        score = me.Evaluate();
                        if (score < partMin)
                        {
                            chosenItem = i;
                            chosenScore = score;
                            changed = true;
                            partMin = score;
                        }
                    }

                    current.charSlot.item = chosenItem;     // go back to the chosen item (best items from that loop
                }
            }

            //Console.WriteLine($"New Best: {chosenScore:000.0000} : {string.Join(":", chosenOne.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.item.id,6}").ToList())}");
            Console.WriteLine($"{thread:####} Finished:\n{string.Join("\n", chosenOne.itemSlots.Select(x => $"{x.slot}:{x.item.name}").ToList())}\nStats:\n{string.Join("\n", chosenOne.Stats)}\nResists:\n{string.Join("\n", chosenOne.Resists)}");

            finalists.Enqueue(chosenOne);
        }
    }
}
