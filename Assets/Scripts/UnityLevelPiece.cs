using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLevelPiece : MonoBehaviour, IEngineId
{
    private int _id;
    public int EngineId {get => _id; }

    private List<UnityConnector> myUnityConnectors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityConnector FindConnectorById(int coreId)
    {
        foreach (UnityConnector c in myUnityConnectors)
        {
            if(c.EngineId == coreId)
            {
                return c;
            }
        }
        return null;
    }
}
