namespace RestInPatches.Patches;

// Timber/ore stockpiles use the generic SmartDrawer (SmartDrawer.cs) whose atoms
// evaluate bounded conditions like ConcreteItemsBetween(N, M). When a capacity-
// inflating mod lets inventory exceed M, every bounded atom falls false and the
// pile disappears. This postfix force-activates the highest-threshold atom in
// each group when count exceeds it, so the visual clamps at "full" instead of
// vanishing. No-op on vanilla.
[Harmony]
public static class StockpilePatches
{
    private static readonly ConditionalWeakTable<SmartDrawer, SmartDrawerAtom[]> MaxAtomsCache = new();
    private static readonly SmartDrawerAtom[] EmptyAtoms = Array.Empty<SmartDrawerAtom>();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SmartDrawer), nameof(SmartDrawer.Redraw))]
    public static void SmartDrawer_Redraw_Postfix(SmartDrawer __instance)
    {
        if (__instance == null || __instance._wgo == null)
        {
            return;
        }

        if (!MaxAtomsCache.TryGetValue(__instance, out var maxAtoms))
        {
            maxAtoms = ComputeMaxAtoms(__instance);
            MaxAtomsCache.Add(__instance, maxAtoms);
        }

        if (maxAtoms.Length == 0)
        {
            return;
        }

        var wgo = __instance._wgo;
        var inventory = wgo.data?.inventory;
        if (inventory == null)
        {
            return;
        }

        for (var i = 0; i < maxAtoms.Length; i++)
        {
            var atom = maxAtoms[i];
            if (atom?.obj == null)
            {
                continue;
            }

            int count;
            int threshold;
            switch (atom.condition)
            {
                case SmartDrawerAtom.SmartDrawingCondition.ConcreteItemsBetween:
                    count = wgo.data.GetItemsCount(atom.string_value);
                    threshold = atom.int_value_2;
                    break;
                case SmartDrawerAtom.SmartDrawingCondition.ItemsEqual:
                    count = inventory.Count;
                    threshold = atom.int_value;
                    break;
                case SmartDrawerAtom.SmartDrawingCondition.TotalItemsCount:
                    count = 0;
                    for (var j = 0; j < inventory.Count; j++)
                    {
                        count += inventory[j].value;
                    }
                    threshold = atom.int_value_2;
                    break;
                default:
                    continue;
            }

            if (count > threshold)
            {
                atom.obj.SetActive(true);
            }
        }
    }

    private static SmartDrawerAtom[] ComputeMaxAtoms(SmartDrawer drawer)
    {
        var atoms = drawer.smart_drawer_atoms;
        if (atoms == null || atoms.Count == 0)
        {
            return EmptyAtoms;
        }

        var maxPerGroup = new Dictionary<(SmartDrawerAtom.SmartDrawingCondition condition, string key), SmartDrawerAtom>();

        for (var i = 0; i < atoms.Count; i++)
        {
            var atom = atoms[i];
            if (atom?.obj == null)
            {
                continue;
            }

            int candidateThreshold;
            switch (atom.condition)
            {
                case SmartDrawerAtom.SmartDrawingCondition.ConcreteItemsBetween:
                case SmartDrawerAtom.SmartDrawingCondition.TotalItemsCount:
                    candidateThreshold = atom.int_value_2;
                    break;
                case SmartDrawerAtom.SmartDrawingCondition.ItemsEqual:
                    candidateThreshold = atom.int_value;
                    break;
                default:
                    continue;
            }

            var key = (atom.condition, atom.string_value ?? string.Empty);
            if (!maxPerGroup.TryGetValue(key, out var cur))
            {
                maxPerGroup[key] = atom;
                continue;
            }

            var curThreshold = cur.condition == SmartDrawerAtom.SmartDrawingCondition.ItemsEqual
                ? cur.int_value
                : cur.int_value_2;

            if (candidateThreshold > curThreshold)
            {
                maxPerGroup[key] = atom;
            }
        }

        if (maxPerGroup.Count == 0)
        {
            return EmptyAtoms;
        }

        var result = new SmartDrawerAtom[maxPerGroup.Count];
        var idx = 0;
        foreach (var kv in maxPerGroup)
        {
            result[idx++] = kv.Value;
        }
        return result;
    }
}
