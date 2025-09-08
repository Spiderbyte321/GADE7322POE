using UnityEngine;

public static class MathUtilities
{
    public static float RoundToTwoDecimalPLaces(float AFloat)
    {
        float SecondsToWait = AFloat/ 60;
        float RoundedSeconds = Mathf.Round(SecondsToWait * 100);
        return RoundedSeconds /=100;
    }
}
