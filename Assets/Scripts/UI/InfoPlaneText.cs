using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPlaneText : MonoBehaviour
{
    [SerializeField] TMP_Text txt;

    public void MakeVisible()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        if(txt != null) txt.text = text;
    }    
}
