namespace StationeersCommunityPatches
{
    using Assets.Scripts;
    using UnityEngine;
    using System;
    using BepInEx;
    using HarmonyLib;
    using System.Collections;

    class L
    {
        private static BepInEx.Logging.ManualLogSource _logger;

        public static void SetLogger(BepInEx.Logging.ManualLogSource logger)
        {
            _logger = logger;
        }

        public static void Debug(string message)
        {
            _logger?.LogDebug(message);
        }

        public static void Log(string message)
        {
            _logger?.LogInfo(message);
        }

        public static void Info(string message)
        {
            _logger?.LogInfo(message);
        }

        public static void Error(string message)
        {
            _logger?.LogError(message);
        }

        public static void Warning(string message)
        {
            _logger?.LogWarning(message);
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


    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class CommunityPatchesPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "aproposmath-stationeers-community-pathes";
        public const string PluginName = "Stationeers Community Patches";
        public const string PluginVersion = VersionInfo.Version;

        private void Awake()
        {
            try
            {
                L.SetLogger(this.Logger);
                this.Logger.LogInfo(
                    $"Awake ${PluginName} {VersionInfo.VersionGit}, build time {VersionInfo.BuildTime}");

                var harmony = new Harmony(PluginGuid);
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Error during ${PluginName} {VersionInfo.VersionGit} init: {ex}");
            }
        }
    }
}
