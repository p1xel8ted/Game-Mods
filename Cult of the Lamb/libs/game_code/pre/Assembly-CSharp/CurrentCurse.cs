// Decompiled with JetBrains decompiler
// Type: CurrentCurse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CurrentCurse : MonoBehaviour
{
  private EquipmentType Curse = EquipmentType.None;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private Image Background;
  [SerializeField]
  private TextMeshProUGUI LevelText;

  private void Start()
  {
    if (!DataManager.Instance.EnabledSpells)
      this.gameObject.SetActive(false);
    else if (DataManager.Instance.CurrentCurse == EquipmentType.None || !GameManager.IsDungeon(PlayerFarming.Location) || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.transform.localScale = Vector3.zero;
    PlayerSpells.OnCurseChanged += new PlayerSpells.CurseEvent(this.SetCurse);
    Interaction_WeaponSelectionPodium.OnHighlightCurse += new Action<bool>(this.HighlightCurse);
  }

  private void OnDestroy()
  {
    PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.SetCurse);
    Interaction_WeaponSelectionPodium.OnHighlightCurse -= new Action<bool>(this.HighlightCurse);
  }

  private void HighlightCurse(bool Toggle)
  {
    if (this.Curse == EquipmentType.None)
      return;
    this.Background.transform.DOKill();
    this.transform.DOKill();
    this.Background.enabled = Toggle;
    if (this.Background.enabled)
    {
      this.Background.transform.localScale = Vector3.one * 1.1f;
      this.Background.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    else
      this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  private void SetCurse(EquipmentType curse, int Level) => this.SetCurse(curse, Level, true);

  private void SetCurse(EquipmentType curse, int Level, bool punch)
  {
    if (DataManager.Instance.EnabledSpells)
      this.gameObject.SetActive(true);
    this.Curse = curse;
    this.icon.enabled = curse != EquipmentType.None;
    this.Background.enabled = curse != EquipmentType.None;
    this.LevelText.text = Level.ToNumeral();
    if (curse == EquipmentType.None || (UnityEngine.Object) EquipmentManager.GetCurseData(curse) == (UnityEngine.Object) null)
      return;
    this.icon.sprite = EquipmentManager.GetCurseData(curse).UISprite;
    this.transform.localScale = Vector3.one;
    if (!punch)
      return;
    this.icon.transform.localScale = Vector3.one * 2f;
    this.icon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.Background.transform.DOKill();
    this.Background.enabled = true;
    this.Background.transform.localScale = Vector3.one * 0.9f;
    this.Background.transform.DOScale(Vector3.one * 3f, 0.5f);
    Color c = this.Background.color;
    DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
      this.Background.color = c;
      this.Background.enabled = false;
    }));
  }
}
