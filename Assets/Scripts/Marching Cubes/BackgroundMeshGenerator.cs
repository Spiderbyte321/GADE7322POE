using System.Collections.Generic;
using UnityEngine;

public class BackgroundMeshGenerator : MonoBehaviour
{
   
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int Depth;
    [SerializeField]private float NoiseResolution = 0.5f;
    [SerializeField] private float HeightThreshold = 0.6f;
    [SerializeField] private MeshFilter Filter;
    
    
    private List<Vector3> Vertices = new List<Vector3>();
    private List<int> TriangleIndices = new List<int>();
    private float[,,] Volume;

    private void Start()
    {
        Volume = new float[Width, Height, Depth];

        //assign to each float value within the volume a random number between 0 and 1
        for(int X = 0; X <Width-1; X++)
        {
            for(int Y=0;Y<Height-1;Y++)
            {
                for(int Z=0;Z<Depth-1;Z++)
                {
                    float ResolutionModifier = Random.Range(0.9f, 2.5f);
                    float CornerHeight = Height * Mathf.PerlinNoise(X * NoiseResolution*ResolutionModifier, Z * NoiseResolution*ResolutionModifier);
                    float NewCornerHeight;
                    
                    if (Y > CornerHeight)// ensure that the height value doesn't go into the negatives
                    {
                        NewCornerHeight = Y - CornerHeight;
                    }
                    else
                    {
                        NewCornerHeight = CornerHeight - Y;
                    }
                    
                    Volume[X, Y, Z] = NewCornerHeight;

                }
            }
        }

        
        for (int X = 0; X <Width-1; X++)//loop over entire grid and march the cubes
        {
            for (int Y = 0; Y <Height-1; Y++)
            {
                for (int Z = 0; Z <Depth-1; Z++)
                {
                    float[] CubeCornerValues = new float[8];

                    for(int i = 0; i < CubeCornerValues.Length; i++)
                    {
                        Vector3Int CubeCorner = new Vector3Int(X, Y, Z) + MarchingTable.Corners[i];
                        CubeCornerValues[i] = Volume[CubeCorner.x, CubeCorner.y, CubeCorner.z];
                    }
                    
                    MarchCube(new Vector3(X,Y,Z),ReturnConfigurationIndex(CubeCornerValues));
                }
            }
        }
        
        
        
        
        //might not be what we want and or need according to POE
        
        //Assign our generated mesh to a filter to display
        Mesh mesh = new Mesh();
        Vertices.Reverse();
        mesh.vertices = Vertices.ToArray();
        TriangleIndices.Reverse();
        mesh.triangles = TriangleIndices.ToArray();
        mesh.RecalculateNormals();
        Filter.mesh = mesh;

    }




    private int ReturnConfigurationIndex(float[] CubeCornerValues)//get the configindex for a cube
    {
        int ConfigurationIndex=0;
        for (int i = 0; i < CubeCornerValues.Length; i++)
        {
            if(CubeCornerValues[i] > HeightThreshold)
            { 
                ConfigurationIndex |= 1 << i;
            }
        }

        return ConfigurationIndex;
    }

    private void MarchCube(Vector3 Aposition, int ConfigurationIndex)//connect up the vertices accordingto config
    {
        if(ConfigurationIndex==0||ConfigurationIndex==255)
        {
         return;   
        }

        int EdgeIndex = 0;
        
        for (int t = 0; t < 5; t++)
        {
            for (int v = 0; v < 3; v++)
            {
                int TritableValue = MarchingTable.Triangles[ConfigurationIndex, EdgeIndex];
                
                if(TritableValue==-1)
                    return;

                Vector3 Start = Aposition + MarchingTable.Edges[TritableValue, 0];
                Vector3 End = Aposition + MarchingTable.Edges[TritableValue, 1];
                
                Vector3 PlacedVertex = (Start + End) / 2;
                
                Vertices.Add(PlacedVertex);
                TriangleIndices.Add(Vertices.Count-1);
                EdgeIndex++;

            }
        }
    }
}
