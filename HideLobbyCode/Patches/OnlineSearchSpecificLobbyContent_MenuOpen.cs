using HarmonyLib;
using Nick;
using UnityEngine;
using TMPro;
using SMU.Reflection;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineSearchSpecificLobbyContent), "MenuOpen")]
    class OnlineSearchSpecificLobbyContent_MenuOpen
    {
        static void Postfix(OnlineSearchSpecificLobbyContent __instance, ref MenuTextInput ____input)
        {
            ____input.Input.contentType = TMP_InputField.ContentType.Password;
            Plugin.searchBoxOpen = true;

            // Create UI stuff
            var nav = __instance.transform.Find("move/Canvas/enterpassword/NavigationHolder");
            var rejectButton = nav.Find("RejectButton");
            rejectButton.localPosition = new Vector3(225.5f,74,0);
            var button = GameObject.Instantiate(rejectButton, nav);
            button.name = "PasteButton";
            button.transform.localPosition = new Vector3(-85.5f, 74, 0);
            button.Find("Text").GetComponent<TextMeshProUGUI>().text = "Paste";
            var buttonImage = button.Find("ButtonImage").GetComponent<ButtonImage>();
            buttonImage.SetField("buttonType", MenuButtonGraphics.RequestButton.opt2);
        }
    }
}
