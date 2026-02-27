// Decompiled with JetBrains decompiler
// Type: UIExtinguishMiniGameOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIExtinguishMiniGameOverlay : MonoBehaviour
{
  [SerializeField]
  public RectTransform cursor;
  [SerializeField]
  public LineRenderer[] lineRenderers;
  [SerializeField]
  public List<RectTransform> points;
  [SerializeField]
  public RectTransform targetsContainer;
  [SerializeField]
  public List<UIExtinguishMiniGameOverlay.Target> targets;
  public List<int> keyboard_actions = new List<int>()
  {
    9,
    68,
    16 /*0x10*/,
    94
  };
  public List<int> controller_actions = new List<int>()
  {
    9,
    68,
    67,
    66
  };
  public bool active;
  public int difficulty;
  public DG.Tweening.Sequence gameSequence;
  public const float DEFAULT_SPEED = 1.25f;
  public const float SPEED_INCREASER = 0.4f;

  public event UIExtinguishMiniGameOverlay.ExtinguishEvent OnSuccess;

  public event UIExtinguishMiniGameOverlay.ExtinguishEvent OnFailure;

  public void Configure(int difficulty)
  {
    this.difficulty = difficulty;
    this.Rebuild();
    for (int index = 0; index < this.targets.Count; ++index)
    {
      do
      {
        this.targets[index].ControlPrompt.Category = 0;
        this.targets[index].ControlPrompt.Action = InputManager.General.InputIsController() ? this.controller_actions[UnityEngine.Random.Range(0, this.controller_actions.Count)] : this.keyboard_actions[UnityEngine.Random.Range(0, this.keyboard_actions.Count)];
      }
      while (index != 0 && this.targets[index - 1].ControlPrompt.Action == this.targets[index].ControlPrompt.Action);
    }
    Canvas component = this.GetComponent<Canvas>();
    component.worldCamera = CameraFollowTarget.Instance.GetComponent<Camera>();
    component.sortingLayerName = "Lvl1 Game UI";
  }

  public void Begin()
  {
    this.gameSequence = DOTween.Sequence();
    for (int index = 1; index < this.points.Count; ++index)
      this.gameSequence.Append((Tween) this.cursor.DOLocalMove(this.points[index].localPosition, Vector3.Distance(this.points[index].localPosition, this.points[index - 1].localPosition) / 100f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear));
    this.gameSequence.DOTimeScale(1.25f, 0.0f);
    this.gameSequence.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => this.Close(true)));
    this.active = true;
  }

  public void Rebuild()
  {
    float num1 = 250f;
    float num2 = -105f;
    if (Application.isPlaying)
    {
      if (this.difficulty <= 1)
      {
        this.RemoveTargets(1);
        this.RemoveTargets(3);
        num2 = -140f;
      }
      else if (this.difficulty <= 2)
      {
        this.RemoveTargets(2);
        num2 = -120f;
      }
    }
    for (int index = 0; index < this.targets.Count; ++index)
    {
      this.targets[index].Container.transform.localPosition = new Vector3((float) UnityEngine.Random.Range(-100, 100), num1 + num2 * (float) index + (float) UnityEngine.Random.Range(-10, 10));
      this.points[index + 1].transform.position = this.targets[index].Container.transform.position;
    }
    this.points[this.points.Count - 1].transform.localPosition = new Vector3((float) UnityEngine.Random.Range(-100, 100), -280f);
    foreach (LineRenderer lineRenderer in this.lineRenderers)
    {
      lineRenderer.positionCount = this.points.Count;
      for (int index = 0; index < lineRenderer.positionCount; ++index)
        lineRenderer.SetPosition(index, this.points[index].localPosition);
    }
    this.cursor.transform.localPosition = this.points[0].localPosition;
  }

  public void Update()
  {
    if (!this.active)
      return;
    bool flag = false;
    UIExtinguishMiniGameOverlay.Target target1 = (UIExtinguishMiniGameOverlay.Target) null;
    foreach (UIExtinguishMiniGameOverlay.Target target2 in this.targets)
    {
      if ((double) Vector3.Distance(this.cursor.position, target2.ControlPrompt.transform.position) < 0.05000000074505806 && !target2.Succeeded)
      {
        target2.Activated = true;
        flag = true;
        target1 = target2;
        break;
      }
      if (target2.Activated && !target2.Succeeded)
        this.PromptFail(target2);
    }
    if (!flag)
      return;
    target1.Background.color = Color.blue;
    if (!InputManager.General.GetAnyButton())
      return;
    if (InputManager.Gameplay.GetButtonDown(target1.ControlPrompt.Action))
      this.PromptSuccess(target1);
    else
      this.PromptFail(target1);
  }

  public void PromptSuccess(UIExtinguishMiniGameOverlay.Target target)
  {
    target.Background.color = Color.green;
    target.Succeeded = true;
    this.gameSequence.DOTimeScale(this.gameSequence.timeScale + 0.4f, 0.0f);
  }

  public void PromptFail(UIExtinguishMiniGameOverlay.Target target)
  {
    this.active = false;
    target.Background.color = Color.red;
    target.Activated = true;
    this.gameSequence.Kill();
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => this.Close(false)));
  }

  public void Close(bool success)
  {
    if (success)
    {
      UIExtinguishMiniGameOverlay.ExtinguishEvent onSuccess = this.OnSuccess;
      if (onSuccess != null)
        onSuccess();
    }
    else
    {
      UIExtinguishMiniGameOverlay.ExtinguishEvent onFailure = this.OnFailure;
      if (onFailure != null)
        onFailure();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void RemoveTargets(int index)
  {
    this.targets[index].Container.gameObject.SetActive(false);
    this.targets.RemoveAt(index);
    this.points.RemoveAt(index + 1);
  }

  [CompilerGenerated]
  public void \u003CBegin\u003Eb__21_0() => this.Close(true);

  [CompilerGenerated]
  public void \u003CPromptFail\u003Eb__25_0() => this.Close(false);

  [Serializable]
  public class Target
  {
    public GameObject Container;
    public MMControlPrompt ControlPrompt;
    public Image Background;
    [CompilerGenerated]
    public bool \u003CActivated\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CSucceeded\u003Ek__BackingField;

    public bool Activated
    {
      get => this.\u003CActivated\u003Ek__BackingField;
      set => this.\u003CActivated\u003Ek__BackingField = value;
    }

    public bool Succeeded
    {
      get => this.\u003CSucceeded\u003Ek__BackingField;
      set => this.\u003CSucceeded\u003Ek__BackingField = value;
    }
  }

  public delegate void ExtinguishEvent();
}
