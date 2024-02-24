using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
    public class RandomSeed : MonoBehaviour
    {
        private readonly int randomSeedProperty = Shader.PropertyToID("_RandomSeed");
        private MaterialPropertyBlock propertyBlock;

        //If you want to randomize UI Images, you'll need to create different materials since materials are always shared
        //This can be done at runtime with scripting or manually in the editor
        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            if(renderer != null)
            {
                propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetFloat(randomSeedProperty, Random.Range(0f, 100f));
                renderer.SetPropertyBlock(propertyBlock);
            }
            else
            {
                Image image = GetComponent<Image>();
                if (image != null)
                {
                    if (image.material != null)
                    {
                        image.material.SetFloat(randomSeedProperty, Random.Range(0, 1000f));
                    }
                    else Debug.LogError("Missing Material on UI Image: " + gameObject.name);
                }
                else Debug.LogError("Missing Renderer or UI Image on: " + gameObject.name);
            }
        }
    }
}