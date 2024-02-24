#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

[CanEditMultipleObjects]
public class AllIn1SpriteShaderMaterialInspector : ShaderGUI
{
    private Material targetMat;
    private BlendMode srcMode, dstMode;
    private CompareFunction zTestMode = CompareFunction.LessEqual;
    private CullMode cullMode;

    private GUIStyle propertiesStyle, bigLabelStyle, smallLabelStyle, toggleButtonStyle;
    private const int bigFontSize = 16, smallFontSize = 11;
    private string[] oldKeyWords;
    private int effectCount = 1;
    private Material originalMaterialCopy;
    private MaterialEditor matEditor;
    private MaterialProperty[] matProperties;
    private uint[] materialDrawers = new uint[] { 1, 2, 4 };
    bool[] currEnabledDrawers;
    private const uint advancedConfigDrawer = 0;
    private const uint colorFxShapeDrawer = 1;
    private const uint uvFxShapeDrawer = 2;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        matEditor = materialEditor;
        matProperties = properties;
        targetMat = materialEditor.target as Material;
        effectCount = 1;
        oldKeyWords = targetMat.shaderKeywords;
        propertiesStyle = new GUIStyle(EditorStyles.helpBox);
        propertiesStyle.margin = new RectOffset(0, 0, 0, 0);
        bigLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        bigLabelStyle.fontSize = bigFontSize;
        smallLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        smallLabelStyle.fontSize = smallFontSize;
        toggleButtonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, richText = true };
        currEnabledDrawers = new bool[materialDrawers.Length];
        uint iniDrawers = (uint)ShaderGUI.FindProperty("_EditorDrawers", matProperties).floatValue;
        for(int i = 0; i < materialDrawers.Length; i++) currEnabledDrawers[i] = (materialDrawers[i] & iniDrawers) > 0;

        GUILayout.Label("General Properties", bigLabelStyle);
        DrawProperty(0);
        DrawProperty(1);
        DrawProperty(2);
        
        
        currEnabledDrawers[advancedConfigDrawer] = GUILayout.Toggle(currEnabledDrawers[advancedConfigDrawer], new GUIContent("<size=12>Show Advanced Configuration</size>"), toggleButtonStyle);
        if(currEnabledDrawers[advancedConfigDrawer])
        {
            
            EditorGUILayout.BeginVertical(propertiesStyle);
            Blending();
            DrawLine(Color.grey, 1, 3);
            Culling();
            DrawLine(Color.grey, 1, 3);
            ZTest();
            DrawLine(Color.grey, 1, 3);
            ZWrite();
            DrawLine(Color.grey, 1, 3);
            GenericEffect("Unity Fog", "FOG_ON", -1, -1, false, boldToggleLetters: false);
            DrawLine(Color.grey, 1, 3);
            Billboard("Bilboard active", "BILBOARD_ON");
            DrawLine(Color.grey, 1, 3);
            SpriteAtlas("Sprite inside an atlas?", "ATLAS_ON");
            DrawLine(Color.grey, 1, 3);
            materialEditor.EnableInstancingField();
            DrawLine(Color.grey, 1, 3);
            materialEditor.RenderQueueField();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Separator();
        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("Color Effects", bigLabelStyle);

        currEnabledDrawers[colorFxShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[colorFxShapeDrawer], new GUIContent("Show Color Effects"), toggleButtonStyle);
        if(currEnabledDrawers[colorFxShapeDrawer])
        {
            Glow("Glow", "GLOW_ON");
            GenericEffect("Fade", "FADE_ON", 7, 13);
            Outline("Outline", "OUTBASE_ON");
            GenericEffect("Alpha Outline", "ALPHAOUTLINE_ON", 26, 30, true, "A more performant but less flexible outline");
            InnerOutline("Inner Outline", "INNEROUTLINE_ON", 66, 69);
            Gradient("Gradient & Radial Gradient", "GRADIENT_ON");
            GenericEffect("Color Swap", "COLORSWAP_ON", 36, 42, true, "You will need a mask texture (see Documentation)", new int[] { 154 });
            GenericEffect("Hue Shift", "HSV_ON", 43, 45);
            ColorChange("Change 1 Color", "CHANGECOLOR_ON");
            ColorRamp("Color Ramp", "COLORRAMP_ON");
            GenericEffect("Hit Effect", "HITEFFECT_ON", 46, 48);
            GenericEffect("Negative", "NEGATIVE_ON", 49, 49);
            GenericEffect("Pixelate", "PIXELATE_ON", 50, 50, true, "Looks bad with distorition effects");
            GreyScale("GreyScale", "GREYSCALE_ON");
            Posterize("Posterize", "POSTERIZE_ON");
            Blur("Blur", "BLUR_ON");
            GenericEffect("Motion Blur", "MOTIONBLUR_ON", 62, 63);
            GenericEffect("Ghost", "GHOST_ON", 64, 65, true, "This effect will not affect the outline", new int[] { 157 });
            GenericEffect("Hologram", "HOLOGRAM_ON", 73, 77, true, null, new int[] { 140, 158 });
            GenericEffect("Chromatic Aberration", "CHROMABERR_ON", 78, 79);
            GenericEffect("Glitch", "GLITCH_ON", 80, 80, true, null, new int[] { 139 });
            GenericEffect("Flicker", "FLICKER_ON", 81, 83);
            GenericEffect("Shadow", "SHADOW_ON", 84, 87);
            GenericEffect("Shine", "SHINE_ON", 133, 138);
            GenericEffect("Contrast & Brightness", "CONTRAST_ON", 152, 153);
            Overlay("Overlay Texture", "OVERLAY_ON");
            GenericEffect("Alpha Cutoff", "ALPHACUTOFF_ON", 70, 70);
            GenericEffect("Alpha Round", "ALPHAROUND_ON", 144, 144);
        }

        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("UV Effects", bigLabelStyle);

        currEnabledDrawers[uvFxShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[uvFxShapeDrawer], new GUIContent("Show Alpha Effects"), toggleButtonStyle);
        if(currEnabledDrawers[uvFxShapeDrawer])
        {
            GenericEffect("Hand Drawn", "DOODLE_ON", 88, 89);
            Grass("Grass Movement / Wind", "WIND_ON");
            GenericEffect("Wave", "WAVEUV_ON", 94, 98);
            GenericEffect("Round Wave", "ROUNDWAVEUV_ON", 127, 128);
            GenericEffect("Rect Size (Enable wireframe to see result)", "RECTSIZE_ON", 99, 99, true, "Only on single sprites spritesheets NOT supported");
            GenericEffect("Offset", "OFFSETUV_ON", 100, 101);
            GenericEffect("Clipping / Fill Amount", "CLIPPING_ON", 102, 105);
            GenericEffect("Texture Scroll", "TEXTURESCROLL_ON", 106, 107, true, "Set Texture Wrap Mode to Repeat");
            GenericEffect("Zoom", "ZOOMUV_ON", 108, 108);
            GenericEffect("Distortion", "DISTORT_ON", 109, 112);
            GenericEffect("Twist", "TWISTUV_ON", 113, 116);
            GenericEffect("Rotate", "ROTATEUV_ON", 117, 117, true, "_Tip_ Use Clipping effect to avoid possible undesired parts");
            GenericEffect("Polar Coordinates (Tile texture for good results)", "POLARUV_ON", -1, -1);
            GenericEffect("Fish Eye", "FISHEYE_ON", 118, 118);
            GenericEffect("Pinch", "PINCH_ON", 119, 119);
            GenericEffect("Shake", "SHAKEUV_ON", 120, 122);
        }

        SetAndSaveEnabledDrawers(iniDrawers);
    }
    
    private void SetAndSaveEnabledDrawers(uint iniDrawers)
    {
        uint currDrawers = 0;
        for(int i = 0; i < currEnabledDrawers.Length; i++)
        {
            if(currEnabledDrawers[i]) currDrawers |= materialDrawers[i];
        }

        if(iniDrawers != currDrawers) ShaderGUI.FindProperty("_EditorDrawers", matProperties).floatValue = currDrawers;
    }

    private void Blending()
    {
        MaterialProperty srcM = ShaderGUI.FindProperty("_MySrcMode", matProperties);
        MaterialProperty dstM = ShaderGUI.FindProperty("_MyDstMode", matProperties);
        if(srcM.floatValue == 0 && dstM.floatValue == 0)
        {
            srcM.floatValue = 5;
            dstM.floatValue = 10;
        }

        GUILayout.Label("Look for 'ShaderLab: Blending' if you don't know what this is", smallLabelStyle);
        if(GUILayout.Button("Back To Default Blending"))
        {
            srcM.floatValue = 5;
            dstM.floatValue = 10;
            targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
        }

        srcMode = (BlendMode)srcM.floatValue;
        dstMode = (BlendMode)dstM.floatValue;
        srcMode = (BlendMode)EditorGUILayout.EnumPopup("SrcMode", srcMode);
        dstMode = (BlendMode)EditorGUILayout.EnumPopup("DstMode", dstMode);
        srcM.floatValue = (float)(srcMode);
        dstM.floatValue = (float)(dstMode);

        bool ini = oldKeyWords.Contains("PREMULTIPLYALPHA_ON");
        bool toggle = EditorGUILayout.Toggle("Premultiply Alpha?", ini);
        if(ini != toggle) Save();
        if(toggle) targetMat.EnableKeyword("PREMULTIPLYALPHA_ON");
        else targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
    }

    private void Billboard(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = inspector;
        toggle = GUILayout.Toggle(toggle, effectNameLabel);

        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);
            GUILayout.Label("Don't use this feature on UI elements!", smallLabelStyle);
            DrawProperty(129, true);
            MaterialProperty billboardY = matProperties[129];
            if(billboardY.floatValue == 1) targetMat.EnableKeyword("BILBOARDY_ON");
            else targetMat.DisableKeyword("BILBOARDY_ON");
        }
        else targetMat.DisableKeyword(keyword);
    }

    private void ZWrite()
    {
        MaterialProperty zWrite = ShaderGUI.FindProperty("_ZWrite", matProperties);
        bool toggle = zWrite.floatValue > 0.9f ? true : false;
        EditorGUILayout.BeginHorizontal();
        {
            float tempValue = zWrite.floatValue;
            toggle = GUILayout.Toggle(toggle, new GUIContent("Enable Z Write"));
            if(toggle) zWrite.floatValue = 1.0f;
            else zWrite.floatValue = 0.0f;
            if(tempValue != zWrite.floatValue && !Application.isPlaying) Save();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ZTest()
    {
        MaterialProperty zTestM = ShaderGUI.FindProperty("_ZTestMode", matProperties);
        float tempValue = zTestM.floatValue;
        zTestMode = (UnityEngine.Rendering.CompareFunction)zTestM.floatValue;
        zTestMode = (UnityEngine.Rendering.CompareFunction)EditorGUILayout.EnumPopup("Z TestMode", zTestMode);
        zTestM.floatValue = (float)(zTestMode);
        if(tempValue != zTestM.floatValue && !Application.isPlaying) Save();
    }

    private void Culling()
    {
        MaterialProperty cullO = ShaderGUI.FindProperty("_CullingOption", matProperties);;
        float tempValue = cullO.floatValue;
        cullMode = (UnityEngine.Rendering.CullMode)cullO.floatValue;
        cullMode = (UnityEngine.Rendering.CullMode)EditorGUILayout.EnumPopup("Culling Mode", cullMode);
        cullO.floatValue = (float)(cullMode);
        if(tempValue != cullO.floatValue && !Application.isPlaying) Save();
    }

    private void SpriteAtlas(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = GUILayout.Toggle(toggle, inspector);
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("Make sure SpriteAtlasUV component is added \n " +
                                "*Check documentation if unsure what this does or how it works", smallLabelStyle);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);
    }

    private void Outline(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + ".Outline";
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("OUTBASE_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(14);
                DrawProperty(15);
                DrawProperty(16);
                DrawEffectSubKeywordToggle("Outline High Resolution?", "OUTBASE8DIR_ON");

                DrawLine(Color.grey, 1, 3);
                bool outlinePixelPerf = DrawEffectSubKeywordToggle("Outline is Pixel Perfect?", "OUTBASEPIXELPERF_ON");
                if(outlinePixelPerf) DrawProperty(18);
                else DrawProperty(17);

                DrawLine(Color.grey, 1, 3);
                bool outlineTexture = DrawEffectSubKeywordToggle("Outline uses texture?", "OUTTEX_ON");
                if(outlineTexture)
                {
                    DrawProperty(19);
                    DrawProperty(20);
                    DrawProperty(21);
                }

                DrawLine(Color.grey, 1, 3);
                bool outlineDistort = DrawEffectSubKeywordToggle("Outline uses distortion?", "OUTDIST_ON");
                if(outlineDistort)
                {
                    DrawProperty(22);
                    DrawProperty(23);
                    DrawProperty(24);
                    DrawProperty(25);
                }

                DrawLine(Color.grey, 1, 3);
                DrawEffectSubKeywordToggle("Only render outline?", "ONLYOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("OUTBASE_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void GenericEffect(string inspector, string keyword, int first, int last, bool effectCounter = true, string preMessage = null, int[] extraProperties = null, bool boldToggleLetters = true)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        if(effectCounter)
        {
            effectNameLabel.text = effectCount + "." + inspector;
            effectCount++;
        }
        else effectNameLabel.text = inspector;
        if(boldToggleLetters) toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);
        else toggle = GUILayout.Toggle(toggle, effectNameLabel);

        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);
            if(first > 0)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);
                {
                    if(preMessage != null) GUILayout.Label(preMessage, smallLabelStyle);
                    for(int i = first; i <= last; i++) DrawProperty(i);
                    if(extraProperties != null)
                        foreach(int i in extraProperties)
                            DrawProperty(i);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(keyword);

        if(boldToggleLetters) EditorGUILayout.EndToggleGroup();
    }

    private void Glow(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("GLOW_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                bool useGlowTex = DrawEffectSubKeywordToggle("Use Glow Texture?", "GLOWTEX_ON");
                if(useGlowTex) DrawProperty(6);

                DrawProperty(3);
                DrawProperty(4);
                DrawProperty(5, true);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("GLOW_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void ColorRamp(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("COLORRAMP_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                bool useEditableGradient = false;
                if(AssetDatabase.Contains(targetMat))
                {
                    useEditableGradient = oldKeyWords.Contains("GRADIENTCOLORRAMP_ON");
                    bool gradientTex = useEditableGradient;
                    gradientTex = GUILayout.Toggle(gradientTex, new GUIContent("Use Editable Gradient?"));
                    if(useEditableGradient != gradientTex)
                    {
                        Save();
                        if(gradientTex)
                        {
                            useEditableGradient = true;
                            targetMat.EnableKeyword("GRADIENTCOLORRAMP_ON");
                        }
                        else targetMat.DisableKeyword("GRADIENTCOLORRAMP_ON");
                    }

                    if(useEditableGradient) matEditor.ShaderProperty(matProperties[159], matProperties[159].displayName);
                }
                else GUILayout.Label("*Save to folder to allow for dynamic Gradient property", smallLabelStyle);

                if(!useEditableGradient) DrawProperty(51);

                DrawProperty(52);
                DrawProperty(53, true);
                MaterialProperty colorRampOut = matProperties[53];
                if(colorRampOut.floatValue == 1) targetMat.EnableKeyword("COLORRAMPOUTLINE_ON");
                else targetMat.DisableKeyword("COLORRAMPOUTLINE_ON");
                DrawProperty(155);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("COLORRAMP_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void ColorChange(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("CHANGECOLOR_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                for(int i = 123; i < 127; i++) DrawProperty(i);

                DrawLine(Color.grey, 1, 3);
                ini = oldKeyWords.Contains("CHANGECOLOR2_ON");
                bool toggle2 = ini;
                toggle2 = EditorGUILayout.Toggle("Use Color 2", ini);
                if(ini != toggle2) Save();
                if(toggle2)
                {
                    targetMat.EnableKeyword("CHANGECOLOR2_ON");
                    for(int i = 146; i < 149; i++) DrawProperty(i);
                }
                else targetMat.DisableKeyword("CHANGECOLOR2_ON");

                DrawLine(Color.grey, 1, 3);
                ini = oldKeyWords.Contains("CHANGECOLOR3_ON");
                toggle2 = ini;
                toggle2 = EditorGUILayout.Toggle("Use Color 3", toggle2);
                if(ini != toggle2) Save();
                if(toggle2)
                {
                    targetMat.EnableKeyword("CHANGECOLOR3_ON");
                    for(int i = 149; i < 152; i++) DrawProperty(i);
                }
                else targetMat.DisableKeyword("CHANGECOLOR3_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("CHANGECOLOR_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void GreyScale(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("GREYSCALE_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(54);
                DrawProperty(56);
                DrawProperty(55, true);
                MaterialProperty greyScaleOut = matProperties[55];
                if(greyScaleOut.floatValue == 1) targetMat.EnableKeyword("GREYSCALEOUTLINE_ON");
                else targetMat.DisableKeyword("GREYSCALEOUTLINE_ON");
                DrawProperty(156);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("GREYSCALE_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void Posterize(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("POSTERIZE_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(57);
                DrawProperty(58);
                DrawProperty(59, true);
                MaterialProperty posterizeOut = matProperties[59];
                if(posterizeOut.floatValue == 1) targetMat.EnableKeyword("POSTERIZEOUTLINE_ON");
                else targetMat.DisableKeyword("POSTERIZEOUTLINE_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("POSTERIZE_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void Blur(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("BLUR_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("This effect will not affect the outline", smallLabelStyle);
                DrawProperty(60);
                DrawProperty(61, true);
                MaterialProperty blurIsHd = matProperties[61];
                if(blurIsHd.floatValue == 1) targetMat.EnableKeyword("BLURISHD_ON");
                else targetMat.DisableKeyword("BLURISHD_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("BLUR_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void Grass(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword("WIND_ON");
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(90);
                DrawProperty(91);
                DrawProperty(145);
                DrawProperty(92);
                DrawProperty(93, true);
                MaterialProperty grassManual = matProperties[92];
                if(grassManual.floatValue == 1) targetMat.EnableKeyword("MANUALWIND_ON");
                else targetMat.DisableKeyword("MANUALWIND_ON");
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword("WIND_ON");

        EditorGUILayout.EndToggleGroup();
    }

    private void InnerOutline(string inspector, string keyword, int first, int last)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);
            if(first > 0)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);
                {
                    for(int i = first; i <= last; i++) DrawProperty(i);

                    EditorGUILayout.Separator();
                    DrawProperty(72, true);
                    MaterialProperty onlyInOutline = matProperties[72];
                    if(onlyInOutline.floatValue == 1) targetMat.EnableKeyword("ONLYINNEROUTLINE_ON");
                    else targetMat.DisableKeyword("ONLYINNEROUTLINE_ON");
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(keyword);

        EditorGUILayout.EndToggleGroup();
    }

    private void Gradient(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);

            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(143, true);
                MaterialProperty gradIsRadial = matProperties[143];
                if(gradIsRadial.floatValue == 1)
                {
                    targetMat.EnableKeyword("RADIALGRADIENT_ON");
                    DrawProperty(31);
                    DrawProperty(32);
                    DrawProperty(34);
                    DrawProperty(141);
                }
                else
                {
                    targetMat.DisableKeyword("RADIALGRADIENT_ON");
                    bool simpleGradient = oldKeyWords.Contains("GRADIENT2COL_ON");
                    bool simpleGradToggle = EditorGUILayout.Toggle("2 Color Gradient?", simpleGradient);
                    if(simpleGradient && !simpleGradToggle) targetMat.DisableKeyword("GRADIENT2COL_ON");
                    else if(!simpleGradient && simpleGradToggle) targetMat.EnableKeyword("GRADIENT2COL_ON");
                    DrawProperty(31);
                    DrawProperty(32);
                    if(!simpleGradToggle) DrawProperty(33);
                    DrawProperty(34);
                    if(!simpleGradToggle) DrawProperty(35);
                    if(!simpleGradToggle) DrawProperty(141);
                    DrawProperty(142);
                }
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);

        EditorGUILayout.EndToggleGroup();
    }

    private void Overlay(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);

        effectCount++;
        if(ini != toggle) Save();
        if(toggle)
        {
            targetMat.EnableKeyword(keyword);
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                bool multModeOn = oldKeyWords.Contains("OVERLAYMULT_ON");
                bool isMultMode = multModeOn;
                isMultMode = GUILayout.Toggle(isMultMode, new GUIContent("Is overlay multiplicative?"));
                if(multModeOn != isMultMode)
                {
                    Save();
                    if(isMultMode)
                    {
                        multModeOn = true;
                        targetMat.EnableKeyword("OVERLAYMULT_ON");
                    }
                    else targetMat.DisableKeyword("OVERLAYMULT_ON");
                }

                if(multModeOn) GUILayout.Label("Overlay is set to multiplicative mode", smallLabelStyle);
                else GUILayout.Label("Overlay is set to additive mode", smallLabelStyle);

                for(int i = 160; i <= 163; i++) DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }
        else targetMat.DisableKeyword(keyword);

        EditorGUILayout.EndToggleGroup();
    }

    private void DrawProperty(int index, bool noReset = false)
    {
        MaterialProperty targetProperty = matProperties[index];

        EditorGUILayout.BeginHorizontal();
        {
            GUIContent propertyLabel = new GUIContent();
            propertyLabel.text = targetProperty.displayName;
            propertyLabel.tooltip = targetProperty.name + " (C#)";

            matEditor.ShaderProperty(targetProperty, propertyLabel);

            if(!noReset)
            {
                GUIContent resetButtonLabel = new GUIContent();
                resetButtonLabel.text = "R";
                resetButtonLabel.tooltip = "Resets to default value";
                if(GUILayout.Button(resetButtonLabel, GUILayout.Width(20))) ResetProperty(targetProperty);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ResetProperty(MaterialProperty targetProperty)
    {
        if(originalMaterialCopy == null) originalMaterialCopy = new Material(targetMat.shader);
        if(targetProperty.type == MaterialProperty.PropType.Float || targetProperty.type == MaterialProperty.PropType.Range)
        {
            targetProperty.floatValue = originalMaterialCopy.GetFloat(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Vector)
        {
            targetProperty.vectorValue = originalMaterialCopy.GetVector(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Color)
        {
            targetProperty.colorValue = originalMaterialCopy.GetColor(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Texture)
        {
            targetProperty.textureValue = originalMaterialCopy.GetTexture(targetProperty.name);
        }
    }

    private bool DrawEffectSubKeywordToggle(string inspector, string keyword, bool setCustomConfigAfter = false)
    {
        GUIContent propertyLabel = new GUIContent();
        propertyLabel.text = inspector;
        propertyLabel.tooltip = keyword + " (C#)";

        bool ini = oldKeyWords.Contains(keyword);
        bool toggle = ini;
        toggle = GUILayout.Toggle(toggle, propertyLabel);
        if(ini != toggle)
        {
            if(toggle) targetMat.EnableKeyword(keyword);
            else targetMat.DisableKeyword(keyword);
        }

        return toggle;
    }

    private void Save()
    {
        if(!Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.SetDirty(targetMat);
    }

    private void DrawLine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += (padding / 2);
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
#endif