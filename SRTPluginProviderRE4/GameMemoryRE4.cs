
using SRTPluginProviderRE4.Structs;
using SRTPluginProviderRE4.Structs.GameStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SRTPluginProviderRE4
{
    public class GameMemoryRE4 : IGameMemoryRE4
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";
        public string GameName => "RE4";

        // Versioninfo
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // GameInfo
        public string GameInfo { get => _gameInfo; set => _gameInfo = value; }
        internal string _gameInfo;

        // Game Data
        public GameSaveData GameData { get => _gameData; set => _gameData = value; }
        internal GameSaveData _gameData;

        public PlayerKills GamePlayerKills { get => _playerKills; set => _playerKills = value; }
        internal PlayerKills _playerKills;

        public InventoryIDs GamePlayerItemID { get => _itemID; set => _itemID = value; }
        internal InventoryIDs _itemID;

        public GamePlayer Player { get => _player; set => _player = value; }
        internal GamePlayer _player;

        public string PlayerName { get => _playerName; set => _playerName = value; }
        internal string _playerName;

        public GamePlayer Player2 { get => _player2; set => _player2 = value; }
        internal GamePlayer _player2;

        public string PlayerName2 { get => _playerName2; set => _playerName2 = value; }
        internal string _playerName2;
        
        public EnemyHP[] EnemyHealth { get => _enemyHealth; set => _enemyHealth = value; }
        internal EnemyHP[] _enemyHealth;

        public int AttacheCaseId { get => _attacheCaseId; set => _attacheCaseId = value; }
        internal int _attacheCaseId;

        public int AttacheCaseWidth { get => _attacheCaseWidth; set => _attacheCaseWidth = value; }
        internal int _attacheCaseWidth;

        public int AttacheCaseHeight { get => _attacheCaseHeight; set => _attacheCaseHeight = value; }
        internal int _attacheCaseHeight;

        public List<InventoryItem> InventoryItems { get => _inventoryItems; set => _inventoryItems = value; }
        internal List<InventoryItem> _inventoryItems = new List<InventoryItem>();

        public TimeSpan IGTTimeSpan
        {
            get
            {
                TimeSpan timespanIGT;

                if (GameData.IGTFrames >= 0f)
                    timespanIGT = TimeSpan.FromSeconds(GameData.IGTFrames / 60);
                else
                    timespanIGT = new TimeSpan();

                return timespanIGT;
            }
        }

        public string InventoryDebugString
        {
            get
            {
                if (_inventoryItems == null || _inventoryItems.Count == 0)
                    return "No Items";

                var sb = new StringBuilder();

                foreach (var item in _inventoryItems)
                {
                    sb.AppendLine($"{item.Name} ({item.Amount}) @ [{item.GridX},{item.GridY}]");
                }

                return sb.ToString();
            }
        }

        public string IGTFormattedString => IGTTimeSpan.ToString(IGT_TIMESPAN_STRING_FORMAT, CultureInfo.InvariantCulture);
    }
}
