using UnityEngine;
using UnityEngine.UI;

public class ShopGrab : MonoBehaviour
{
    public Shop shopSelected;
    private Text textScript;

    void Start()
    {
        textScript = GetComponent<Text>();
    }

    public void UpdateDialog()
    {
        if (shopSelected != null)
        {
            textScript.text = shopSelected.message + "\nCost: " + shopSelected.price;

            textScript.enabled = true;
        }
        else
            textScript.enabled = false;
    }
}
