using System;
using UnityEngine;

namespace HexMap
{
    [System.Serializable]
    public struct HexCoordinates
    {
        [SerializeField] private int x, z;

        public int X => x;

        public int Z => z;

        public int Y => -X - Z;

        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public static HexCoordinates FromOffsetCoordinates(int x, int z)
        {
            return new HexCoordinates(x - z/2, z);
        }
        public string GetSeparateLineHexText()
        {
            return $"{X}\n{Y}\n{Z}";
        }

        public string GetHexGameObjectName()
        {
            return $"HexCell_{X}_{Y}_{Z}";
        }
        public string GetDrawerHexText()
        {
            return $"({X},{Y},{Z})";
        }
    }
}