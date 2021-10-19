using HarmonyLib;
using Nick;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineSearchSpecificLobbyContent), "SearchLobby")]
    class OnlineSearchSpecificLobbyContent_SearchLobby
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var instructions = codes.ToList(); // use list to access elements by index

            var currentInstruction = instructions.FindIndex(x => x.opcode == OpCodes.Callvirt);

            var getText = typeof(TMP_InputField).GetProperty("text").GetGetMethod();

            instructions[++currentInstruction].operand = getText; // use the correct method
            instructions.RemoveAt(++currentInstruction); // don't need this anymore

            return instructions;
        }
    }
}
