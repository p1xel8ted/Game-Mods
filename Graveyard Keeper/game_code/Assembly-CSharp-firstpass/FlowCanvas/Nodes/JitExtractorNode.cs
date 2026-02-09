// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.JitExtractorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace FlowCanvas.Nodes;

public class JitExtractorNode : BaseReflectedExtractorNode
{
  public static Type[] DynParamTypes = new Type[1]
  {
    typeof (UniversalDelegateParam[])
  };
  public Type[] tmpTypes = new Type[1];
  public UniversalDelegateParam[] delegateParams;

  public void CreateDelegates()
  {
    int index1 = -1;
    for (int index2 = 0; index2 <= this.delegateParams.Length - 1; ++index2)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index2];
      if (delegateParam != null && delegateParam.paramDef.paramMode == ParamMode.Instance)
        index1 = index2;
    }
    for (int index3 = 0; index3 <= this.delegateParams.Length - 1; ++index3)
    {
      UniversalDelegateParam delegateParam1 = this.delegateParams[index3];
      if (delegateParam1 != null)
      {
        ParamDef paramDef = delegateParam1.paramDef;
        FieldInfo presentedInfo1 = paramDef.presentedInfo as FieldInfo;
        MethodInfo presentedInfo2 = paramDef.presentedInfo as MethodInfo;
        if (paramDef.paramMode != ParamMode.Instance && (!FieldInfo.op_Inequality(presentedInfo1, (FieldInfo) null) || !presentedInfo1.IsStatic || !presentedInfo1.IsReadOnly()))
        {
          DynamicMethod dynamicMethod = new DynamicMethod($"{this.TargetType.Name}_{paramDef.portId}_Extractor", (Type) null, JitExtractorNode.DynParamTypes, typeof (JitFieldNode));
          ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
          int num1 = -1;
          int num2 = 0;
          if (index1 >= 0)
          {
            UniversalDelegateParam delegateParam2 = this.delegateParams[index1];
            ilGenerator.DeclareLocal(delegateParam2.GetCurrentType());
            ++num1;
            ++num2;
          }
          ilGenerator.DeclareLocal(delegateParam1.GetCurrentType());
          if (index1 >= 0)
          {
            UniversalDelegateParam delegateParam3 = this.delegateParams[index1];
            ilGenerator.Emit(OpCodes.Ldarg, 0);
            ilGenerator.Emit(OpCodes.Ldc_I4, num1);
            ilGenerator.Emit(OpCodes.Ldelem_Ref);
            ilGenerator.Emit(OpCodes.Ldfld, delegateParam3.ValueField);
            ilGenerator.Emit(OpCodes.Stloc, num1);
          }
          if (FieldInfo.op_Inequality(presentedInfo1, (FieldInfo) null))
          {
            if (index1 >= 0)
            {
              ilGenerator.Emit(OpCodes.Ldloc, num1);
              ilGenerator.Emit(OpCodes.Ldfld, presentedInfo1);
            }
            else
              ilGenerator.Emit(OpCodes.Ldsfld, presentedInfo1);
          }
          if (MethodInfo.op_Inequality(presentedInfo2, (MethodInfo) null))
          {
            if (index1 >= 0)
              ilGenerator.Emit(this.delegateParams[index1].GetCurrentType().RTIsValueType() ? OpCodes.Ldloca : OpCodes.Ldloc, num1);
            if (index1 < 0 || this.delegateParams[index1].GetCurrentType().RTIsValueType())
              ilGenerator.Emit(OpCodes.Call, presentedInfo2);
            else
              ilGenerator.Emit(OpCodes.Callvirt, presentedInfo2);
          }
          ilGenerator.Emit(OpCodes.Stloc, num2);
          ilGenerator.Emit(OpCodes.Ldarg, 0);
          ilGenerator.Emit(OpCodes.Ldc_I4, num2);
          ilGenerator.Emit(OpCodes.Ldelem_Ref);
          ilGenerator.Emit(OpCodes.Ldloc, num2);
          ilGenerator.Emit(OpCodes.Stfld, delegateParam1.ValueField);
          ilGenerator.Emit(OpCodes.Ret);
          delegateParam1.referencedDelegate = (UniversalDelegate) ((MethodInfo) dynamicMethod).CreateDelegate(typeof (UniversalDelegate));
          UniversalDelegateParam universalDelegateParam = delegateParam1;
          UniversalDelegateParam[] universalDelegateParamArray;
          if (index1 < 0)
            universalDelegateParamArray = new UniversalDelegateParam[1]
            {
              delegateParam1
            };
          else
            universalDelegateParamArray = new UniversalDelegateParam[2]
            {
              this.delegateParams[index1],
              delegateParam1
            };
          universalDelegateParam.referencedParams = universalDelegateParamArray;
        }
      }
    }
  }

  public void Call(UniversalDelegateParam targetParam)
  {
    if (targetParam == null || targetParam.referencedDelegate == null || targetParam.referencedParams == null)
      return;
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      if (delegateParam != null && delegateParam.paramDef.paramMode == ParamMode.Instance)
      {
        delegateParam.SetFromInput();
        break;
      }
    }
    targetParam.referencedDelegate(targetParam.referencedParams);
  }

  public override bool InitInternal()
  {
    int length = 0;
    if (this.Params.instanceDef.paramMode == ParamMode.Instance)
      ++length;
    List<ParamDef> paramDefList = this.Params.paramDefinitions ?? new List<ParamDef>();
    for (int index = 0; index <= paramDefList.Count - 1; ++index)
    {
      ParamDef paramDef = paramDefList[index];
      if (paramDef.paramMode == ParamMode.Out && !MemberInfo.op_Equality(paramDef.presentedInfo, (MemberInfo) null))
        ++length;
    }
    this.delegateParams = new UniversalDelegateParam[length];
    int index1 = 0;
    if (this.Params.instanceDef.paramMode == ParamMode.Instance)
    {
      this.tmpTypes[0] = this.TargetType;
      this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
      this.delegateParams[index1].paramDef = this.Params.instanceDef;
      ++index1;
    }
    for (int index2 = 0; index2 <= paramDefList.Count - 1; ++index2)
    {
      ParamDef paramDef = paramDefList[index2];
      if (paramDef.paramMode == ParamMode.Out && !MemberInfo.op_Equality(paramDef.presentedInfo, (MemberInfo) null))
      {
        this.tmpTypes[0] = paramDef.paramType;
        this.delegateParams[index1] = (UniversalDelegateParam) typeof (UniversalDelegateParam<>).RTMakeGenericType(this.tmpTypes).CreateObject();
        this.delegateParams[index1].paramDef = paramDef;
        ++index1;
      }
    }
    try
    {
      this.CreateDelegates();
    }
    catch
    {
      return false;
    }
    return true;
  }

  public override void RegisterPorts(FlowNode node)
  {
    for (int index = 0; index <= this.delegateParams.Length - 1; ++index)
    {
      UniversalDelegateParam delegateParam = this.delegateParams[index];
      if (delegateParam != null)
      {
        ParamDef paramDef = delegateParam.paramDef;
        if (paramDef.paramMode == ParamMode.Instance)
          delegateParam.RegisterAsInput(node);
        if (paramDef.paramMode == ParamMode.Out)
        {
          FieldInfo presentedInfo = paramDef.presentedInfo as FieldInfo;
          if (FieldInfo.op_Inequality(presentedInfo, (FieldInfo) null) && presentedInfo.IsStatic && presentedInfo.IsReadOnly())
          {
            delegateParam.SetFromValue(presentedInfo.GetValue((object) null));
            delegateParam.RegisterAsOutput(node);
          }
          else
            delegateParam.RegisterAsOutput(node, new Action<UniversalDelegateParam>(this.Call));
        }
      }
    }
  }
}
