using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using RelampagoP.Models;

namespace RelampagoP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcedimientoController : ControllerBase
    {
        private readonly string _connectionString = "Server=tiusr39pl.cuc-carrera-ti.ac.cr;DataBase=ProyectoRelampago_EJJ;User Id=ejj;Password=K3y708mf$;TrustServerCertificate=True";

        [HttpGet("FilterProcedimientos")]
        public async Task<IActionResult> FilterProcedimientos(
            [FromQuery] int? idEje,
            [FromQuery] int? idArea,
            [FromQuery] int? idDependencia,
            [FromQuery] string? tipoProcedimiento,
            [FromQuery] string? estado,
            [FromQuery] string? teletrabajado,
            [FromQuery] int? idMacroproceso,
            [FromQuery] int? idEjeEstrategico,
            [FromQuery] string? tipoDocumento,
            [FromQuery] string? apoyoTecnologico,
            [FromQuery] int? anioActualizacion)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("[ejj].[Filter_Procedimiento]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@idEje", (object?)idEje ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idArea", (object?)idArea ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idDependencia", (object?)idDependencia ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tipoProcedimiento", (object?)tipoProcedimiento ?? DBNull.Value);
                        command.Parameters.AddWithValue("@estado", (object?)estado ?? DBNull.Value);
                        command.Parameters.AddWithValue("@teletrabajado", (object?)teletrabajado ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idMacroproceso", (object?)idMacroproceso ?? DBNull.Value);
                        command.Parameters.AddWithValue("@idEjeEstrategico", (object?)idEjeEstrategico ?? DBNull.Value);
                        command.Parameters.AddWithValue("@tipoDocumento", (object?)tipoDocumento ?? DBNull.Value);
                        command.Parameters.AddWithValue("@apoyoTecnologico", (object?)apoyoTecnologico ?? DBNull.Value);
                        command.Parameters.AddWithValue("@anioActualizacion", (object?)anioActualizacion ?? DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var procedimientos = new List<dynamic>();

                            while (await reader.ReadAsync())
                            {
                                procedimientos.Add(new
                                {
                                    idProcedimiento = reader["idProcedimiento"],
                                    idEje = reader["idEje"],
                                    idArea = reader["idArea"],
                                    idDependencia = reader["idDependencia"],
                                    tipoProcedimiento = reader["tipoProcedimiento"],
                                    estado = reader["estado"],
                                    teletrabajado = reader["teletrabajado"],
                                    idMacroproceso = reader["idMacroproceso"],
                                    idEjeEstrategico = reader["idEjeEstrategico"],
                                    tipoDocumento = reader["tipoDocumento"],
                                    apoyoTecnologico = reader["apoyoTecnologico"],
                                    anioActualizacion = reader["anioActualizacion"]
                                });
                            }

                            return Ok(procedimientos);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { Message = "Error al ejecutar el procedimiento almacenado.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error inesperado.", Details = ex.Message });
            }
        }
    }
}