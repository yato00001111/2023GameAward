using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Current_Quota : MonoBehaviour
{
    public ImageRenderer imageRenderer;

    [SerializeField] PlayDirector playDirector = default!;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        imageRenderer._Update(playDirector.GetQuotaCount());
    }
}
