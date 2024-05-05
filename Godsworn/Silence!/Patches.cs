using System;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Silence;

[Harmony]
public static class Patches
{
    private static string[] voiceClipNames = [];

    private static string previousClipName = string.Empty;

    private static GameObject VoiceSlider { get; set; }
    private static GameObject DuplicateToggle { get; set; }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayVoicesAudio), typeof(SoundFX), typeof(Entity))]
    public static void AudioManager_PlayVoicesAudio_Prefix(ref AudioManager __instance, ref SoundFX sfx,
        ref Entity Speaker)
    {
        voiceClipNames = Speaker.IsLocalPlayer() ? sfx.Clips.Select(clip => clip.name).ToArray() : [];
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), typeof(ulong))]
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), typeof(double))]
    [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), [])]
    public static void AudioManager_PlayVoicesAudio_Prefix(ref AudioSource __instance)
    {
        ApplyMutingLogic(__instance);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(DataManagerUI), nameof(DataManagerUI.OptionsMenu))]
    public static void DataManagerUI_OptionsMenu(ref DataManagerUI __instance)
    {
        var backgroundColour = new Color(0.1958f, 0.2577f, 0.5f, 0.7647f);

        if (DuplicateToggle is null)
        {
            var template = GameObject.Find("MainUI/Options/Gameplay/Content/ShowRepairHoverAlways");
            var parent = GameObject.Find("MainUI/Options/Gameplay/Content");

            DuplicateToggle = Object.Instantiate(template, parent.transform);
            DuplicateToggle.name = "DontPlayIfJustPlayed";

            var localize = DuplicateToggle.GetComponentInChildren<LocalizeTextMesh>();
            if (localize is not null)
            {
                Object.Destroy(localize);
            }

            var text = DuplicateToggle.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Don't play voice line if just played";

            var button = DuplicateToggle.GetComponentInChildren<Button>();
            button.onClick = null;
            var buttonEvent = new Button.ButtonClickedEvent();
            buttonEvent.AddListener(new Action(() =>
            {
                var toggle = button.transform.GetChild(0);
                toggle.gameObject.SetActive(!toggle.gameObject.activeSelf);
                Plugin.DontPlayIfJustPlayed.Value = toggle.gameObject.activeSelf;
            }));

            button.onClick = buttonEvent;
        }


        if (VoiceSlider is null)
        {
            var template = GameObject.Find("MainUI/Options/Graphics/Content/FpsCap");
            var parent = GameObject.Find("MainUI/Options/Gameplay/Content");

            VoiceSlider = Object.Instantiate(template, parent.transform);
            VoiceSlider.name = "VoiceChance";

            var localize = DuplicateToggle.GetComponentInChildren<LocalizeTextMesh>();
            if (localize is not null)
            {
                Object.Destroy(localize);
            }

            var backgroundImage = VoiceSlider.GetComponent<Image>();
            backgroundImage.color = backgroundColour;

            var defaultLine = VoiceSlider.transform.Find("Default");
            defaultLine.gameObject.SetActive(false);

            var text = VoiceSlider.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"Play voice line chance: {Plugin.VoicePlayChance.Value}%";

            var slider = VoiceSlider.transform.Find("Slider").GetComponent<Slider>();
            slider.onValueChanged = null;
            
            var limits = Plugin.VoicePlayChance.Description.AcceptableValues as AcceptableValueRange<int>;
            slider.maxValue = limits!.MaxValue;
            slider.minValue = limits.MinValue;
            
            slider.gameObject.SetActive(Plugin.ControlVoicePlayChance.Value);

            var sliderEvent = new Slider.SliderEvent();
            sliderEvent.AddListener(new Action<float>(value =>
            {
                Plugin.VoicePlayChance.Value = (int) value;
                text.text = $"Play voice line chance: {Plugin.VoicePlayChance.Value}%";
            }));
            slider.onValueChanged = sliderEvent;
            
            slider.value = Plugin.VoicePlayChance.Value;

            var button = VoiceSlider.GetComponentInChildren<Button>();

            button.onClick = null;

            var toggle = button.transform.GetChild(0);
            toggle.gameObject.SetActive(Plugin.ControlVoicePlayChance.Value);

            var buttonEvent = new Button.ButtonClickedEvent();
            buttonEvent.AddListener(new Action(() =>
            {
                toggle.gameObject.SetActive(!toggle.gameObject.activeSelf);
                Plugin.ControlVoicePlayChance.Value = toggle.gameObject.activeSelf;
                slider.gameObject.SetActive(toggle.gameObject.activeSelf);
            }));

            button.onClick = buttonEvent;
        }

        //corrects UI scaling button labels
        var largeScale = GameObject.Find("MainUI/Options/Screen/Content/UIScale/Btn_Scale 1.1/Text")
            .GetComponent<TextMeshProUGUI>();
        largeScale.text = "L";

        var medScale = GameObject.Find("MainUI/Options/Screen/Content/UIScale/Btn_Scale 1.0/Text")
            .GetComponent<TextMeshProUGUI>();
        medScale.text = "M";
    }

    private static void ApplyMutingLogic(AudioSource source)
    {
        if (source?.clip is null) return;

        var clips = voiceClipNames.ToList();
        clips.RemoveAll(a => a.Contains("respect", StringComparison.OrdinalIgnoreCase));

        var validClips = clips.Any(keyword =>
            source.clip.name.Equals(keyword, StringComparison.OrdinalIgnoreCase));


        if (validClips)
        {
            var isDuplicate = previousClipName.Equals(source.clip.name, StringComparison.OrdinalIgnoreCase);

            // Mute duplicate clips every time
            if (isDuplicate && Plugin.DontPlayIfJustPlayed.Value)
            {
                source.mute = true;
                Plugin.Logger.LogInfo($"Muting duplicate clip! {source.clip.name}");
            }
            else
            {
                if (!Plugin.ControlVoicePlayChance.Value) return;

                // For non-duplicate clips, mute based on play chance
                var chance = UnityEngine.Random.Range(0, 100);
                Plugin.Logger.LogInfo($"Rolled: {chance}, Needs to be less than or equal to: {Plugin.VoicePlayChance.Value}. Audio will play? {chance <= Plugin.VoicePlayChance.Value}");
                if (chance <= Plugin.VoicePlayChance.Value)
                {
                    source.mute = false;
                }
                else
                {
                    source.mute = true;
                    Plugin.Logger.LogInfo($"Muting clip by chance! {source.clip.name}");
                }

                // Update the previous clip name if it's not a duplicate
                previousClipName = source.clip.name;
            }
        }
        else
        {
            // Ensure the source is not muted if it does not contain the specified keywords
            source.mute = false;
        }
    }
}