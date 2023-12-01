using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSUI : MonoBehaviour
{
        [SerializeField] TextMeshProUGUI fpsText;
        float deltaTime;
        
        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + " fps";
        }
}
