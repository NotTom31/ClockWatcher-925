using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Part of the ColorChangeGame. This button's color slowly degrades but can be restored by clicking.
public class ColorButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ColorChangeGameLogic logic;
    [SerializeField] Color goodColor;
    [SerializeField] Color badColor;
    public float changeRate { get; set; }   // colorValue change per second. This should be negative
    private float colorValue;               // must be between 1 and 0. 1 is the good color, 0 is the bad color
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        ResetColor();
    }

    // Each frame, the color degrades at the specified rate.
    void Update()
    {
        UpdateValue();
        UpdateColor();
    }

    private void UpdateValue()
    {
        if (colorValue == 0f)
            return;

        colorValue += changeRate * Time.deltaTime;
        if (colorValue > 1f)
            colorValue = 1f;
        else if (colorValue < 0f)
            colorValue = 0f;

        bool scoreDraining = colorValue == 0f;
        logic.scoreDraining = scoreDraining;
    }

    private void UpdateColor()
    {
        image.color = Color.Lerp(badColor, goodColor, colorValue);
    }

    public void ResetColor()
    {
        colorValue = 1f;
        UpdateColor();
        logic.scoreDraining = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetColor();
    }
}
