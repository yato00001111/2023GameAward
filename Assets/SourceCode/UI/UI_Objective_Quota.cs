using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Objective_Quota : MonoBehaviour
{
    public ImageRenderer imageRenderer;

    [SerializeField]
    private int Num; // ƒmƒ‹ƒ}”

    // Start is called before the first frame update
    void Start()
    {
        // ƒmƒ‹ƒ}”‰Šú‰»‚·‚é
        Num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        imageRenderer._Update(Num);
    }

    // ƒmƒ‹ƒ}”‚ğİ’è‚·‚éŠÖ”
    public void SetObjectiveQuota(int num) { Num = num; }

    public int GetObjectiveQuota() { return Num; }
}
