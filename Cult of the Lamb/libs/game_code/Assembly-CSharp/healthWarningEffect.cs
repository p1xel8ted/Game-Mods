// Decompiled with JetBrains decompiler
// Type: healthWarningEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class healthWarningEffect : MonoBehaviour
{
  public Image healthOverlay;
  public Image poisonOverlay;
  public Image fireOverlay;
  public float playerHealth;
  public float playerPoison;
  public float playerFire;
  public float hpCache;
  public bool turnedonPoison;
  public bool turnedonFire;
  public bool turnedonHealth;

  public void Start()
  {
    this.healthOverlay.gameObject.SetActive(false);
    this.poisonOverlay.gameObject.SetActive(false);
    this.fireOverlay.gameObject.SetActive(false);
    this.healthOverlay.color = new Color(this.healthOverlay.color.r, this.healthOverlay.color.g, this.healthOverlay.color.b, 0.0f);
  }

  public IEnumerator DisableHealth()
  {
    yield return (object) new WaitForSeconds(1f);
    this.healthOverlay.gameObject.SetActive(false);
    this.turnedonHealth = false;
  }

  public void Update()
  {
    this.playerHealth = float.MaxValue;
    this.playerPoison = 0.0f;
    this.playerFire = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if (!((Object) player == (Object) null) && !((Object) player.health == (Object) null))
      {
        float currentHp = player.health.CurrentHP;
        if ((double) currentHp < (double) this.playerHealth)
          this.playerHealth = currentHp;
        if ((double) player.health.PLAYER_TOTAL_HEALTH > 4.0)
          flag1 = true;
        if ((double) this.playerHealth < (double) player.health.PLAYER_TOTAL_HEALTH / 2.0)
          flag2 = true;
        this.playerPoison = player.health.poisonTimer / 2f;
        this.playerFire = player.health.burnTimer / 2f;
        if ((double) this.playerPoison > 0.0)
        {
          if (!this.turnedonPoison)
          {
            this.turnedonPoison = true;
            this.poisonOverlay.gameObject.SetActive(true);
          }
          this.poisonOverlay.color = new Color(this.poisonOverlay.color.r, this.poisonOverlay.color.g, this.poisonOverlay.color.b, this.playerPoison);
        }
        else
        {
          this.poisonOverlay.color = new Color(this.poisonOverlay.color.r, this.poisonOverlay.color.g, this.poisonOverlay.color.b, 0.0f);
          if (this.turnedonPoison)
          {
            this.poisonOverlay.gameObject.SetActive(false);
            this.turnedonPoison = false;
          }
        }
        if ((double) this.playerFire > 0.0)
        {
          if (!this.turnedonFire)
          {
            this.turnedonFire = true;
            this.fireOverlay.gameObject.SetActive(true);
          }
          this.fireOverlay.color = new Color(this.fireOverlay.color.r, this.fireOverlay.color.g, this.fireOverlay.color.b, this.playerFire);
        }
        else
        {
          this.fireOverlay.color = new Color(this.fireOverlay.color.r, this.fireOverlay.color.g, this.fireOverlay.color.b, 0.0f);
          if (this.turnedonFire)
          {
            this.fireOverlay.gameObject.SetActive(false);
            this.turnedonFire = false;
          }
        }
      }
    }
    if ((double) this.playerHealth == (double) this.hpCache || (double) this.playerHealth == 3.4028234663852886E+38)
      return;
    this.healthOverlay.DOKill();
    float playerHealth = this.playerHealth;
    if ((double) playerHealth <= 1.0)
    {
      if ((double) playerHealth != 0.0)
      {
        if ((double) playerHealth == 1.0)
        {
          this.healthOverlay.DOKill();
          DOTweenModuleUI.DOFade(this.healthOverlay, 1f, 0.5f);
        }
      }
      else
      {
        this.healthOverlay.DOKill();
        DOTweenModuleUI.DOFade(this.healthOverlay, 0.0f, 0.5f);
      }
    }
    else if ((double) playerHealth != 2.0)
    {
      if ((double) playerHealth != 3.0)
      {
        if ((double) playerHealth == 4.0)
        {
          this.healthOverlay.DOKill();
          DOTweenModuleUI.DOFade(this.healthOverlay, 0.2f, 0.5f);
        }
      }
      else
      {
        this.healthOverlay.DOKill();
        DOTweenModuleUI.DOFade(this.healthOverlay, 0.33f, 0.5f);
      }
    }
    else
    {
      this.healthOverlay.DOKill();
      DOTweenModuleUI.DOFade(this.healthOverlay, 0.66f, 0.5f);
    }
    bool flag3 = false;
    if ((double) this.playerHealth <= 4.0 & flag1)
      flag3 = true;
    else if (flag1)
      flag3 = flag2;
    if (flag3)
    {
      this.turnedonHealth = true;
      this.healthOverlay.gameObject.SetActive(true);
    }
    else if (this.turnedonHealth)
    {
      this.StartCoroutine((IEnumerator) this.DisableHealth());
      DOTweenModuleUI.DOFade(this.healthOverlay, 0.0f, 0.5f);
    }
    this.hpCache = this.playerHealth;
  }
}
