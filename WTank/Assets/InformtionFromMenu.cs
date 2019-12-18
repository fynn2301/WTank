using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformtionFromMenu : MonoBehaviour
{
    public static int playerIndex = 1;
    public Text debugtext;

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        
    }
    public int GetPlayerIndex()
    {
        debugtext.text = playerIndex.ToString();
        return playerIndex;
    }
    
}
