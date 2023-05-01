using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpectrum : MonoBehaviour
{

    public AudioSpectrum spectrum;

    //オブジェクト配列(後々改善)
    public Transform[] cubes;
    public Transform[] cubes2;
    public Transform[] cubes3;
    public Transform[] cubes4;
    public Transform[] cubes5;
    public Transform[] cubes6;

    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            var cube  = cubes[i];
            var cube2 = cubes2[i];
            var cube3 = cubes3[i];
            var cube4 = cubes4[i];
            var cube5 = cubes5[i];
            var cube6 = cubes6[i];

            var localScale  = cube.localScale;
            var localScale2 = cube2.localScale;
            var localScale3 = cube3.localScale;
            var localScale4 = cube4.localScale;
            var localScale5 = cube5.localScale;
            var localScale6 = cube6.localScale;

            localScale.y  = spectrum.Levels[i] * (scale * 1.8f);
            localScale2.y = spectrum.Levels[i] * (scale * 1.2f);
            localScale3.y = spectrum.Levels[i] * (scale * 0.6f);
            localScale4.y = spectrum.Levels[i] * (scale * 1.1f);
            localScale5.y = spectrum.Levels[i] * (scale * 0.8f);
            localScale6.y = spectrum.Levels[i] * (scale * 2.0f);

            cube.localScale  = localScale;
            cube2.localScale = localScale2;
            cube3.localScale = localScale3;
            cube4.localScale = localScale4;
            cube5.localScale = localScale5;
            cube6.localScale = localScale6;
        }
    }
}
