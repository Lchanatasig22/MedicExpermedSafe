using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MedicExpermed.Models
{
    public class Consultation
    {
        public int Id { get; set; } // Identificador único de la consulta
        public DateTime FechaCreacion { get; set; } // Fecha de creación de la consulta
        public string UsuarioCreacion { get; set; } // Usuario que crea la consulta
        public string HistorialConsulta { get; set; } // Historial médico del paciente
        public string SecuencialConsulta { get; set; } // Número secuencial de la consulta
        public int PacienteId { get; set; } // Id del paciente relacionado
        public string MotivoConsulta { get; set; } // Motivo de la consulta
        public string EnfermedadConsulta { get; set; } // Descripción de la enfermedad
        public string NombrePariente { get; set; } // Nombre del pariente de contacto
        public string SignosAlarma { get; set; } // Signos de alarma que presenta el paciente
        public string ReconocimientoFarmacologico { get; set; } // Medicamentos conocidos o reconocidos
        public int TipoPariente { get; set; } // Tipo de pariente
        public string TelefonoPariente { get; set; } // Teléfono del pariente
        public string Temperatura { get; set; } // Temperatura del paciente
        public string FrecuenciaRespiratoria { get; set; } // Frecuencia respiratoria
        public string PresionArterialSistolica { get; set; } // Presión arterial sistólica
        public string PresionArterialDiastolica { get; set; } // Presión arterial diastólica
        public string Pulso { get; set; } // Pulso del paciente
        public string Peso { get; set; } // Peso del paciente
        public string Talla { get; set; } // Talla del paciente
        public string PlanTratamiento { get; set; } // Plan de tratamiento
        public string Observacion { get; set; } // Observaciones generales de la consulta
        public string AntecedentesPersonales { get; set; } // Antecedentes personales del paciente
        public int DiasIncapacidad { get; set; } // Días de incapacidad
        public int MedicoId { get; set; } // Id del médico que realiza la consulta
        public int EspecialidadId { get; set; } // Id de la especialidad médica
        public int TipoConsultaId { get; set; } // Tipo de consulta
        public string NotasEvolucion { get; set; } // Notas de evolución del paciente
        public string ConsultaPrincipal { get; set; } // Consulta principal de la visita
        public int EstadoConsulta { get; set; } // Estado de la consulta

        // Relaciones con otras tablas
        public ICollection<ConsultaAlergia> Alergias { get; set; } // Lista de alergias asociadas a la consulta
        public ICollection<ConsultaCirugia> Cirugias { get; set; } // Lista de cirugías asociadas a la consulta
        public ICollection<ConsultaMedicamento> Medicamentos { get; set; } // Lista de medicamentos asociados
        public ICollection<ConsultaLaboratorio> Laboratorios { get; set; } // Lista de laboratorios asociados
        public ICollection<ConsultaImagen> Imagenes { get; set; } // Lista de imágenes asociadas
        public ICollection<ConsultaDiagnostico> Diagnosticos { get; set; } // Lista de diagnósticos asociados
        public OrganosSistema OrganosSistemas { get; set; } // Órganos y sistemas asociados
        public ExamenFisico ExamenFisico { get; set; } // Examen físico asociado
        public AntecedentesFamiliare AntecedentesFamiliares { get; set; } // Antecedentes familiares asociados

        // Constructor que inicializa los objetos complejos
        public Consultation()
        {
            Alergias = new List<ConsultaAlergia>();
            Cirugias = new List<ConsultaCirugia>();
            Medicamentos = new List<ConsultaMedicamento>();
            Laboratorios = new List<ConsultaLaboratorio>();
            Imagenes = new List<ConsultaImagen>();
            Diagnosticos = new List<ConsultaDiagnostico>();
            OrganosSistemas = new OrganosSistema();
            ExamenFisico = new ExamenFisico();
            AntecedentesFamiliares = new AntecedentesFamiliare();
        }
    }
}