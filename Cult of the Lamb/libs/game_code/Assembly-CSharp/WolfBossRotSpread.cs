// Decompiled with JetBrains decompiler
// Type: WolfBossRotSpread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using UnityEngine;

#nullable disable
public class WolfBossRotSpread : MonoBehaviour
{
  public float lowerStairsMaxY = -7.9f;
  public float upperStairsMinY = 3.5f;
  public float movementSpeed;
  public EventInstance rotSpreadLoop;
  public EventInstance rotSpreadMusic;

  public void Start() => this.StartCoroutine((IEnumerator) this.InitPlayerWalking());

  public void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    if (PlayerFarming.Instance.GoToAndStopping)
    {
      this.movementSpeed = 1f;
    }
    else
    {
      this.movementSpeed = Mathf.Lerp(this.movementSpeed, (double) PlayerFarming.Instance.playerController.speed > 0.0 ? 1f : 0.0f, 2f * Time.deltaTime);
      if ((double) PlayerFarming.Instance.playerController.speed <= 0.0 || this.IsPlayerOutsideRotBounds(PlayerFarming.Instance))
        this.movementSpeed = 0.0f;
    }
    AudioManager.Instance.SetEventInstanceParameter(this.rotSpreadLoop, "playerMovementSpeed", this.movementSpeed);
    AudioManager.Instance.SetEventInstanceParameter(this.rotSpreadMusic, "playerMovementSpeed", this.movementSpeed);
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.rotSpreadLoop);
    AudioManager.Instance.StopLoop(this.rotSpreadMusic);
  }

  public bool IsPlayerOutsideRotBounds(PlayerFarming playerFarming)
  {
    return (double) playerFarming.transform.position.y > (double) this.lowerStairsMaxY && (double) playerFarming.transform.position.y < (double) this.upperStairsMinY;
  }

  public IEnumerator InitPlayerWalking()
  {
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
    this.StartRotLoops();
  }

  public void StartRotLoops()
  {
    this.rotSpreadLoop = AudioManager.Instance.CreateLoop("event:/dlc/env/rot/spread_loop", PlayerFarming.Instance.gameObject, true);
    this.rotSpreadMusic = AudioManager.Instance.CreateLoop("event:/dlc/music/marchosias/rot_spread_loop", true);
  }
}
