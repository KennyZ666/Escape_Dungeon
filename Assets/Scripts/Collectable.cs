using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected bool collected;

    protected override void Oncollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Oncollect();
        }
    }

    protected virtual void Oncollect()
    {
        collected = true;
    }
}
