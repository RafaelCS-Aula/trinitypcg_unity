using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trinitygencore;

public class UnityLevelPiece : MonoBehaviour, IEngineLayer<LevelGeometry>
{
    public LevelGeometry CoreParams {get => _coreParams;}
    [SerializeField] private LevelGeometry _coreParams;
    private UnityConnector[] myUnityConnectors;
    
    public LevelGeometry LinkComponents()
    {
        FindConnectors();
        Connector[] connectors = new Connector[myUnityConnectors.Length];
        for (int i = 0; i < myUnityConnectors.Length; i++)
        {
            connectors[i] = myUnityConnectors[i].LinkComponents(); 
        }
        return new LevelGeometry(this,connectors);
    }

    private void FindConnectors()
    {
        myUnityConnectors = GetComponentsInChildren<UnityConnector>();
    }
}
