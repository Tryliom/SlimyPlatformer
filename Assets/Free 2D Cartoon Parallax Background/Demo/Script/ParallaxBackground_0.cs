using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    [Header("Layer Setting")] public float[] Layer_Speed = new float[7];
    public GameObject[] Layer_Objects = new GameObject[7];

    private float[] startPos = new float[7];
    private float boundSizeX;
    private float sizeX;
    private GameObject Layer_0;
    
    [SerializeField] private GameObject _followedObject;

    void Start()
    {
        sizeX = Layer_Objects[0].transform.localScale.x;
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        
        for (var i = 1; i < 5; i++)
        {
            startPos[i] = _followedObject.transform.position.x;
        }
    }

    private void Update()
    {
        Layer_Objects[0].transform.position = new Vector2(_followedObject.transform.position.x, Layer_Objects[0].transform.position.y);
        
        for (var i = 1; i < 5; i++)
        {
            var temp = (_followedObject.transform.position.x * (1 - Layer_Speed[i]));
            var distance = _followedObject.transform.position.x * Layer_Speed[i];

            Layer_Objects[i].transform.position = new Vector2(startPos[i] + distance, Layer_Objects[i].transform.position.y);
            
            if (temp > startPos[i] + boundSizeX * sizeX)
            {
                startPos[i] += boundSizeX * sizeX;
            }
            else if (temp < startPos[i] - boundSizeX * sizeX)
            {
                startPos[i] -= boundSizeX * sizeX;
            }
        }
    }
}