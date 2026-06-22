// Decompiled with JetBrains decompiler
// Type: X.SpeedrunTimer_Patch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using InnerNet;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (ShipStatus), "CalculateLightRadius")]
public static class SpeedrunTimer_Patch
{
  public static void Prefix()
  {
    if (!((InnerNetClient) AmongUsClient.Instance).AmHost || !ChocooPlugin.TaskSpeedrunEnabled || !Object.op_Inequality((Object) ShipStatus.Instance, (Object) null))
      return;
    ChocooPlugin.SpeedrunTimer += Time.deltaTime;
  }
}
