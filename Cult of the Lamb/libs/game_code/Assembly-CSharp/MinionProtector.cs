// Decompiled with JetBrains decompiler
// Type: MinionProtector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MinionProtector : MonoBehaviour
{
  public UnitObject[] protectedUnits;
  public float damageMultiplier = 0.1f;
  public bool showLineToMinions;
  public Material healthLineMaterial;
  public float healthLineLerpSpeed;
  public VFXParticle protectedVfxParticle;
  public System.Action destroyedAction;
  public List<LineRenderer> healthLines;

  public void Start()
  {
    for (int index = 0; index < this.protectedUnits.Length; ++index)
    {
      UnitObject protectedUnit = this.protectedUnits[index];
      Health component = protectedUnit.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.protector = this;
        if (this.showLineToMinions)
        {
          LineRenderer lineRenderer = component.gameObject.AddComponent<LineRenderer>();
          lineRenderer.positionCount = 2;
          lineRenderer.material = this.healthLineMaterial;
          lineRenderer.startWidth = 0.05f;
          lineRenderer.endWidth = 0.1f;
          lineRenderer.SetPosition(0, this.transform.position);
          lineRenderer.SetPosition(1, component.transform.position);
        }
        if ((UnityEngine.Object) this.protectedVfxParticle != (UnityEngine.Object) null)
        {
          VFXParticle vfxParticle = UnityEngine.Object.Instantiate<VFXParticle>(this.protectedVfxParticle, protectedUnit.transform);
          vfxParticle.name = "ProtectionVfx";
          vfxParticle.gameObject.SetActive(true);
          vfxParticle.loopedSoundSFX = "event:/enemy/shielded_enemy_loop";
          vfxParticle.Init();
          vfxParticle.PlayVFX(0.0f, (PlayerFarming) null, true);
        }
      }
    }
  }

  public void Update()
  {
    for (int index = 0; index < this.protectedUnits.Length; ++index)
    {
      UnitObject protectedUnit = this.protectedUnits[index];
      if ((UnityEngine.Object) protectedUnit != (UnityEngine.Object) null)
      {
        Health component1 = protectedUnit.GetComponent<Health>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          LineRenderer component2 = component1.gameObject.GetComponent<LineRenderer>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            if ((double) this.healthLineLerpSpeed == 0.0)
            {
              component2.SetPosition(0, this.transform.position);
            }
            else
            {
              Vector3 position = Vector3.Lerp(component2.GetPosition(0), this.transform.position, this.healthLineLerpSpeed * Time.deltaTime);
              component2.SetPosition(0, position);
            }
            component2.SetPosition(1, component1.transform.position);
          }
        }
      }
    }
  }

  public void OnDestroy()
  {
    System.Action destroyedAction = this.destroyedAction;
    if (destroyedAction != null)
      destroyedAction();
    for (int index = 0; index < this.protectedUnits.Length; ++index)
    {
      UnitObject protectedUnit = this.protectedUnits[index];
      if (!((UnityEngine.Object) protectedUnit == (UnityEngine.Object) null))
      {
        if ((UnityEngine.Object) this.protectedVfxParticle != (UnityEngine.Object) null)
        {
          Transform transform = protectedUnit.transform.Find("ProtectionVfx");
          if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
          {
            VFXParticle component = transform.GetComponent<VFXParticle>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.StopVFX();
            UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject, 2f);
          }
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) protectedUnit.gameObject.GetComponent<LineRenderer>());
      }
    }
  }
}
