using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Objective_Quota : MonoBehaviour
{
    public ImageRenderer imageRenderer;

    [SerializeField]
    private int Num; // �m���}��

    // Start is called before the first frame update
    void Start()
    {
        // �m���}������������
        Num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        imageRenderer._Update(Num);
    }

    // �m���}����ݒ肷��֐�
    public void SetObjectiveQuota(int num) { Num = num; }

    public int GetObjectiveQuota() { return Num; }
}
