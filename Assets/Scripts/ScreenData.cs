using UnityEngine;

public class ScreenData
{
    public static Vector2 CalculateFrustrumSize()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        float height = edgeVector.y * 2;
        float width = edgeVector.x * 2;

        Vector2 result = new Vector2(width, height);
        return result;
    }

    public static Vector3 CalculateBlockScale()
    {
        Vector2 frustrumSize = CalculateFrustrumSize();

        int colsNum = 7;

        Vector2 textureBaseSizePixels = new Vector2(4, 4);

        float blockScaleX = frustrumSize.x / colsNum * textureBaseSizePixels.x;
        float blockScaleY = blockScaleX * 2f;

        Vector3 result = new Vector3(blockScaleX, blockScaleY, 1f);

        return result;
    }

    public static Vector2 CalculateBlockStep(int cols)
    {
        Vector2 frustrumSize = CalculateFrustrumSize();

        float stepX = frustrumSize.x / cols;
        float stepY = stepX * 2f;

        return new Vector2(stepX, stepY);
    }



}
