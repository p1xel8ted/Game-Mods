using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace AnAlchemicalCollection;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class DialoguePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BubbleText), nameof(BubbleText.TypingAnim))]
    public static bool BubbleText_TypingAnim(ref BubbleText __instance)
    {
        if (!Plugin.InstantDialogue.Value) return true;
        __instance.dialogTxt.enableWordWrapping = true;
        __instance.dialogTxt.text = __instance.completeDialog;
        __instance.animationState = BubbleText.BubbleTextState.TYPING_COMPLETE;
        __instance.buttonHint.SetActive(true);
        __instance.SkipDialog();
        return false;
    }
    

    [HarmonyPrefix]
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BubbleText), nameof(BubbleText.Set))]
    [HarmonyPatch(typeof(BubbleText), nameof(BubbleText.SetBubbleTextEmotion))]
    [HarmonyPatch(typeof(BubbleText), nameof(BubbleText.TypingAnim))]
    public static void BubbleText_Set(ref BubbleText __instance)
    {
        if (!Plugin.FasterDialogue.Value) return;
        __instance.typeDelay = 0;
        __instance.typingSpeed = float.MaxValue;
        __instance.typeDelayCnt = 0f;
    } 
}