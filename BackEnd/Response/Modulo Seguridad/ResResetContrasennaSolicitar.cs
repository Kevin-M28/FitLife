using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Response.Modulo_Usuario
{
    public class ResResetContrasennaSolicitar : ResBase
    {
        public int UsuarioID { get; set; }
        public string ResetToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
