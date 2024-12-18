using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Baselib.Mono;

/// <summary>
/// Custom implementation of BaseGhost.
/// </summary>
public class CustomBaseGhost : BaseGhost
{
    public Base.CellType CellType;

    public Int3 CellSize = new Int3(1);

    public override void SetupGhost()
    {
        ghostBase.SetSize(new Int3(1));
        SetGhostBaseCell();
        RebuildGhostGeometry();
    }

    public override bool UpdatePlacement(Transform camera, float placeMaxDistance, out bool positionFound, out bool geometryChanged, ConstructableBase ghostModelParentConstructableBase)
    {
        positionFound = false;
        geometryChanged = false;

        UpdateRotation(ghostModelParentConstructableBase.transform);
        targetBase = FindBase(camera);
        if (targetBase)
        {
            targetBase.SetPlacementGhost(this);
            positionFound = true;

            Vector3 placePoint = camera.position + camera.forward * ghostModelParentConstructableBase.placeDefaultDistance;
            Int3 gridPlacePoint = targetBase.WorldToGrid(placePoint);

            if (targetBase.HasSpaceFor(gridPlacePoint, CellSize))
                return false;
            if (IsBlockingHatch(gridPlacePoint, CellSize))
                return false;

            if (IsCellEmpty(gridPlacePoint))
            {
                targetBase.PickCell(camera, placePoint, CellSize);
            }
            if (IsCellEmpty(gridPlacePoint) && IsCameraInPlacePoint(camera, gridPlacePoint) && targetBase.PickFace(camera, out var face))
            {
                gridPlacePoint = Base.GetAdjacent(face);
            }
            
            if (targetOffset != gridPlacePoint)
            {
                targetOffset = gridPlacePoint;
                RebuildGhostGeometry();
                geometryChanged = true;
            }

            ghostModelParentConstructableBase.transform.position = targetBase.GridToWorld(gridPlacePoint);
            return true;
        }
        else
        {
            if (PlaceWithBoundsCast(camera.position, camera.forward, Builder.aaBounds.extents, ghostModelParentConstructableBase.placeDefaultDistance, 1, 10, out var center))
            {
                ghostModelParentConstructableBase.transform.position = center;
                return true;
            }
            return false;
        }
    }

    private void UpdateRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(90*Builder.lastRotation, 0, 0);
    }

    //TODO: Move to some BaseUtils thing. all of the below
    
    private bool IsCellEmpty(Int3 cell)
    {
        var cellEmpty = targetBase.GetCell(cell) == Base.CellType.Empty;
        var isUnderConstructionCell = targetBase.IsCellUnderConstruction(cell);
        return cellEmpty || !isUnderConstructionCell;
    }

    private bool IsCameraInPlacePoint(Transform camera, Int3 gridPlacePoint)
    {
        return targetBase.WorldToGrid(camera.position) == gridPlacePoint;
    }

    //END TODO.
    private void SetGhostBaseCell()
    {
        var cellIndex = ghostBase.GetCellIndex(Int3.zero);
        ghostBase.cells[cellIndex] = CellType;
        ghostBase.links[cellIndex] = Base.PackOffset(CellSize);
    }
}
