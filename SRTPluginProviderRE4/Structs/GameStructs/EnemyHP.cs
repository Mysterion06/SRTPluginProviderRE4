using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTPluginProviderRE4.Structs.GameStructs
{
    public class Boss
    {
        //public static float[] HitPoints = { 2900, 3400, 3700, 9000, 25000, 26000, 30000, 75000, 100000 };
        public static Dictionary<short, string> Bosses = new Dictionary<short, string>()
        {
            { 2900, "Bella" },
            { 3400, "Cassandra" },
            { 3700, "Daniella" },
            { 9000, "Lady D" },
            { 25000, "Urias" },
            { 26000, "Moreau" },
            { 30000, "Miranda" },
            { 31000, "Urias" },
            { 32000, "Heisenberg" }
        };
    }

    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct EnemyHP
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsTrigger)
                {
                    return string.Format("TRIGGER", CurrentHP, MaximumHP, Percentage);
                }
                else if (IsAlive)
                {
                    return string.Format("{0} / {1} ({2:P1})", CurrentHP, MaximumHP, Percentage);
                }
                return "DEAD / DEAD (0%)";
            }
        }

        public bool IsBoss => Boss.Bosses.ContainsKey(_maximumHP);
        public string BossName => IsBoss ? Boss.Bosses[_maximumHP] : "";
        public short MaximumHP { get => _maximumHP; }
        internal short _maximumHP;
        public short CurrentHP { get => _currentHP; }
        internal short _currentHP;
        public bool IsTrigger => MaximumHP <= 10 || MaximumHP > 32000 || MaximumHP == 1000;
        public bool IsNaN => false;
        public bool IsAlive => !IsNaN && !IsTrigger && CurrentHP > 0;
        public float Percentage => ((IsAlive) ? (float)CurrentHP / MaximumHP : 0f);
        public string DebugPointerAddress { get; set; }
        public string DebugInfo { get; set; }
    }
}
