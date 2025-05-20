using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqCreateRoutine
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DifficultyLevelName { get; set; }
        public int DurationInDays { get; set; }

        public Entities.Routine ToEntity()
        {
            return new Entities.Routine
            {
                Name = this.Name,
                Description = this.Description,
                DifficultyLevelName = this.DifficultyLevelName,
                DurationInDays = this.DurationInDays
            };
        }
    }

}
