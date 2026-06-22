// Decompiled with JetBrains decompiler
// Type: X.CancelRejoinOnExit
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using HarmonyLib;

#nullable disable
namespace X;

[HarmonyPatch(typeof (EndGameNavigation), "Exit")]
public static class CancelRejoinOnExit
{
  public static void Prefix() => AutoRejoinPatch.Cancelled = true;
}
