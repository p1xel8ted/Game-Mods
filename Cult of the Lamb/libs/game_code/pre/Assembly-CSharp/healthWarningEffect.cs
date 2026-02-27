// Decompiled with JetBrains decompiler
// Type: healthWarningEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class healthWarningEffect : MonoBehaviour
{
  public Image healthOverlay;
  public Image poisonOverlay;
  public float playerHealth;
  public float playerPoison;
  private float hpCache;
  private bool turnedonPoison;
  private bool turnedonHealth;

  private void Start()
  {
    this.healthOverlay.gameObject.SetActive(false);
    this.poisonOverlay.gameObject.SetActive(false);
    this.healthOverlay.color = new Color(this.healthOverlay.color.r, this.healthOverlay.color.g, this.healthOverlay.color.b, 0.0f);
  }

  private IEnumerator DisableHealth()
  {
    yield return (object) new WaitForSeconds(1f);
    this.healthOverlay.gameObject.SetActive(false);
    this.turnedonHealth = false;
  }

  private void Update()
  {
    if ((Object) PlayerFarming.Instance == (Object) null || (Object) PlayerFarming.Instance.health == (Object) null)
      return;
    this.playerHealth = PlayerFarming.Instance.health.HP + PlayerFarming.Instance.health.BlueHearts + PlayerFarming.Instance.health.BlackHearts + PlayerFarming.Instance.health.SpiritHearts;
    this.playerPoison = PlayerFarming.Instance.health.poisonTimer / 2f;
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
    if ((double) this.playerHealth == (double) this.hpCache)
      return;
    this.healthOverlay.DOKill();
    float playerHealth = this.playerHealth;
    float num = 4f;
    if (!num.Equals(playerHealth))
    {
      num = 3f;
      if (!num.Equals(playerHealth))
      {
        num = 2f;
        if (!num.Equals(playerHealth))
        {
          num = 1f;
          if (!num.Equals(playerHealth))
          {
            num = 0.0f;
            if (num.Equals(playerHealth))
            {
              this.healthOverlay.DOKill();
              DOTweenModuleUI.DOFade(this.healthOverlay, 0.0f, 0.5f);
            }
          }
          else
          {
            this.healthOverlay.DOKill();
            DOTweenModuleUI.DOFade(this.healthOverlay, 1f, 0.5f);
          }
        }
        else
        {
          this.healthOverlay.DOKill();
          DOTweenModuleUI.DOFade(this.healthOverlay, 0.66f, 0.5f);
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
      DOTweenModuleUI.DOFade(this.healthOverlay, 0.2f, 0.5f);
    }
    bool flag = false;
    if ((double) this.playerHealth <= 4.0 && (double) DataManager.Instance.PLAYER_TOTAL_HEALTH > 4.0)
      flag = true;
    else if ((double) DataManager.Instance.PLAYER_TOTAL_HEALTH <= 4.0)
      flag = (double) this.playerHealth < (double) DataManager.Instance.PLAYER_TOTAL_HEALTH / 2.0;
    if (flag)
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
