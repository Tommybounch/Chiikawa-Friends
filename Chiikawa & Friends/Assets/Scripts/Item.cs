using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [TextArea]
    [SerializeField]
    private string itemDescription;


    [SerializeField]
    private Sprite sprite;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0){
                Destroy(gameObject);
            } else {
                quantity = leftOverItems;
            }
            
        }
    }
}
