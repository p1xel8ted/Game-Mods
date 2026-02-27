// Decompiled with JetBrains decompiler
// Type: WeatherVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteAlways]
[RequireComponent(typeof (BoxCollider2D))]
public class WeatherVolume : MonoBehaviour
{
  public List<WeatherManagerVolume> weatherVolume = new List<WeatherManagerVolume>();
  public float BlendTime = 2f;
  public bool ShowInSceneView = true;
  public bool activateObject;
  public GameObject objectToActivate;
  public List<WeatherManagerVolume> MyList;
  public bool inTrigger;

  private void Start()
  {
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(false);
    this.MyList = new List<WeatherManagerVolume>();
    for (int index = 0; index < this.weatherVolume.Count; ++index)
      this.MyList.Add(this.weatherVolume[index]);
    this.GetStartingValues();
  }

  public void activate()
  {
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(true);
    foreach (WeatherManagerVolume weatherManagerVolume in this.MyList)
    {
      switch (weatherManagerVolume.weatherEffect)
      {
        case WeatherManagerVolume.WeatherEffect.WindDensity:
          WeatherManager.instance.windDensity = weatherManagerVolume.valueToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindSpeed:
          WeatherManager.instance.windSpeed = weatherManagerVolume.valueToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindDirection:
          WeatherManager.instance.windDirection = weatherManagerVolume.Vector2ToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.RainIntensity:
          WeatherManager.instance.RainIntensity = weatherManagerVolume.valueToGoTo;
          continue;
        default:
          continue;
      }
    }
  }

  public void deactivate()
  {
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(true);
    foreach (WeatherManagerVolume weatherManagerVolume in this.MyList)
    {
      switch (weatherManagerVolume.weatherEffect)
      {
        case WeatherManagerVolume.WeatherEffect.WindDensity:
          WeatherManager.instance.windDensity = weatherManagerVolume.StartvalueToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindSpeed:
          WeatherManager.instance.windSpeed = weatherManagerVolume.StartvalueToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindDirection:
          WeatherManager.instance.windDirection = weatherManagerVolume.StartVector2ToGoTo;
          continue;
        case WeatherManagerVolume.WeatherEffect.RainIntensity:
          WeatherManager.instance.RainIntensity = weatherManagerVolume.StartvalueToGoTo;
          continue;
        default:
          continue;
      }
    }
  }

  private void GetStartingValues()
  {
    foreach (WeatherManagerVolume weatherManagerVolume in this.MyList)
    {
      switch (weatherManagerVolume.weatherEffect)
      {
        case WeatherManagerVolume.WeatherEffect.WindDensity:
          weatherManagerVolume.StartvalueToGoTo = WeatherManager.instance.windDensity;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindSpeed:
          weatherManagerVolume.StartvalueToGoTo = WeatherManager.instance.windSpeed;
          continue;
        case WeatherManagerVolume.WeatherEffect.WindDirection:
          weatherManagerVolume.StartVector2ToGoTo = WeatherManager.instance.windDirection;
          continue;
        case WeatherManagerVolume.WeatherEffect.RainIntensity:
          weatherManagerVolume.StartvalueToGoTo = WeatherManager.instance.RainIntensity;
          continue;
        default:
          continue;
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    this.inTrigger = true;
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(true);
    this.activate();
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    this.inTrigger = false;
    this.deactivate();
    if (!((Object) this.objectToActivate != (Object) null))
      return;
    this.objectToActivate.SetActive(false);
  }

  public void manualExitAndDeactive()
  {
    this.inTrigger = false;
    this.deactivate();
    if ((Object) this.objectToActivate != (Object) null)
      this.objectToActivate.SetActive(false);
    this.gameObject.SetActive(false);
  }

  private void OnDisable() => this.manualExitAndDeactive();

  private void OnDrawGizmos()
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

  private void OnDrawGizmosSelected()
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
}
