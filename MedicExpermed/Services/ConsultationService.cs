using MedicExpermed.Controllers;
using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{
    public class ConsultationService
    {
        private readonly medicossystembdIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;

        public ConsultationService(medicossystembdIContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;


        }
        /// <summary>
        /// Obtener todas las consultas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Consultum>> GetAllConsultasAsync()
        {
            // Obtener el nombre de usuario de la sesión
            var loginUsuario = _httpContextAccessor.HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(loginUsuario))
            {
                throw new Exception("El nombre de usuario no está disponible en la sesión.");
            }

            // Filtrar las consultas por el usuario de creación y el estado igual a 0
            var consultas = await _context.Consulta
                .Where(c => c.UsuariocreacionConsulta == loginUsuario)
                .Include(c => c.ConsultaDiagnostico)
                .Include(c => c.ConsultaImagen)
                .Include(c => c.ConsultaLaboratorio)
                .Include(c => c.ConsultaMedicamentos)
                .Include(c => c.PacienteConsultaPNavigation)
                .OrderBy(c => c.FechacreacionConsulta) // Ordenar por fecha de la consulta Ocupar esto mismo para cualquier tabla 

                .ToListAsync();

            return consultas;
        }

        /// <summary>
        /// Busqueda de pacientes
        /// </summary>
        /// <param name="cedula"></param>
        /// <param name="primerNombre"></param>
        /// <param name="primerApellido"></param>
        /// <returns></returns>

        public async Task<IEnumerable<Paciente>> BuscarPacientesAsync(int? cedula, string primerNombre, string primerApellido)
        {
            var query = _context.Pacientes.AsQueryable();

            if (cedula.HasValue)
            {
                query = query.Where(p => p.CiPacientes == cedula.Value);
            }

            if (!string.IsNullOrEmpty(primerNombre))
            {
                query = query.Where(p => p.PrimernombrePacientes.Contains(primerNombre));
            }

            if (!string.IsNullOrEmpty(primerApellido))
            {
                query = query.Where(p => p.PrimerapellidoPacientes.Contains(primerApellido));
            }

            // Aquí combinamos las condiciones usando lógica OR
            query = _context.Pacientes.Where(p =>
                (cedula.HasValue && p.CiPacientes == cedula.Value) ||
                (!string.IsNullOrEmpty(primerNombre) && p.PrimernombrePacientes.Contains(primerNombre)) ||
                (!string.IsNullOrEmpty(primerApellido) && p.PrimerapellidoPacientes.Contains(primerApellido))
            );

            return await query.ToListAsync();
        }

        /// <summary>
        /// Crear una nueva consulta
        /// </summary>
        /// <param name="consultation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(int IdConsulta, string Secuencial)> CreateConsultationAsync(Consultation consultaDto)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                var command = new SqlCommand("sp_CreateConsultation", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Agregar parámetros al comando
                command.Parameters.AddWithValue("@FechaCreacion", consultaDto.FechaCreacion);
                command.Parameters.AddWithValue("@UsuarioCreacion", consultaDto.UsuarioCreacion);
                command.Parameters.AddWithValue("@Historial", consultaDto.Historial);
                command.Parameters.AddWithValue("@PacienteID", consultaDto.PacienteID);
                command.Parameters.AddWithValue("@Motivo", consultaDto.Motivo);
                command.Parameters.AddWithValue("@Enfermedad", consultaDto.Enfermedad);
                command.Parameters.AddWithValue("@NombrePariente", consultaDto.NombrePariente);
                command.Parameters.AddWithValue("@SignosAlarma", consultaDto.SignosAlarma);
                command.Parameters.AddWithValue("@ReconocFarmacologicas", consultaDto.ReconocFarmacologicas);
                command.Parameters.AddWithValue("@TipoParienteID", consultaDto.TipoParienteID);
                command.Parameters.AddWithValue("@Telefono", consultaDto.Telefono);
                command.Parameters.AddWithValue("@Temperatura", consultaDto.Temperatura);
                command.Parameters.AddWithValue("@FrecuenciaRespiratoria", consultaDto.FrecuenciaRespiratoria);
                command.Parameters.AddWithValue("@PresionArterialSistolica", consultaDto.PresionArterialSistolica);
                command.Parameters.AddWithValue("@PresionArterialDiastolica", consultaDto.PresionArterialDiastolica);
                command.Parameters.AddWithValue("@Pulso", consultaDto.Pulso);
                command.Parameters.AddWithValue("@Peso", consultaDto.Peso);
                command.Parameters.AddWithValue("@Talla", consultaDto.Talla);
                command.Parameters.AddWithValue("@PlanTratamiento", consultaDto.PlanTratamiento);
                command.Parameters.AddWithValue("@Observacion", consultaDto.Observacion);
                command.Parameters.AddWithValue("@AntecedentesPersonales", consultaDto.AntecedentesPersonales);
                command.Parameters.AddWithValue("@DiasIncapacidad", consultaDto.DiasIncapacidad);
                command.Parameters.AddWithValue("@MedicoID", consultaDto.MedicoID);
                command.Parameters.AddWithValue("@EspecialidadID", consultaDto.EspecialidadID);
                command.Parameters.AddWithValue("@AlergiasID", consultaDto.AlergiasID);
                command.Parameters.AddWithValue("@ObserAlergias", consultaDto.ObserAlergias);
                command.Parameters.AddWithValue("@CirugiasID", consultaDto.CirugiasID);
                command.Parameters.AddWithValue("@ObserCirugias", consultaDto.ObserCirugias);
                command.Parameters.AddWithValue("@EstadoConsulta", consultaDto.EstadoConsulta);
                command.Parameters.AddWithValue("@TipoConsulta", consultaDto.TipoConsulta);
                command.Parameters.AddWithValue("@NotasEvolucion", consultaDto.NotasEvolucion);
                command.Parameters.AddWithValue("@ConsultaPrincipal", consultaDto.ConsultaPrincipal);
                command.Parameters.AddWithValue("@ActivoConsulta", consultaDto.ActivoConsulta);

                // Parámetros para las tablas secundarias (JSON)
                command.Parameters.AddWithValue("@Medicamentos", consultaDto.Medicamentos);
                command.Parameters.AddWithValue("@Laboratorios", consultaDto.Laboratorios);
                command.Parameters.AddWithValue("@Imagenes", consultaDto.Imagenes);
                command.Parameters.AddWithValue("@Diagnosticos", consultaDto.Diagnosticos);
                command.Parameters.AddWithValue("@Cirugias", consultaDto.Cirugias);
                command.Parameters.AddWithValue("@Alergias", consultaDto.Alergias);

                // Parámetros para tablas de uno a uno (JSON)
                command.Parameters.AddWithValue("@AntecedentesFamiliares", consultaDto.ConsultaAntecedentesFamiliares);
                command.Parameters.AddWithValue("@OrganosSistemas", consultaDto.ConsultaOrganosSistemas);
                command.Parameters.AddWithValue("@ExamenFisico", consultaDto.ConsultaExamenFisico);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return (reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }

            return (0, null); // Retornar valor por defecto si no se obtiene un resultado
        }
    }
}
