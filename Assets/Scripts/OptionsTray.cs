using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsTray : MonoBehaviour
{

    public GameboardSetup gameboard;
    List<Token> tokenList;
    public int NumOptions { get { return tokenList.Count; } }

    [SerializeField]
    DropTray tray;
    void Start()
    {
        FindAllOptions();
        HideOptions();
        
        GameObject placeholder = FindChild("SelectedOptions");

        bool isActive = tokenList[0].gameObject.active;
        tokenList[0].gameObject.SetActive(true);
        Vector3 tokenSize = tokenList[0].GetComponent<SphereCollider>().bounds.size;
        tokenList[0].gameObject.SetActive(isActive);

        List<Vector3> positions = Utils.GetPositionDistribution(4, this.transform.position, placeholder.transform.position.y, this.GetComponent<MeshRenderer>().bounds.size.x, tokenSize.x);

        List<Token> tokens = SelectOptions(placeholder, positions);

        //List<Vector3> test1 = GetPositionDistribution(3);
        /*List<Vector3> test3 = GetPositionDistribution(5);
        List<Vector3> test4 = GetPositionDistribution(6);*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }  

    List<Token> SelectOptions(GameObject placeholder, List<Vector3> positions)
    {
        List<Token> tokensChosen = new List<Token>();
        int num = positions.Count;
        HashSet<int> previousChoices = new HashSet<int>();
        for(int i=0; i<num; i++)
        {
            int choice = UnityEngine.Random.Range(0, NumOptions);
            Token chosenToken = tokenList[choice];
            Vector3 position = positions[i];
            var go = Instantiate<Token>(chosenToken, position, chosenToken.transform.rotation);
            go.transform.parent = placeholder.transform;
            go.gameObject.SetActive(true);
            tokensChosen.Add(go);
        }
        return tokensChosen;
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
