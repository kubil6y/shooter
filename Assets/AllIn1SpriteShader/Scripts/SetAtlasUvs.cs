using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AllIn1SpriteShader
{
    [ExecuteInEditMode]
    public class SetAtlasUvs : MonoBehaviour
    {
        [SerializeField] private bool updateEveryFrame = false;
        [Tooltip("If using a Sprite Renderer it will use the material property instead of sharedMaterial"), SerializeField] private bool useMaterialInstanceIfPossible = false;
        private Renderer render;
        private SpriteRenderer spriteRender;
        private Image uiImage;
        private bool isUI = false;
        private readonly int minXuv = Shader.PropertyToID("_MinXUV");
        private readonly int maxXuv = Shader.PropertyToID("_MaxXUV");
        private readonly int minYuv = Shader.PropertyToID("_MinYUV");
        private readonly int maxYuv = Shader.PropertyToID("_MaxYUV");

        private void Start()
        {
            Setup();
        }

        private void Reset()
        {
            Setup();
        }

        private void Setup()
        {
            if (GetRendererReferencesIfNeeded()) GetAndSetUVs();
            if (!updateEveryFrame && Application.isPlaying && this != null) this.enabled = false;
        }

        private void OnWillRenderObject()
        {
            if (updateEveryFrame)
            {
                GetAndSetUVs();
            }
        }

        public void GetAndSetUVs()
        {
            if (!GetRendererReferencesIfNeeded()) return;

            if (!isUI)
            {
                Sprite sprite = spriteRender.sprite;
                Rect r = sprite.textureRect;
                r.x /= sprite.texture.width;
                r.width /= sprite.texture.width;
                r.y /= sprite.texture.height;
                r.height /= sprite.texture.height;

                if(useMaterialInstanceIfPossible && Application.isPlaying)
                {
                    render.material.SetFloat(minXuv, r.xMin);
                    render.material.SetFloat(maxXuv, r.xMax);
                    render.material.SetFloat(minYuv, r.yMin);
                    render.material.SetFloat(maxYuv, r.yMax);   
                }
                else
                {
                    render.sharedMaterial.SetFloat(minXuv, r.xMin);
                    render.sharedMaterial.SetFloat(maxXuv, r.xMax);
                    render.sharedMaterial.SetFloat(minYuv, r.yMin);
                    render.sharedMaterial.SetFloat(maxYuv, r.yMax);   
                }
            }
            else
            {
                Rect r = uiImage.sprite.textureRect;
                r.x /= uiImage.sprite.texture.width;
                r.width /= uiImage.sprite.texture.width;
                r.y /= uiImage.sprite.texture.height;
                r.height /= uiImage.sprite.texture.height;

                uiImage.material.SetFloat(minXuv, r.xMin);
                uiImage.material.SetFloat(maxXuv, r.xMax);
                uiImage.material.SetFloat(minYuv, r.yMin);
                uiImage.material.SetFloat(maxYuv, r.yMax);
            }
        }

        public void ResetAtlasUvs()
        {
            if (!GetRendererReferencesIfNeeded()) return;

            if (!isUI)
            {
                if(useMaterialInstanceIfPossible && Application.isPlaying)
                {
                    render.material.SetFloat(minXuv, 0f);
                    render.material.SetFloat(maxXuv, 1f);
                    render.material.SetFloat(minYuv, 0f);
                    render.material.SetFloat(maxYuv, 1f);
                }
                else
                {
                    render.sharedMaterial.SetFloat(minXuv, 0f);
                    render.sharedMaterial.SetFloat(maxXuv, 1f);
                    render.sharedMaterial.SetFloat(minYuv, 0f);
                    render.sharedMaterial.SetFloat(maxYuv, 1f);   
                }
            }
            else
            {
                uiImage.material.SetFloat(minXuv, 0f);
                uiImage.material.SetFloat(maxXuv, 1f);
                uiImage.material.SetFloat(minYuv, 0f);
                uiImage.material.SetFloat(maxYuv, 1f);
            }
        }

        public void UpdateEveryFrame(bool everyFrame)
        {
            updateEveryFrame = everyFrame;
        }

        private bool GetRendererReferencesIfNeeded()
        {
            if (spriteRender == null) spriteRender = GetComponent<SpriteRenderer>();
            if (spriteRender != null)
            {
                if (spriteRender.sprite == null)
                {
                    #if UNITY_EDITOR
                    EditorUtility.DisplayDialog("No sprite found", "The object: " + gameObject.name + ",has Sprite Renderer but no sprite", "Ok");
                    #endif
                    DestroyImmediate(this);
                    return false;
                }
                if (render == null) render = GetComponent<Renderer>();
                isUI = false;
            }
            else
            {
                if (uiImage == null)
                {
                    uiImage = GetComponent<Image>();
                    if (uiImage != null)
                    {
                        #if UNITY_EDITOR
                        Debug.Log("You added the SetAtlasUv component to: " + gameObject.name + " that has a UI Image\n " +
                        "This SetAtlasUV component will only work properly on UI Images if each Image has a DIFFERENT material instance (See Documentation Sprite Atlases section for more info)");
                        #endif
                    }
                    else
                    {
                        #if UNITY_EDITOR
                        EditorUtility.DisplayDialog("No Renderer or UI Graphic found", "This SetAtlasUV component will now get destroyed", "Ok");
                        #endif
                        DestroyImmediate(this);
                        return false;
                    }
                }
                if (render == null) render = GetComponent<Renderer>();
                isUI = true;
            }

            if (spriteRender == null && uiImage == null)
            {
                #if UNITY_EDITOR
                EditorUtility.DisplayDialog("No Renderer or UI Graphic found", "This SetAtlasUV component will now get destroyed", "Ok");
                #endif
                DestroyImmediate(this);
                return false;
            }
            return true;
        }
    }
}