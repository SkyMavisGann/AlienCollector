using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchDecorationSprite : MonoBehaviour
{
    private DecorData decorationButton;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        decorationButton = FindDecoration();
        if (decorationButton != null)
        {
            image.sprite = decorationButton.gameObject.GetComponent<Image>().sprite;
        }
    }
    private DecorData FindDecoration()
    {
        Transform objParent = transform.parent;
        int loopCatcher = 0;
        while(objParent != null && loopCatcher < 20)
        {
            if (objParent.GetComponent<DecorData>() != null)
            {
                return objParent.GetComponent<DecorData>();
            }
            objParent = objParent.parent;
            loopCatcher++;
        }
        return null;
    }
}
