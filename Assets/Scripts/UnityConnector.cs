using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trinitygencore;
public class UnityConnector : MonoBehaviour, IEngineLayer<Connector>
{

    public Connector CoreParams {get => _coreParams;}
    [SerializeField] private Connector _coreParams;

    [SerializeField] private  float _pinSpacing = 0.5f;

    [SerializeField] private Color _gizmoColor;



    public Connector LinkComponents() => new Connector(this,_coreParams.pinCount, CoreParams.ConnectorColor);

    private void OnDrawGizmos()
        {

            _gizmoColor.a = 1;
            Gizmos.color = _gizmoColor;
    
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

            Vector3 pos;

            for(float i = 0 - CoreParams.pinCount / 2; i <=  CoreParams.pinCount / 2; i++)
            {
                if(CoreParams.pinCount % 2 == 0 && i == 0)
                {

                    continue;

                }
                //pos.x = transform.position.x + (i * connectorSpacing);
                pos = transform.position + transform.right * i * _pinSpacing;
                //pos.z = transform.position.z * transform.right.z  + (i * connectorSpacing);
                    Gizmos.DrawWireCube(pos , new Vector3(
                        _pinSpacing,
                        _pinSpacing,
                        _pinSpacing)); 

            }
        

            
        }
}
