using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class Utils
{
    static public List<Vector3> GetPositionDistribution(int numPositions, Vector3 center, float yPosition, float trayWidth, float tokenWidth, bool fullWidthAcross = false)
    {
        center.y = yPosition;

        // we need to enable this for the SphereCollider to have a non-zero value
        float margin = tokenWidth * 2.4f; //150% of the normal width

        Vector3 startingPosition = center;
        float offsetXPerToken;
        if (fullWidthAcross)
        {
            float workingWidth = trayWidth - margin;
            offsetXPerToken = workingWidth / (float)(numPositions - 1);
            startingPosition.x -= workingWidth / 2;
        }
        else
        {
            offsetXPerToken = margin;
            int num = numPositions;
            if (num % 1 == 0)// is even
            {
                // we need to stradle the midpoint. In the case of 4,
                // we need to subtract the margin and 1/2 margin: (num-1)/2
                float halfPlusNum = (float)(numPositions - 1) / 2.0f;
                startingPosition.x -= offsetXPerToken * halfPlusNum;
            }
            else
            {
                startingPosition.x -= offsetXPerToken * (numPositions / 2);
            }
        }

        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < numPositions; i++)
        {
            positions.Add(startingPosition);
            startingPosition.x += offsetXPerToken;
        }

        return positions;
    }
    static public GameObject FindChild(GameObject parent, string name)
    {
        foreach (Transform eachChild in parent.transform)
        {
            if (eachChild.name == name)
            {
                return eachChild.gameObject;
            }
        }
        return null;
    }
}
