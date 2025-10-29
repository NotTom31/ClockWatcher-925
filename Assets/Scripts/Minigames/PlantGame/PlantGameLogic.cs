using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantGameLogic : MinigameLogic
{
    [SerializeField] List<Sprite> underwateredSprites;
    [SerializeField] List<Sprite> plantSprites;
    [SerializeField] List<Sprite> overwateredSprites;
    List<float> growthThresholds;
    int growthTier;
    float growthValue; // between 0 and 100. Trying to reach 100 before end of day
    [SerializeField] float growthCoefficient;

    float waterValue; // between 0 and 100. 50 is ideal
    [SerializeField] float waterGainRate;
    [SerializeField] float waterDepleteRate;
    bool watering;

    [SerializeField] float feedbackDelay;
    int realFeedbackTier = 1;
    int delayedFeedbackTier = 1;

    [SerializeField] Image plant;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] List<Color> feedbackColors;
    [SerializeField] List<string> feedbackMessages;

    private void Update()
    {
        UpdateWaterValue();
        UpdateGrowthValue();
        //Debug.Log("water value: " + waterValue + ", realFeedbackTier: " + realFeedbackTier + ", delayedFeedbackTier " +
        //    delayedFeedbackTier + ", growth value: " + growthValue + ", growthTier: " + growthTier);
    }

    private void UpdateWaterValue()
    {
        if (watering)
            waterValue += waterGainRate * Time.deltaTime;
        else
            waterValue -= waterDepleteRate * Time.deltaTime;
        if (waterValue < 0)
            waterValue = 0;
        else if (waterValue > 100)
            waterValue = 100;
        int newTier = 0;
        if (waterValue > 33.3f)
            newTier = 1;
        if (waterValue > 66.6f)
            newTier = 2;
        if (newTier != realFeedbackTier)
        {
            StartCoroutine(DelayedFeedback(newTier));
            realFeedbackTier = newTier;
        }
    }

    private IEnumerator DelayedFeedback(int newTier)
    {
        yield return new WaitForSeconds(feedbackDelay);
        delayedFeedbackTier = newTier;
        feedbackText.text = feedbackMessages[newTier];
        UpdateTreeSprite();
    }

    private void UpdateGrowthValue()
    {
        float growthRate = ((-Mathf.Abs(waterValue - 50f) / 50f) + 1f) * growthCoefficient;
        growthValue += growthRate * Time.deltaTime;
        if (growthTier < plantSprites.Count - 1 && growthValue > growthThresholds[growthTier + 1])
        {
            growthTier++;
            UpdateTreeSprite();
        }
        if (growthValue >= 100f)
        {
            growthValue = 100f;
            feedbackText.text = feedbackMessages[3];
        }
    }

    private void UpdateTreeSprite()
    {
        Sprite s;
        switch (delayedFeedbackTier)
        {
            case 0:
                //s = underwateredSprites[growthTier];
                s = plantSprites[growthTier];
                break;
            case 1:
                s = plantSprites[growthTier];
                break;
            case 2:
            default:
                //s = overwateredSprites[growthTier];
                s = plantSprites[growthTier];
                break;
        }
        plant.sprite = s;
        plant.color = feedbackColors[delayedFeedbackTier];
        
        // TODO: replace color changes with sprite changes
    }

    public override float EvaluateScore()
    {
        return growthValue;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        growthThresholds = new List<float>();
        float interval = 100f / plantSprites.Count;
        for (int ii = 0; ii < plantSprites.Count; ii++)
        {
            growthThresholds.Add(ii * interval);
        }
        waterValue = 50f;
        growthValue = 0f;
        UpdateTreeSprite();
    }

    public void SetWatering(bool b)
    {
        watering = b;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
