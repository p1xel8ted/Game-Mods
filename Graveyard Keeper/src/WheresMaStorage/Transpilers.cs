namespace WheresMaStorage;

[HarmonyPatch]
public static class Transpilers
{

    [HarmonyPatch(typeof(AutopsyGUI))]
    public static class AutopsyGuiTranspiler
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            var inner = typeof(AutopsyGUI).GetNestedType("<>c__DisplayClass23_0", AccessTools.all) ?? throw new Exception("Inner Not Found");

            foreach (var method in inner.GetMethods(AccessTools.all))
                if (method.Name.Contains("<OnBodyItemPress>") && method.GetParameters().Length == 2)
                    yield return method;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            if (!Plugin.HideInvalidSelections.Value) return instructionsList.AsEnumerable();
            instructionsList[5].opcode = OpCodes.Ldc_I4_1;

            return instructionsList.AsEnumerable();
        }
    }

    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.CraftReally))]
    public static class CraftComponentTranspiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            if (!Plugin.SharedInventory.Value) return codes.AsEnumerable();

            var usedMultiField = AccessTools.Field(typeof(CraftComponent), nameof(CraftComponent.used_multi_inventory));
            var otherObj = AccessTools.Field(typeof(CraftComponent), nameof(CraftComponent.other_obj));
            var miGetter = AccessTools.Method(typeof(Invents), nameof(Invents.GetMi));
            var insertIndex = -1;
            for (var i = 0; i < codes.Count; i++)
                if (codes[i].opcode == OpCodes.Ldfld && codes[i].operand.ToString().Contains("item_needs") && codes[i - 1].opcode == OpCodes.Ldarg_1)
                {
                    insertIndex = i;
                    Helpers.Log($"[CraftReally]: Found insert index! {i}");
                    break;
                }

            var newCodes = new List<CodeInstruction>
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, usedMultiField),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, otherObj),
                new(OpCodes.Call, miGetter),
                new(OpCodes.Stfld, usedMultiField)
            };
            if (insertIndex != -1)
            {
                codes.InsertRange(insertIndex, newCodes);
                Helpers.Log($"[CraftReally]: Inserted range into {insertIndex}");
            }
            else
            {
                Helpers.Log("[CraftReally]: Insert range not found!", true);
            }

            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(OrganEnhancerGUI))]
    public static class OrganEnhancerGuiTranspiler
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            var inner = typeof(OrganEnhancerGUI).GetNestedType("<>c", AccessTools.all) ?? throw new Exception("Inner Not Found");

            foreach (var method in inner.GetMethods(AccessTools.all))
                if (method.Name.Contains("<OnItemSelect>") && method.GetParameters().Length == 2)
                    yield return method;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            if (!Plugin.HideInvalidSelections.Value) return instructionsList.AsEnumerable();
            instructionsList[5].opcode = OpCodes.Ldc_I4_1;
            instructionsList[49].opcode = OpCodes.Ldc_I4_1;

            return instructionsList.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(RatCellGUI))]
    public static class RatCellGuiTranspiler
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            var inner = typeof(RatCellGUI).GetNestedType("<>c", AccessTools.all) ?? throw new Exception("Inner Not Found");

            foreach (var method in inner.GetMethods(AccessTools.all))
                if (method.Name.Contains("<OnRatInsertionButtonPressed>") && method.GetParameters().Length == 2)
                    yield return method;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            if (!Plugin.HideInvalidSelections.Value) return instructionsList.AsEnumerable();
            instructionsList[5].opcode = OpCodes.Ldc_I4_1;

            return instructionsList.AsEnumerable();
        }
    }
}
