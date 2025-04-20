using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request
{
    public class ReqLogin
    {
        public string IdUsuario { get; set; }
        public string Password { get; set; }
        public string DireccionIP { get; set; }
    }
}
