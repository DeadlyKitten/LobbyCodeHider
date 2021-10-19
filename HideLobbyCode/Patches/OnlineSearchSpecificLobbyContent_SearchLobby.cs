using System;
using System.Collections.Generic;
using System.Text;
using Nick;
using HarmonyLib;
using TMPro;
using System.Reflection.Emit;
using System.Linq;

namespace HideLobbyCode.Patches
{
    [HarmonyPatch(typeof(OnlineSearchSpecificLobbyContent), "SearchLobby")]
    class OnlineSearchSpecificLobbyContent_SearchLobby
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var instructions = codes.ToList();

            var currentInstruction = instructions.FindIndex(x => x.opcode == OpCodes.Callvirt);

            var getText = typeof(TMP_InputField).GetProperty("text").GetGetMethod();

            instructions[++currentInstruction].operand = getText;
            instructions.RemoveAt(++currentInstruction);

            return instructions;
        }
    }
}
