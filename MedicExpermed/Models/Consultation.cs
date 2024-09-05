using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MedicExpermed.Models
{
    public class Consultation
    {
        public Consultation()
        {

        }
        // Propiedades básicas
       
            // Parámetros de la tabla consulta
            public string UsuarioCreacion { get; set; }
            public string Historial { get; set; }
            public int PacienteId { get; set; }
            public string Motivo { get; set; }
            public string Enfermedad { get; set; }
            public string NombrePariente { get; set; }
            public string SignosAlarma { get; set; }
            public string ReconoFarmacologicas { get; set; }
            public int TipoPariente { get; set; }
            public string TelefonoPariente { get; set; }
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
            public int MedicoId { get; set; }
            public int EspecialidadId { get; set; }
            public int TipoConsulta { get; set; }
            public string NotasEvolucion { get; set; }
            public string ConsultaPrincipal { get; set; }
            public int EstadoConsulta { get; set; }

            // Parámetros para organos_sistemas
            public bool OrgSentidos { get; set; }
            public string ObserOrgSentidos { get; set; }
            public bool Respiratorio { get; set; }
            public string ObserRespiratorio { get; set; }
            public bool CardioVascular { get; set; }
            public string ObserCardioVascular { get; set; }
            public bool Digestivo { get; set; }
            public string ObserDigestivo { get; set; }
            public bool Genital { get; set; }
            public string ObserGenital { get; set; }
            public bool Urinario { get; set; }
            public string ObserUrinario { get; set; }
            public bool MEsqueletico { get; set; }
            public string ObserMEsqueletico { get; set; }
            public bool Endocrino { get; set; }
            public string ObserEndocrino { get; set; }
            public bool Linfatico { get; set; }
            public string ObserLinfatico { get; set; }
            public bool Nervioso { get; set; }
            public string ObserNervioso { get; set; }

            // Parámetros para examen_fisico
            public bool Cabeza { get; set; }
            public string ObserCabeza { get; set; }
            public bool Cuello { get; set; }
            public string ObserCuello { get; set; }
            public bool Torax { get; set; }
            public string ObserTorax { get; set; }
            public bool Abdomen { get; set; }
            public string ObserAbdomen { get; set; }
            public bool Pelvis { get; set; }
            public string ObserPelvis { get; set; }
            public bool Extremidades { get; set; }
            public string ObserExtremidades { get; set; }

            // Parámetros para antecedentes_familiares
            public bool Cardiopatia { get; set; }
            public string ObserCardiopatia { get; set; }
            public bool Diabetes { get; set; }
            public string ObserDiabetes { get; set; }
            public bool EnfCardiovascular { get; set; }
            public string ObserEnfCardiovascular { get; set; }
            public bool Hipertension { get; set; }
            public string ObserHipertension { get; set; }
            public bool Cancer { get; set; }
            public string ObserCancer { get; set; }
            public bool Tuberculosis { get; set; }
            public string ObserTuberculosis { get; set; }
            public bool EnfMental { get; set; }
            public string ObserEnfMental { get; set; }
            public bool EnfInfecciosa { get; set; }
            public string ObserEnfInfecciosa { get; set; }
            public bool MalFormacion { get; set; }
            public string ObserMalFormacion { get; set; }
            public bool Otro { get; set; }
            public string ObserOtro { get; set; }

            // Parámetros tipo tabla (pueden ser listas de objetos)
            public DataTable Alergias { get; set; }
            public DataTable Cirugias { get; set; }
            public DataTable Medicamentos { get; set; }
            public DataTable Laboratorio { get; set; }
            public DataTable Imagenes { get; set; }
            public DataTable Diagnosticos { get; set; }
        }

    }

