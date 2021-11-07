using HarmonyLib;
using Nick;
using TMPro;
using UnityEngine;
using SMU.Reflection;
using UnityEngine.UI;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineLobbyContent), "SetupOnlineDataStuff")]
    class OnlineLobbyContent_SetupOnlineDataStuff
    {
        static void Postfix(MenuSharedState ___sharedState, TextMeshProUGUI ___lobbyID)
        {
            var data = ___sharedState.GetData("online_lobbydata") as OnlineLobbyContent.Data;

            Plugin.lobbyCode = data.lobby.LobbySettings.pass;
            Plugin.lobbyIDText = ___lobbyID;
            OnlineMenuContent.GetData(___sharedState).client.CurrentRegion(out Plugin.region);

            // set up Tooltip for copying lobby code
            var copyTooltip = GameObject.Instantiate(___lobbyID.transform.parent.gameObject, ___lobbyID.transform.parent);
            copyTooltip.name = "Copy Tooltip";
            copyTooltip.transform.localPosition = new Vector3(20, 90, 0);
            Component.Destroy(copyTooltip.GetComponent<Image>());
            Component.Destroy(copyTooltip.transform.Find("Icon").GetComponent<Image>());
            copyTooltip.transform.Find("Icon/ButtonImage").localPosition = new Vector3(175, 0, 0);
            copyTooltip.GetComponent<ButtonImage>().SetField("buttonType", MenuButtonGraphics.RequestButton.opt2);
            copyTooltip.GetComponentInChildren<TextMeshProUGUI>().text = "Copy to Clipboard";

        }
    }
}
