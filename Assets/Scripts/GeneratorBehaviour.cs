using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trinitygencore;
using trinitygencore.GenerationMethods;

public class GeneratorBehaviour : MonoBehaviour
{

    

    [SerializeField] private GeneratorMain coreGeneratorParameters;
    [SerializeField] private List<GenMethEnum> generationPasses;

    [Header("Engine Parameters")]
    [SerializeField] private float geometryOffset;
    [SerializeField] private List<UnityLevelPiece> levelPieces;

    private List<UnityLevelPiece> _placedUnityPieces;


    // Start is called before the first frame update
    void Start()
    {   
        
        
        coreGeneratorParameters.OnPairFound += PlaceTentative;
    }

        /// <summary>
        /// Gets the correct position and rotation of the tentative piece so its
        /// connector matches the guide piece's.
        /// </summary>
        private void PlaceTentative(int guidePieceCoreId,
         int guideConCoreId, int tentPieceCoreId, int tentConCoreId)
        {
            
            UnityConnector tentativeUnityConnector = null;
            UnityLevelPiece  tentativeUnityGeometry = null;

            UnityLevelPiece guideUnityGeometry = null;
            UnityConnector guideUnityConnector = null;



            // Find the corresponding tentative unity pieces and connectors
            foreach (UnityLevelPiece uPiece in levelPieces)
            {
                    if(uPiece.EngineId == tentPieceCoreId)
                    {
                        tentativeUnityGeometry = uPiece;
                        tentativeUnityConnector = 
                            uPiece.FindConnectorById(tentConCoreId);
                        break;
                    }

            }

            // Find the corresponding guide unity pieces and connectors
            foreach (UnityLevelPiece uPiece in _placedUnityPieces)
            {
                    if(uPiece.EngineId == guidePieceCoreId)
                    {
                        guideUnityGeometry = uPiece;
                        guideUnityConnector = 
                            uPiece.FindConnectorById(guideConCoreId);
                        break;
                    }

            }

            GameObject spawned = 
                Instantiate(tentativeUnityGeometry.gameObject, transform.position, transform.rotation);

            Transform newPieceTrn = tentativeUnityConnector.transform;
            Quaternion connectorPointRotation = new Quaternion();

            // temprarily revert parenting so we can move the connector
            // group and have the geometry follow.

            tentativeUnityConnector.transform.SetParent(null, true);
            tentativeUnityGeometry.transform.SetParent(tentativeUnityConnector.transform, true);
        
            // Put the other piece on my connector
            newPieceTrn.position = guideUnityConnector.transform.position;




                // Have the other connector group look towards my connector group
                connectorPointRotation.SetLookRotation(
                    -guideUnityConnector.transform.forward,
                    transform.up);

                // Apply the rotation acquired above
                newPieceTrn.rotation = connectorPointRotation;

            


            // move the pieces away from each other based on an offset
            newPieceTrn.position -= tentativeUnityConnector.transform.forward
                 * geometryOffset;

            // get the parenting back to normal to safeguard future transformations.
            tentativeUnityGeometry.transform.SetParent(null, true);
            tentativeUnityConnector.transform.SetParent(tentativeUnityGeometry.transform, true);
                
            spawned.transform.position = newPieceTrn.position;
            spawned.transform.rotation = newPieceTrn.rotation;
            
        }

}
