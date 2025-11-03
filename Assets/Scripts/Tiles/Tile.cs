using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

    public class Tile:MonoBehaviour
    {
        
        [SerializeField] private List<TileSet> TileInfo;
        [SerializeField] private TileSet Default;
        private TileSet collapseInfo;
        private bool collapsed;
        private bool reserved;
        private Vector2Int tilePosition;

        public bool Collapsed => collapsed;
        public int Entropy => TileInfo.Count;
        public TileSet CollapseInfo => collapseInfo;
        public Vector2Int TilePosition => tilePosition;

        public bool Reserved => reserved;

        

        public void InitialiseTile()
        { 
            tilePosition = new Vector2Int((int)gameObject.transform.position.x,(int) gameObject.transform.position.z);
        }


        public void AmendTile(IEdgeConstraint AConstraint,EDirection ADirection)
        {
            for(int i = TileInfo.Count-1; i >=0; i--)
            {
                if(!TileInfo[i].OppositeConstraint(ADirection).Matches(AConstraint))
                {
                    TileInfo.RemoveAt(i);
                }
            }
        }//takes in direction of origin constraint and what it is check if each of our tilesets have that if not chuck it


        public void AmendTileInfo(TileSet[] ANewTileInfo)
        {
            TileInfo.Clear();
            foreach (TileSet newTileSet in ANewTileInfo)
            {
                TileInfo.Add(newTileSet);
            }

            reserved = true;
        }
        public void Collapse()//Randomly selects any
        {
            if(collapsed)
                return;
            
            collapsed = true;
            if(TileInfo.Count == 0)
            {
                CollapseToDefault();
                return;
            }
            collapseInfo = TileInfo[Random.Range(0, TileInfo.Count)];
            Instantiate(CollapseInfo.TileMeshObject,this.gameObject.transform);
            TileInfo.Clear();
            TileInfo.Add(collapseInfo);
        
        }

        public void Collapse(TileSet ATileSet)//Allows for specification of what you want
        {
            if(collapsed)
            {
                 return;
            }
               
            
            collapsed = true;
            TileSet ComparedSet = ATileSet;
            collapseInfo = ComparedSet;
            Instantiate(CollapseInfo.TileMeshObject,gameObject.transform);
            TileInfo.Clear();
            TileInfo.Add(collapseInfo);
            
        }
        

        private void CollapseToDefault()
        {
            collapsed = true;
            collapseInfo = Default;
            Instantiate(CollapseInfo.TileMeshObject,gameObject.transform);
            TileInfo.Clear();
            TileInfo.Add(collapseInfo); 
        }
    }