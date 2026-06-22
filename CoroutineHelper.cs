// Decompiled with JetBrains decompiler
// Type: X.CoroutineHelper
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using BepInEx.Unity.IL2CPP.Utils;
using System.Collections;
using UnityEngine;

#nullable disable
namespace X;

public static class CoroutineHelper
{
  private static GameObject _helperObject;

  public static Coroutine Start(IEnumerator routine)
  {
    if (Object.op_Equality((Object) CoroutineHelper._helperObject, (Object) null))
    {
      CoroutineHelper._helperObject = new GameObject(nameof (CoroutineHelper));
      Object.DontDestroyOnLoad((Object) CoroutineHelper._helperObject);
      CoroutineHelper._helperObject.AddComponent<CoroutineHelper.CoroutineRunner>();
    }
    return MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) CoroutineHelper._helperObject.GetComponent<CoroutineHelper.CoroutineRunner>(), routine);
  }

  private class CoroutineRunner : MonoBehaviour
  {
  }
}
