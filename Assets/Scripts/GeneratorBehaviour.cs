using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trinitygencore;
using trinitygencore.GenerationMethods;

public class GeneratorBehaviour : MonoBehaviour, IEngineLayer<GeneratorMain>
{


    public GeneratorMain CoreParams {get => _generatorParams;}

    [SerializeField] private GeneratorMain _generatorParams;

    // Uncomment to work on multiple gen passes
    //[SerializeField] private List<GenMethEnum> generationPasses;
    

    [Header("Engine Parameters")]
    
    [SerializeField] private bool CreateOnStart = true;
    
    [SerializeField] private float geometryOffset;
    [SerializeField] private List<UnityLevelPiece> levelPieces;
    [SerializeField] private List<UnityLevelPiece> startPieces;

    [Header("Method Parameters")]
    [SerializeField] private GenMethEnum generationMethod;
    [SerializeField] private ArenaGM arenaParams;
    [SerializeField] private CorridorGM corridorParams;
    [SerializeField] private StarGM starParams;
    [SerializeField] private BranchGM branchParams;


    private List<UnityLevelPiece> _placedUnityPieces;


    // Start is called before the first frame update
    private void Start() 
    {
        if(CreateOnStart)
            CallGenerator();    
    }

    public GeneratorMain LinkComponents()
    {
        GeneratorMain gm = new GeneratorMain();
        gm.generationPasses = CoreParams.generationPasses;
        gm.pieceList = new List<LevelGeometry>();
        for (int i = 0; i < levelPieces.Count; i++)
        {
            gm.pieceList.Add(levelPieces[i].LinkComponents());
        }
        for (int i = 0; i < startPieces.Count; i++)
        {
            gm.starterList.Add(startPieces[i].LinkComponents());
        }
        gm.pinTolerance = CoreParams.pinTolerance;
        gm.starterConnectorTolereance = CoreParams.starterConnectorTolereance;
        gm.useStarter = CoreParams.useStarter;
        
        // Single generation passes
        
        gm.generationPasses = new GenerationMethod[1];
        GenerationMethod m = null;

        switch(generationMethod)
        {
            case GenMethEnum.ARENA:
                m = new ArenaGM(arenaParams.MaxPieces);
                break;
            case GenMethEnum.CORRIDOR:
                m = new CorridorGM(corridorParams.MaxPieces,corridorParams.pinchEnd, corridorParams.useEndPiece);
                break;
            case GenMethEnum.STAR:
                m = new StarGM(starParams.spokeLength);
                break;
            case GenMethEnum.BRANCH:
                m = new BranchGM(branchParams.maxBranches, branchParams.branchLength, branchParams.branchLengthVariance, branchParams.pieceJumpSize);
                break;
            

        }
        gm.generationPasses[0] = m;

        gm.OnPairFound += PlaceTentative;

        return gm;
    }
    

    
    public void CallGenerator()
    {
        LinkComponents().CreateLevel();
    }

        /// <summary>
        /// Gets the correct position and rotation of the tentative piece so its
        /// connector matches the guide piece's.
        /// </summary>
        private void PlaceTentative(
            IEngineLayer<LevelGeometry> guidePiece,
            IEngineLayer<Connector> guideCon,
            IEngineLayer<LevelGeometry> tentPiece,
            IEngineLayer<Connector> tentCon)
        {
            
            UnityConnector tentativeUnityConnector = tentCon as UnityConnector;
            UnityLevelPiece  tentativeUnityGeometry = tentPiece as UnityLevelPiece;

            UnityLevelPiece guideUnityGeometry = guidePiece as UnityLevelPiece;
            UnityConnector guideUnityConnector = guideCon as UnityConnector;

            GameObject spawned = 
                Instantiate(tentativeUnityGeometry.gameObject, transform.position, transform.rotation);
            GameObject spawnedConnector = Instantiate(tentativeUnityConnector.gameObject);

        foreach (UnityConnector c in spawned.GetComponentsInChildren<UnityConnector>())
        {
            if (c.Equals(tentativeUnityConnector))
                spawnedConnector = c.gameObject;

        }
            



            Transform newPieceTrn = spawnedConnector.transform;
            Quaternion connectorPointRotation = new Quaternion();

            // temprarily revert parenting so we can move the connector
            // group and have the geometry follow.

            spawnedConnector.transform.SetParent(null, true);
            spawned.transform.SetParent(spawnedConnector.transform, true);
        
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
            spawned.transform.SetParent(null, true);
            spawnedConnector.transform.SetParent(spawned.transform, true);
                
            spawned.transform.position = newPieceTrn.position;
            spawned.transform.rotation = newPieceTrn.rotation;

        spawned.transform.SetParent(guideUnityGeometry.gameObject.transform);

        print("Placed tentative" + tentativeUnityGeometry + " with guide  " + guideUnityGeometry);
            
        }

}
