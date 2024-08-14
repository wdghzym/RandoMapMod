using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using ItemChanger;
using MapChanger;
using RandoMapMod.Settings;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SD = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapMod.Pins
{
    internal class PinSpriteManager : HookModule
    {
        internal static readonly Dictionary<string, string> PoolGroupSpriteLookup = new()
        {
            {"Dreamers", "Dreamer"},
            {"Skills", "Skill"},
            {"Charms", "Charm"},
            {"Keys", "Key"},
            {"Mask Shards", "Mask"},
            {"Vessel Fragments", "Vessel"},
            {"Charm Notches", "Notch"},
            {"Pale Ore", "Ore"},
            {"Geo Chests", "Geo"},
            {"Rancid Eggs", "Egg"},
            {"Relics", "Relic"},
            {"Whispering Roots", "Root"},
            {"Boss Essence", "EssenceBoss"},
            {"Grubs", "Grub"},
            {"Mimics", "Grub"},
            {"Maps", "Map"},
            {"Stags", "Stag"},
            {"Lifeblood Cocoons", "Cocoon"},
            {"Grimmkin Flames", "Flame"},
            {"Journal Entries", "Journal"},
            {"Geo Rocks", "Rock"},
            {"Boss Geo", "Geo"},
            {"Soul Totems", "Totem"},
            {"Lore Tablets", "Lore"},
            {"Shops", "Shop"},
            {"Levers", "Lever"},
            {"Mr Mushroom", "Lore"},
            {"Benches", "Bench"},
            {"Other", "Unknown"}
        };

        internal static Dictionary<string, ScaledPinSprite> BuiltInPinSprites = [];
        internal static Dictionary<TaggableObject, ScaledPinSprite> ConnectionPinSprites = [];

        public override void OnEnterGame() { }

        public override void OnQuitToMenu()
        {
            BuiltInPinSprites.Clear();
            ConnectionPinSprites.Clear();
        }

        internal static ScaledPinSprite GetLocationSprite(string key)
        {
            if (PoolGroupSpriteLookup.TryGetValue(key, out string otherKey)) return GetLocationSprite(otherKey);

            if (RandoMapMod.GS.QMarks is QMarkSetting.Red)
            {
                key = key switch
                {
                    "Shop" => "Shop",
                    _ => "Unknown",
                };
            }
            else if (RandoMapMod.GS.QMarks is QMarkSetting.Mix)
            {
                key = key switch
                {
                    "Grub" => "UnknownGrub",
                    "Cocoon" => "UnknownLifeblood",
                    "Rock" => "UnknownGeoRock",
                    "Totem" => "UnknownTotem",
                    "Shop" => "Shop",
                    _ => "Unknown",
                };
            }

            return GetSpriteInternal(key);
        }

        internal static ScaledPinSprite GetSprite(string key)
        {
            if (PoolGroupSpriteLookup.TryGetValue(key, out string otherKey)) return GetSpriteInternal(otherKey);
            
            return GetSpriteInternal(key);
        }

        private static ScaledPinSprite GetSpriteInternal(string key)
        {
            if (BuiltInPinSprites.TryGetValue(key, out ScaledPinSprite sprite)) return sprite;

            sprite = new(key);

            BuiltInPinSprites.Add(key, sprite);

            return sprite;
        }

        internal static ScaledPinSprite GetLocationSprite(AbstractPlacement placement)
        {
            if (RandoMapMod.GS.QMarks is not QMarkSetting.Off)
            {
                return GetLocationSprite(SD.Of(placement).Get(InjectedProps.LocationPoolGroup));
            }

            if (ConnectionPinSprites.TryGetValue(placement, out ScaledPinSprite sprite)) return sprite;

            if (SD.Of(placement).Get(InteropProperties.LocationPinSpriteKey) is string key)
            {
                sprite = GetLocationSprite(key);
            }
            else if (SD.Of(placement).Get(InteropProperties.LocationPinSprite) is ISprite iSprite)
            {
                sprite = new(iSprite, SD.Of(placement).Get(InteropProperties.LocationPinSpriteSize));
            }
            else
            {
                sprite = GetLocationSprite(SD.Of(placement).Get(InjectedProps.LocationPoolGroup));
            }

            ConnectionPinSprites.Add(placement, sprite);
            
            return sprite;
        }

        internal static ScaledPinSprite GetSprite(AbstractItem item)
        {
            if (ConnectionPinSprites.TryGetValue(item, out ScaledPinSprite sprite)) return sprite;

            if (SD.Of(item).Get(InteropProperties.ItemPinSpriteKey) is string key)
            {
                //RandoMapMod.Instance.LogError($"key -{key}");
                sprite = GetSprite(key);
            }
            else if (SD.Of(item).Get(InteropProperties.ItemPinSprite) is ISprite iSprite)
            {
                //RandoMapMod.Instance.LogError($"key -{placement.Name}");
                sprite = new(iSprite, SD.Of(item).Get(InteropProperties.ItemPinSpriteSize));
            }
            else
            {
                //wdblzym 多世界下 物品名称为 {username}'s_{itemname} 无法的到正常图标
                //RandoMapMod.Instance.LogError($"key itemName-{item.name} {SD.Of(item).Get(InjectedProps.ItemPoolGroup)}");
                string sn = SD.Of(item).Get(InjectedProps.ItemPoolGroup);
                if (sn=="Other")
                {
                    sn = SD.Of(item).Get(ItemPoolGroup);
                }
                sprite = GetSprite(sn);
                
            }

            ConnectionPinSprites.Add(item, sprite);
            
            return sprite;
        }
        public static readonly MetadataProperty<AbstractItem, string> ItemPoolGroup = new MetadataProperty<AbstractItem, string>("PoolGroup", GetDefaultItemPoolGroup);

        private static string GetDefaultItemPoolGroup(AbstractItem item)
        {
            string itemName = item.name;
            int idx = item.name.IndexOf("'s_");
            if (idx >= 0)
                itemName = item.name.Substring(idx+3);
            //RandoMapMod.Instance.LogError($"  {idx}   itemName-{itemName}");
            return SubcategoryFinder.GetItemPoolGroup(itemName).FriendlyName();
        }
    }
}
