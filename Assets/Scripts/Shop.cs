using UnityEngine;

public class Shop : MonoBehaviour
{
    public enum Product { Ammunation, SimpleWeapon, MediumWeapon, GoodWeapon, Armor, Upgrade, Door }
    public Product product;
    public string message;
    public int price;

    public void buyItem(GameObject player, ref int wallet)
    {
        if(wallet >= price)
        {
            wallet -= price;

            switch(product)
            {
                case Product.SimpleWeapon:
                    break;

                case Product.MediumWeapon:
                    break;

                case Product.GoodWeapon:
                    break;

                case Product.Armor:
                    break;

                case Product.Ammunation:
                    player.GetComponentInChildren<Gun>().ammoQtd = player.GetComponentInChildren<Gun>().maxAmmoInInventory;
                    price += (int)(price * 0.2f);

                    break;

                case Product.Upgrade:
                    break;

                case Product.Door:
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                    Destroy(gameObject, 2f); // Da um tempo para sair o som do audioSource

                    break;
            }

            AudioSource sound = GetComponent<AudioSource>(); // Toca o som do objeto
            if (sound != null)
                sound.Play();
        }
    }
}