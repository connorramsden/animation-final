using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkeleton : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> targetNodes;
    public List<float> distances;
    public GameObject bonePrefab;

    private List<GameObject> localBonePrefabs; //make sure this is standard unity size of 1
    private Vector3 targetPos;
    

    void Start()
    {
        
        if (targetNodes.Count != 0)
        {
            for (int i = 0; i < targetNodes.Count; i++)
            {
                //localBonePrefabs.Add(Instantiate(bonePrefab, this.transform));
            }
        }
        else
        {
            //localBonePrefabs.Add(Instantiate(bonePrefab, this.transform));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetNodes.Count != 0)
        {
            for (int i = 0; i < targetNodes.Count; i++)
            {
                targetPos = targetNodes[i].transform.position;
                Debug.DrawLine(this.transform.position, targetPos, Color.red, 5f);

                //Debug.Log("Bone Length: " + (targetPos - this.transform.position).magnitude);
                distances.Add((targetPos - this.transform.position).magnitude);

                //localBonePrefabs[i].transform.position = (this.transform.position - targetPos) * ((this.transform.position - targetPos).magnitude * 0.5f);
                //localBonePrefab.transform.localScale = localBonePrefab.transform.localScale * ((targetPos - this.transform.position).magnitude);
            }
        }
        else
        {
            //Debug.Log("count == 0");
        }
    }
}
