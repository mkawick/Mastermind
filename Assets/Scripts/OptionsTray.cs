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
        GameObject tokenOptions = FindChild("TokensOptions");

        tokenList = new List<Token>();
        foreach (Transform child in tokenOptions.transform)
        {
            Token t = child.GetComponent<Token>();
            tokenList.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
