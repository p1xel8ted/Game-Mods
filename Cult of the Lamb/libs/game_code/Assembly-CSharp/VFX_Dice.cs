// Decompiled with JetBrains decompiler
// Type: VFX_Dice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class VFX_Dice : VFXObject
{
  [SerializeField]
  public MeshRenderer dice;
  [SerializeField]
  public ParticleSystem diceRolledParticles;
  public Action<bool> OnDiceRolled;

  public override void Init()
  {
    if (!this.Initialized && (UnityEngine.Object) this.diceRolledParticles != (UnityEngine.Object) null)
    {
      ParticleSystem.MainModule main = this.diceRolledParticles.main with
      {
        playOnAwake = false,
        stopAction = ParticleSystemStopAction.Callback
      };
    }
    base.Init();
  }

  public override void PlayVFX(float addEmissionDelay = 0.0f, PlayerFarming playerFarming = null, bool playSFX = true)
  {
    base.PlayVFX(addEmissionDelay, playSFX: playSFX);
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    BiomeConstants.Instance.EmitDustCloudParticles(this.dice.gameObject.transform.position);
    this.dice.gameObject.transform.position = new Vector3(this.dice.gameObject.transform.position.x, this.dice.gameObject.transform.position.y, -3f);
    if ((UnityEngine.Object) this.diceRolledParticles != (UnityEngine.Object) null)
      this.diceRolledParticles.gameObject.transform.position = new Vector3(this.diceRolledParticles.gameObject.transform.position.x, this.diceRolledParticles.gameObject.transform.position.y, -2f);
    this.dice.gameObject.transform.localScale = Vector3.zero;
    this.dice.gameObject.transform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.dice.gameObject.transform.position = new Vector3(playerFarming.transform.position.x, playerFarming.transform.position.y, this.dice.gameObject.transform.position.z);
      if (!((UnityEngine.Object) this.diceRolledParticles != (UnityEngine.Object) null))
        return;
      this.diceRolledParticles.gameObject.transform.position = new Vector3(playerFarming.transform.position.x, playerFarming.transform.position.y, this.diceRolledParticles.gameObject.transform.position.z);
    }));
    BiomeConstants.Instance.DepthOfFieldTween(1f, 4.5f, 10f, 1f, 145f);
    Material m = new Material(this.dice.material);
    this.dice.material = m;
    int num = UnityEngine.Random.Range(0, 10);
    if (TrinketManager.HasTrinket(TarotCards.Card.RabbitFoot, playerFarming))
      num += 2;
    bool win = num >= 5;
    this.dice.transform.localRotation = Quaternion.identity;
    float z = !win ? (float) (90 + 360 * UnityEngine.Random.Range(3, 6)) : (float) (360 * UnityEngine.Random.Range(3, 6));
    if (playSFX)
      AudioManager.Instance.PlayOneShot("event:/knuckle_bones/die_roll", this.transform.gameObject);
    UnityEngine.Debug.Log((object) ("SPIN AMOUNT: " + z.ToString()));
    this.dice.gameObject.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, z), 1f, RotateMode.FastBeyond360).SetRelative<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutCirc).SetDelay<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(1f).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() =>
    {
      if ((UnityEngine.Object) this.diceRolledParticles != (UnityEngine.Object) null & win)
        this.diceRolledParticles.Play();
      if (playSFX)
        AudioManager.Instance.PlayOneShot("event:/knuckle_bones/die_place", this.transform.gameObject);
      this.dice.gameObject.transform.DOShakePosition(0.5f, Vector3.right * 0.2f, 16 /*0x10*/).SetEase<Tweener>(Ease.OutCirc).SetUpdate<Tweener>(true);
      if (playSFX)
        AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", this.gameObject);
      m.DOFloat(1f, "_FillAmount", 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      if (win)
      {
        m.DOColor(StaticColors.GreenColor, "_FillColor", 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        m.DOColor(StaticColors.GreenColor * 2f, "_EmissionColor", 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
      else
      {
        m.DOColor(StaticColors.OffWhiteColor, "_FillColor", 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        m.DOColor(StaticColors.OffWhiteColor * 2f, "_EmissionColor", 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
      m.DOFloat(0.0f, "_FillAmount", 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.75f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      m.DOFloat(1f, "_EmissionAmount", 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        Action<bool> onDiceRolled = this.OnDiceRolled;
        if (onDiceRolled != null)
          onDiceRolled(win);
        Time.timeScale = 1f;
        this.dice.gameObject.transform.DOScale(Vector3.zero, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
        this.dice.gameObject.transform.DOMove(playerFarming.transform.position, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.\u003C\u003En__0()));
      }));
      m.DOFloat(0.0f, "_EmissionAmount", 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(1.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }));
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0() => this.CancelVFX();
}
