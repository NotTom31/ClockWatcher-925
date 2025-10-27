using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// Minigame description: Several sortees spawn onscreen. They must be dragged into their respective sort areas.
// Player can click a submit button to finish this minigame early.
public class SortGameLogic : MinigameLogic
{
    // Expected prefabs list: BlueSortee, RedSortee

    [SerializeField] RectTransform spawnBounds; //area in which Sortees will spawn on initializing
    [SerializeField] RectTransform sorteeDragArea; //area in which you can drag the Sortees
    [SerializeField] RectTransform blueArea;
    [SerializeField] RectTransform redArea;
    [SerializeField] List<int> sorteesByDifficulty;
    List<RectTransform> blueSortees = new();
    List<RectTransform> redSortees = new();

    public override float EvaluateScore()
    {
        float goal = blueSortees.Count + redSortees.Count;
        if (goal == 0)
        {
            Debug.LogError("No sortees to sort");
            return 0f;
        }
        int sorted = 0;
        RectTransform rt = GetComponent<RectTransform>();
        foreach (RectTransform sortee in blueSortees)
            if (OverlapsRelativeToAncestor(blueArea, sortee, rt))
                sorted++;
        foreach (RectTransform sortee in redSortees)
            if (OverlapsRelativeToAncestor(redArea, sortee, rt))
                sorted++;
        return sorted / goal * 100f;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        int num;
        if (difficulty >= sorteesByDifficulty.Count)
        {
            Debug.Log("No specified sortee count for this difficulty; defaulting to 3");
            num = 3;
        }
        else
            num = sorteesByDifficulty[difficulty];

        // Spawn sortees
        RectTransform rt;
        for (int ii = 0; ii < num; ii++)
        {
            rt = (RectTransform)Instantiate(prefabs[0], sorteeDragArea).transform;
            rt.localPosition = RandomSpawnPosition();
            blueSortees.Add(rt);

            rt = (RectTransform)Instantiate(prefabs[1], sorteeDragArea).transform;
            rt.localPosition = RandomSpawnPosition();
            redSortees.Add(rt);
        }
    }

    // returns a random point in local space on the spawnBounds RectTransform
    private Vector2 RandomSpawnPosition()
    {
        Vector3[] boundCorners = new Vector3[4];
        spawnBounds.GetLocalCorners(boundCorners);
        return new Vector2(Random.Range(boundCorners[0].x, boundCorners[2].x), Random.Range(boundCorners[0].y, boundCorners[2].y));
    }

    public static bool OverlapsRelativeToAncestor(RectTransform rectA, RectTransform rectB, RectTransform commonAncestor)
    {
        if (rectA == null || rectB == null || commonAncestor == null)
            return false;

        // Get the 4 world corners for each rect
        Vector3[] worldCornersA = new Vector3[4];
        Vector3[] worldCornersB = new Vector3[4];
        rectA.GetWorldCorners(worldCornersA);
        rectB.GetWorldCorners(worldCornersB);

        // Convert world corners into ancestor local space
        Vector3[] localA = new Vector3[4];
        Vector3[] localB = new Vector3[4];

        for (int ii = 0; ii < 4; ii++)
        {
            localA[ii] = commonAncestor.InverseTransformPoint(worldCornersA[ii]);
            localB[ii] = commonAncestor.InverseTransformPoint(worldCornersB[ii]);
        }

        // Build Rects from local coordinates
        Rect rectAInAncestor = GetRectFromLocalCorners(localA);
        Rect rectBInAncestor = GetRectFromLocalCorners(localB);

        // Axis-aligned overlap check
        return rectAInAncestor.Overlaps(rectBInAncestor, true);
    }

    private static Rect GetRectFromLocalCorners(Vector3[] corners)
    {
        float xMin = corners[0].x;
        float xMax = corners[0].x;
        float yMin = corners[0].y;
        float yMax = corners[0].y;

        for (int ii = 1; ii < 4; ii++)
        {
            Vector3 c = corners[ii];
            if (c.x < xMin) xMin = c.x;
            if (c.x > xMax) xMax = c.x;
            if (c.y < yMin) yMin = c.y;
            if (c.y > yMax) yMax = c.y;
        }

        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }

    public void Submit()
    {
        float score = EvaluateScore();
        Debug.Log("SortGame score: " + score);
        if (score >= 100f)
            MinigamesManager.Instance.EndMinigame(this);
    }
}
