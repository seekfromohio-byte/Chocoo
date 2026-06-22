// Decompiled with JetBrains decompiler
// Type: X.VersionShowerPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using HarmonyLib;
using TMPro;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (VersionShower), "Start")]
[HarmonyPriority(0)]
public static class VersionShowerPatch
{
  private static int _gradientOffset;
  private static float _gradientTimer;

  public static void Postfix(VersionShower __instance)
  {
    if (Object.op_Equality((Object) __instance?.text, (Object) null))
      return;
    foreach (MonoBehaviour component in ((Component) __instance).gameObject.GetComponents<MonoBehaviour>())
    {
      if (Object.op_Inequality((Object) component, (Object) null) && component.GetType().Name.Contains("VersionShower") && component.GetType() != typeof (VersionShowerUpdater))
        Object.Destroy((Object) component);
    }
    string text = ((TMP_Text) __instance.text).text;
    VersionShowerUpdater versionShowerUpdater = ((Component) __instance).gameObject.AddComponent<VersionShowerUpdater>();
    versionShowerUpdater.versionShower = __instance;
    versionShowerUpdater.originalText = text;
  }
}
