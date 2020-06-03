using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OptionsTray : MonoBehaviour
{

    public GameboardSetup gameboard;
    List<Token> referenceTokenList;
    List<Token> selectedTokenList;
    public int NumOptions { get { return referenceTokenList.Count; } }
    public int NumChosen { get { return selectedTokenList.Count; } }

    [SerializeField]
    public DropTray dropTray;
    public bool isInTest = false;
    void Start()
    {
        if(isInTest)
            Init(5);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        foreach (var token in selectedTokenList)
        {
            Destroy(token);
        }
    }

    public void Init(int numOptions = 3)
    {
        FindAllOptions();
        HideOptions();

        GameObject placeholder = FindChild("SelectedOptions");

        bool isActive = referenceTokenList[0].gameObject.active;
        referenceTokenList[0].gameObject.SetActive(true);
        Vector3 tokenSize = referenceTokenList[0].GetComponent<SphereCollider>().bounds.size;
        referenceTokenList[0].gameObject.SetActive(isActive);

        List<Vector3> positions = Utils.GetPositionDistribution(numOptions, this.transform.position, placeholder.transform.position.y, this.GetComponent<MeshRenderer>().bounds.size.x, tokenSize.x);

        selectedTokenList = SelectOptions(placeholder, positions);

        //List<Vector3> test1 = GetPositionDistribution(3);
        /*List<Vector3> test3 = GetPositionDistribution(5);
        List<Vector3> test4 = GetPositionDistribution(6);*/
    }

    List<Token> SelectOptions(GameObject placeholder, List<Vector3> positions)
    {
        List<Token> tokensChosen = new List<Token>();
        int num = positions.Count;
        //HashSet<int> previousChoices = new HashSet<int>();
        for(int i=0; i<num; i++)
        {
            int choice = UnityEngine.Random.Range(0, NumOptions);
            Token chosenToken = referenceTokenList[choice];
            Vector3 position = positions[i];
            var go = Instantiate<Token>(chosenToken, position, chosenToken.transform.rotation);
            go.transform.parent = placeholder.transform;
            go.gameObject.SetActive(true);
            go.optionsTray = this;
            go.choiceIndex = choice;
            tokensChosen.Add(go);
        }
        return tokensChosen;
    }

    public void RemoveToken(Token token)
    {
        foreach (var listToken in selectedTokenList)
        {
            if(listToken == token)
            {
                selectedTokenList.Remove(token);
                return;
            }
        }
    }

    public List <int> GetSelectedTokenIndices()
    { 
        List<int> items = new List<int>();
        foreach(var token in selectedTokenList)
        {
            items.Add(token.choiceIndex);
        }
        return items;
       /* var mine = tokensChosen.Where(x => allItems.Select(y => y.ItemID).ToList();
        var filtered = ctx.PurchasedItems.Where(x => allItems.Select(y => y.ItemID).ToList().Contains(x.FK_ItemID)).ToList();*/
    }

    internal bool SuccessfulDrop(Token token)
    {
        if(dropTray.CanDropToken(token) == true)
        {
            return true;
        }
        return false;
    }

    void FindAllOptions()
    {
        GameObject tokenOptions = FindChild("TokensOptions");

        referenceTokenList = new List<Token>();
        int i = 0; 
        foreach (Transform child in tokenOptions.transform)
        {
            Token t = child.GetComponent<Token>();
            t.choiceIndex = i++;
            referenceTokenList.Add(t);
        }
    }

    private void HideOptions()
    {
        foreach(var token in referenceTokenList)
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
