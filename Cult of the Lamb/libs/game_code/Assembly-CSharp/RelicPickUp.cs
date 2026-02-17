// Decompiled with JetBrains decompiler
// Type: RelicPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.Prompts;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RelicPickUp : Interaction
{
  public static List<RelicPickUp> RelicPickUps = new List<RelicPickUp>();
  [SerializeField]
  public SpriteRenderer icon;
  public RelicData relicData;
  public UIRelicPickupPromptController _relicPickupUI;

  public RelicData RelicData => this.relicData;

  public void Start()
  {
    this.ConfigureRandomise();
    RelicPickUp.RelicPickUps.Add(this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    RelicPickUp.RelicPickUps.Remove(this);
  }

  public void Configure(RelicData relicData)
  {
    this.relicData = relicData;
    this.icon.sprite = EquipmentManager.GetRelicIcon(relicData.RelicType);
    this.icon.transform.localScale /= 2f;
    this.transform.position += Vector3.forward * -1f;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(1f, 0.25f);
    if (relicData.RelicType.ToString().Contains("Blessed") && DataManager.Instance.ForceBlessedRelic)
      DataManager.Instance.ForceBlessedRelic = false;
    else if (relicData.RelicType.ToString().Contains("Dammed") && DataManager.Instance.ForceDammedRelic)
      DataManager.Instance.ForceDammedRelic = false;
    if (DataManager.Instance.SpawnedRelicsThisRun.Contains(relicData.RelicType))
      return;
    DataManager.Instance.SpawnedRelicsThisRun.Add(relicData.RelicType);
  }

  public void ConfigureRandomise()
  {
    if ((UnityEngine.Object) this.relicData != (UnityEngine.Object) null)
      return;
    this.Configure(EquipmentManager.GetRandomRelicData(false, this.playerFarming));
  }

  public override void GetLabel()
  {
    this.Label = $"{ScriptLocalization.Interactions.PickUp} <color=#FFD201>{RelicData.GetTitleLocalisation(this.relicData.RelicType)}";
  }

  public override void OnInteract(StateMachine state)
  {
    PlayerFarming component = state.GetComponent<PlayerFarming>();
    base.OnInteract(state);
    component.playerRelic.EquipRelic(this.relicData, initialEquip: true);
    component.indicator.HideTopInfo();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    if ((UnityEngine.Object) this._relicPickupUI == (UnityEngine.Object) null)
    {
      this._relicPickupUI = MonoSingleton<UIManager>.Instance.RelicPickupPromptControllerTemplate.Instantiate<UIRelicPickupPromptController>();
      UIRelicPickupPromptController relicPickupUi = this._relicPickupUI;
      relicPickupUi.OnHidden = relicPickupUi.OnHidden + (System.Action) (() => this._relicPickupUI = (UIRelicPickupPromptController) null);
    }
    this._relicPickupUI.Show(this.relicData, playerFarming.playerRelic.CurrentRelic, playerFarming);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    if (!((UnityEngine.Object) this._relicPickupUI != (UnityEngine.Object) null))
      return;
    this._relicPickupUI.Hide();
  }

  [CompilerGenerated]
  public void \u003CIndicateHighlighted\u003Eb__12_0()
  {
    this._relicPickupUI = (UIRelicPickupPromptController) null;
  }
}
