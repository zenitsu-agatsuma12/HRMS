using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repository.SqlRepository
{
    public class PositionDBRepository : IPositionRepository
    {
        HRMSDBContext _dbcontext;

        public PositionDBRepository(HRMSDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public Position AddPosition(Position newPosition)
        {
            _dbcontext.Add(newPosition);
            _dbcontext.SaveChanges();
            return newPosition;
        }

        public Position DeletePosition(int PositionId)
        {
            var Dept = GetPositionById(PositionId);
            if (Dept != null)
            {
                _dbcontext.Positions.Remove(Dept);
                _dbcontext.SaveChanges();
            }
            return Dept;
        }

        public List<Position> Filter(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                List<Position> positions = _dbcontext.Positions
                          .Where(e => e.Name.Contains(searchValue))
                          .ToList();

                return positions;
            }
            List<Position> Positions = _dbcontext.Positions.ToList();
            return Positions;
        }

        public Position GetPositionById(int Id)
        {
            return _dbcontext.Positions.AsNoTracking().ToList().FirstOrDefault(x => x.PosId == Id);
        }

        public List<Position> ListOfPosition()
        {
            return _dbcontext.Positions.AsNoTracking().ToList();
        }

        public Position UpdatePosition(int PositionId, Position newPosition)
        {
            _dbcontext.Positions.Update(newPosition);
            _dbcontext.SaveChanges();
            return newPosition;
        }


    }
}
