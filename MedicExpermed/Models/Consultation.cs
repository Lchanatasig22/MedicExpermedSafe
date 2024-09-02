using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicExpermed.Models
{
    public class Consultation
    {
        public Consultation()
        {

            FechaCreacion = DateTime.Now;

            ConsultaAntecedentesFamiliares = new AntecedentesFamiliare();
            ConsultaOrganosSistemas = new OrganosSistema();
            ConsultaExamenFisico = new ExamenFisico();
        }
        // Propiedades básicas
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Historial { get; set; }
        public int PacienteID { get; set; }
        public string Motivo { get; set; }
        public string Enfermedad { get; set; }
        public string NombrePariente { get; set; }
        public string SignosAlarma { get; set; }
        public string ReconocFarmacologicas { get; set; }
        public int TipoParienteID { get; set; }
        public string Telefono { get; set; }
        public string Temperatura { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string PresionArterialSistolica { get; set; }
        public string PresionArterialDiastolica { get; set; }
        public string Pulso { get; set; }
        public string Peso { get; set; }
        public string Talla { get; set; }
        public string PlanTratamiento { get; set; }
        public string Observacion { get; set; }
        public string AntecedentesPersonales { get; set; }
        public int DiasIncapacidad { get; set; }
        public int MedicoID { get; set; }
        public int EspecialidadID { get; set; }
        public int EstadoConsulta { get; set; }
        public int TipoConsulta { get; set; }
        public string NotasEvolucion { get; set; }
        public string ConsultaPrincipal { get; set; }
        public int ActivoConsulta { get; set; }

        // Propiedades que son listas (uno a muchos)
        public List<int> AlergiasID { get; set; } // Asumiendo que debes manejar múltiples alergias
        public string ObserAlergias { get; set; }
        public List<int> CirugiasID { get; set; } // Asumiendo que debes manejar múltiples cirugías
        public string ObserCirugias { get; set; }

        // Propiedades para relaciones de uno a uno
        public OrganosSistema ConsultaOrganosSistemas { get; set; }
        public ExamenFisico ConsultaExamenFisico { get; set; }
        public AntecedentesFamiliare ConsultaAntecedentesFamiliares { get; set; }

        // Propiedades adicionales para tablas secundarias
        public List<ConsultaMedicamento> Medicamentos { get; set; }
        public List<ConsultaLaboratorio> Laboratorios { get; set; }
        public List<ConsultaImagen> Imagenes { get; set; }
        public List<ConsultaDiagnostico> Diagnosticos { get; set; }
        public List<ConsultaCirugia> Cirugias { get; set; }
        public List<ConsultaAlergia> Alergias { get; set; }
    }
}
