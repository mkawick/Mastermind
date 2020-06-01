using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsTray : MonoBehaviour
{

    public GameboardSetup gameboard;
    List<Token> tokenList;
    public int NumOptions { get { return tokenList.Count; } }
    void Start()
    {
        FindAllOptions();
        HideOptions();
        //List<Vector3> test1 = GetPositionDistribution(3);
        List<Vector3> positions = GetPositionDistribution(4);
        List<Token> tokens = SelectOptions(positions);
        /*List<Vector3> test3 = GetPositionDistribution(5);
        List<Vector3> test4 = GetPositionDistribution(6);*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }  

    List<Token> SelectOptions(List<Vector3> positions)
    {
        GameObject placeholder = FindChild("SelectedOptions");
        List<Token> tokensChosen = new List<Token>();
        int num = positions.Count;
        for(int i=0; i<num; i++)
        {
            int choice = UnityEngine.Random.Range(0, NumOptions);
            Token chosenToken = tokenList[choice];
            var go = Instantiate<Token>(chosenToken, positions[i], chosenToken.transform.rotation);
            //go.transform.parent = placeholder.transform;
            go.gameObject.SetActive(true);
            tokensChosen.Add(go);
        }
        return tokensChosen;
    }

    List <Vector3>GetPositionDistribution(int numPositions, bool fullWidthAcross = false)
    {
        GameObject placeholder = FindChild("SelectedOptions");
        Vector3 scale = this.GetComponent<MeshRenderer>().bounds.size;
        Vector3 center = this.transform.position;
        center.y = placeholder.transform.position.y;
        //center.y += 0.2f; // raise them above the parent

        Vector3 tokenSize = tokenList[0].GetComponent<SphereCollider>().bounds.size;
        float margin = tokenSize.x / 2 * 3; //150% of the normal width

        
        Vector3 startingPosition = center;
        float offsetXPerToken;
        if (fullWidthAcross)
        {
            float workingWidth = scale.x - margin;
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
                float halfPlusNum = (float) (numPositions-1) / 2.0f;
                startingPosition.x -= offsetXPerToken * halfPlusNum;
            }
            else
            {
                startingPosition.x -= offsetXPerToken * (numPositions / 2);
            }
        }

        List<Vector3> positions = new List<Vector3>();
        for (int i=0; i<numPositions; i++)
        {
            positions.Add(startingPosition);
            startingPosition.x += offsetXPerToken;
        }

        //NewChunkWasGenerated(chunk.transform, scale.x / 2, scale.z / 2);

        return positions;
    }

    void FindAllOptions()
    {
        GameObject tokenOptions = FindChild("TokensOptions");

        tokenList = new List<Token>();
        foreach (Transform child in tokenOptions.transform)
        {
            Token t = child.GetComponent<Token>();
            tokenList.Add(t);
        }
    }

    private void HideOptions()
    {
        foreach(var token in tokenList)
        {
            token.gameObject.SetActive(false);
        }
    }
    GameObject FindChild(string name)
    {
        foreach (Transform eachChild in transform)
        {
            if (eachChild.name == name)
            {
                return eachChild.gameObject;
            }
        }
        return null;
    }
}
