// Decompiled with JetBrains decompiler
// Type: WeaponPickUpUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class WeaponPickUpUI : BaseMonoBehaviour
{
  [SerializeField]
  public TMP_Text title;
  [SerializeField]
  public TMP_Text damageText;
  [SerializeField]
  public TMP_Text speedText;
  [SerializeField]
  public RectTransform container;
  [SerializeField]
  public TMP_Text descriptionText;
  [SerializeField]
  public TMP_Text loreText;
  [SerializeField]
  public InfoCardOutlineRenderer _outlineRenderer;
  public CanvasGroup canvasGroup;
  [Space]
  [SerializeField]
  public Vector3 offset;
  public RectTransform rectTransform;
  public GameObject lockPosition;
  [SerializeField]
  public GameObject SpeedAndDamageContainer;
  public Camera camera;
  public Canvas canvas;
  public int _weaponLevel;
  public float _damage;
  public float _speed;
  public EquipmentType _weaponType;
  public List<WeaponAttachmentData> _attachments;

  public void Play(
    EquipmentType weaponType,
    int weaponLevel,
    Sprite weaponImage,
    float damage,
    float speed,
    List<WeaponAttachmentData> attachments,
    GameObject lockPos,
    bool HideDamageAndSpeed,
    Interaction_WeaponSelectionPodium.Types type)
  {
    this._weaponType = weaponType;
    this._weaponLevel = weaponLevel;
    this._damage = damage;
    this._speed = speed;
    if (attachments != null)
      this._attachments = new List<WeaponAttachmentData>((IEnumerable<WeaponAttachmentData>) attachments);
    this.LocalizeText();
    this.camera = CameraManager.instance.CameraRef;
    this.lockPosition = lockPos;
    this.rectTransform = this.transform as RectTransform;
    this.canvas = GlobalCanvasReference.CanvasInstance;
    this._outlineRenderer.BadgeVariant = type != Interaction_WeaponSelectionPodium.Types.Curse ? 6 : 7;
    Vector3 TargetPosition = new Vector3(0.0f, 130f);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.01f);
    sequence.AppendCallback((TweenCallback) (() => this.rectTransform.localPosition = TargetPosition + Vector3.up * 50f));
    sequence.Append((Tween) this.rectTransform.DOLocalMove(TargetPosition, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    sequence.Play<DG.Tweening.Sequence>();
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    if (!((Object) this.gameObject != (Object) null) || !((Object) this.canvasGroup != (Object) null))
      return;
    this.canvasGroup.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.canvasGroup.alpha), (DOSetter<float>) (x => this.canvasGroup.alpha = x), 1f, 0.5f);
    this.SpeedAndDamageContainer.SetActive(!HideDamageAndSpeed);
  }

  public void SetWeaponDataText(float damage, float speed, int weaponLevel)
  {
    damage = Mathf.Round(damage * 100f) / 100f;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(this.gameObject);
    double averageWeaponDamage = (double) farmingComponent.playerWeapon.GetAverageWeaponDamage(farmingComponent.currentWeapon, farmingComponent.currentWeaponLevel);
    float weaponSpeed = farmingComponent.playerWeapon.GetWeaponSpeed(farmingComponent.currentWeapon);
    string damage1 = ScriptLocalization.UI_WeaponSelect.Damage;
    string str1 = "";
    string str2 = "<color=#F5EDD5>";
    if (averageWeaponDamage > (double) damage)
    {
      str1 = "<sprite name=\"icon_FaithDown\">";
      str2 = "<color=#FF1C1C>";
    }
    if (averageWeaponDamage < (double) damage)
    {
      str1 = "<sprite name=\"icon_FaithUp\">";
      str2 = "<color=#2DFF1C>";
    }
    string speed1 = ScriptLocalization.UI_WeaponSelect.Speed;
    string str3 = "";
    string str4 = "<color=#F5EDD5>";
    if ((double) weaponSpeed > (double) speed)
    {
      str3 = "<sprite name=\"icon_FaithDown\">";
      str4 = "<color=#FF1C1C>";
    }
    if ((double) weaponSpeed < (double) speed)
    {
      str3 = "<sprite name=\"icon_FaithUp\">";
      str4 = "<color=#2DFF1C>";
    }
    this.damageText.text = string.Format($"{damage1}: {str1}{{0}}{{1}}</color>", (object) str2, (object) damage);
    this.speedText.text = string.Format($"{speed1}: {str3}{{0}}{{1}}</color>", (object) str4, (object) speed);
  }

  public void Shake(float progress, float normAmount)
  {
    this.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, normAmount);
    this.container.localPosition = (Vector3) (Random.insideUnitCircle * progress * 2f);
  }

  public void LateUpdate()
  {
    int num = (Object) this.lockPosition == (Object) null ? 1 : 0;
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void LocalizeText()
  {
    this.title.text = $"{EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedTitle()} {this._weaponLevel.ToNumeral()}";
    this.descriptionText.text = EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedDescription();
    this.loreText.text = EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedLore();
    if (!((Object) EquipmentManager.GetWeaponData(this._weaponType) != (Object) null))
      return;
    this.SetWeaponDataText(this._damage, this._speed, this._weaponLevel);
  }
}
