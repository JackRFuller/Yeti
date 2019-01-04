using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : PlayerComponent
{
    private int numberOfHeldKeys;

    public int NumberOfHeldKeys { get {return numberOfHeldKeys;}}


    public void AddKey()
    {
        numberOfHeldKeys++;
    }

    public void RemoveKeys(int numberOfKeysRequired)
    {
        numberOfHeldKeys -= numberOfKeysRequired;
    }
}
