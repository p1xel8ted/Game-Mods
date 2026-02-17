// Decompiled with JetBrains decompiler
// Type: Interaction_CrownShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CrownShrine : Interaction
{
  [SerializeField]
  public int cultLeaderStatue = -1;
  [SerializeField]
  public Material crownMaterial;
  [SerializeField]
  public Material defaultMaterial;
  public SpriteRenderer _spriteRenderer;
  public bool active;

  public void Start()
  {
    this.Interactable = false;
    this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    if (this.active)
      this._spriteRenderer.material = this.crownMaterial;
    else
      this._spriteRenderer.material = this.defaultMaterial;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.PrayRoutine());
  }

  public IEnumerator PrayRoutine()
  {
    Interaction_CrownShrine interactionCrownShrine = this;
    interactionCrownShrine.active = !interactionCrownShrine.active;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionCrownShrine.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionCrownShrine.playerFarming.gameObject, 12f);
    interactionCrownShrine.playerFarming.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionCrownShrine.gameObject, 10f);
    string TermToSpeak = string.Format(interactionCrownShrine.active ? LocalizationManager.GetTranslation("Conversation_NPC/CrownShrine_Convo_0") : LocalizationManager.GetTranslation("Conversation_NPC/CrownShrine_Convo_1"), (object) interactionCrownShrine.GetCultLeaderName());
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(interactionCrownShrine.gameObject, TermToSpeak)
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 400f;
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    interactionCrownShrine._spriteRenderer.material = interactionCrownShrine.active ? interactionCrownShrine.crownMaterial : interactionCrownShrine.defaultMaterial;
    interactionCrownShrine._spriteRenderer.gameObject.transform.DOShakePosition(1f, new Vector3(0.1f, 0.0f, 0.0f));
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success, interactionCrownShrine.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", interactionCrownShrine.transform.position);
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void GetLabel()
  {
    if (!this.Interactable)
      return;
    this.Label = ScriptLocalization.Interactions.Pray;
  }

  public string GetCultLeaderName()
  {
    string str = "";
    switch (this.cultLeaderStatue)
    {
      case 1:
        str = ScriptLocalization.NAMES_CultLeaders.Dungeon1;
        break;
      case 2:
        str = ScriptLocalization.NAMES_CultLeaders.Dungeon2;
        break;
      case 3:
        str = ScriptLocalization.NAMES_CultLeaders.Dungeon3;
        break;
      case 4:
        str = ScriptLocalization.NAMES_CultLeaders.Dungeon4;
        break;
    }
    return $"<color=#FFD201>{str}</color>";
  }
}
