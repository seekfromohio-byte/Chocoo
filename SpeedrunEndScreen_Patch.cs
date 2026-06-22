// Decompiled with JetBrains decompiler
// Type: X.SpeedrunEndScreen_Patch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (EndGameManager), "SetEverythingUp")]
public static class SpeedrunEndScreen_Patch
{
  public static void Postfix(EndGameManager __instance)
  {
    if (!ChocooPlugin.TaskSpeedrunEnabled)
      return;
    using (IEnumerator<PoolablePlayer> enumerator = ((Component) ((Component) __instance).transform).GetComponentsInChildren<PoolablePlayer>().GetEnumerator())
    {
      do
        ;
      while (enumerator.MoveNext() && Object.op_Equality((Object) enumerator.Current.cosmetics?.nameText, (Object) null));
    }
  }
}
