// Decompiled with JetBrains decompiler
// Type: BiomeConstantsVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteAlways]
[RequireComponent(typeof (BoxCollider2D))]
public class BiomeConstantsVolume : MonoBehaviour
{
  public List<BiomeVolume> biomeVolume = new List<BiomeVolume>();
  public float BlendTime = 2f;
  public bool ShowInSceneView = true;
  public bool activateObject;
  public GameObject objectToActivate;
  public BiomeConstantsVolume.ShaderTypes type;
  public List<BiomeVolume> MyList;
  public bool inTrigger;

  public void Update()
  {
    if (Application.isPlaying || !Application.isEditor)
      return;
    foreach (BiomeVolume biomeVolume in this.biomeVolume)
      biomeVolume.shaderName = biomeVolume.getName(biomeVolume._ShaderNames);
  }

  public void Start()
  {
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(false);
    this.MyList = new List<BiomeVolume>();
    for (int index = 0; index < this.biomeVolume.Count; ++index)
      this.MyList.Add(this.biomeVolume[index]);
  }

  public void activate()
  {
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(true);
    GameManager.startCoroutineAdjustGlobalShaders(this.MyList, this.BlendTime, 0.0f, 1f);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    this.inTrigger = true;
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(true);
    GameManager.startCoroutineAdjustGlobalShaders(this.MyList, this.BlendTime, 0.0f, 1f);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    this.inTrigger = false;
    GameManager.startCoroutineAdjustGlobalShaders(this.MyList, this.BlendTime, 1f, 0.0f);
    if (!((Object) this.objectToActivate != (Object) null))
      return;
    this.objectToActivate.SetActive(false);
  }

  public void manualExitAndDeactive()
  {
    this.inTrigger = false;
    if ((Object) GameManager.GetInstance() != (Object) null)
      GameManager.startCoroutineAdjustGlobalShaders(this.MyList, this.BlendTime, 1f, 0.0f);
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(false);
    this.gameObject.SetActive(false);
  }

  public void OnDisable() => this.manualExitAndDeactive();

  public void OnDrawGizmos()
  {
    if (!this.ShowInSceneView)
      return;
    BoxCollider component1 = this.GetComponent<BoxCollider>();
    BoxCollider2D component2 = this.GetComponent<BoxCollider2D>();
    if (!((Object) component1 != (Object) null) && !((Object) component2 != (Object) null))
      return;
    Vector3 center;
    Vector3 size;
    if ((Object) component1 != (Object) null)
    {
      center = component1.center;
      size = component1.size;
    }
    else
    {
      center = (Vector3) component2.offset;
      size = (Vector3) component2.size;
    }
    Gizmos.color = Color.green;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawWireCube(center, size);
  }

  public void OnDrawGizmosSelected()
  {
    BoxCollider component1 = this.GetComponent<BoxCollider>();
    BoxCollider2D component2 = this.GetComponent<BoxCollider2D>();
    if (!((Object) component1 != (Object) null) && !((Object) component2 != (Object) null))
      return;
    Gizmos.color = Color.green with { a = 0.2f };
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Vector3 center;
    Vector3 size;
    if ((Object) component1 != (Object) null)
    {
      center = component1.center;
      size = component1.size;
    }
    else
    {
      center = (Vector3) component2.offset;
      size = (Vector3) component2.size;
    }
    Gizmos.DrawCube(center, size);
  }

  public enum ShaderTypes
  {
    Float,
    Color,
    Texture,
  }
}
