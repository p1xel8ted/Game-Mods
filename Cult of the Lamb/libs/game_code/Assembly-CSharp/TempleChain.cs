// Decompiled with JetBrains decompiler
// Type: TempleChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class TempleChain : BaseMonoBehaviour
{
  public DataManager.Chains Chain;

  public void OnEnable()
  {
    switch (this.Chain)
    {
      case DataManager.Chains.Chain1:
        this.gameObject.SetActive(!DataManager.Instance.Chain1);
        break;
      case DataManager.Chains.Chain2:
        this.gameObject.SetActive(!DataManager.Instance.Chain2);
        break;
      case DataManager.Chains.Chain3:
        this.gameObject.SetActive(!DataManager.Instance.Chain3);
        break;
    }
  }

  public void Play()
  {
    this.StartCoroutine((IEnumerator) this.PlayRoutine());
    this.StartCoroutine((IEnumerator) this.FadeRoutine());
  }

  public IEnumerator PlayRoutine()
  {
    TempleChain templeChain = this;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(templeChain.gameObject, 10f);
    float Progress = 0.0f;
    float Duration = 5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      templeChain.transform.localPosition = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.1f, 0.1f), -0.01f);
      GameManager.GetInstance().CameraSetZoom((float) (10.0 - 4.0 * (double) Progress / (double) Duration));
      CameraManager.shakeCamera((float) (0.10000000149011612 + 0.60000002384185791 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
  }

  public IEnumerator FadeRoutine()
  {
    yield return (object) new WaitForSeconds(2.5f);
    switch (this.Chain)
    {
      case DataManager.Chains.Chain1:
        DataManager.Instance.Chain1 = true;
        break;
      case DataManager.Chains.Chain2:
        DataManager.Instance.Chain2 = true;
        break;
      case DataManager.Chains.Chain3:
        DataManager.Instance.Chain3 = true;
        break;
    }
    GameManager.ToShip(Duration: 3f, Effect: MMTransition.Effect.WhiteFade);
  }
}
