using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballScript : MonoBehaviour
{
    private bool moving = false;
    private Vector3 endPos;
    private Vector3 startPos;
    private float moveTime = 0;
    public void Launch(Vector3 direction, Vector3 start)
    {
        endPos = (direction - start) * 22;
        startPos = start;
        moving = true;
        //transform.LookAt(new Vector3(transform.position.x, transform.position.y, Vector3.Angle(endPos, startPos)));
        transform.rotation = Quaternion.FromToRotation(Vector3.up, endPos - startPos);
        transform.rotation *= Quaternion.Euler(0, 0, -90);
    }

    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(endPos - startPos);
        
        if(moving)
        {
            moveTime += Time.deltaTime * 0.5f;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, moveTime);
            if (gameObject.transform.position == endPos)
            {
                moveTime = 0;
                moving = false;
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().Play();
        Debug.Log("kokkup6rge");
        if (coll.gameObject.tag == "enemy")
        {
            coll.gameObject.GetComponent<enemy_controller>().EnemyHealth(1);
        }
        Destroy(gameObject);
    }

}
