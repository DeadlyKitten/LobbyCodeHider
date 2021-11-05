using HarmonyLib;
using Nick;
using TMPro;

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
        }
    }
}
