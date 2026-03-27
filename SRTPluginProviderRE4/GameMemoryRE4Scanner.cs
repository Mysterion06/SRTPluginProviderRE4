using ProcessMemory;
using SRTPluginProviderRE4.Structs.GameStructs;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRE4
{
    internal unsafe class GameMemoryRE4Scanner : IDisposable
    {
        private static readonly int MAX_ENEMIES = 32;
        private const int INVENTORY_ENTRY_SIZE = 14;
        private const int MAX_ENTRIES = 128;

        // Variables
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryRE4 gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;


        // Pointer Address Variables
        private int pointerAddressGameData;
        private int pointerAddressKills;
        private int pointerAddressLastItemID;
        private int pointerAddressHP;
        private int pointerAddressHP2;
        private int pointerAddressEnemyHP;
        private int pointerAddressInventory;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer[] PointerEnemyHP { get; set; }
        private MultilevelPointer PointerInventory { get; set; }

        internal GameMemoryRE4Scanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryRE4();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName));

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_32BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                PointerInventory = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressInventory));

                gameMemoryValues._enemyHealth = new EnemyHP[MAX_ENEMIES];
                for (int i = 0; i < MAX_ENEMIES; ++i)
                    gameMemoryValues._enemyHealth[i] = new EnemyHP();

                GenerateEnemyEntries();
            }
        }
        private unsafe void GenerateEnemyEntries()
        {
            if (PointerEnemyHP == null)
            {
                PointerEnemyHP = new MultilevelPointer[MAX_ENEMIES];

                for (int i = 0; i < MAX_ENEMIES; i++)
                {
                    int[] offsets = new int[i];

                    for (int j = 0; j < offsets.Length; j++)
                        offsets[j] = 0x8;

                    PointerEnemyHP[i] = new MultilevelPointer(memoryAccess,IntPtr.Add(BaseAddress, pointerAddressEnemyHP),offsets);
                }
            }
        }

        private void SelectPointerAddresses(GameVersion gv)
        {
            if (gv == GameVersion.RE4_1_1_0)
            {
                pointerAddressGameData = 0x85F6F4;
                pointerAddressKills = 0x862BC4;
                pointerAddressLastItemID = 0x858EE4;
                pointerAddressHP = 0x85F714;
                pointerAddressHP2 = 0x85F718;
                pointerAddressEnemyHP = 0x867594;
                pointerAddressInventory = 0x870FE8;
            } else if(gv == GameVersion.RE4_1_0_6)
            {
                pointerAddressGameData = 0x85BE74;
                pointerAddressKills = 0x85F344;
                pointerAddressLastItemID = 0x855664;
                pointerAddressHP = 0x85BE94;
                pointerAddressHP2 = 0x85BE98;
                pointerAddressEnemyHP = 0x867594;
                pointerAddressInventory = 0x870FE8;
            } else if(gv == GameVersion.RE4_JP)
            {
                pointerAddressGameData = 0x85BE74;
                pointerAddressKills = 0x85F344;
                pointerAddressLastItemID = 0x855664;
                pointerAddressHP = 0x85BE94;
                pointerAddressHP2 = 0x85BE98;
                pointerAddressEnemyHP = 0x867594;
                pointerAddressInventory = 0x870FE8;
            } else if(gv == GameVersion.RE4_GER)
            {
                pointerAddressGameData = 0x85BE74;
                pointerAddressKills = 0x85F344;
                pointerAddressLastItemID = 0x855664;
                pointerAddressHP = 0x85BE94;
                pointerAddressHP2 = 0x85BE98;
                pointerAddressEnemyHP = 0x867594;
                pointerAddressInventory = 0x870FE8;
            }
            else
            {
                gameMemoryValues._gameInfo = "Unknown Release";
            }
        }

        internal void UpdatePointers()
        {
            PointerInventory?.UpdatePointers();
            for (int i = 0; i < PointerEnemyHP.Length; ++i)
                PointerEnemyHP[i].UpdatePointers();
        }

        internal unsafe IGameMemoryRE4 Refresh()
        {
            UpdatePointers();
            // Game Data
            gameMemoryValues._gameData = memoryAccess.GetAt<GameSaveData>(IntPtr.Add(BaseAddress, pointerAddressGameData));
            gameMemoryValues._playerKills = memoryAccess.GetAt<PlayerKills>(IntPtr.Add(BaseAddress, pointerAddressKills));
            gameMemoryValues._itemID = memoryAccess.GetAt<InventoryIDs>(IntPtr.Add(BaseAddress, pointerAddressLastItemID));

            gameMemoryValues._player = memoryAccess.GetAt<GamePlayer>(IntPtr.Add(BaseAddress, pointerAddressHP));
            gameMemoryValues._playerName = "Leon: ";

            gameMemoryValues._player2 = memoryAccess.GetAt<GamePlayer>(IntPtr.Add(BaseAddress, pointerAddressHP2));
            gameMemoryValues._playerName2 = "Ashley: ";

            GetEnemies();
            DebugInventoryTest();
            HasScanned = true;
            return gameMemoryValues;
        }

        private void GetEnemies()
        {
            for (int i = 0; i < gameMemoryValues.EnemyHealth.Length; ++i)
            {
                if (PointerEnemyHP[i].Address != IntPtr.Zero)
                {
                    GamePlayer enemyHP = PointerEnemyHP[i].Deref<GamePlayer>(0x324);
                    gameMemoryValues.EnemyHealth[i]._maximumHP = enemyHP.MaxHP;
                    gameMemoryValues.EnemyHealth[i]._currentHP = enemyHP.CurrentHP;
                }
                else
                {
                    gameMemoryValues.EnemyHealth[i]._maximumHP = 0;
                    gameMemoryValues.EnemyHealth[i]._currentHP = 0;
                }
            }
        }
        private InventoryItem ParseInventoryItem(byte[] data)
        {
            if (data == null || data.Length < INVENTORY_ENTRY_SIZE)
                return null;

            int itemId = BitConverter.ToUInt16(data, 0x0);
            int amount = BitConverter.ToUInt16(data, 0x2);
            int existsRaw = BitConverter.ToUInt16(data, 0x4);
            int upgradesRaw = BitConverter.ToUInt16(data, 0x6);
            int loadedAmmo = BitConverter.ToUInt16(data, 0x8);
            int positionRaw1 = data[0xA];
            int positionRaw2 = data[0xB];
            int rotation = BitConverter.ToUInt16(data, 0xC);

            InventoryDefinitions.TryGetDefinition(itemId, out InventoryDefinition definition);

            string itemName = ItemDatabase.ItemNames.TryGetValue(itemId, out string name)
                ? name
                : "Unknown";

            ItemSize effectiveSize = InventoryDefinitions.GetEffectiveSize(itemId, rotation);

            DecodePosition(positionRaw1, positionRaw2, effectiveSize.Width, effectiveSize.Height, out int gridX, out int gridY);

            return new InventoryItem
            {
                ItemId = itemId,
                Name = itemName,
                Amount = amount,
                Exists = existsRaw == 1,
                LoadedAmmo = loadedAmmo,
                GridX = gridX,
                GridY = gridY,
                Rotation = rotation,
                Width = effectiveSize.Width,
                Height = effectiveSize.Height,
                UpgradeFireSpeed = (upgradesRaw >> 0) & 0xF,
                UpgradeFirepower = (upgradesRaw >> 4) & 0xF,
                UpgradeCapacity = (upgradesRaw >> 8) & 0xF,
                UpgradeReloadSpeed = (upgradesRaw >> 12) & 0xF
            };
        }

        private void DecodePosition(int pp1, int pp2, int width, int height, out int x, out int y)
        {
            x = (pp1 - (width - 1)) / 2;
            y = (pp2 - (height - 1)) / 2;
        }

        private bool TryReadInventoryItem(IntPtr address, out InventoryItem item)
        {
            item = null;

            if (!SafeReadByteArray(address, INVENTORY_ENTRY_SIZE, out byte[] bytes))
                return false;

            item = ParseInventoryItem(bytes);
            return item != null;
        }

        private int ReadAttacheCaseId(IntPtr inventoryBaseAddress)
        {
            if (!SafeReadByteArray(inventoryBaseAddress, INVENTORY_ENTRY_SIZE, out byte[] bytes))
                return -1;

            return BitConverter.ToUInt16(bytes, 0x0);
        }

        private void GetAttacheCaseDimensions(int caseId, out int width, out int height)
        {
            switch (caseId)
            {
                case 124: // S
                    width = 10;
                    height = 6;
                    break;
                case 125: // M
                    width = 10;
                    height = 7;
                    break;
                case 126: // L
                    width = 10;
                    height = 8;
                    break;
                case 127: // XL
                    width = 15;
                    height = 8;
                    break;
                default:
                    width = 0;
                    height = 0;
                    break;
            }
        }

        private void ReadInventory(IntPtr inventoryBaseAddress, int maxEntries)
        {
            gameMemoryValues._attacheCaseId = ReadAttacheCaseId(inventoryBaseAddress);
            GetAttacheCaseDimensions(
                gameMemoryValues._attacheCaseId,
                out gameMemoryValues._attacheCaseWidth,
                out gameMemoryValues._attacheCaseHeight);

            ReadInventoryItems(inventoryBaseAddress, maxEntries);
        }

        private void ReadInventoryItems(IntPtr inventoryBaseAddress, int maxEntries)
        {
            gameMemoryValues._inventoryItems.Clear();

            for (int i = 0; i < maxEntries; i++)
            {
                IntPtr entryAddress = IntPtr.Add(
                    inventoryBaseAddress,
                    INVENTORY_ENTRY_SIZE + (i * INVENTORY_ENTRY_SIZE));

                if (!TryReadInventoryItem(entryAddress, out InventoryItem item))
                    break;

                if (item == null)
                    continue;

                if (!item.Exists)
                    continue;

                if (item.ItemId < 0 || item.ItemId > 172)
                    continue;

                if (item.Amount <= 0 && item.LoadedAmmo <= 0)
                    continue;

                gameMemoryValues._inventoryItems.Add(item);
            }
        }

        internal void DebugInventoryTest()
        {
            if (PointerInventory == null || PointerInventory.Address == IntPtr.Zero)
            {
                gameMemoryValues._gameInfo = "Inventory pointer invalid";
                gameMemoryValues._inventoryItems.Clear();
                return;
            }

            IntPtr inventoryBaseAddress = PointerInventory.Address;
            ReadInventory(inventoryBaseAddress, MAX_ENTRIES);

            gameMemoryValues._gameInfo =
                $"CaseID: {gameMemoryValues.AttacheCaseId} ({gameMemoryValues.AttacheCaseWidth}x{gameMemoryValues.AttacheCaseHeight})\n" +
                gameMemoryValues.InventoryDebugString;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls


        private unsafe bool SafeReadByteArray(IntPtr address, int size, out byte[] readBytes)
        {
            readBytes = new byte[size];
            fixed (byte* p = readBytes)
            {
                return memoryAccess.TryGetByteArrayAt(address, size, p);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}