using ProcessMemory;
using SRTPluginProviderRE4.Structs.GameStructs;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRE4
{
    internal unsafe class GameMemoryRE4Scanner : IDisposable
    {

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

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }

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
            } else if(gv == GameVersion.RE4_1_0_6)
            {
                pointerAddressGameData = 0x85BE74;
                pointerAddressKills = 0x85F344;
                pointerAddressLastItemID = 0x855664;
                pointerAddressHP = 0x85BE94;
                pointerAddressHP2 = 0x85BE98;
            }
            else
            {
                gameMemoryValues._gameInfo = "Unknown Release";
            }
        }

        internal unsafe IGameMemoryRE4 Refresh()
        {
            // Game Data
            gameMemoryValues._gameData = memoryAccess.GetAt<GameSaveData>(IntPtr.Add(BaseAddress, pointerAddressGameData));
            gameMemoryValues._playerKills = memoryAccess.GetAt<PlayerKills>(IntPtr.Add(BaseAddress, pointerAddressKills));
            gameMemoryValues._itemID = memoryAccess.GetAt<InventoryIDs>(IntPtr.Add(BaseAddress, pointerAddressLastItemID));

            gameMemoryValues._player = memoryAccess.GetAt<GamePlayer>(IntPtr.Add(BaseAddress, pointerAddressHP));
            gameMemoryValues._playerName = "Leon: ";

            gameMemoryValues._player2 = memoryAccess.GetAt<GamePlayer>(IntPtr.Add(BaseAddress, pointerAddressHP2));
            gameMemoryValues._playerName2 = "Ashley: ";

            HasScanned = true;
            return gameMemoryValues;
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