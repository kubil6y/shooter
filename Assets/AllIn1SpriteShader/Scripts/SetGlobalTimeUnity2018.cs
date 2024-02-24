using UnityEngine;

namespace AllIn1SpriteShader
{
    //This script is made with Unity version 2018 and previous ones in mind
    //If you are in Unity 2019 or onward you probably want to use SetGlobalTimeNew.cs instead
    
    //This script will pass in the Scaled Time to the shader so the effects stop being animated when the game is paused
    //Set shaders to Scaled Time variant and add this script to an active GameObject to see the results
    //Video tutorial about it: https://youtu.be/7_BggIufV-w
    [ExecuteInEditMode]
    public class SetGlobalTimeUnity2018 : MonoBehaviour
    {
        int globalTime;

        void Start()
        {
            globalTime = Shader.PropertyToID("globalUnscaledTime");
        }

        void Update()
        {
            Shader.SetGlobalFloat(globalTime, Time.time / 20f);
        }
    }
}