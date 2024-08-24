namespace CultOfQoL.Patches.AutoCollect;

[Harmony]
public static class AutoCollectTranspilers
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.Update))]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        var codes = new List<CodeInstruction>(instructions);
        var delayField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Delay");

        if (Plugin.EnableAutoCollect.Value && originalMethod.GetRealDeclaringType().Name.Contains("Interaction_CollectResourceChest"))
        {
            var activatingField = AccessTools.Field(originalMethod.GetRealDeclaringType(), "Activating");
            
            var updateMethod = AccessTools.Method(typeof(Interaction), nameof(Interaction.Update));
            var delayBetweenChecksField = AccessTools.Field(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.delayBetweenChecks));

            var delayStartIndex = -1;
            var delayEndIndex = -1;
            Label? delayTargetLabel = null;
            
            var activatingStartIndex = -1;
            var activatingEndIndex = -1;
            Label? activatingTargetLabel = null;

            for (var i = 0; i < codes.Count; i++)
            {
                // removes this block of code
                // if ((this.delayBetweenChecks -= Time.deltaTime) >= 0f && !InputManager.Gameplay.GetInteractButtonHeld(null))
                // {
                //     this.Activating = false;
                //     return;
                // }
                // this.delayBetweenChecks = 0.4f;
                if (codes[i].Calls(updateMethod) && delayStartIndex == -1)
                {
                    delayStartIndex = i + 1;
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found Start Delay at {delayStartIndex}");
                }
        
                if (codes[i].StoresField(delayBetweenChecksField) && codes[i - 1].opcode == OpCodes.Ldc_R4 && delayStartIndex != -1)
                {
                    delayEndIndex = i + 1;
                    delayTargetLabel = codes[i + 1].operand as Label?;
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found End Delay at {delayEndIndex}");
                    break;
                }
            }
        
            if (delayStartIndex != -1 && delayEndIndex != -1)
            {
                var delayLabels = codes[delayStartIndex].labels.ToList();
                codes.RemoveRange(delayStartIndex, delayEndIndex - delayStartIndex);
        
                if (delayLabels.Any())
                {
                    codes[delayStartIndex].labels.AddRange(delayLabels);
                }
        
                if (delayTargetLabel.HasValue)
                {
                    codes[delayStartIndex].labels.Add(delayTargetLabel.Value);
                }
        
                foreach (var t in codes)
                {
                    if (t.operand is not Label label || !label.Equals(delayTargetLabel)) continue;
        
                    if (delayTargetLabel != null)
                    {
                        t.operand = codes[delayStartIndex].labels.Count > 0 ? codes[delayStartIndex].labels[0] : delayTargetLabel.Value;
                    }
                }
            }
            else
            {
                if (!originalMethod.GetRealDeclaringType().Name.Contains("Lumber"))
                {
                    Plugin.Log.LogError($"{originalMethod.GetRealDeclaringType().Name}: Could not find start and end delay");
                }
            }
        
        
            for (var i = 0; i < codes.Count; i++)
            {
                // removes this block of code
                // if (this.Activating && (this.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp(base.playerFarming) || Vector3.Distance(base.transform.position, base.playerFarming.transform.position) > this.DistanceToTriggerDeposits))
                // {
                //     this.Activating = false;
                // }
                if (codes[i].LoadsField(activatingField) && activatingStartIndex == -1)
                {
                    activatingStartIndex = i - 1;
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found Start Activating at {activatingStartIndex}");
                }
        
                if (codes[i].StoresField(activatingField) && activatingStartIndex != -1)
                {
                    activatingEndIndex = i + 1;
                    activatingTargetLabel = codes[i + 1].operand as Label?;
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found End Activating at {activatingEndIndex}");
                    break;
                }
            }
        
            if (activatingStartIndex != -1 && activatingEndIndex != -1)
            {
                var activatingLabels = codes[activatingStartIndex].labels.ToList();
                codes.RemoveRange(activatingStartIndex, activatingEndIndex - activatingStartIndex);
        
                if (activatingLabels.Any())
                {
                    codes[activatingStartIndex].labels.AddRange(activatingLabels);
                }
        
                if (activatingTargetLabel.HasValue)
                {
                    codes[activatingStartIndex].labels.Add(activatingTargetLabel.Value);
                }
        
                foreach (var t in codes)
                {
                    if (t.operand is not Label label || !label.Equals(activatingTargetLabel)) continue;
        
                    if (activatingTargetLabel != null)
                    {
                        t.operand = codes[activatingStartIndex].labels.Count > 0 ? codes[activatingStartIndex].labels[0] : activatingTargetLabel.Value;
                    }
                }
            }
            else
            {
                Plugin.Log.LogError($"{originalMethod.GetRealDeclaringType().Name}: Could not find start and end activating");
            }
        }

        if (Plugin.FastCollecting.Value)
        {
            for (var i = 0; i < codes.Count; i++)
            {
                //changes delay
                if (codes[i].opcode == OpCodes.Ldc_R4 && i + 1 < codes.Count && codes[i + 1].StoresField(delayField))
                {
                    Plugin.L($"{originalMethod.GetRealDeclaringType().Name}: Found Delay at {i}");
                    codes[i].operand = originalMethod.GetRealDeclaringType().Name.Contains("Lumber") ? 0.025f : 0.01f;
                }
            }
        }
        return codes.AsEnumerable();
    }

}