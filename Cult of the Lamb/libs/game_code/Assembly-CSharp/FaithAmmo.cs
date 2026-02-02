// Decompiled with JetBrains decompiler
// Type: FaithAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FaithAmmo : BaseMonoBehaviour
{
  public GameObject Container;
  public Image Background;
  public System.Action OnAmmoChanged;
  public System.Action OnAmmoCountChanged;
  public System.Action OnCantAfford;
  public TextMeshProUGUI AmmoText;
  public Image AmmoGlow;
  [SerializeField]
  public Material AmmoGlowLamb;
  [SerializeField]
  public Material AmmoGlowGoat;
  [SerializeField]
  public Image containerSpriteRenderer;
  [SerializeField]
  public Sprite containerGoat;
  [SerializeField]
  public Sprite containerLamb;
  [SerializeField]
  public Image outOfAmmoBG;
  [SerializeField]
  public Image redGlowImage;
  public UI_Transitions transition;
  public float _Ammo = 54f;
  public Transform ProgressBar;
  public Image WhiteFlash;
  public GameObject[] RedGlow;
  public List<FaithAmmo.BiomeColour> Colours;
  [CompilerGenerated]
  public bool \u003CDoFlash\u003Ek__BackingField = true;
  public bool IsOnTotalAmmoShakePerformed;
  public PlayerFarming playerFarming;

  public float Ammo
  {
    set
    {
      int num1 = 44;
      int num2 = Mathf.FloorToInt(this.Ammo / (float) num1);
      int num3 = Mathf.FloorToInt(value / (float) num1);
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.playerFarming.playerSpells != (UnityEngine.Object) null)
          num1 = this.playerFarming.playerSpells.AmmoCost;
        num2 = Mathf.FloorToInt(this.Ammo / (float) num1);
        num3 = Mathf.FloorToInt(value / (float) num1);
        if ((double) value < (double) num1 && (double) this._Ammo >= (double) num1)
        {
          this.playerFarming.playerSpells.playerFarming.SetSkin(true);
          foreach (GameObject gameObject in this.RedGlow)
            gameObject.SetActive(false);
        }
        if ((double) value >= (double) num1 && (double) this._Ammo < (double) num1 && (bool) (UnityEngine.Object) this.playerFarming.playerSpells.playerFarming)
        {
          this.playerFarming.playerSpells.playerFarming.SetSkin(false);
          this.playerFarming.playerSpells.playerFarming.growAndFade.Play();
          foreach (GameObject gameObject in this.RedGlow)
            gameObject.SetActive(true);
        }
      }
      this._Ammo = Mathf.Clamp(value, 0.0f, this.Total);
      System.Action onAmmoChanged = this.OnAmmoChanged;
      if (onAmmoChanged != null)
        onAmmoChanged();
      if (num2 == num3)
        return;
      System.Action ammoCountChanged = this.OnAmmoCountChanged;
      if (ammoCountChanged == null)
        return;
      ammoCountChanged();
    }
    get => this._Ammo;
  }

  public float Total
  {
    get
    {
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_3))
        return 264f;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_2))
        return 220f;
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_1) ? 176f : 132f;
    }
  }

  public bool DoFlash
  {
    get => this.\u003CDoFlash\u003Ek__BackingField;
    set => this.\u003CDoFlash\u003Ek__BackingField = value;
  }

  public void Start() => this.Ammo = this.Total;

  public void Awake()
  {
  }

  public void Init(PlayerFarming playerFarmingVar)
  {
    this.playerFarming = playerFarmingVar;
    this.transition = this.GetComponent<UI_Transitions>();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null | !GameManager.IsDungeon(PlayerFarming.Location) || PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.transform.localScale = Vector3.zero;
    if (PlayerFarming.players.Count <= 1 || (UnityEngine.Object) this.containerSpriteRenderer == (UnityEngine.Object) null || (UnityEngine.Object) this.AmmoGlow == (UnityEngine.Object) null || (UnityEngine.Object) this.redGlowImage == (UnityEngine.Object) null || (UnityEngine.Object) this.outOfAmmoBG == (UnityEngine.Object) null)
      return;
    if (!this.playerFarming.isLamb)
    {
      this.containerSpriteRenderer.sprite = this.containerGoat;
      this.AmmoGlow.material = this.AmmoGlowGoat;
      this.outOfAmmoBG.color = StaticColors.GoatPurple;
      this.redGlowImage.color = StaticColors.GoatPurple;
    }
    else
    {
      this.containerSpriteRenderer.sprite = this.containerLamb;
      this.AmmoGlow.material = this.AmmoGlowLamb;
      this.redGlowImage.color = StaticColors.RedColor;
      this.outOfAmmoBG.color = StaticColors.RedColor;
    }
  }

  public void DoShake()
  {
    this.Container.transform.DOKill();
    this.Container.transform.localPosition = this.transition.StartPos;
    this.Container.transform.DOShakePosition(0.5f, new Vector3(15f, 0.0f), randomness: 0.0f);
  }

  public void OnEnable()
  {
    PlayerFarming.OnGetBlackSoul += new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    this.OnAmmoChanged += new System.Action(this.UpdateBar);
    this.UpdateBar();
    if (this.Colours.Count <= 0)
      return;
    this.Background.color = this.Colours[0].Color;
    foreach (FaithAmmo.BiomeColour colour in this.Colours)
    {
      if (colour.Location == PlayerFarming.Location)
      {
        this.Background.color = colour.Color;
        break;
      }
    }
  }

  public void OnDisable()
  {
    this.Container.transform.DOKill();
    this.AmmoGlow.DOKill();
    PlayerFarming.OnGetBlackSoul -= new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    this.OnAmmoChanged -= new System.Action(this.UpdateBar);
  }

  public void OnDestroy() => this.DOKill();

  public bool CanAfford(float Amount)
  {
    if (SettingsManager.Settings.Accessibility == null)
    {
      Debug.Log((object) "settings null");
      return false;
    }
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
    {
      Debug.Log((object) "playerfarming null");
      return false;
    }
    return SettingsManager.Settings.Accessibility.UnlimitedFervour || this.playerFarming.playerRelic.UnlimitedFervour || (double) this.Ammo - (double) Amount >= 0.0;
  }

  public bool UseAmmo(float Amount, bool curseAttack = true)
  {
    if (!this.CanAfford(Amount))
    {
      this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());
      this.DoShake();
      if (curseAttack)
      {
        System.Action onCantAfford = this.OnCantAfford;
        if (onCantAfford != null)
          onCantAfford();
      }
      return false;
    }
    if (!SettingsManager.Settings.Accessibility.UnlimitedFervour && !this.playerFarming.playerRelic.UnlimitedFervour)
      this.Ammo -= Amount;
    return true;
  }

  public void Flash() => this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());

  public void OnGetSoul(int DeltaValue, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) playerFarming)
      return;
    if ((double) this.Ammo < (double) this.Total)
    {
      this.Ammo += (float) (DeltaValue * 2);
      this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());
      this.IsOnTotalAmmoShakePerformed = false;
    }
    if ((double) this.Ammo < (double) this.Total || this.IsOnTotalAmmoShakePerformed)
      return;
    this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());
    this.DoShake();
    this.IsOnTotalAmmoShakePerformed = true;
  }

  public void Reload() => this.Ammo = this.Total;

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || (UnityEngine.Object) this.playerFarming.playerSpells == (UnityEngine.Object) null)
      return;
    this.ProgressBar.transform.localScale = new Vector3(1f, (float) (1.0 - (0.20000000298023224 + (double) this.Ammo / (double) this.Total * 0.800000011920929)));
    this.AmmoText.text = $"{Mathf.FloorToInt(this.Ammo / (float) this.playerFarming.playerSpells.AmmoCost)} {"<sprite name=\"icon_UI_Curse\">"}";
    if ((double) this.Ammo >= (double) this.Total)
    {
      if (this.AmmoGlow.gameObject.activeSelf)
        return;
      this.AmmoGlow.gameObject.SetActive(true);
      this.AmmoGlow.color = new Color(1f, 1f, 1f, 0.0f);
      this.AmmoGlow.DOKill();
      DOTweenModuleUI.DOFade(this.AmmoGlow, 1f, 1f);
    }
    else
    {
      if (!this.AmmoGlow.gameObject.activeSelf)
        return;
      this.AmmoGlow.DOKill();
      DOTweenModuleUI.DOFade(this.AmmoGlow, 0.0f, 1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        if ((double) this.Ammo >= (double) this.Total)
          return;
        this.AmmoGlow.gameObject.SetActive(false);
      }));
    }
  }

  public void SetUnlimited(float duration)
  {
    this.StartCoroutine((IEnumerator) this.SetUnlimitedIE(duration));
  }

  public IEnumerator SetUnlimitedIE(float duration)
  {
    float ammo = this.Ammo;
    this.Ammo = float.MaxValue;
    float fontSize = this.AmmoText.fontSize;
    this.AmmoText.text = "<sprite name=\"icon_Infinity\">";
    yield return (object) new WaitForSeconds(duration);
    this.Ammo = ammo;
    this.AmmoText.fontSize = fontSize;
  }

  public IEnumerator WhiteFlashRoutine()
  {
    if (this.DoFlash)
    {
      this.WhiteFlash.enabled = true;
      float Progress = 0.0f;
      float Duration = 0.3f;
      Color StartColor = new Color(1f, 1f, 1f, 0.4f);
      Color TargetColor = new Color(1f, 1f, 1f, 0.0f);
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        this.WhiteFlash.color = Color.Lerp(StartColor, TargetColor, Progress / Duration);
        yield return (object) null;
      }
      this.WhiteFlash.enabled = false;
    }
  }

  [CompilerGenerated]
  public void \u003CUpdateBar\u003Eb__44_0()
  {
    if ((double) this.Ammo >= (double) this.Total)
      return;
    this.AmmoGlow.gameObject.SetActive(false);
  }

  [Serializable]
  public struct BiomeColour
  {
    public Color Color;
    public FollowerLocation Location;
  }
}
