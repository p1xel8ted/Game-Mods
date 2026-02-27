// Decompiled with JetBrains decompiler
// Type: BloodSplatterScreenOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BloodSplatterScreenOverlay : MonoBehaviour
{
  public List<Image> bloodImage = new List<Image>();
  public static BloodSplatterScreenOverlay instance;
  public static int NoiseThreshold = Shader.PropertyToID("_NoiseThreshold");
  public Vector3 cachePos;

  public void Start()
  {
    foreach (Component component in this.bloodImage)
      component.gameObject.SetActive(false);
    this.cachePos = this.transform.position;
    BloodSplatterScreenOverlay.instance = this;
  }

  public void OnDisable()
  {
    foreach (Graphic graphic in this.bloodImage)
      graphic.material.SetFloat(BloodSplatterScreenOverlay.NoiseThreshold, 0.07f);
  }

  public void PlayRoutine()
  {
    Debug.Log((object) "Play");
    this.StartCoroutine(this.PlayBlood());
  }

  public IEnumerator PlayBlood()
  {
    BloodSplatterScreenOverlay splatterScreenOverlay = this;
    BloodSplatterScreenOverlay.instance = splatterScreenOverlay;
    foreach (Image image in splatterScreenOverlay.bloodImage)
    {
      image.gameObject.SetActive(false);
      image.enabled = false;
    }
    splatterScreenOverlay.transform.position = splatterScreenOverlay.cachePos;
    Image blood = splatterScreenOverlay.bloodImage[Random.Range(0, splatterScreenOverlay.bloodImage.Count - 1)];
    blood.enabled = true;
    blood.gameObject.SetActive(true);
    blood.material.DOKill();
    blood.material.SetFloat(BloodSplatterScreenOverlay.NoiseThreshold, 0.07f);
    splatterScreenOverlay.transform.DOKill();
    splatterScreenOverlay.transform.DOPunchScale(Vector3.one * 0.005f, 0.33f);
    AudioManager.Instance.PlayOneShot("event:/enemy/gethit_large", PlayerFarming.Instance.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    yield return (object) new WaitForSeconds(1.5f);
    splatterScreenOverlay.transform.DOLocalMoveY(-50f, 4f);
    blood.material.DOKill();
    blood.material.DOFloat(1f, BloodSplatterScreenOverlay.NoiseThreshold, 4f);
    yield return (object) new WaitForSeconds(4f);
    splatterScreenOverlay.transform.position = splatterScreenOverlay.cachePos;
    blood.material.SetFloat(BloodSplatterScreenOverlay.NoiseThreshold, 0.07f);
    blood.gameObject.SetActive(false);
  }
}
