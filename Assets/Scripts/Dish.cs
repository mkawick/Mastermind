using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    internal DropTray tray;
    internal Token droppedToken;
    internal int expectedTokenIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (droppedToken != null)
            Destroy(droppedToken);
    }

    void OnMouseOver()
    {
        if(droppedToken == null)// cannot drop on twice
            tray.MouseHovering(this);
    }

    void OnMouseExit()
    {
        tray.MouseStopHovering(this);
    }

    internal Token DropToken(Token token)
    {
        if (droppedToken == null)
        {
            droppedToken = token;
            Vector3 position = this.transform.position;
            position.y += 0.02f;
            iTween.MoveTo(token.gameObject, position, 0.2f);
        }
      /*  else
        {
            Token oldToken = droppedToken;
            droppedToken = token;
            Vector3 position = this.transform.position;
            position.y += 0.02f;
            iTween.MoveTo(token.gameObject, position, 0.2f);
            iTween.MoveTo(oldToken.gameObject, oldToken.originalPosition, 0.5f);

            return oldToken;
        }*/

        return null;
    }
    public bool IsTokenCorrect()
    {
        return droppedToken.choiceIndex == expectedTokenIndex;
    }

    public bool HasToken()
    {
        return droppedToken != null;
    }
    public int GetTokenIndex()
    {
        if (droppedToken != null)
            return droppedToken.choiceIndex;
        return -1;
    }
}
