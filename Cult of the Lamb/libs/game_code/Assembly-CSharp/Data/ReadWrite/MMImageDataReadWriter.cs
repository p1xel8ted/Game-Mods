// Decompiled with JetBrains decompiler
// Type: Data.ReadWrite.MMImageDataReadWriter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Data.ReadWrite;

public class MMImageDataReadWriter : MMImageReadWriterBase<Texture2D>
{
  public const string kSaveDirectory = "Photos";

  public override void Write(Texture2D data, string filename)
  {
    bool flag = true;
    FileStream fileStream = (FileStream) null;
    Debug.Log((object) ("MMImageDataReadWriter - Write File " + MMImageDataReadWriter.GetFilepath(filename)).Colour(Color.yellow));
    try
    {
      string directory = MMImageDataReadWriter.GetDirectory();
      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);
      byte[] jpg = data.EncodeToJPG(100);
      using (fileStream = new FileStream(MMImageDataReadWriter.GetFilepath(filename), FileMode.Create))
        fileStream.Write(jpg, 0, jpg.Length);
    }
    catch (Exception ex)
    {
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError != null)
        onWriteError(new MMReadWriteError(ex.Message));
      Debug.Log((object) ex.Message.Colour(Color.red));
      Debug.LogException((Exception) ex);
      flag = false;
    }
    finally
    {
      fileStream?.Dispose();
      if (flag)
      {
        System.Action onWriteCompleted = this.OnWriteCompleted;
        if (onWriteCompleted != null)
          onWriteCompleted();
      }
    }
  }

  public override void Read(string filename)
  {
    Debug.Log((object) ("MMImageDataReadWriter - Read File " + MMImageDataReadWriter.GetFilepath(filename)).Colour(Color.yellow));
    bool flag = true;
    Texture2D tex = (Texture2D) null;
    try
    {
      if (!this.FileExists(filename))
        return;
      tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
      byte[] data = File.ReadAllBytes(MMImageDataReadWriter.GetFilepath(filename));
      tex.LoadImage(data);
      tex.Apply(false, true);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Debug.LogException((Exception) ex);
      Action<MMReadWriteError> onReadError = this.OnReadError;
      if (onReadError != null)
        onReadError(new MMReadWriteError(ex.Message));
      flag = false;
    }
    finally
    {
      if (flag)
      {
        Action<Texture2D> onReadCompleted = this.OnReadCompleted;
        if (onReadCompleted != null)
          onReadCompleted(tex);
      }
    }
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("MMImageDataReadWriter - Delete " + MMImageDataReadWriter.GetFilepath(filename)).Colour(Color.yellow));
    if (!this.FileExists(filename))
      return;
    Debug.Log((object) ("MMImageDataReadWriter - Deletion Successful " + MMImageDataReadWriter.GetFilepath(filename)).Colour(Color.yellow));
    File.Delete(MMImageDataReadWriter.GetFilepath(filename));
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  public override bool FileExists(string filename)
  {
    Debug.Log((object) ("MMImageDataReadWriter - File Exists? " + MMImageDataReadWriter.GetFilepath(filename)).Colour(Color.yellow));
    return File.Exists(MMImageDataReadWriter.GetFilepath(filename));
  }

  public override string[] GetFiles()
  {
    if (!Directory.Exists(MMImageDataReadWriter.GetDirectory()))
      return Array.Empty<string>();
    FileInfo[] array = ((IEnumerable<FileInfo>) ((IEnumerable<FileInfo>) new DirectoryInfo(MMImageDataReadWriter.GetDirectory()).GetFiles()).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (p => p.CreationTime)).ToArray<FileInfo>()).Reverse<FileInfo>().ToArray<FileInfo>();
    string[] files = new string[array.Length];
    for (int index = 0; index < array.Length; ++index)
      files[index] = array[index].Name.Replace(array[index].Extension, string.Empty);
    return files;
  }

  public static string GetDirectory() => Path.Combine(Application.persistentDataPath, "Photos");

  public static string GetFilepath(string filename)
  {
    return Path.Combine(MMImageDataReadWriter.GetDirectory(), filename + ".jpeg");
  }
}
