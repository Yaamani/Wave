using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject cube;
    public List<GameObject> cubes = new List<GameObject>();

    public int firstCubeXPos;
    public int lastCubeXPos;

    public float lowestY;
    public float highestY;

    public Color beginningColor;
    public Color endingColor;

    private float distributionOrigin;
    public float distributionSpeed;
    private float distributionFinalPos;
    private float distributionInitialPos;
    public float distributionWidth;//the boundary, u could say ...

    // Use this for initialization
    void Start()
    {
        for (int i = firstCubeXPos; i < lastCubeXPos; i++)
        {
            for (int j = firstCubeXPos; j < lastCubeXPos; j++)
            {
                cubes.Add(Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity));
            }
        }
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        //You can say that the distributionOrigin is like a "point" that travells from the first cube the last one then returns back to the first again and so on ..
        //Important Note : the distributionOrigin moves parallel to the z-axis (if you want to imagine it as a travelling point)

        distributionOrigin += distributionSpeed;

        //If you want to know why i added or subtracted the distributionWidth at the next 2 expressions, just remove it and you'll get the idea easily
        distributionFinalPos = lastCubeXPos + distributionWidth;
        distributionInitialPos = firstCubeXPos - distributionWidth;

        if (distributionOrigin >= distributionFinalPos) distributionOrigin = distributionInitialPos;
        else if (distributionOrigin <= distributionInitialPos) distributionOrigin = distributionFinalPos;
    }


    void Update()
    {
        foreach (GameObject cube in cubes)
        {
            float distance = Mathf.Abs(cube.transform.position.z - distributionOrigin);

            if (distance <= distributionWidth)
            {
                //this affects only the cubes that are near the distributionOrigin. Exactly those which is far from the distributionOrigin by a distance whcih is less than or equal to the distributionWidth

                float distributionEquation = (Mathf.Cos((distance / distributionWidth) * Mathf.PI) + 1) / 2;
                //to make the value ocilate between 0 and 1 only, I divided by distributionWidth, multiplyied by Mathf.PI, added 1 and finally divided by 2

                cube.transform.localScale = new Vector3(cube.transform.localScale.x,
                                                        Mathf.Lerp(lowestY, highestY, distributionEquation),
                                                        cube.transform.localScale.z);

                cube.GetComponent<Renderer>().material.color = Color.Lerp(beginningColor, endingColor, distributionEquation);
            }
            else
            {
                //the rest of the cubes return to thier normal state (lowestY and red color)
                cube.transform.localScale = new Vector3(cube.transform.localScale.x, lowestY, cube.transform.localScale.z);
                cube.GetComponent<Renderer>().material.color = beginningColor;
            }
        }
    }


}
