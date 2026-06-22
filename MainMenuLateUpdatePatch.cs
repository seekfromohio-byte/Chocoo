// Decompiled with JetBrains decompiler
// Type: X.MainMenuLateUpdatePatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using HarmonyLib;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (MainMenuManager), "LateUpdate")]
[HarmonyPriority(800)]
public static class MainMenuLateUpdatePatch
{
  private static float lastCheckTime;

  public static void Postfix(MainMenuManager __instance)
  {
    if ((double) Time.time - (double) MainMenuLateUpdatePatch.lastCheckTime <= 0.5)
      return;
    MainMenuLateUpdatePatch.lastCheckTime = Time.time;
    if (Object.op_Equality((Object) GameObject.Find("ChocooMenuBackground"), (Object) null))
      MainMenuBackgroundPatch.Postfix(__instance);
  }
}
