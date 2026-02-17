// Decompiled with JetBrains decompiler
// Type: FaithBarFake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class FaithBarFake : MonoBehaviour
{
  [SerializeField]
  public BarController barController;
  [SerializeField]
  public GameObject lockParent;
  [SerializeField]
  public GameObject lockIcon;
  public bool hiding;

  public static void Play(float faithAmount)
  {
    float newFaithNormalised = (float) (((double) CultFaithManager.CurrentFaith + (double) faithAmount) / 85.0);
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/UI/Faith Bar Fake.prefab", GameObject.FindGameObjectWithTag("Canvas").transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      FaithBarFake component = obj.Result.GetComponent<FaithBarFake>();
      component.transform.localScale = Vector3.zero;
      component.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      component.barController.SetBarSize(FollowerBrainStats.BrainWashed ? 1f : CultFaithManager.CultFaithNormalised, false, true);
      component.StartCoroutine((IEnumerator) component.SequenceIE(newFaithNormalised));
      component.lockParent.SetActive(FollowerBrainStats.BrainWashed);
    }));
  }

  public void Update()
  {
    if (HUD_Manager.Instance.Hidden || this.hiding)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.SequenceOut());
    this.hiding = true;
  }

  public IEnumerator SequenceOut()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FaithBarFake faithBarFake = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) faithBarFake.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    faithBarFake.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SequenceIE(float newFaithNormalised)
  {
    FaithBarFake faithBarFake = this;
    yield return (object) new WaitForSecondsRealtime(1f);
    if (faithBarFake.lockParent.activeSelf)
      faithBarFake.lockIcon.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
    else
      faithBarFake.barController.SetBarSize(newFaithNormalised, true, true);
    yield return (object) new WaitForSecondsRealtime(2f);
    faithBarFake.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    UnityEngine.Object.Destroy((UnityEngine.Object) faithBarFake.gameObject);
  }
}
