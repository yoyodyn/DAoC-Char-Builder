using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    public class FlagsType
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
    }

    public class BonusType
    {
        public int type { get; set; }
        public int value { get; set; }
        public int id { get; set; }
    }

    public class TypeDataType
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
    }

    public class RequirementsType
    {
        public int level_required { get; set; }
        public List<int> usable_by { get; set; }
        public int skill_trains_required { get; set; }
        public int champion_level_required { get; set; }
    }

    public class AbilitiesType
    {
        public int spell { get; set; }
        public int position { get; set; }
        public int power_level { get; set; }
        public int magic_type { get; set; }
        public int max_charges { get; set; }
        public int requirement_id { get; set; }
        public int level_required { get; set; }
    }
    public class Item
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

    }

    public class Items
    {
        public List<Item> items { get; set; }
        public int count { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\static_objects.json";
            //string jsonFilePath = @"C:\Projects\Roger\DAoC\daoc_item_database_combined\oneObject.json";
            string json = File.ReadAllText(jsonFilePath);

            var obj = JsonDocument.Parse(json);
            
            var obj2 = JsonSerializer.Deserialize<Items>(json);

            var items = obj.RootElement.GetProperty("items").EnumerateArray();

            List<string> useFlags = new List<string>();
            Dictionary<int, int> slotCounts = new Dictionary<int, int>();

            int itemCount = 0;
            foreach(var item in items)
            {
                if (item.TryGetProperty("realm", out var realm))
                {
                    int realmValue;
                    realm.TryGetInt32(out realmValue);

                    if (realmValue != 3 && realmValue != 0)
                        continue;
                }
                if (item.TryGetProperty("slot", out var slot))
                {
                    int slotPosition = 0;
                    slot.TryGetInt32(out slotPosition);
                    if (slotCounts.ContainsKey(slotPosition))
                    {
                        slotCounts[slotPosition]++;
                    }
                    else
                    {
                        slotCounts.Add(slotPosition, 1);
                    }
                }
                if (item.TryGetProperty("name", out var name))
                {
                    Console.WriteLine($"{++itemCount} {name}");
                }
                //if (item.TryGetProperty("flags", out var flags))
                if (item.TryGetProperty("abilities", out var flags))
                {
                    var theFlags = flags.EnumerateArray();
                    //var theFlags = flags.EnumerateObject();
                    //var theFlags = item.EnumerateObject();
                    foreach (var flag in theFlags)
                    {
                        var theElements = flag.EnumerateObject();
                        foreach(var element in theElements)
                        {
                            if (!useFlags.Contains(element.Name))
                            {
                                useFlags.Add(element.Name);
                            }
                        }
                    }
                }
            }

            //dynamic array = JsonSerializer..Deserialize(json);
            //foreach (var item in array)
            //{
            //    Console.WriteLine("{0} {1}", item.temp, item.vcc);
            //}

            Console.WriteLine("Hello World!");
        }
    }
}
