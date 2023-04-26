using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        anime = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("IN_Animation");
        }

        if(Input.GetMouseButtonDown(1))
        {
            anime.SetTrigger("OUT_Animation");
        }
    }
}
