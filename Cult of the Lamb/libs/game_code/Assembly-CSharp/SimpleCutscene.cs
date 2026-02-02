// Decompiled with JetBrains decompiler
// Type: SimpleCutscene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SimpleCutscene : BaseMonoBehaviour
{
  public bool TriggerOnCollision;
  public Vector3 TriggerPosition = Vector3.zero;
  public float TriggerRadius = 1f;
  [Space]
  public List<SimpleCutscene.CutsceneScene> Cutscene = new List<SimpleCutscene.CutsceneScene>();
  public GameObject Player;

  public void AddScene()
  {
    this.Cutscene.Add(new SimpleCutscene.CutsceneScene("Scene " + (this.Cutscene.Count + 1).ToString()));
  }

  public void Play()
  {
    Debug.Log((object) "PLAY CUTSCENE!");
    this.StartCoroutine((IEnumerator) this.PlayRoutine());
  }

  public void Update()
  {
    if (!this.TriggerOnCollision || (UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null || (double) Vector3.Distance(this.Player.transform.position, this.transform.position + this.TriggerPosition) >= (double) this.TriggerRadius)
      return;
    this.TriggerOnCollision = false;
    this.Play();
  }

  public IEnumerator PlayRoutine()
  {
    SimpleCutscene simpleCutscene = this;
    foreach (SimpleCutscene.CutsceneScene cutsceneScene in simpleCutscene.Cutscene)
    {
      foreach (SimpleCutscene.CutsceneObject c in cutsceneScene.Scene)
      {
        switch (c.Type)
        {
          case SimpleCutscene.CutsceneObject.TypeOfScene.Animate:
            yield return (object) simpleCutscene.StartCoroutine((IEnumerator) simpleCutscene.AnimateRoutine(c));
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.Delay:
            yield return (object) new WaitForSeconds(c.Delay);
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.Destroy:
            UnityEngine.Object.Destroy((UnityEngine.Object) c.ObjectToDestroy);
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.Move:
            if (c.WaitForEndOfMove)
            {
              yield return (object) simpleCutscene.StartCoroutine((IEnumerator) simpleCutscene.MoveRoutine(c));
              break;
            }
            simpleCutscene.StartCoroutine((IEnumerator) simpleCutscene.MoveRoutine(c));
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.BeginCutscene:
            GameManager.GetInstance().OnConversationNew();
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.MoveCutsceneCamera:
            GameManager.GetInstance().OnConversationNext(c.CameraFocus, c.Zoom);
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.EndCutscene:
            GameManager.GetInstance().OnConversationEnd();
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.AddObjectToCamera:
            GameManager.GetInstance().AddToCamera(c.AddToCamera);
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.ScreenShake:
            CameraManager.instance.ShakeCameraForDuration(c.MinimumShake, c.MaximumShake, c.ShakeDuration);
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.SetScale:
            c.ObjectToScale.transform.localScale = c.Scale;
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.Callback:
            c.Callback.Invoke();
            break;
          case SimpleCutscene.CutsceneObject.TypeOfScene.Conversation:
            MMConversation.Play(new ConversationObject(c.Entries, (List<MMTools.Response>) null, (System.Action) null), c.EndLetterBoxAfterConversation);
            while (MMConversation.isPlaying)
              yield return (object) null;
            break;
        }
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public IEnumerator AnimateRoutine(SimpleCutscene.CutsceneObject c)
  {
    SimpleCutscene simpleCutscene = this;
    c.Spine.AnimationState.SetAnimation(0, c.Animation, c.Loop);
    if (c.WaitForEndOfAnimation)
      yield return (object) new WaitForSeconds(c.Spine.AnimationState.GetCurrent(0).Animation.Duration);
    if (c.DestroyAfterAnimation)
      simpleCutscene.StartCoroutine((IEnumerator) simpleCutscene.DestroyAfterDelay(c, c.WaitForEndOfAnimation ? 0.0f : c.Spine.AnimationState.GetCurrent(0).Animation.Duration));
  }

  public IEnumerator DestroyAfterDelay(SimpleCutscene.CutsceneObject c, float Duration)
  {
    yield return (object) new WaitForSeconds(Duration);
    UnityEngine.Object.Destroy((UnityEngine.Object) c.Spine.gameObject);
  }

  public IEnumerator MoveRoutine(SimpleCutscene.CutsceneObject c)
  {
    float Progress = 0.0f;
    Vector3 StartingPosition = c.MoveObject.transform.position;
    if (c.isPlayer && (UnityEngine.Object) this.Player != (UnityEngine.Object) null)
      StartingPosition = this.Player.transform.position;
    float Distance = Vector3.Distance(StartingPosition, StartingPosition + c.MoveDestination);
    while ((double) Progress < 1.0)
    {
      Progress += c.MoveSpeed / Distance * Time.deltaTime;
      if ((double) Progress >= 1.0)
        Progress = 1f;
      c.MoveObject.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + c.MoveDestination, Progress);
      yield return (object) null;
    }
  }

  public void OnDrawGizmos()
  {
    if (this.TriggerOnCollision)
      Utils.DrawCircleXY(this.transform.position + this.TriggerPosition, this.TriggerRadius, Color.yellow);
    foreach (SimpleCutscene.CutsceneScene cutsceneScene in this.Cutscene)
    {
      List<SimpleCutscene.ObjectAndPositions> objectAndPositionsList = new List<SimpleCutscene.ObjectAndPositions>();
      int index1 = -1;
      while (++index1 < cutsceneScene.Scene.Count)
      {
        SimpleCutscene.CutsceneObject cutsceneObject = cutsceneScene.Scene[index1];
        if (cutsceneObject.Type == SimpleCutscene.CutsceneObject.TypeOfScene.Move && (UnityEngine.Object) cutsceneObject.MoveObject != (UnityEngine.Object) null)
        {
          SimpleCutscene.ObjectAndPositions objectAndPositions1 = (SimpleCutscene.ObjectAndPositions) null;
          foreach (SimpleCutscene.ObjectAndPositions objectAndPositions2 in objectAndPositionsList)
          {
            if ((UnityEngine.Object) objectAndPositions2.g == (UnityEngine.Object) cutsceneObject.MoveObject)
            {
              objectAndPositions1 = objectAndPositions2;
              break;
            }
          }
          if (objectAndPositions1 == null)
            objectAndPositionsList.Add(new SimpleCutscene.ObjectAndPositions(cutsceneObject.MoveObject, cutsceneObject.MoveObject.transform.position, cutsceneObject.MoveObject.transform.position + cutsceneObject.MoveDestination));
          else
            objectAndPositions1.AddPosition(cutsceneObject.MoveDestination);
        }
      }
      foreach (SimpleCutscene.ObjectAndPositions objectAndPositions in objectAndPositionsList)
      {
        int index2 = -1;
        while (++index2 < objectAndPositions.Positions.Count)
        {
          if (index2 > 0)
            Utils.DrawLine(objectAndPositions.Positions[index2 - 1], objectAndPositions.Positions[index2], objectAndPositions.color);
          Utils.DrawCircleXY(objectAndPositions.Positions[index2], 0.3f, objectAndPositions.color);
        }
      }
    }
  }

  public class ObjectAndPositions
  {
    public GameObject g;
    public List<Vector3> Positions = new List<Vector3>();
    public Color color;

    public ObjectAndPositions(
      GameObject Object,
      Vector3 StartingPosition,
      Vector3 DestinationPosition)
    {
      this.g = Object;
      this.Positions.Add(StartingPosition);
      this.Positions.Add(DestinationPosition);
      this.color = Color.blue;
    }

    public void AddPosition(Vector3 NewPosition)
    {
      this.Positions.Add(this.Positions[this.Positions.Count - 1] + NewPosition);
    }
  }

  [Serializable]
  public class CutsceneScene
  {
    public string Label = nameof (Scene);
    public List<SimpleCutscene.CutsceneObject> Scene = new List<SimpleCutscene.CutsceneObject>();

    public void AddStep()
    {
      this.Scene.Add(new SimpleCutscene.CutsceneObject(SimpleCutscene.CutsceneObject.TypeOfScene.Animate));
    }

    public CutsceneScene(string Label) => this.Label = Label;
  }

  [Serializable]
  public class CutsceneObject
  {
    public SimpleCutscene.CutsceneObject.TypeOfScene Type;
    public SkeletonAnimation Spine;
    [SpineAnimation("", "Spine", true, false)]
    public string Animation;
    public bool Loop;
    public bool WaitForEndOfAnimation = true;
    public bool DestroyAfterAnimation;
    public float Delay = 0.5f;
    public GameObject ObjectToDestroy;
    public GameObject CameraFocus;
    public float Zoom = 6f;
    public GameObject AddToCamera;
    public float MinimumShake = 0.2f;
    public float MaximumShake = 0.3f;
    public float ShakeDuration = 0.5f;
    public GameObject MoveObject;
    public Vector3 MoveDestination;
    public float MoveSpeed = 5f;
    public bool WaitForEndOfMove = true;
    public bool isPlayer;
    public GameObject ObjectToScale;
    public Vector3 Scale = Vector3.one;
    public UnityEvent Callback;
    public List<ConversationEntry> Entries;
    public bool EndLetterBoxAfterConversation = true;

    public CutsceneObject(SimpleCutscene.CutsceneObject.TypeOfScene Type) => this.Type = Type;

    public enum TypeOfScene
    {
      Animate,
      Delay,
      Destroy,
      Move,
      BeginCutscene,
      MoveCutsceneCamera,
      EndCutscene,
      AddObjectToCamera,
      ScreenShake,
      SetScale,
      Callback,
      PlaySound,
      Conversation,
    }
  }
}
