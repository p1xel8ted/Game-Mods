// Decompiled with JetBrains decompiler
// Type: HUD_Hearts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_Hearts : BaseMonoBehaviour
{
  public static HUD_Hearts Instance;
  public List<HUD_Heart> HeartIcons = new List<HUD_Heart>();
  public int _health;
  private EventInstance loopedSound;
  private bool createdloop;
  private int count;

  private void Start()
  {
    HUD_Hearts.Instance = this;
    HealthPlayer.OnTotalHPUpdated += new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
    HealthPlayer.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHPUpdated);
    if (!(bool) (Object) PlayerFarming.Instance)
      return;
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    if (!(bool) (Object) component)
      return;
    this.OnHPUpdated(component);
  }

  private void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    HUD_Hearts.Instance = (HUD_Hearts) null;
    HealthPlayer.OnHPUpdated -= new HealthPlayer.HPUpdated(this.OnHPUpdated);
    HealthPlayer.OnTotalHPUpdated -= new HealthPlayer.HPUpdated(this.OnTotalHPUpdated);
  }

  private void OnDisable() => AudioManager.Instance.StopLoop(this.loopedSound);

  private void OnHPUpdated(HealthPlayer Target)
  {
    this.UpdateHearts(Target, true);
    this.lowHealthCheck();
  }

  private void lowHealthCheck()
  {
    int num1 = (int) DataManager.Instance.PLAYER_HEALTH + (int) DataManager.Instance.PLAYER_SPIRIT_HEARTS + (int) DataManager.Instance.PLAYER_BLUE_HEARTS + (int) DataManager.Instance.PLAYER_BLACK_HEARTS;
    this._health = num1;
    if (num1 == 2 && (double) DataManager.Instance.PLAYER_TOTAL_HEALTH != 2.0)
    {
      this.StopAllCoroutines();
      if (!this.createdloop)
      {
        this.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/misc/whispers", PlayerFarming.Instance.gameObject, true);
        this.createdloop = true;
      }
      int num2 = (int) this.loopedSound.setVolume(0.5f);
      this.StartCoroutine((IEnumerator) this.lowHealthEffect(2f, this.HeartIcons[0]));
    }
    else if (num1 == 1)
    {
      this.StopAllCoroutines();
      if (!this.createdloop)
      {
        if ((double) Time.timeScale != 0.0)
          AudioManager.Instance.SetMusicFilter("filter", 0.666f);
        this.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/misc/whispers", PlayerFarming.Instance.gameObject, true);
        this.createdloop = true;
      }
      int num3 = (int) this.loopedSound.setVolume(1f);
      this.StartCoroutine((IEnumerator) this.lowHealthEffect(1f, this.HeartIcons[0]));
    }
    else
    {
      if (this.createdloop && (double) Time.timeScale != 0.0)
        AudioManager.Instance.SetMusicFilter("filter", 0.0f);
      AudioManager.Instance.StopLoop(this.loopedSound);
      this.createdloop = false;
      this.StopAllCoroutines();
    }
  }

  private IEnumerator lowHealthEffect(float waitTime, HUD_Heart Heart)
  {
    this.count = 4;
    while ((int) DataManager.Instance.PLAYER_HEALTH + (int) DataManager.Instance.PLAYER_SPIRIT_HEARTS + (int) DataManager.Instance.PLAYER_BLUE_HEARTS + (int) DataManager.Instance.PLAYER_BLACK_HEARTS <= 2)
    {
      yield return (object) new WaitForSeconds(waitTime);
      --this.count;
      if (this.count >= 0)
        UIManager.PlayAudio("event:/ui/heartbeat");
      Heart.transform.DOKill();
      Heart.transform.localScale = Vector3.one;
      Heart.transform.DOPunchScale(new Vector3(0.2f, -0.5f), 0.5f);
    }
  }

  private void UpdateHearts(HealthPlayer health, bool DoEffects)
  {
    int index = -1;
    int hp = (int) health.HP;
    int playerTotalHealth = (int) DataManager.Instance.PLAYER_TOTAL_HEALTH;
    int spiritHearts = (int) health.SpiritHearts;
    int totalSpiritHearts = (int) health.TotalSpiritHearts;
    int blueHearts = (int) health.BlueHearts;
    int blackHearts = (int) health.BlackHearts;
    while (++index < this.HeartIcons.Count)
    {
      HUD_Heart heartIcon = this.HeartIcons[index];
      if ((double) Mathf.Ceil(DataManager.Instance.PLAYER_TOTAL_HEALTH / 2f) + (double) Mathf.Ceil(health.TotalSpiritHearts / 2f) + (double) Mathf.Ceil(DataManager.Instance.PLAYER_BLUE_HEARTS / 2f) + (double) Mathf.Ceil(DataManager.Instance.PLAYER_BLACK_HEARTS / 2f) <= (double) index)
      {
        if (heartIcon.MyState == HUD_Heart.HeartState.HeartHalf && heartIcon.MyHeartType == HUD_Heart.HeartType.Blue)
          heartIcon.Activate(false, true);
        else
          heartIcon.Activate(false, false);
      }
      else
      {
        heartIcon.Activate(true, false);
        if (playerTotalHealth >= 1)
        {
          if (hp >= 2)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects);
            hp -= 2;
          }
          else if (hp == 1)
          {
            if (playerTotalHealth >= 2)
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalfFull, DoEffects);
            else
              heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects);
            --hp;
          }
          else if (playerTotalHealth == 1)
            heartIcon.SetSprite(HUD_Heart.HeartState.HalfHeartContainer, DoEffects);
          else
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer, DoEffects);
          playerTotalHealth -= 2;
        }
        else if (totalSpiritHearts >= 1)
        {
          if (spiritHearts >= 2)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Spirit);
            spiritHearts -= 2;
          }
          else if (spiritHearts == 1)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Spirit);
            --spiritHearts;
          }
          else if (totalSpiritHearts == 1)
            heartIcon.SetSprite(HUD_Heart.HeartState.HalfHeartContainer, DoEffects, HUD_Heart.HeartType.Spirit);
          else
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer, DoEffects, HUD_Heart.HeartType.Spirit);
          totalSpiritHearts -= 2;
        }
        else if ((double) index >= (double) Mathf.Ceil(DataManager.Instance.PLAYER_TOTAL_HEALTH / 2f) + (double) Mathf.Ceil(health.TotalSpiritHearts / 2f))
        {
          if (blueHearts >= 2)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Blue);
            blueHearts -= 2;
          }
          else if (blueHearts == 1)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Blue);
            --blueHearts;
          }
          else if (blackHearts >= 2)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartFull, DoEffects, HUD_Heart.HeartType.Black);
            blackHearts -= 2;
          }
          else if (blackHearts == 1)
          {
            heartIcon.SetSprite(HUD_Heart.HeartState.HeartHalf, DoEffects, HUD_Heart.HeartType.Black);
            --blackHearts;
          }
        }
      }
    }
  }

  private void OnTotalHPUpdated(HealthPlayer Target)
  {
    int index = -1;
    double hp = (double) Target.HP;
    double blueHearts = (double) Target.BlueHearts;
    float num = -0.5f;
    while (++index < this.HeartIcons.Count)
    {
      if ((double) Mathf.Ceil(DataManager.Instance.PLAYER_TOTAL_HEALTH / 2f) + (double) Mathf.Ceil(Target.TotalSpiritHearts / 2f) + (double) Mathf.Ceil(DataManager.Instance.PLAYER_BLUE_HEARTS / 2f) > (double) index)
      {
        HUD_Heart heartIcon = this.HeartIcons[index];
        if (!heartIcon.gameObject.activeSelf)
        {
          Debug.Log((object) ("i: " + (object) index));
          heartIcon.ActivateAndScale(num += 0.5f);
          heartIcon.SetSprite(HUD_Heart.HeartState.HeartContainer);
        }
      }
    }
    this.UpdateHearts(Target, false);
  }

  public Vector3 GetNextPosition()
  {
    int index = -1;
    while (++index < this.HeartIcons.Count)
    {
      if (!this.HeartIcons[index].gameObject.activeSelf)
        return this.HeartIcons[index - 1].rectTransform.position + Vector3.right * (this.HeartIcons[index - 1].rectTransform.position.x / (float) index);
    }
    return Vector3.zero;
  }
}
