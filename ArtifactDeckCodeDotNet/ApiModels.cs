using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetJsonLocation
    {
        [JsonProperty("cdn_root", Required = Required.Always)]
        public Uri CdnRoot { get; set; }

        [JsonProperty("url", Required = Required.Always)]
        public Uri Url { get; set; }

        [JsonProperty("expire_time", Required = Required.Always)]
        public long ExpireTime { get; set; }

        public Uri GetFullUri()
        {
            Uri.TryCreate(CdnRoot, Url, out var result);
            return result;
        }
    }

    public class CardSetData
    {
        [JsonProperty("card_set", Required = Required.Always)]
        public CardSet CardSet { get; set; }

        /* Not part of official API; used for internal data caching */
        [JsonProperty("expire_time")]
        internal DateTimeOffset ExpireTimeUtc { get; set; }
    }

    public class CardSet
    {
        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("set_info")]
        public SetInfo SetInfo { get; set; }

        [JsonProperty("card_list")]
        public List<Card> CardList { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardType
    {
        [EnumMember(Value = "Stronghold")]
        Stronghold,

        [EnumMember(Value = "Improvement")]
        Improvement,

        [EnumMember(Value = "Spell")]
        Spell,

        [EnumMember(Value = "Ability")]
        Ability,

        [EnumMember(Value = "Passive Ability")]
        PassiveAbility,

        [EnumMember(Value = "Hero")]
        Hero,

        [EnumMember(Value = "Creep")]
        Creep,

        [EnumMember(Value = "Item")]
        Item,

        [EnumMember(Value = "Pathing")]
        Pathing,

        [EnumMember(Value = "Mutation")]
        Mutation
    }

    public class Card
    {
        [JsonProperty("card_id")]
        public long CardId { get; set; }

        [JsonProperty("base_card_id")]
        public long BaseCardId { get; set; }

        [JsonProperty("card_type")]
        public CardType CardType { get; set; }

        [JsonProperty("card_name")]
        public Text CardName { get; set; }

        [JsonProperty("card_text")]
        public Text CardText { get; set; }

        [JsonProperty("mini_image")]
        public Image MiniImage { get; set; }

        [JsonProperty("large_image")]
        public Image LargeImage { get; set; }

        [JsonProperty("ingame_image")]
        public Image IngameImage { get; set; }

        [JsonProperty("hit_points", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? HitPoints { get; set; }

        [JsonProperty("references")]
        public Reference[] References { get; set; }

        [JsonProperty("illustrator", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Illustrator { get; set; }

        [JsonProperty("mana_cost", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? ManaCost { get; set; }

        [JsonProperty("attack", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Attack { get; set; }

        [JsonProperty("is_black", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsBlack { get; set; }

        [JsonProperty("sub_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SubType { get; set; }

        [JsonProperty("gold_cost", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? GoldCost { get; set; }

        [JsonProperty("is_green", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsGreen { get; set; }

        [JsonProperty("is_red", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsRed { get; set; }

        [JsonProperty("armor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Armor { get; set; }

        [JsonProperty("is_blue", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsBlue { get; set; }
    }

    public class Text
    {
        [JsonProperty("english", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string English { get; set; }

        [JsonProperty("german", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string German { get; set; }

        [JsonProperty("french", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string French { get; set; }

        [JsonProperty("italian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Italian { get; set; }

        [JsonProperty("koreana", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Korean { get; set; }

        [JsonProperty("spanish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Spanish { get; set; }

        [JsonProperty("schinese", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string ChineseSimplified { get; set; }

        [JsonProperty("tchinese", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string ChineseTraditional { get; set; }

        [JsonProperty("russian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Russian { get; set; }

        [JsonProperty("thai", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Thai { get; set; }

        [JsonProperty("japanese", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Japanese { get; set; }

        [JsonProperty("portuguese", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Portuguese { get; set; }

        [JsonProperty("polish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Polish { get; set; }

        [JsonProperty("danish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Danish { get; set; }

        [JsonProperty("dutch", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Dutch { get; set; }

        [JsonProperty("finnish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Finnish { get; set; }

        [JsonProperty("norwegian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Norwegian { get; set; }

        [JsonProperty("swedish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Swedish { get; set; }

        [JsonProperty("hungarian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Hungarian { get; set; }

        [JsonProperty("czech", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Czech { get; set; }

        [JsonProperty("romanian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Romanian { get; set; }

        [JsonProperty("turkish", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Turkish { get; set; }

        [JsonProperty("brazilian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Brazilian { get; set; }

        [JsonProperty("bulgarian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Bulgarian { get; set; }

        [JsonProperty("greek", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Greek { get; set; }

        [JsonProperty("ukrainian", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Ukrainian { get; set; }

        [JsonProperty("latam", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string LatinAmerican { get; set; }

        [JsonProperty("vietnamese", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string Vietnamese { get; set; }
    }

    public class Image
    {
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Default { get; set; }

        [JsonProperty("german", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri German { get; set; }

        [JsonProperty("french", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri French { get; set; }

        [JsonProperty("italian", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Italian { get; set; }

        [JsonProperty("koreana", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Korean { get; set; }

        [JsonProperty("spanish", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Spanish { get; set; }

        [JsonProperty("schinese", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri ChineseSimplified { get; set; }

        [JsonProperty("tchinese", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri ChineseTraditional { get; set; }

        [JsonProperty("russian", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Russian { get; set; }

        [JsonProperty("japanese", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Japanese { get; set; }

        [JsonProperty("brazilian", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri Brazilian { get; set; }

        [JsonProperty("latam", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri LatinAmerican { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RefType
    {
        [EnumMember(Value = "includes")]
        Includes,

        [EnumMember(Value = "references")]
        References,

        [EnumMember(Value = "active_ability")]
        ActiveAbility,

        [EnumMember(Value = "passive_ability")]
        PassiveAbility,
    }

    public class Reference
    {
        [JsonProperty("card_id")]
        public long CardId { get; set; }

        [JsonProperty("ref_type")]
        public RefType RefType { get; set; }

        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? Count { get; set; }
    }

    public class SetInfo
    {
        [JsonProperty("set_id")]
        public long SetId { get; set; }

        [JsonProperty("pack_item_def")]
        public long PackItemDef { get; set; }

        [JsonProperty("name")]
        public Text Name { get; set; }
    }
}
