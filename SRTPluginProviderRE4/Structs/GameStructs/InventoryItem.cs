using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTPluginProviderRE4.Structs.GameStructs
{
    public class InventoryItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = "Unknown";

        public int Amount { get; set; }
        public bool Exists { get; set; }

        public int LoadedAmmo { get; set; }

        public int GridX { get; set; }
        public int GridY { get; set; }

        public int Rotation { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int UpgradeFireSpeed { get; set; }
        public int UpgradeFirepower { get; set; }
        public int UpgradeCapacity { get; set; }
        public int UpgradeReloadSpeed { get; set; }
    }
}