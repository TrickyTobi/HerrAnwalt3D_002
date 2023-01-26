using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMaterialSelection : MonoBehaviour
{
    public enum SurfaceMaterial { None, HardFloor, Gras, HallWay, WoodenPlank, Sand, MetalCavity };

    [SerializeField] SurfaceMaterial _groundMaterial; public SurfaceMaterial GroundMaterial { get => _groundMaterial; set => _groundMaterial = value; }
}


