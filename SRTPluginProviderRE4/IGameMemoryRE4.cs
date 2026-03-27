using SRTPluginProviderRE4.Structs;
using SRTPluginProviderRE4.Structs.GameStructs;
using System;
using System.Collections.Generic;

namespace SRTPluginProviderRE4
{
    public interface IGameMemoryRE4
    {
        // Versioninfo
        string GameName { get; }
        string VersionInfo { get; }
        string GameInfo { get; set; }
        GameSaveData GameData { get; set; }
        PlayerKills GamePlayerKills { get; set; }
        InventoryIDs GamePlayerItemID { get; set; }
        GamePlayer Player { get; set; }
        GamePlayer Player2 { get; set; }
        string PlayerName { get; set; }
        string PlayerName2 { get; set; }
        EnemyHP[] EnemyHealth { get; set; }
        int AttacheCaseId { get; set; }
        int AttacheCaseWidth { get; set; }
        int AttacheCaseHeight { get; set; }
        List<InventoryItem> InventoryItems { get; set; }
        TimeSpan IGTTimeSpan { get; }
        string IGTFormattedString { get; }
        string InventoryDebugString { get; }
    }
}