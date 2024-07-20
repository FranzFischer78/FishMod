using RaftModLoader;
﻿using UnityEngine;
using HMLLibrary;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

public class FishMod : Mod
{
	Harmony harmony = new Harmony("com.franzfischer78.fishmod");

	public void Start()
    {
		var original = typeof(Stat_Oxygen).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
		var transpiler = typeof(OxygenUnderwaterPatch).GetMethod("Transpiler");
		harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
		Debug.Log("Mod FishMod has been loaded!");
	}

    public void OnModUnload()
    {
		harmony.UnpatchAll();
		Debug.Log("Mod FishMod has been unloaded!");
    }
}


public static class OxygenUnderwaterPatch
{
	public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		var codes = new List<CodeInstruction>(instructions);
		for (int i = 0; i < codes.Count; i++)
		{
			if (codes[i].opcode == OpCodes.Ldc_I4_2)
			{
				codes[i].opcode = OpCodes.Ldc_I4_0;
				codes.RemoveRange(i + 2, 4);
				break;
			}
		}
		return codes;
	}
}