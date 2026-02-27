// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.SerializableStackFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Unity.Cloud;

public class SerializableStackFrame
{
  public SerializableStackFrame()
  {
  }

  public SerializableStackFrame(StackFrame stackFrame)
  {
    MethodBase method = stackFrame.GetMethod();
    Type declaringType = method.DeclaringType;
    this.DeclaringType = declaringType != (Type) null ? declaringType.FullName : (string) null;
    this.Method = method.ToString();
    this.MethodName = method.Name;
    this.FileName = stackFrame.GetFileName();
    this.FileLine = stackFrame.GetFileLineNumber();
    this.FileColumn = stackFrame.GetFileColumnNumber();
  }

  public string DeclaringType { get; set; }

  public int FileColumn { get; set; }

  public int FileLine { get; set; }

  public string FileName { get; set; }

  public string Method { get; set; }

  public string MethodName { get; set; }
}
