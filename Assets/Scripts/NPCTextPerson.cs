using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string message;
    public Dialogue dialogue;

    private float cooldown = 40.0f;
    private float lastShout = -40.0f;
    private bool NotCollided;

    protected override void Start()
    {
        base.Start();
        NotCollided = GameManager.instance.IsDialogueTriggered;
    }

    protected override void Oncollide(Collider2D coll)
    {
        if (!NotCollided && Time.time - lastShout > cooldown)
        {
            //Debug.Log(coll.name);
            lastShout = Time.time;
            NotCollided = true;
            GameManager.instance.IsDialogueTriggered = true;
            GameManager.instance.dialogueTrigger.TriggerDialogue();
            //if (Time.time - lastShout > cooldown)
            //{
            //    lastShout = Time.time;
            //    GameManager.instance.ShowText(message, 25, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
            //}
        }



    }
}
