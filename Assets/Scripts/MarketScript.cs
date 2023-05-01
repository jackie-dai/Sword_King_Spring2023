using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketScript : MonoBehaviour
{
    public Item itemProvided;
    public int cost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buy(Player player)
    {
        if (player.getGold() >= cost) {
            player.setGold(cost);
            player.addItem(itemProvided);
        }
    }

    public void sell(Player player)
    {
        player.removeItem(itemProvided);
        player.setGold(-cost);
    }

    public string info()
    {
        return itemProvided.name + ": $" + cost;
    }
}
