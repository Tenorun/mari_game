using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class pushBarScript : MonoBehaviour
{
    public Image IndicatorBack;
    public Image Indicator;

    public void moveIndicator(float inputPower)
    {
        float moveMax = IndicatorBack.rectTransform.rect.width - Indicator.rectTransform.rect.width;
        Indicator.rectTransform.localPosition = new Vector2(inputPower * moveMax - (IndicatorBack.rectTransform.rect.width/2), 0);
    }
}
