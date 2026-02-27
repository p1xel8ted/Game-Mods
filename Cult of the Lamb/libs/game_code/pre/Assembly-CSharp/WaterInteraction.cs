// Decompiled with JetBrains decompiler
// Type: WaterInteraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WaterInteraction : BaseMonoBehaviour
{
  public bool inTrigger;
  public Collider2D playerCollider;
  public float timer;
  public Vector3 PlayerOldPosition;
  public float TimeToSpawn = 1f;
  public GameObject waterVFX;
  public Vector3 prefabOffset;

  private void Start()
  {
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!(other.gameObject.tag == "Player"))
      return;
    this.playerCollider = other;
    this.inTrigger = true;
    AudioManager.Instance.playerFootstepOverride = "event:/player/footstep_water";
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (!(other.gameObject.tag == "Player"))
      return;
    this.playerCollider = other;
    this.inTrigger = false;
    AudioManager.Instance.playerFootstepOverride = string.Empty;
  }

  private void OnDisable() => AudioManager.Instance.playerFootstepOverride = string.Empty;

  private void OnDestroy() => AudioManager.Instance.playerFootstepOverride = string.Empty;

  private void Update()
  {
    if (!this.inTrigger || !((Object) this.playerCollider != (Object) null) || (double) (this.timer += Time.deltaTime) < (double) this.TimeToSpawn || !(this.PlayerOldPosition != this.playerCollider.gameObject.transform.position))
      return;
    Vector3 position = this.playerCollider.gameObject.transform.position;
    this.PlayerOldPosition = position;
    Object.Instantiate<GameObject>(this.waterVFX, position + this.prefabOffset, Quaternion.identity);
    this.timer = 0.0f;
  }
}
