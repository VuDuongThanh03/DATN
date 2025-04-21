using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFitScreen : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;

    private float _ratioScreen;

    private void Awake()
    {
        CalculateSize();
    }

    private void CalculateSize()
    {
        _ratioScreen = (float)Screen.width / (float)Screen.height;
        _canvasScaler.referenceResolution = new Vector2(_ratioScreen * _canvasScaler.referenceResolution.y, _canvasScaler.referenceResolution.y);
    }
}
