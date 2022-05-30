using System;
using SRTPluginProviderRE4.Structs;
using SRTPluginProviderRE4.Structs.GameStructs;

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
        TimeSpan IGTTimeSpan { get; }
        string IGTFormattedString { get; }
    }
}