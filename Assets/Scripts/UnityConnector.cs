using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trinitygencore;
public class UnityConnector : MonoBehaviour, IEngineId
{

    public Connector coreParameters;

    private int _id;
    public int EngineId {get => _id; }


    [SerializeField] private  float _pinSpacing = 0.5f;

    [SerializeField] private Color _gizmoColor;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetId(int id) => _id = id; //_id = Random.Range(1, int.MaxValue);

    private void OnDrawGizmos()
        {

            _gizmoColor.a = 1;
            Gizmos.color = _gizmoColor;
    
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

            Vector3 pos;

            for(float i = 0 - coreParameters.pinCount / 2; i <=  coreParameters.pinCount / 2; i++)
            {
                if(coreParameters.pinCount % 2 == 0 && i == 0)
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
