using System.IO;
using UnityEngine;

/*
    The internal system configs of moji talk
*/

class ChattingConfig
{
    public static float[] namePadding = {10f, 5f}; // width, height
    public static float[] contentPadding = {15f, 7.5f}; // width, height

    public static float[,] gradientSettings;
    public static byte[] backgroundColorRGB = {11, 150, 198, 255};

    public ChattingConfig()
    {
        gradientSettings = new float[,] {{0f, 0f}, {0.4f, 0.4f}, {1f, 1f}}; // time, alpha
    }
}

public class EmojiGeneratingConfig
{
    public static float GenCycle = 3f;
    public static float GenCycleOffset = 3f;
    public static float GenPositionOffset = 3f;
    public static int GenMaximunEmojis = 4;
    public static float[] GenMaximumEmojisRatio = {0, 1, 2, 3, 1};
}
