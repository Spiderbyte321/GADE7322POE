using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

    public class Tile:MonoBehaviour
    {
        
        [SerializeField] private List<TileSet> TileInfo;
        private TileSet collapseInfo;
        private bool collapsed;
        private Vector2Int tilePosition;

        public bool Collapsed => collapsed;
        public int Entropy => TileInfo.Count;
        public TileSet CollapseInfo => collapseInfo;
        public Vector2Int TilePosition => tilePosition;


        private void Start()
        {
            tilePosition = new Vector2Int((int)gameObject.transform.position.x,(int) gameObject.transform.position.z);
        }


        public void AmendTile(IEdgeConstraint AConstraint,EDirection ADirection)
        {
            for(int i = 0; i < TileInfo.Count; i++)
            {
                if(!TileInfo[i].OppositeConstraint(ADirection).Matches(AConstraint))
                {
                    TileInfo.Remove(TileInfo[i]);
                }
            }
        }//takes in direction of origin constraint and what it is check if each of our tilesets have that if not chuck it

        public void Collapse()//Randomly selects any
        {
            collapsed = true;
            collapseInfo = TileInfo[Random.Range(0, TileInfo.Count)];
            Instantiate(CollapseInfo.TileMeshObject,this.gameObject.transform);
            TileInfo.Clear();
            TileInfo.Add(collapseInfo);
        
        }

        public void Collapse(TileSet ATileSet)//Allows for specification of what you want
        {
            if(!TileInfo.Contains(ATileSet))
            {
                Debug.Log("WronType");
                return;
            }
            collapsed = true;
            TileSet ComparedSet = null;
            foreach (TileSet HeldSet in TileInfo)
            {
                if(HeldSet == ATileSet)
                {
                    ComparedSet = HeldSet;
                }
            }
            collapseInfo = ComparedSet;
            Instantiate(CollapseInfo.TileMeshObject,gameObject.transform);
            TileInfo.Clear();
            TileInfo.Add(collapseInfo);
        }
    }