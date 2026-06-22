// Decompiled with JetBrains decompiler
// Type: X.MainMenuBackgroundPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using System;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (MainMenuManager), "Start")]
[HarmonyPriority(800)]
public static class MainMenuBackgroundPatch
{
  private static GameObject customBackground;
  private static bool hasInitialized;

  public static void Postfix(MainMenuManager __instance)
  {
    if (ChocooPlugin.DisableCustomTheme)
    {
      if (Object.op_Inequality((Object) MainMenuBackgroundPatch.customBackground, (Object) null))
      {
        Object.Destroy((Object) MainMenuBackgroundPatch.customBackground);
        MainMenuBackgroundPatch.customBackground = (GameObject) null;
      }
      MainMenuBackgroundPatch.hasInitialized = false;
    }
    else
    {
      try
      {
        MainMenuBackgroundPatch.InitializeChocooMainMenu(__instance);
      }
      catch (Exception ex)
      {
      }
    }
  }

  private static void InitializeChocooMainMenu(MainMenuManager __instance)
  {
    Sprite sprite = ChocooPlugin.LoadSpriteFromEmbedded("Area51files.Resources.mountain-background.png");
    if (Object.op_Equality((Object) sprite, (Object) null))
      return;
    MainMenuBackgroundPatch.DestroyOtherModBackgrounds();
    if (Object.op_Inequality((Object) __instance.screenTint, (Object) null))
    {
      ((Component) __instance.screenTint).gameObject.transform.localPosition = new Vector3(10000f, 0.0f, 0.0f);
      ((Renderer) __instance.screenTint).enabled = false;
    }
    if (Object.op_Inequality((Object) __instance.gameModeButtons, (Object) null))
    {
      Transform transform = __instance.gameModeButtons.gameObject.transform.Find("Divider");
      if (Object.op_Inequality((Object) transform, (Object) null))
        ((Component) transform).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) __instance.onlineButtons, (Object) null))
    {
      Transform transform1 = __instance.onlineButtons.gameObject.transform.Find("Divider");
      if (Object.op_Inequality((Object) transform1, (Object) null))
        ((Component) transform1).gameObject.SetActive(false);
      Transform transform2 = __instance.onlineButtons.gameObject.transform.Find("AspectSize");
      if (Object.op_Inequality((Object) transform2, (Object) null))
      {
        Transform transform3 = ((Component) transform2).gameObject.transform.Find("Scaler");
        if (Object.op_Inequality((Object) transform3, (Object) null))
        {
          Transform transform4 = ((Component) transform3).gameObject.transform.Find("Line");
          if (Object.op_Inequality((Object) transform4, (Object) null))
            ((Component) transform4).gameObject.SetActive(false);
        }
      }
    }
    if (Object.op_Inequality((Object) __instance.enterCodeButtons, (Object) null))
    {
      Transform transform = __instance.enterCodeButtons.gameObject.transform.Find("Divider");
      if (Object.op_Inequality((Object) transform, (Object) null))
        ((Component) transform).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) __instance.accountButtons, (Object) null))
    {
      Transform transform = __instance.accountButtons.gameObject.transform.Find("Divider");
      if (Object.op_Inequality((Object) transform, (Object) null))
        ((Component) transform).gameObject.SetActive(false);
    }
    GameObject gameObject1 = GameObject.Find("BackgroundTexture");
    if (Object.op_Inequality((Object) gameObject1, (Object) null))
      gameObject1.SetActive(false);
    GameObject gameObject2 = GameObject.Find("WindowShine");
    if (Object.op_Inequality((Object) gameObject2, (Object) null))
      gameObject2.SetActive(false);
    GameObject gameObject3 = GameObject.Find("ScreenCover");
    if (Object.op_Inequality((Object) gameObject3, (Object) null))
      gameObject3.SetActive(false);
    GameObject gameObject4 = GameObject.Find("RightPanel");
    if (Object.op_Inequality((Object) gameObject4, (Object) null))
    {
      SpriteRenderer component1 = gameObject4.GetComponent<SpriteRenderer>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        ((Renderer) component1).enabled = false;
      Transform transform = gameObject4.transform.Find("MaskedBlackScreen");
      if (Object.op_Inequality((Object) transform, (Object) null))
      {
        SpriteRenderer component2 = ((Component) transform).GetComponent<SpriteRenderer>();
        if (Object.op_Inequality((Object) component2, (Object) null))
          ((Renderer) component2).enabled = false;
      }
    }
    GameObject gameObject5 = GameObject.Find("LeftPanel");
    if (Object.op_Inequality((Object) gameObject5, (Object) null))
    {
      SpriteRenderer component3 = gameObject5.GetComponent<SpriteRenderer>();
      if (Object.op_Inequality((Object) component3, (Object) null))
        ((Renderer) component3).enabled = false;
      Transform transform = gameObject5.transform.Find("Divider");
      if (Object.op_Inequality((Object) transform, (Object) null))
      {
        SpriteRenderer component4 = ((Component) transform).GetComponent<SpriteRenderer>();
        if (Object.op_Inequality((Object) component4, (Object) null))
          ((Renderer) component4).enabled = false;
      }
    }
    try
    {
      PlayerParticles objectOfType = Object.FindObjectOfType<PlayerParticles>();
      if (Object.op_Inequality((Object) objectOfType, (Object) null) && Object.op_Inequality((Object) ((Component) objectOfType).gameObject, (Object) null))
        ((Component) objectOfType).gameObject.SetActive(false);
    }
    catch (Exception ex)
    {
    }
    if (Object.op_Inequality((Object) MainMenuBackgroundPatch.customBackground, (Object) null))
      Object.Destroy((Object) MainMenuBackgroundPatch.customBackground);
    GameObject gameObject6 = GameObject.Find("Divider");
    if (Object.op_Inequality((Object) gameObject6, (Object) null))
      gameObject6.SetActive(false);
    foreach (GameObject gameObject7 in Object.FindObjectsOfType<GameObject>())
    {
      if (((Object) gameObject7).name == "Line" || ((Object) gameObject7).name == "Divider")
        gameObject7.SetActive(false);
    }
    MainMenuBackgroundPatch.customBackground = new GameObject("ChocooMenuBackground");
    MainMenuBackgroundPatch.customBackground.transform.SetParent(((Component) __instance).transform, false);
    MainMenuBackgroundPatch.customBackground.layer = LayerMask.NameToLayer("UI");
    SpriteRenderer spriteRenderer = MainMenuBackgroundPatch.customBackground.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = sprite;
    MainMenuBackgroundPatch.customBackground.transform.position = new Vector3(0.0f, 0.0f, 600f);
    MainMenuBackgroundPatch.customBackground.transform.localPosition = new Vector3(0.0f, 0.0f, 600f);
    float num1 = Camera.main.orthographicSize * 2f;
    float num2 = num1 * Camera.main.aspect;
    Bounds bounds1 = sprite.bounds;
    float x = ((Bounds) ref bounds1).size.x;
    Bounds bounds2 = sprite.bounds;
    float y = ((Bounds) ref bounds2).size.y;
    float num3 = (float) ((double) num2 / (double) x * 1.2000000476837158);
    float num4 = (float) ((double) num1 / (double) y * 1.2000000476837158);
    MainMenuBackgroundPatch.customBackground.transform.localScale = new Vector3(num3, num4, 1f);
    ((Renderer) spriteRenderer).sortingLayerName = "UI";
    ((Renderer) spriteRenderer).sortingOrder = -1000;
    MainMenuBackgroundPatch.hasInitialized = true;
  }

  private static void DestroyOtherModBackgrounds()
  {
    try
    {
      GameObject gameObject1 = GameObject.Find("SplashArt");
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
        Object.Destroy((Object) gameObject1);
      GameObject gameObject2 = GameObject.Find("TuffMenuBackground");
      if (Object.op_Inequality((Object) gameObject2, (Object) null))
        Object.Destroy((Object) gameObject2);
      foreach (SpriteRenderer spriteRenderer in Object.FindObjectsOfType<SpriteRenderer>())
      {
        if (((Object) ((Component) spriteRenderer).gameObject).name.Contains("Background") && ((Object) ((Component) spriteRenderer).gameObject).name != "ChocooMenuBackground" && (double) ((Component) spriteRenderer).transform.position.z > 500.0)
          Object.Destroy((Object) ((Component) spriteRenderer).gameObject);
      }
    }
    catch (Exception ex)
    {
    }
  }
}
