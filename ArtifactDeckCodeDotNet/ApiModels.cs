using Newtonsoft.Json;
using System;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetJsonLocation
    {
        [JsonProperty("cdn_root")]
        public Uri CdnRoot { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("expire_time")]
        public long ExpireTime { get; set; }
    }

    public class CardSetData
    {
        [JsonProperty("card_set")]
        public CardSet CardSet { get; set; }
    }

    public class CardSet
    {
        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("set_info")]
        public SetInfo SetInfo { get; set; }

        [JsonProperty("card_list")]
        public CardList[] CardList { get; set; }
    }

    public class CardList
    {
        [JsonProperty("card_id")]
        public long CardId { get; set; }

        [JsonProperty("base_card_id")]
        public long BaseCardId { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_name")]
        public Name CardName { get; set; }

        [JsonProperty("card_text")]
        public Name CardText { get; set; }

        [JsonProperty("mini_image")]
        public Image MiniImage { get; set; }

        [JsonProperty("large_image")]
        public Image LargeImage { get; set; }

        [JsonProperty("ingame_image")]
        public Image IngameImage { get; set; }

        [JsonProperty("hit_points", NullValueHandling = NullValueHandling.Ignore)]
        public long? HitPoints { get; set; }

        [JsonProperty("references")]
        public Reference[] References { get; set; }

        [JsonProperty("illustrator", NullValueHandling = NullValueHandling.Ignore)]
        public string Illustrator { get; set; }

        [JsonProperty("mana_cost", NullValueHandling = NullValueHandling.Ignore)]
        public long? ManaCost { get; set; }

        [JsonProperty("attack", NullValueHandling = NullValueHandling.Ignore)]
        public long? Attack { get; set; }

        [JsonProperty("is_black", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBlack { get; set; }

        [JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SubType { get; set; }

        [JsonProperty("gold_cost", NullValueHandling = NullValueHandling.Ignore)]
        public long? GoldCost { get; set; }

        [JsonProperty("is_green", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsGreen { get; set; }

        [JsonProperty("is_red", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRed { get; set; }

        [JsonProperty("armor", NullValueHandling = NullValueHandling.Ignore)]
        public long? Armor { get; set; }

        [JsonProperty("is_blue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBlue { get; set; }
    }

    public class Name
    {
        [JsonProperty("english", NullValueHandling = NullValueHandling.Ignore)]
        public string English { get; set; }
    }

    public class Image
    {
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Default { get; set; }
    }

    public class Reference
    {
        [JsonProperty("card_id")]
        public long CardId { get; set; }

        [JsonProperty("ref_type")]
        public string RefType { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }
    }

    public class SetInfo
    {
        [JsonProperty("set_id")]
        public long SetId { get; set; }

        [JsonProperty("pack_item_def")]
        public long PackItemDef { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }
    }
}
