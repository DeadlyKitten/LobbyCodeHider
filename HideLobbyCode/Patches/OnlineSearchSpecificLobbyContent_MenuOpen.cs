using HarmonyLib;
using Nick;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineSearchSpecificLobbyContent), "MenuOpen")]
    class OnlineSearchSpecificLobbyContent_MenuOpen
    {
        static void Postfix(ref MenuTextInput ____input) => ____input.Input.contentType = TMPro.TMP_InputField.ContentType.Password;
    }
}
