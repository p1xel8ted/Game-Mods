// Decompiled with JetBrains decompiler
// Type: WaterInteractionRiver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WaterInteractionRiver : BaseMonoBehaviour
{
  public GameObject waterVFX;
  public float splashFrequency = 0.075f;
  public float lastPlayerSplash;

  public void Start()
  {
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.gameObject.CompareTag("Player"))
      return;
    AudioManager.Instance.playerFootstepOverride = "event:/player/footstep_water";
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(other.gameObject);
    if (!((Object) farmingComponent != (Object) null))
      return;
    farmingComponent.SetActiveDustEffect(false);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (!other.gameObject.CompareTag("Player"))
      return;
    AudioManager.Instance.playerFootstepOverride = string.Empty;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(other.gameObject);
    if (!((Object) farmingComponent != (Object) null))
      return;
    farmingComponent.SetActiveDustEffect(true);
  }

  public void OnTriggerStay2D(Collider2D other)
  {
    UnitObject component = other.GetComponent<UnitObject>();
    if (!(bool) (Object) component)
      return;
    if (component.gameObject.CompareTag("Player") && (double) this.lastPlayerSplash < (double) Time.realtimeSinceStartup - 0.30000001192092896)
    {
      Object.Instantiate<GameObject>(this.waterVFX, other.transform.position, Quaternion.identity);
      this.lastPlayerSplash = Time.realtimeSinceStartup;
    }
    if ((double) Random.value >= (double) this.splashFrequency)
      return;
    Object.Instantiate<GameObject>(this.waterVFX, other.transform.position, Quaternion.identity);
  }

  public void OnDisable() => AudioManager.Instance.playerFootstepOverride = string.Empty;

  public void OnDestroy() => AudioManager.Instance.playerFootstepOverride = string.Empty;
}
