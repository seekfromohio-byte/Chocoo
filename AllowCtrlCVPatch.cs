// Decompiled with JetBrains decompiler
// Type: X.AllowCtrlCVPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (TextBoxTMP), "Update")]
public static class AllowCtrlCVPatch
{
  public static void Postfix(TextBoxTMP __instance)
  {
    if (!ChocooPlugin.AllowCtrlCVEnabled || !__instance.hasFocus || !Input.GetKey((KeyCode) 306) && !Input.GetKey((KeyCode) 305))
      return;
    if (Input.GetKeyDown((KeyCode) 99))
      GUIUtility.systemCopyBuffer = __instance.text;
    if (Input.GetKeyDown((KeyCode) 118))
      __instance.SetText(__instance.text + GUIUtility.systemCopyBuffer, "");
    if (!Input.GetKeyDown((KeyCode) 120))
      return;
    GUIUtility.systemCopyBuffer = __instance.text;
    __instance.SetText("", "");
  }
}
