using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HideLobbyCode.Utilities;
using Nick;
using SlapNetwork;
using SMU.Reflection;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HideLobbyCode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin Instance;
        private Coroutine routine;

        internal static TextMeshProUGUI lobbyIDText;
        internal static string lobbyCode;
        internal static RegionData region;
        internal static bool showCopyText;
        internal static bool searchBoxOpen;

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
            if ((bool)MenuSystem.MainInput?.IsButtonDown(MenuAction.ActionButton.Opt2))
            {
                if (searchBoxOpen)
                {
                    var searchBox = Resources.FindObjectsOfTypeAll<OnlineSearchSpecificLobbyContent>().Where(x => x.isActiveAndEnabled).FirstOrDefault();
                    var textField = searchBox.GetField<OnlineSearchSpecificLobbyContent, MenuTextInput>("_input");
                    textField.Input.text = WindowsClipboard.GetText();
                }
                else if (lobbyIDText) TryCopyToClipboard();
            }
            if (showCopyText && (bool)MenuSystem.MainInput?.IsButtonDown(MenuAction.ActionButton.Opt3))
                showCopyText = false;
        }

        private void TryCopyToClipboard()
        {
            try
            {
                WindowsClipboard.SetText(lobbyCode);

                showCopyText = true;
                lobbyIDText.text = "Copied to clipboard!";
            }
            catch (Exception e)
            {
                lobbyIDText.text = "Failed to copy code to clipboard!\nSee log output for details";
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
            showCopyText = false;
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
