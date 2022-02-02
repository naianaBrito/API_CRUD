using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_CRUD.Models
{
    public class Empregado
    {
        public int IdEmpregado { get; set; }

        public string NomeEmpregado { get; set; }

        public string Departamento { get; set; }

        public string DataInicio { get; set; }

        public string NomeArquivoFoto { get; set; }
    }
}
