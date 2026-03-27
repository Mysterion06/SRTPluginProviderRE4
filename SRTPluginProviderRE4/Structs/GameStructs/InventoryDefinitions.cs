using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTPluginProviderRE4.Structs.GameStructs
{
    internal readonly record struct ItemSize(int Width, int Height);

    internal class InventoryDefinition
    {
        public int Id { get; init; }
        public string Name { get; init; } = "Unknown";
        public ItemSize Size { get; init; }
        public bool CanRotate { get; init; }
    }

    internal static class InventoryDefinitions
    {
        public static readonly Dictionary<int, InventoryDefinition> Items = new()
        {
            { 1, new InventoryDefinition { Id = 1, Name = "Hand Grenade", Size = new ItemSize(1, 2), CanRotate = true } },
            { 2, new InventoryDefinition { Id = 2, Name = "Incendiary Grenade", Size = new ItemSize(1, 2), CanRotate = true } },
            { 4,  new InventoryDefinition { Id = 4,  Name = "Handgun Ammo", Size = new ItemSize(2, 1), CanRotate = true } },
            { 5,  new InventoryDefinition { Id = 5,  Name = "First Aid Spray", Size = new ItemSize(1, 2), CanRotate = true } },
            { 6,  new InventoryDefinition { Id = 6,  Name = "Green Herb", Size = new ItemSize(1, 2), CanRotate = true } },
            { 14, new InventoryDefinition { Id = 14, Name = "Flash Grenade", Size = new ItemSize(1, 2), CanRotate = true } },
            { 23,  new InventoryDefinition { Id = 23,  Name = "Rocket Launcher Special", Size = new ItemSize(8, 2), CanRotate = true } },
            { 24, new InventoryDefinition { Id = 24, Name = "Shotgun Ammo", Size = new ItemSize(2, 1), CanRotate = true } },
            { 25, new InventoryDefinition { Id = 25, Name = "Red Herb", Size = new ItemSize(1, 2), CanRotate = true } },
            { 28, new InventoryDefinition { Id = 28, Name = "Yellow Herb", Size = new ItemSize(1, 2), CanRotate = true } },
            { 32, new InventoryDefinition { Id = 32, Name = "TMP Ammo", Size = new ItemSize(2, 1), CanRotate = true } },
            { 33,  new InventoryDefinition { Id = 33,  Name = "Punisher", Size = new ItemSize(3, 2), CanRotate = true } },
            { 34,  new InventoryDefinition { Id = 34,  Name = "Punisher with Silencer", Size = new ItemSize(3, 2), CanRotate = true } },

            { 35,  new InventoryDefinition { Id = 35,  Name = "Handgun", Size = new ItemSize(3, 2), CanRotate = true } },
            { 36,  new InventoryDefinition { Id = 36,  Name = "Handgun w/ Silencer", Size = new ItemSize(3, 2), CanRotate = true } },

            { 37,  new InventoryDefinition { Id = 37,  Name = "Red9", Size = new ItemSize(4, 2), CanRotate = true } },
            { 38,  new InventoryDefinition { Id = 38,  Name = "Red9 w/ Stock", Size = new ItemSize(4, 2), CanRotate = true } },

            { 39,  new InventoryDefinition { Id = 39,  Name = "Blacktail", Size = new ItemSize(3, 2), CanRotate = true } },
            { 40,  new InventoryDefinition { Id = 40,  Name = "Blacktail w/ Silencer", Size = new ItemSize(3, 2), CanRotate = true } },

            { 41,  new InventoryDefinition { Id = 41,  Name = "Broken Butterfly", Size = new ItemSize(4, 2), CanRotate = true } },
            { 42,  new InventoryDefinition { Id = 42,  Name = "Killer7", Size = new ItemSize(4, 2), CanRotate = true } },
            { 43,  new InventoryDefinition { Id = 43,  Name = "Killer7 w/ Silencer", Size = new ItemSize(4, 2), CanRotate = true } },

            { 44,  new InventoryDefinition { Id = 44,  Name = "Shotgun", Size = new ItemSize(8, 2), CanRotate = true } },
            { 45,  new InventoryDefinition { Id = 45,  Name = "Striker", Size = new ItemSize(5, 2), CanRotate = true } },

            { 46,  new InventoryDefinition { Id = 46,  Name = "Rifle", Size = new ItemSize(9, 1), CanRotate = true } },
            { 47,  new InventoryDefinition { Id = 47,  Name = "Rifle (Semi-Auto)", Size = new ItemSize(7, 2), CanRotate = true } },

            { 48,  new InventoryDefinition { Id = 48,  Name = "TMP", Size = new ItemSize(3, 2), CanRotate = true } },
            { 50,  new InventoryDefinition { Id = 50,  Name = "TMP w/ Stock", Size = new ItemSize(3, 2), CanRotate = true } },

            { 52,  new InventoryDefinition { Id = 52,  Name = "Chicago Typewriter (Infinite)", Size = new ItemSize(7, 3), CanRotate = true } },
            { 53,  new InventoryDefinition { Id = 53,  Name = "Rocket Launcher", Size = new ItemSize(8, 2), CanRotate = true } },

            { 54,  new InventoryDefinition { Id = 54,  Name = "Mine Thrower", Size = new ItemSize(5, 2), CanRotate = true } },
            { 55,  new InventoryDefinition { Id = 55,  Name = "Handcannon", Size = new ItemSize(4, 2), CanRotate = true } },
            { 56,  new InventoryDefinition { Id = 56,  Name = "Combat Knife", Size = new ItemSize(1, 3), CanRotate = true } },

            { 62,  new InventoryDefinition { Id = 62,  Name = "Custom TMP", Size = new ItemSize(3, 2), CanRotate = true } },

            { 65,  new InventoryDefinition { Id = 65,  Name = "P.R.L. 412", Size = new ItemSize(7, 3), CanRotate = true } },

            { 68,  new InventoryDefinition { Id = 68,  Name = "Scope (Rifle)", Size = new ItemSize(3, 1), CanRotate = true } },
            { 69,  new InventoryDefinition { Id = 69,  Name = "Scope (Semi-Auto Rifle)", Size = new ItemSize(3, 1), CanRotate = true } },
            { 70,  new InventoryDefinition { Id = 70,  Name = "Mine-Darts", Size = new ItemSize(2, 1), CanRotate = true } },

            { 71,  new InventoryDefinition { Id = 71,  Name = "Shotgun**", Size = new ItemSize(8, 2), CanRotate = true } },

            { 81,  new InventoryDefinition { Id = 81,  Name = "Rifle (Semi-Auto) w/ Infrared Scope", Size = new ItemSize(7, 2), CanRotate = true } },
            { 82,  new InventoryDefinition { Id = 82,  Name = "Krauser's Bow", Size = new ItemSize(5, 2), CanRotate = true } },
            { 83,  new InventoryDefinition { Id = 83,  Name = "Chicago Typewriter (Regular)", Size = new ItemSize(7, 3), CanRotate = true } },

            { 107, new InventoryDefinition { Id = 107, Name = "Rifle + Scope", Size = new ItemSize(9, 1), CanRotate = true } },
            { 108, new InventoryDefinition { Id = 108, Name = "Rifle (Semi-Auto) w/ Scope", Size = new ItemSize(7, 2), CanRotate = true } },
            { 109, new InventoryDefinition { Id = 109, Name = "Infinite Launcher", Size = new ItemSize(8, 2), CanRotate = true } },

            { 148, new InventoryDefinition { Id = 148, Name = "Riot Gun", Size = new ItemSize(8, 2), CanRotate = true } },
            { 153, new InventoryDefinition { Id = 153, Name = "Rifle w/ Infrared Scope", Size = new ItemSize(9, 1), CanRotate = true } },

            { 170, new InventoryDefinition { Id = 170, Name = "Scope (Mine-Thrower)", Size = new ItemSize(2, 2), CanRotate = true } },
            { 171, new InventoryDefinition { Id = 171, Name = "Mine Thrower + Scope", Size = new ItemSize(5, 2), CanRotate = true } },

            { 197, new InventoryDefinition { Id = 197, Name = "Infrared Scope", Size = new ItemSize(3, 1), CanRotate = true } },

            { 256, new InventoryDefinition { Id = 256, Name = "Punisher", Size = new ItemSize(3, 2), CanRotate = true } },
            { 257, new InventoryDefinition { Id = 257, Name = "Handgun", Size = new ItemSize(3, 2), CanRotate = true } },
            { 258, new InventoryDefinition { Id = 258, Name = "Shotgun", Size = new ItemSize(8, 2), CanRotate = true } },
            { 259, new InventoryDefinition { Id = 259, Name = "Mine Thrower", Size = new ItemSize(5, 2), CanRotate = true } },
            { 260, new InventoryDefinition { Id = 260, Name = "Handcannon", Size = new ItemSize(4, 2), CanRotate = true } },
            { 261, new InventoryDefinition { Id = 261, Name = "Mine Thrower + Scope", Size = new ItemSize(5, 2), CanRotate = true } },
        };

        public static bool TryGetDefinition(int itemId, out InventoryDefinition definition)
        {
            return Items.TryGetValue(itemId, out definition);
        }

        public static ItemSize GetEffectiveSize(int itemId, int rotation)
        {
            if (!Items.TryGetValue(itemId, out InventoryDefinition definition))
                return new ItemSize(1, 1);

            bool swapsDimensions = rotation is 257 or 259 or 261 or 263;

            if (!definition.CanRotate || !swapsDimensions)
                return definition.Size;

            return new ItemSize(definition.Size.Height, definition.Size.Width);
        }
    }
}
