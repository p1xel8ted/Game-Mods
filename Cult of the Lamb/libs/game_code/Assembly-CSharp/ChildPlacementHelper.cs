// Decompiled with JetBrains decompiler
// Type: ChildPlacementHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ChildPlacementHelper : BaseMonoBehaviour
{
  public float circleRadius = 2.5f;
  public float circlePlacementOffset;
  public GameObject[] GameObjects;
  public float angle;
  public Vector3 dir;
  public bool FaceTowards = true;
  public bool flipToFaceCenter;
  public float zOffset = 0.1f;
  public float yOffset = 0.1f;
  public float xOffset = 0.1f;
  public Color Color_0;
  public Color Color_1;
  public float zPosMin;
  public float zPosMax = -1f;

  public void GetChildren()
  {
    this.GameObjects = new GameObject[this.transform.childCount];
    int index = -1;
    while (++index < this.transform.childCount)
      this.GameObjects[index] = this.transform.GetChild(index).gameObject;
  }

  public void Start()
  {
  }

  public void Update()
  {
  }

  public void PutObjectsInCircle()
  {
    int index = -1;
    while (++index < this.GameObjects.Length)
    {
      float f = (float) ((double) index * (360.0 / (double) this.GameObjects.Length) * (Math.PI / 180.0)) + this.circlePlacementOffset;
      if ((UnityEngine.Object) this.GameObjects[index] == (UnityEngine.Object) null)
      {
        Health componentInParent = this.GetComponentInParent<Health>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
          Debug.LogError((object) ("Null object in GameObject list for prefab: " + componentInParent.gameObject.name));
        else
          Debug.LogError((object) ("Null object in GameObject list for object of name " + this.gameObject.name));
      }
      else
      {
        Vector3 vector3 = this.transform.position + new Vector3(this.circleRadius * Mathf.Cos(f), this.circleRadius * Mathf.Sin(f));
        this.GameObjects[index].transform.position = vector3;
      }
    }
    foreach (GameObject gameObject in this.GameObjects)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        StateMachine component = gameObject.GetComponent<StateMachine>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          if (this.FaceTowards)
          {
            Vector3 vector3 = this.transform.InverseTransformDirection(this.transform.position - gameObject.transform.position);
            this.angle = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
          }
          else
          {
            Vector3 direction = gameObject.transform.position - this.transform.position;
            Vector3 vector3 = gameObject.transform.InverseTransformDirection(direction);
            this.angle = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
          }
          component.LookAngle = this.angle;
          component.facingAngle = this.angle;
          if (this.flipToFaceCenter)
          {
            if ((double) this.angle > 90.0 && (double) this.angle < -90.0 || (double) this.angle > -90.0 && (double) this.angle < 90.0)
            {
              if ((double) Mathf.Sign(gameObject.transform.localScale.x) == 1.0)
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
            else if ((double) Mathf.Sign(gameObject.transform.localScale.x) == -1.0)
              gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
          }
        }
      }
    }
  }

  public void SpawnObjectsThroughNoise()
  {
    int index = -1;
    while (++index < this.GameObjects.Length)
    {
      float f = (float) ((double) index * (360.0 / (double) this.GameObjects.Length) * (Math.PI / 180.0));
      if ((UnityEngine.Object) this.GameObjects[index] == (UnityEngine.Object) null)
      {
        Health componentInParent = this.GetComponentInParent<Health>();
        if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
          Debug.LogError((object) ("Null object in GameObject list for prefab: " + componentInParent.gameObject.name));
        else
          Debug.LogError((object) ("Null object in GameObject list for object of name " + this.gameObject.name));
      }
      else
      {
        Vector3 vector3_1 = this.transform.position + new Vector3(this.circleRadius * Mathf.Cos(f), this.circleRadius * Mathf.Sin(f));
        Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.circleRadius;
        vector2.x += this.transform.position.x;
        vector2.y += this.transform.position.y;
        Vector3 vector3_2 = new Vector3(vector2.x, vector2.y, this.gameObject.transform.position.z);
        this.GameObjects[index].transform.position = vector3_2;
      }
    }
  }

  public void IncemementZ()
  {
    int index = -1;
    float z = this.GameObjects[0].transform.position.z;
    while (++index < this.GameObjects.Length)
    {
      this.GameObjects[index].transform.position = new Vector3(this.GameObjects[index].transform.position.x, this.GameObjects[index].transform.position.y, z);
      z += this.zOffset;
    }
  }

  public void IncemementY()
  {
    int index = -1;
    float y = this.GameObjects[0].transform.position.y;
    while (++index < this.GameObjects.Length)
    {
      this.GameObjects[index].transform.position = new Vector3(this.GameObjects[index].transform.position.x, y, this.GameObjects[index].transform.position.z);
      y += this.yOffset;
    }
  }

  public void IncemementX()
  {
    int index = -1;
    float x = this.GameObjects[0].transform.position.x;
    while (++index < this.GameObjects.Length)
    {
      this.GameObjects[index].transform.position = new Vector3(x, this.GameObjects[index].transform.position.y, this.GameObjects[index].transform.position.z);
      x += this.xOffset;
    }
  }

  public void Incemement_Color()
  {
    int index = -1;
    float t = 0.0f;
    while (++index < this.GameObjects.Length)
    {
      SpriteRenderer component = this.GameObjects[index].gameObject.GetComponent<SpriteRenderer>();
      t += 0.1f;
      Color color = Color.Lerp(this.Color_0, this.Color_1, t);
      component.color = color;
    }
  }

  public void Incemement_Color(Color Color_0, Color Color_1)
  {
    int index = -1;
    float t = 0.0f;
    while (++index < this.GameObjects.Length)
    {
      SpriteRenderer component = this.GameObjects[index].gameObject.GetComponent<SpriteRenderer>();
      t += 0.1f;
      Color color = Color.Lerp(Color_0, Color_1, t);
      component.color = color;
    }
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.circleRadius, Color.green);
  }

  public void RandomZPosition()
  {
    int index = -1;
    while (++index < this.GameObjects.Length)
    {
      float z = UnityEngine.Random.Range(this.zPosMin, this.zPosMax);
      this.GameObjects[index].transform.position = new Vector3(this.GameObjects[index].transform.position.x, this.GameObjects[index].transform.position.y, z);
    }
  }
}
