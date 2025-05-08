using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadisticasMetas
    {
        public int TotalMetas { get; set; }
        public int MetasActivas { get; set; }
        public int MetasCumplidas { get; set; }
        public int MetasFallidas { get; set; }
        public decimal PorcentajeExito { get; set; }
    }

}
