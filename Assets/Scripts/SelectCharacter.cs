using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public Character character;
    Animator anim;
    public SelectCharacter[] chars;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("OHS")) DataMgr.instance.currentCharacter = Character.OHS;
                if (hit.transform.gameObject.CompareTag("THS")) DataMgr.instance.currentCharacter = Character.THS;
                anim = hit.transform.gameObject.GetComponent<Animator>();
                onSelect();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (chars[i].gameObject != hit.transform.gameObject)
                    {
                        anim = chars[i].GetComponent<Animator>();
                        onDeSelect();
                    }
                }
            }
        }
    }

    void onSelect()
    {
        anim.SetBool("Move", true);
    }

    void onDeSelect()
    {
        anim.SetBool("Move", false) ;
    }
}
