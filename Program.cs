﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Stats = 1,
        Skills = 2,
        HitPoints = 4,
        Resistance = 5,
        Focus = 6,
        TOAMeleeDamage = 8,
        TOAMagicDamage = 9,
        TOAStyleDamage = 10,
        TOAArcheryRange = 11,
        TOASpellRange = 12,
        TOASpellDuration = 13,
        TOABuffBonus = 14,
        TOADebuffBonus = 15,
        TOAHealBonus = 16,
        TOAFatigue = 17,
        TOAMeleeSpeed = 19,
        TOAArcherySpeed = 20,
        TOACastSpeed = 21,
        ArmorFactor = 22,
        CraftingMinQuality = 23,
        CraftingQuality = 24,
        CraftingSpeed = 25,
        CraftingSkillGain = 26,
        TOAArcheryDamage = 27,
        TOAOvercapp = 28,
        TOAHitPointsCap = 29,
        TOAPowerPoolCap = 30,
        TOAFatigueCap = 31,
        TOAResistancePiece = 32,
        TOAPowerPool = 34,
        TOAArtifact = 35,
        ArrowRecovery = 36,
        SpellPowerCastReduction = 37,
        Concentration = 38,
        SafeFall = 39,
        HealthRegeneration = 40,
        ManaRegeneration = 41,
        PieceAblative = 42,
        DeathExperienceLossReduction = 44,
        NegativeEffectDurationReduction = 46,
        StyleCostReduction = 47,
        ToHitBonus = 48,
        DefensiveBonus = 49,
        BladeturnReinforcement = 50,
        Parry = 51,
        Block = 52,
        Evade = 53,
        ReactionaryStyleDamageBonus = 54,
        MythicalEncumberance = 55,
        MythicalResistanceCap = 57,
        MythicalSeigeSpeed = 58,
        MythicalParry = 60,
        MythicalEvade = 61,
        MythicalBlock = 62,
        MythicalCoin = 63,
        MythicalCapIncrease = 64,
        MythicalCrowdControlDuractionDecrease = 66,
        MythiclEssenceResist = 67,
        MythicalResistAndCap = 68,
        MythicalSeigeDamageAblative = 69,
        MythicalDPS = 71,
        MythicalRealmPoints = 72,
        MythicalSpellFocus = 73,
        MythicalResurectionSicknessReduction = 74,
        MythicalStatAndCapIncrease = 75,
        MythicalHealthRegen = 76,
        MythicalPowerRegen = 77,
        MythicalEnduranceRegen = 78,
        MythicalPhysicalDefence = 80
    }
    public class BonusType : ICloneable
    {
        public int type { get; set; }
        public int value { get; set; }
        public int id { get; set; }

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
        public List<int> usable_by { get; set; }
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
        public int slot { get; set; }
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
        Helm = 1,
        Hands = 2,
        Feet = 3,
        Jewel = 4,
        Torso = 5,
        Cloak = 6,
        Legs = 7,
        Arms = 8,
        Necklace = 9,
        Belt = 12,
        Bracer = 13,
        Ring = 15,
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
        Crush = 1,
        Slash = 2,
        Thrust = 3,
        Siege = 4,
        Heat = 10,
        Spirit2 = 11,       // Didn't find any 11
        Cold = 12,
        Matter2 = 13,       // Didn't find any 13
        Heat2 = 14,         // Didn't find any 14
        Matter = 15,
        Body = 16,
        Spirit = 17,
        Spirit3 = 18,       // Didn't find any 18
        Cold2 = 19,         // Didn't find any 19
        Energy2 = 20,       // Didn't find any 20
        Essence = 21,
        Energy = 22,
        Cold3 = 23,         // Didn't find any 23
        Body2 = 25,         // Didn't find any 25
        Body3 = 26,         // Didn't find any 26
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
        public string characterName { get; set; }
        public Classes characterClass { get; set; }
        public List<SlotType> itemSlots { get; set; }
        public List<StatType> stats { get; set; }
        public List<ResistType> resists { get; set; }

        public int hitpoints { get; set; }
        public int hitcap { get; set; }
        public int power { get; set; }
        public int powercap { get; set; }
        public int powerpool { get; set; }
        public int powerpoolcap { get; set; }

        public Character(string newName, Classes newClass = Classes.Warden)
        {
            characterName = newName;
            characterClass = newClass;

            itemSlots = new List<SlotType>();
            itemSlots.Add(new SlotType { slot = Slots.Helm });
            itemSlots.Add(new SlotType { slot = Slots.Cloak });
            itemSlots.Add(new SlotType { slot = Slots.Arms });
            itemSlots.Add(new SlotType { slot = Slots.Belt });
            itemSlots.Add(new SlotType { slot = Slots.Bracer });
            itemSlots.Add(new SlotType { slot = Slots.Bracer });
            itemSlots.Add(new SlotType { slot = Slots.Feet });
            itemSlots.Add(new SlotType { slot = Slots.Hands });
            itemSlots.Add(new SlotType { slot = Slots.Jewel });
            itemSlots.Add(new SlotType { slot = Slots.Legs });
            itemSlots.Add(new SlotType { slot = Slots.Mythrian });
            itemSlots.Add(new SlotType { slot = Slots.Necklace });
            itemSlots.Add(new SlotType { slot = Slots.Ring });
            itemSlots.Add(new SlotType { slot = Slots.Ring });
            itemSlots.Add(new SlotType { slot = Slots.Torso });

            stats = new List<StatType>();
            stats.Add(new StatType { stat = StatTypes.Strength, statLimit = 75 });
            stats.Add(new StatType { stat = StatTypes.Constitution, statLimit = 75 });
            stats.Add(new StatType { stat = StatTypes.Dexterity, statLimit = 75 });
            stats.Add(new StatType { stat = StatTypes.Quickness, statLimit = 75 });
            stats.Add(new StatType { stat = StatTypes.Piety, statLimit = 75 });
            stats.Add(new StatType { stat = StatTypes.Intellegence });
            stats.Add(new StatType { stat = StatTypes.Charisma });
            stats.Add(new StatType { stat = StatTypes.Empathy });
            stats.Add(new StatType { stat = StatTypes.Acuity });

            resists = new List<ResistType>();
            resists.Add(new ResistType { resist = ResistTypes.Crush, value = 2, cap = 26, hardCap = 43 });
            resists.Add(new ResistType { resist = ResistTypes.Slash, value = 3, cap = 26, hardCap = 44 });
            resists.Add(new ResistType { resist = ResistTypes.Thrust, value = 0, cap = 26, hardCap = 42 });
            resists.Add(new ResistType { resist = ResistTypes.Heat, value = 0, cap = 26, hardCap = 41 });
            resists.Add(new ResistType { resist = ResistTypes.Cold, value = 0, cap = 26, hardCap = 41 });
            resists.Add(new ResistType { resist = ResistTypes.Matter, value = 0, cap = 26, hardCap = 41 });
            resists.Add(new ResistType { resist = ResistTypes.Body, value = 0, cap = 26, hardCap = 41 });
            resists.Add(new ResistType { resist = ResistTypes.Spirit, value = 5, cap = 26, hardCap = 46 });
            resists.Add(new ResistType { resist = ResistTypes.Energy, value = 0, cap = 26, hardCap = 41 });

            hitpoints = 0;
            power = 0;
            powerpool = 0;
            hitcap = 0;
            powercap = 0;
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
                            if (b.type == (int)BonusTypes.Resistance &&
                                b.id == (int)r.resist)
                            {
                                r.value += b.value;
                            }
                            if (b.type == (int)BonusTypes.MythicalResistanceCap &&
                                b.id == (int)r.resist)
                            {
                                r.cap += b.value;
                            }
                            if (b.type == (int)BonusTypes.MythicalResistAndCap &&
                                b.id == (int)r.resist)
                            {
                                r.cap += b.value;
                                r.value += b.value;
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
                            if (b.type == (int)BonusTypes.Stats &&
                                b.id == (int)s.stat)
                            {
                                s.value += b.value;
                            }
                            if (b.type == (int)BonusTypes.TOAOvercapp &&
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                            }
                            if (b.type == (int)BonusTypes.MythicalCapIncrease &&
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                            }
                            if (b.type == (int)BonusTypes.MythicalStatAndCapIncrease &&
                                b.id == (int)s.stat)
                            {
                                s.statLimit += b.value;
                                s.value += b.value;
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
            double S3 = eStat.Sum(x => Math.Pow(x.value - x.hardCap, 2));
            double S4 = eResist.Sum(x => Math.Pow(x.value - x.hardCap, 2));

            return S3 + S4;
            #endregion
        }
    }

    public class SlotSearchType
    {
        public int currentIndex { get; set; }
        public List<Item> items { get; set; }
        public SlotType charSlot { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\static_objects.json";
            //string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\oneObject.json";
            string json = File.ReadAllText(jsonFilePath);

            //var obj = JsonDocument.Parse(json);
            //var items = obj.RootElement.GetProperty("items").EnumerateArray();

            var daocItems = JsonSerializer.Deserialize<Items>(json);

            int thisRealm = 3;                              // Hibernia = 3  Should make this based on the class selected
            Character one = new Character("Tronerth");

            var realmItems = daocItems.items.Where(x => x.realm == 0 || x.realm == thisRealm).ToList();

            // lock in the base pieces
            List<int> lockedPieces = new List<int>();
            lockedPieces.Add(30074);        // 30074 Torso
            lockedPieces.Add(30100);        // 30100 Legs
            lockedPieces.Add(30176);        // 30176 Arms

            foreach(int id in lockedPieces)
            {
                Item i = realmItems.FirstOrDefault(x => x.id == id);
                if (i == null)
                {
                    continue;
                }
                SlotType s = one.itemSlots.FirstOrDefault(x => (int)x.slot == i.slot);
                if (s == null)
                {
                    continue;
                }
                s.item = i;
                s.locked = true;
            }

            Console.WriteLine($"Starting:\n{string.Join("\n", one.itemSlots.Where(x => x.locked).Select(x => x.item.name).ToList())}\nStats:\n{string.Join("\n", one.Stats)}\nResists:\n{string.Join("\n", one.Resists)}");

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
                workers.Add(new Thread(() => RoundRobinMethod(one, realmItems)));
            }

            foreach (var t in workers)
            {
                t.Start();
            }

            //Console.WriteLine($"Starting:\n{string.Join("\n", chosenOne.itemSlots.Where(x => !x.locked).Select(x => x.item.name).ToList())}\nStats:\n{string.Join("\n", chosenOne.Stats)}\nResists:\n{string.Join("\n", chosenOne.Resists)}");

            foreach (var t in workers)
            {
                t.Join();
            }

            List<string> useFlags = new List<string>();
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
        private static void RoundRobinMethod(Character c, List<Item> items)
        {
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

                var titems = items.Where(x => x.slot == (int)s.slot &&
                    //(x.slot == (int)Slots.Mythrian ||
                    //(x.requirements != null &&
                    //x.requirements.level_required > 48)) &&
                    (x.requirements == null || x.requirements.usable_by == null || x.requirements.usable_by.Contains((int)me.characterClass))).ToList();

                // remove items that are not the highest level (hard coding these, would need some kind of look up to do better)
                //if (s.slot == Slots.Arms || s.slot == Slots.Feet || s.slot == Slots.Hands || s.slot == Slots.Legs || s.slot == Slots.Torso || s.slot == Slots.Helm)
                //{
                //    titems.RemoveAll(x => x.material != 67);
                //}

                slotItems.Add(key, new SlotSearchType() { currentIndex = 0, items = titems, charSlot = s });

                int r = rnd.Next(titems.Count);     // Get a random index from the list just added
                s.item = titems[r];                 // Assign a random item to the slot
            }

            double min = double.MaxValue;
            double score = 0;
            Character chosenOne = new Character(me);
            Int64 counter = 0;
            double chosenScore = min;

            Console.WriteLine($"                     {string.Join(":", me.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.slot,6}").ToList())}");
            Console.WriteLine($"                     {string.Join(":", slotItems.Select(x => $"{x.Value.items.Count,6}").ToList())}");

            bool changed = true;

            while (changed)
            {
                Console.Write($"{++counter}\r");
                changed = false;
                score = me.Evaluate();
                if (score < min)
                {
                    chosenOne = new Character(me);
                    Console.WriteLine($"N w Best: {score:000.0000} : {string.Join(":", chosenOne.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.item.id,6}").ToList())}");
                    min = score;
                }

                foreach (var s in slotItems)            // if we can complete this foreach, then it's cycled through all the items on the last slot and we are done.
                {
                    SlotSearchType current = s.Value;
                    int chosenIndex = 0;
                    while (++current.currentIndex < current.items.Count)
                    {
                        current.charSlot.item = current.items[current.currentIndex];
                        score = me.Evaluate();
                        if (score < min)
                        {
                            chosenOne = new Character(me);
                            chosenIndex = current.currentIndex;
                            chosenScore = score;
                            changed = true;
                        }
                    }

                    current.charSlot.item = current.items[chosenIndex];
                    current.currentIndex = 0;
                }
            }

            Console.WriteLine($"New Best: {chosenScore:000.0000} : {string.Join(":", chosenOne.itemSlots.Where(x => !x.locked).OrderBy(x => x.slot).Select(x => $"{x.item.id,6}").ToList())}");
            Console.WriteLine($"Finished:\n{string.Join("\n", chosenOne.itemSlots.Select(x => $"{x.slot}:{x.item.name}").ToList())}\nStats:\n{string.Join("\n", chosenOne.Stats)}\nResists:\n{string.Join("\n", chosenOne.Resists)}");
        }
    }
}
