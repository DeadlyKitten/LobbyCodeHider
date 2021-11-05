using System;
using System.Collections.Generic;
using System.Text;
using Nick;
using TMPro;
using HarmonyLib;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineLobbyContent), "ToggleLobbyId")]
    class OnlineLobbyContent_ToggleLobbyId
    {
        static bool Prefix(bool value, TextMeshProUGUI ___lobbyID) => !Plugin.showCopyText;
    }
}
