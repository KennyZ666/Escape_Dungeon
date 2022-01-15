using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[30];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (boxCollider == null)
        {
            return;
        }
        //Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }
            Oncollide(hits[i]);

            // the array is not cleaned up, so we do it ourself.
            hits[i] = null;
        }
    }

    protected virtual void Oncollide(Collider2D coll)
    {
        Debug.Log("Oncollide is not implemented in " + this.name);
    }
}
