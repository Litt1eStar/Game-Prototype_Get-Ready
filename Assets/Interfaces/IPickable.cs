using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public void PickupObject(GameObject objectHolder);
    public void DropObject();
}