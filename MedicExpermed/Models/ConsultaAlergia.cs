﻿using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ConsultaAlergia
    {
        public ConsultaAlergia()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdConsultaAlergias { get; set; }
        public DateTime? FechacreacionAlergia { get; set; }
        public int? CatalogoalergiaId { get; set; }
        public int? ConsultaAlergiasInt { get; set; }
        public string? ObservacionAlergias { get; set; }
        public int EstadoAlergias { get; set; }

        public virtual Catalogo? Catalogoalergia { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
