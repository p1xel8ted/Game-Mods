// Decompiled with JetBrains decompiler
// Type: HasPet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HasPet : BaseMonoBehaviour
{
  public void Start()
  {
    GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Cat") as GameObject, this.transform.parent, true);
    gameObject.transform.position = this.transform.position - new Vector3(0.0f, -0.5f, 0.0f);
    gameObject.GetComponent<Pet>().Owner = this.gameObject;
  }
}
