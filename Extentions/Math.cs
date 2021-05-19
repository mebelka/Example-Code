using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Math functions not found in Mathf
public static class Math
{

    public static bool Between(this int value, int low, int high) => value >= low && value <= high;

    /// Rounds number to the closest rounded fraction. If rounding is off it will round down to the smallest absolute number.
    public static float RoundNumber(float fraction, float number, bool round = true)
    {
        int numberOfFractions = (int)(number / fraction);
        float remainderOfFraction = number - (numberOfFractions * fraction);

        if (Mathf.Abs(remainderOfFraction) >= (fraction / 2f) && round)
        {
            numberOfFractions = remainderOfFraction > 0 ? numberOfFractions + 1 : numberOfFractions - 1;
        }
        return numberOfFractions * fraction;
    }

    /// Returns the integer that is the closest squared number to the float input, sends warning if float is negitive.
    public static int ClosestSquare(float number, float roundAt = 0)
    {
        if (number < 0)
        {
            Debug.LogWarning("Input negitive number to be rooted.");
            return -1;
        }

        float squareRoot = Mathf.Sqrt(number);
        // Round the square root
        int closestSquare = (squareRoot - (int)squareRoot) > roundAt ? (int)squareRoot + 1 : (int)squareRoot;
        return closestSquare;
    }

    /// Takes a max and min value, and a max and min range. Then given the current ratio, returns the correct value.
    public static float ScaleGivenRatio(float maxValue, float minValue, float maxRatio, float minRatio, float currentRatio)
    {
        if (currentRatio >= maxRatio)
        {
            return maxValue;
        }
        else if (currentRatio <= minRatio)
        {
            return minValue;
        }
        else
        {
            float differenceBetweenRatios = maxRatio - minRatio;
            float percentBetweenValues = (currentRatio - minRatio) / differenceBetweenRatios;
            return PercentValueRange(minValue, maxValue, percentBetweenValues);
        }
    }

    /// Uses linear algerbra to finds the projected distance from a different vector.
    public static float DistanceFromLine(Vector2 point, Vector2 linePoint, Vector2 goal)
    {
        Vector2 unitVector = (goal - linePoint).normalized;
        Vector2 vectorFromPointToLinePoint = linePoint - point;
        float projectedLength = Vector2.Dot(vectorFromPointToLinePoint, unitVector);
        Vector2 lengthWithOrginalVector = projectedLength * unitVector;
        Vector2 projectedVector = vectorFromPointToLinePoint - lengthWithOrginalVector;

        return projectedVector.magnitude;
    }

    /// Takes a given percent and returns the value between the start and end point at that percent.
    public static float PercentValueRange(float start, float end, float percent)
    {
        float percentValue = percent;
        bool invalidPercent = percent > 1f || percent < 0f;

        if (invalidPercent)
        {
            percentValue = percent > 1f ? 1f : 0f;
        }

        return percentValue * (end - start) + start;
    }
}