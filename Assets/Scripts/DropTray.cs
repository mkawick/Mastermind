using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTray : MonoBehaviour
{
    public GameboardSetup gameboard;
    List<Dish> dishesList;
    Dish mouseHoveringDish;

    [SerializeField]
    Dish dish;
    public int NumOptions { get { return dishesList.Count; } }
    // Start is called before the first frame update
    void Start()
    {
        GameObject placeholder = Utils.FindChild(this.gameObject, "SelectedOptions");

        dish.gameObject.SetActive(true);
        Vector3 tokenSize = dish.GetComponent<SphereCollider>().bounds.size;
        dish.gameObject.SetActive(false);

        List<Vector3> positions = Utils.GetPositionDistribution(3, this.transform.position, placeholder.transform.position.y, this.GetComponent<MeshRenderer>().bounds.size.x, tokenSize.x);

        dishesList = SelectOptions(placeholder, positions);

    }

    // Update is called once per frame
    void Update()
    {
        
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
