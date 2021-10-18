using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HideLobbyCode.Utilities;
using Nick;
using System;
using System.Collections;
using UnityEngine;

namespace HideLobbyCode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin Instance;
        bool hiddenLastFrame = false;
        Coroutine routine;

        internal static MenuTextContent lobbyIDText;
        internal static string lobbyCode;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

        void Update()
        {
            if (!lobbyIDText)
            {
                hiddenLastFrame = false;
                return;
            }

            if ((bool)MenuSystem.MainInput?.IsButtonDown(MenuAction.ActionButton.Opt2) && hiddenLastFrame)
            {
                lobbyIDText.SetString($"{Localization.online_lobby_id}: {lobbyCode}");
                hiddenLastFrame = false;
            }
            else if (!(bool)MenuSystem.MainInput?.IsButtonDown(MenuAction.ActionButton.Opt2) && !hiddenLastFrame)
            {
                lobbyIDText.SetString("Left Bumper: Display Code\nRight Bumper: Copy Code");
                hiddenLastFrame = true;
            }

            if ((bool)MenuSystem.MainInput?.IsButtonDown(MenuAction.ActionButton.Opt3))
                TryCopyToClipboard();
        }

        private void TryCopyToClipboard()
        {
            try
            {
                WindowsClipboard.SetText(lobbyCode);
                lobbyIDText.SetString("Copied to clipboard!");
            }
            catch (Exception e)
            {
                lobbyIDText.SetString("Failed to copy code to clipboard!\nSee log output for details");
                LogError($"Failed to copy lobby code to clipboard!");
                LogError($"{e.Message}");
                LogError($"{e.StackTrace}");
            }
            finally
            {
                if (routine != null) StopCoroutine(routine);
                routine = StartCoroutine(ResetAfterDelay(2f));
            }
        }

        IEnumerator ResetAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            hiddenLastFrame = false;
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
