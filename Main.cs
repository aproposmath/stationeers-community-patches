using System;
using System.Diagnostics;
using Assets.Scripts;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace StationeersCommunityPatches
{
    public class Timer : IDisposable
    {
        public Stopwatch stopwatch;
        public string name;

        public Timer(string name)
        {
            this.name = name;
            stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            L.Debug($"{name} took {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    public static class L
    {
        private static ManualLogSource _logger;
#if DEBUG
        private static bool _debugEnabled = true;
#else
        private static bool _debugEnabled = false;
#endif

        public static string Timestamp => DateTime.Now.ToString("HH:mm:ss.fff - ");

        public static void Initialize(ManualLogSource logger)
        {
            _logger = logger;
        }

        public static bool ToggleDebug()
        {
            _debugEnabled = !_debugEnabled;
            return _debugEnabled;
        }

        public static void Log(string message)
        {
            _logger?.LogMessage(Timestamp + message?.ToString());
        }

        public static void Info(object msg)
        {
            _logger?.LogInfo(Timestamp + msg?.ToString());
        }

        public static void Error(object msg)
        {
            _logger?.LogError(Timestamp + msg?.ToString());
        }

        public static void Debug(object msg)
        {
            if (!_debugEnabled)
                return;
            _logger?.LogDebug(Timestamp + msg?.ToString());
        }
    }

    public class BasePatch
    {
        public static bool IsGameNewerOrEqual(
            string firstGoodVersionString = "0.2.5912.26032",
            string patchName = ""
        )
        {
            Version firstGoodVersion = new Version(firstGoodVersionString);
            Version version = new Version(GameManager.GetGameVersion().Trim());
            ;
            bool result = version >= firstGoodVersion;
            if (result)
            {
                L.Log($"Skipping Patch {patchName}, version {version} >= {firstGoodVersion}");
            }
            return result;
        }
    }

    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class CommunityPatchesPlugin : BaseUnityPlugin
    {
        public const string pluginGuid = "4d47fe06-7bd8-46e7-9b16-0925779a7b7f";
        public const string pluginName = "CommunityPatches";
        public const string pluginVersion = VersionInfo.Version;

        private void Awake()
        {
            try
            {
                L.Initialize(Logger);
                L.Info(
                    $"Awake CommunityPatches {VersionInfo.VersionGit} build time {VersionInfo.BuildTime}"
                );

                var harmony = new Harmony(pluginGuid);
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                L.Error($"Error during CommunityPatches {VersionInfo.VersionGit} init: {ex}");
            }
        }
    }
}
