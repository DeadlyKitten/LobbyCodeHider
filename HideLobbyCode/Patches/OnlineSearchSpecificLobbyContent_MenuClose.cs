using HarmonyLib;
using Nick;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineSearchSpecificLobbyContent), "MenuClose")]
    class OnlineSearchSpecificLobbyContent_MenuClose
    {
        static void Postfix(ref MenuTextInput ____input)
        {
            Plugin.searchBoxOpen = false;
        }
    }
}
