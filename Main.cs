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
