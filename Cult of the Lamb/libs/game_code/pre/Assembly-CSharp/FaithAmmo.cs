// Decompiled with JetBrains decompiler
// Type: FaithAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FaithAmmo : BaseMonoBehaviour
{
  public GameObject Container;
  public Image Background;
  public static System.Action OnAmmoChanged;
  public static System.Action OnAmmoCountChanged;
  public static System.Action OnCantAfford;
  public TextMeshProUGUI AmmoText;
  public Image AmmoGlow;
  private UI_Transitions transition;
  private static float _Ammo = 54f;
  public Transform ProgressBar;
  public Image WhiteFlash;
  public static FaithAmmo Instance;
  public GameObject[] RedGlow;
  public List<FaithAmmo.BiomeColour> Colours;
  private bool IsOnTotalAmmoShakePerformed;

  public static float Ammo
  {
    set
    {
      int num1 = Mathf.FloorToInt(FaithAmmo.Ammo / (float) PlayerSpells.AmmoCost);
      int num2 = Mathf.FloorToInt(value / (float) PlayerSpells.AmmoCost);
      if ((double) value < (double) PlayerSpells.AmmoCost && (double) FaithAmmo._Ammo >= (double) PlayerSpells.AmmoCost)
      {
        PlayerFarming.Instance.SetSkin(true);
        if ((UnityEngine.Object) FaithAmmo.Instance != (UnityEngine.Object) null)
        {
          foreach (GameObject gameObject in FaithAmmo.Instance.RedGlow)
            gameObject.SetActive(false);
        }
      }
      if ((double) value >= (double) PlayerSpells.AmmoCost && (double) FaithAmmo._Ammo < (double) PlayerSpells.AmmoCost && (bool) (UnityEngine.Object) PlayerFarming.Instance)
      {
        PlayerFarming.Instance.SetSkin(false);
        PlayerFarming.Instance.growAndFade.Play();
        if ((UnityEngine.Object) FaithAmmo.Instance != (UnityEngine.Object) null)
        {
          foreach (GameObject gameObject in FaithAmmo.Instance.RedGlow)
            gameObject.SetActive(true);
        }
      }
      FaithAmmo._Ammo = Mathf.Clamp(value, 0.0f, FaithAmmo.Total);
      System.Action onAmmoChanged = FaithAmmo.OnAmmoChanged;
      if (onAmmoChanged != null)
        onAmmoChanged();
      if (num1 == num2)
        return;
      System.Action ammoCountChanged = FaithAmmo.OnAmmoCountChanged;
      if (ammoCountChanged == null)
        return;
      ammoCountChanged();
    }
    get => FaithAmmo._Ammo;
  }

  public static float Total
  {
    get
    {
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_1))
        return 180f;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_2))
        return 225f;
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_Ammo_3) ? 270f : 135f;
    }
  }

  public bool DoFlash { get; set; } = true;

  private void Start() => FaithAmmo.Ammo = FaithAmmo.Total;

  private void Awake()
  {
    FaithAmmo.Instance = this;
    this.transition = this.GetComponent<UI_Transitions>();
  }

  private void DoShake()
  {
    this.Container.transform.DOKill();
    this.Container.transform.localPosition = this.transition.StartPos;
    this.Container.transform.DOShakePosition(0.5f, new Vector3(15f, 0.0f), randomness: 0.0f);
  }

  private void OnEnable()
  {
    PlayerFarming.OnGetBlackSoul += new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    FaithAmmo.OnAmmoChanged += new System.Action(this.UpdateBar);
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

  private void OnDisable()
  {
    this.Container.transform.DOKill();
    this.AmmoGlow.DOKill();
    PlayerFarming.OnGetBlackSoul -= new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    FaithAmmo.OnAmmoChanged -= new System.Action(this.UpdateBar);
  }

  private void OnDestroy()
  {
    if ((UnityEngine.Object) FaithAmmo.Instance != (UnityEngine.Object) null)
      FaithAmmo.Instance.DOKill();
    if (!((UnityEngine.Object) FaithAmmo.Instance == (UnityEngine.Object) this))
      return;
    FaithAmmo.Instance = (FaithAmmo) null;
  }

  public static bool CanAfford(float Amount) => (double) FaithAmmo.Ammo - (double) Amount >= 0.0;

  public static bool UseAmmo(float Amount)
  {
    if (!FaithAmmo.CanAfford(Amount))
    {
      if ((UnityEngine.Object) FaithAmmo.Instance != (UnityEngine.Object) null)
      {
        FaithAmmo.Instance.StartCoroutine((IEnumerator) FaithAmmo.Instance.WhiteFlashRoutine());
        FaithAmmo.Instance.DoShake();
      }
      System.Action onCantAfford = FaithAmmo.OnCantAfford;
      if (onCantAfford != null)
        onCantAfford();
      return false;
    }
    FaithAmmo.Ammo -= Amount;
    return true;
  }

  public static void Flash()
  {
    if (!((UnityEngine.Object) FaithAmmo.Instance != (UnityEngine.Object) null))
      return;
    FaithAmmo.Instance.StartCoroutine((IEnumerator) FaithAmmo.Instance.WhiteFlashRoutine());
  }

  private void OnGetSoul(int DeltaValue)
  {
    if ((double) FaithAmmo.Ammo < (double) FaithAmmo.Total)
    {
      FaithAmmo.Ammo += (float) (DeltaValue * 2);
      this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());
      this.IsOnTotalAmmoShakePerformed = false;
    }
    if ((double) FaithAmmo.Ammo < (double) FaithAmmo.Total || this.IsOnTotalAmmoShakePerformed)
      return;
    this.StartCoroutine((IEnumerator) this.WhiteFlashRoutine());
    this.DoShake();
    this.IsOnTotalAmmoShakePerformed = true;
  }

  public static void Reload() => FaithAmmo.Ammo = FaithAmmo.Total;

  private void UpdateBar()
  {
    this.ProgressBar.transform.localScale = new Vector3(1f, (float) (1.0 - (0.20000000298023224 + (double) FaithAmmo.Ammo / (double) FaithAmmo.Total * 0.800000011920929)));
    this.AmmoText.text = $"{Mathf.FloorToInt(FaithAmmo.Ammo / (float) PlayerSpells.AmmoCost)} {"<sprite name=\"icon_UI_Curse\">"}";
    if ((double) FaithAmmo.Ammo >= (double) FaithAmmo.Total)
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
        if ((double) FaithAmmo.Ammo >= (double) FaithAmmo.Total)
          return;
        this.AmmoGlow.gameObject.SetActive(false);
      }));
    }
  }

  private IEnumerator WhiteFlashRoutine()
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

  [Serializable]
  public struct BiomeColour
  {
    public Color Color;
    public FollowerLocation Location;
  }
}
