using System.Collections.Generic;
using System.Linq;
using BeamMeUpGerry.lang;
using FlowCanvas.Nodes;
using HarmonyLib;
using Helper;
using Rewired;
using UnityEngine;

namespace BeamMeUpGerry;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.OnChosen))]
    public static void MultiAnswerGUI_OnChosen_Prefix(ref string answer)
    {
        if (Tools.PlayerDisabled())
        {
            return;
        }

        if (InTutorial()) return;
        if (!_cfg.enableListExpansion) return;
        var canUseStone = CanUseStone();
        if (!canUseStone) return;
        // if (_isNpc) return;
        List<AnswerVisualData> answers;

        _dotSelection = false;

        if (answer == "...")
        {
            // answers = LocationByVectorPartOne.Select(location => new AnswerVisualData() { id = location.Key }).ToList();
            answers = LocationsPartOne.Select(location => new AnswerVisualData() {id = location.Zone}).ToList();
            Show(out answer);
            return;
        }

        if (answer == "....")
        {
            //answers = LocationByVectorPartTwo.Select(location => new AnswerVisualData() { id = location.Key }).ToList();
            answers = LocationsPartTwo.Select(location => new AnswerVisualData() {id = location.Zone}).ToList();
            Show(out answer);
            return;
        }

        void Show(out string answer)
        {
            CrossModFields.TalkingToNpc("BeamMeUpGerry: MultiAnswerGuiOnChosenPatch Prefix: void Show", false);
            var cleanedAnswers = ValidateAnswerList(answers);
            answer = "cancel";
            _dotSelection = true;
            _usingStone = true;
            MainGame.me.player.components.character.control_enabled = false;
            MainGame.me.player.ShowMultianswer(cleanedAnswers, BeamGerryOnChosen);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.OnChosen))]
    public static void MultiAnswerGUI_OnChosen_Postfix(string answer)
    {
        if (Tools.PlayerDisabled())
        {
            return;
        }

        if (InTutorial()) return;
        if (!_cfg.enableListExpansion)
        {
            _usingStone = false;
            _dotSelection = false;
            CrossModFields.TalkingToNpc("BeamMeUpGerry: MultiAnswerGuiOnChosenPatch Postfix: string = cancel, _cfg.enableListExpansion = false", false);
            return;
        }
        if (!CanUseStone()) return;

        Log($"[Answer]: {answer}");

        if (string.Equals("cancel", answer) && !_dotSelection)
        {
            //real cancel
            ShowHud(null, true);
            _usingStone = false;
            _dotSelection = false;
            CrossModFields.TalkingToNpc("BeamMeUpGerry: MultiAnswerGuiOnChosenPatch Postfix: string = cancel, !_dotSelection", false);

            MainGame.me.player.components.character.control_enabled = true;
            return;
        }

        if (string.Equals("cancel", answer) && _dotSelection)
        {
            //fake cancel to close the old menu and open a new one
            _usingStone = true;
            _dotSelection = true;
            MainGame.me.player.components.character.control_enabled = false;
            return;
        }

        if (string.Equals("leave", answer.ToLowerInvariant()))
        {
            //leave option for npcs
            _usingStone = false;
            _dotSelection = false;
            CrossModFields.TalkingToNpc("BeamMeUpGerry: MultiAnswerGuiOnChosenPatch Postfix: string = leave, answer.ToLowerInvariant", false);
            MainGame.me.player.components.character.control_enabled = true;

            return;
        }

        //if (CrossModFields.TalkingToNpc) return;

        _usingStone = false;
        _dotSelection = false;

        MainGame.me.player.components.character.control_enabled = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }

        if (!MainGame.game_started || MainGame.me.player.is_dead || MainGame.me.player.IsDisabled() ||
            MainGame.paused) return;

        if (BaseGUI.all_guis_closed && MainGame.me.player.components.character.control_enabled)
        {
            if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(_cfg.teleportMenuControllerButton))
            {
                DoLoggingAndBeam();
            }

            if (Input.GetKeyUp(_cfg.teleportMenuKeyBind))
            {
                DoLoggingAndBeam();
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape) ||
            (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(3)))
        {
            if (_maGui != null)
            {
                ShowHud(null, true);
                Sounds.OnClosePressed();
                //_maGui.OnChosen("cancel");
                //_maGui.OnChosen("leave");
                _maGui.DestroyBubble();
                _usingStone = false;
                _dotSelection = false;
                CrossModFields.TalkingToNpc("BeamMeUpGerry: TimeOfDayUpdate Prefix: _maGui != null", false);
                MainGame.me.player.components.character.control_enabled = true;
                _maGui = null;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiAnswerGUI), nameof(MultiAnswerGUI.ShowAnswers), typeof(List<AnswerVisualData>), typeof(bool))]
    public static void MultiAnswerGUI_ShowAnswers(ref MultiAnswerGUI __instance)
    {
        if (Tools.PlayerDisabled())
        {
            return;
        }

        if (InTutorial()) return;
        if (__instance == null) return;

        _maGui = __instance;

        if (!_usingStone) return;
        if (_cfg.increaseMenuAnimationSpeed)
        {
            __instance.anim_delay /= 3f;
            __instance.anim_time /= 3f;
        }
    }


    [HarmonyPatch(typeof(Item))]
    [HarmonyPatch(nameof(Item.GetGrayedCooldownPercent))]
    public static class ItemGetGrayedCooldownPercentPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Item __instance, ref int __result)
        {
            if (__instance is not {id: "hearthstone"}) return;

            __result = 0;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(ref WorldGameObject __instance)
    {
        if (Tools.PlayerDisabled())
        {
            return;
        }

        if (InTutorial()) return;
        if (__instance == null) return;

        if (__instance.custom_tag != CrossModFields.ModGerryTag) return;

        var multiAnswer = __instance.gameObject.AddComponent<MultiAnswerGUI>();
        var gui = multiAnswer.gameObject.AddComponent<MultiAnswerOptionGUI>();
        multiAnswer.Init();
        gui.Init();
        var answers = new List<AnswerVisualData> {new() {id = "Please leave..."}};
        multiAnswer.ShowAnswers(answers,false);
       // AccessTools.Method(typeof(MultiAnswerGUI), "ShowAnswers").Invoke(multiAnswer, new object[] {answers, false});
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Flow_MultiAnswer), nameof(Flow_MultiAnswer.RegisterPorts))]
    public static void Flow_MultiAnswer_RegisterPorts(ref Flow_MultiAnswer __instance)
    {
        if (Tools.PlayerDisabled())
        {
            return;
        }

        if (InTutorial()) return;
        if (__instance == null) return;
        if (!_usingStone) return;

        if (!_cfg.enableListExpansion) return;
        if (_dotSelection) return;

        __instance.answers.Insert(__instance.answers.Count - 1, @"...");
    }
}