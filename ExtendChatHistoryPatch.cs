// Decompiled with JetBrains decompiler
// Type: X.ExtendChatHistoryPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;

#nullable disable
namespace X;

[HarmonyPatch(typeof (ChatController), "Awake")]
public static class ExtendChatHistoryPatch
{
  public static void Postfix(ChatController __instance)
  {
    if (!ChocooPlugin.ExtendChatHistoryEnabled)
      return;
    __instance.chatBubblePool.poolSize = 50;
    __instance.chatBubblePool.ReclaimOldest();
  }
}
