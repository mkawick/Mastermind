using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class DropTray : MonoBehaviour
{
    public GameboardSetup gameboard;
    List<Dish> dishesList;
    Dish mouseHoveringDish;

    [SerializeField]
    Dish dish;
    public bool isInTest = false;
    public int NumOptions { get { return dishesList.Count; } }
    // Start is called before the first frame update
    void Start()
    {
        if (isInTest)
            Init(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        foreach (var dish in dishesList)
        {
            Destroy(dish);
        }
    }

    internal void Init(int numTrays = 2)
    {
        GameObject placeholder = Utils.FindChild(this.gameObject, "SelectedOptions");

        dish.gameObject.SetActive(true);
        Vector3 tokenSize = dish.GetComponent<SphereCollider>().bounds.size;
        dish.gameObject.SetActive(false);

        List<Vector3> positions = Utils.GetPositionDistribution(numTrays, this.transform.position, placeholder.transform.position.y, this.GetComponent<MeshRenderer>().bounds.size.x, tokenSize.x);

        dishesList = SelectOptions(placeholder, positions);
    }

    public void FillInChoices(List<int> orderedChoices)
    {
        int index = 0;
        foreach(var dish in dishesList)
        {
            dish.expectedTokenIndex = orderedChoices[index++];
        }
    }
    List<Dish> SelectOptions(GameObject placeholder, List<Vector3> positions)
    {
        List<Dish> tokensChosen = new List<Dish>();
        int num = positions.Count;
        HashSet<int> previousChoices = new HashSet<int>();
        for (int i = 0; i < num; i++)
        {
            Vector3 position = positions[i];
            var go = Instantiate<Dish>(dish, position, dish.transform.rotation);
            go.transform.parent = placeholder.transform;
            go.gameObject.SetActive(true);
            go.tray = this;
            tokensChosen.Add(go);
        }
        return tokensChosen;
    }

    public bool AreAllDishesFilled()
    {
        foreach(var dish in dishesList)
        {
            if (dish.HasToken() == false)
                return false;
        }
        return true;
    }

    int FindFistMatchingIndex(List<int> choices, int search, int startIndex = 0)
    {
        for(int i=startIndex; i<choices.Count; i++)
        {
            if (search == choices[i])
                return i;
        }
        return -1;
    }

    public bool AreAnyTraysStillActive()
    {
        foreach (var dish in dishesList)
        {
            if (dish.gameObject.active == true)
                return true;
        }
        return false;
    }
    public void SendActiveTrayTokensHome()
    {
        foreach (var dish in dishesList)
        {
            if(dish.gameObject.active == true)
            {
                dish.droppedToken.ReturnToOriginalPosition();
                dish.droppedToken = null;
            }
        }
    }

    public List<Dish> SortMatches()
    {
        List<Dish> sortedMatches = new List<Dish>();
        foreach (var dish in dishesList)
        {
            if(dish.expectedTokenIndex == dish.droppedToken.choiceIndex)
            {
                sortedMatches.Add(dish);
            }
        }
        return sortedMatches;
    }
    public List<Dish> SortMatches2 (List<int> choices)
    {
        List<Dish> sortedMatches = new List<Dish>();
        List <Dish> matches = MatchingDishes(choices);
        int matchedIndex = 0;
        for(int i=0; i<matches.Count; i++)
        {
            int test = FindFistMatchingIndex(choices, matches[i].droppedToken.choiceIndex, matchedIndex);
            if (test == -1)
                continue;

            matchedIndex = test + 1;
            sortedMatches.Add(matches[i]);
        }
        return sortedMatches;
    }
    public List<Dish> MatchingDishes(List<int> choices)
    {
        List<Dish> matches = new List<Dish>();
        foreach (var dish in dishesList)
        {
            foreach(var choice in choices)
            {
                if(dish.droppedToken.choiceIndex == choice)
                {
                    matches.Add(dish);
                    break;
                }
            }
            
        }
        return matches;
    }

    public void MouseHovering(Dish dish)
    {
        if (mouseHoveringDish != null)
            return;

        mouseHoveringDish = dish;
        Debug.Log("Mouse is over GameObject.");
    }
    public void MouseStopHovering(Dish dish)
    {
        mouseHoveringDish = null;
        Debug.Log("Mouse is gone.");
    }

    public bool CanDropToken(Token token)
    {
        if (mouseHoveringDish != null)
        {
            mouseHoveringDish.DropToken(token);
            return true;
        }
        return false;
    }
}
