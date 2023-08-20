using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defaultWidth;

    private void OnValidate()
    {
        _defaultWidth = _filledImage.sizeDelta.x;
    }

    public void UpdateHealth(float max, int current)
    {
        float bar_percent = current/max; // То насколько заполнена картинка
        _filledImage.sizeDelta = new Vector2(_defaultWidth * bar_percent, _filledImage.sizeDelta.y);
    }
}
