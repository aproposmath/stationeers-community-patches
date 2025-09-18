using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Networking;
using Assets.Scripts.Networking.Transports;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.UI;
using BepInEx;
using BepInEx.Logging;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json;
using UI.Tooltips;
using UnityEngine;
using Util.Commands;

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
