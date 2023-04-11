using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Repository
{
    public interface IPositionRepository
    {
        Position AddPosition(Position newPosition);
        // GetAllPosition
        List<Position> ListOfPosition();
        // GetId
        Position GetPositionById(int Id);
        // Update
        Position UpdatePosition(int PositionId, Position newPosition);
        // Delete
        Position DeletePosition(int PositionId);
        List<Position> Filter(string a);
    }
}
