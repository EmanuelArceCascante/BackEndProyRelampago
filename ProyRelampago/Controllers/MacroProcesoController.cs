using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RelampagoP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MacroProcesoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MacroProcesoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Conexión a la base de datos
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConn");
            return new SqlConnection(connectionString);
        }

        // Endpoint: Crear Macroproceso
        [HttpPost("create")]
        public IActionResult CreateMacroProceso([FromBody] string nombreMacroproceso)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Macroproceso_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "CREATE");
                        command.Parameters.AddWithValue("@nombreMacroproceso", nombreMacroproceso);

                        var result = command.ExecuteScalar();
                        return Ok(new { Message = "Macroproceso creado exitosamente", NewID = result });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al crear el Macroproceso", Error = ex.Message });
                }
            }
        }

        // Endpoint: Leer todos los Macroprocesos o uno específico
        [HttpGet("read")]
        public IActionResult ReadMacroProceso(int? idMacroproceso = null)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Macroproceso_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "READ");
                        if (idMacroproceso.HasValue)
                            command.Parameters.AddWithValue("@idMacroproceso", idMacroproceso.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            var results = new List<object>();
                            while (reader.Read())
                            {
                                results.Add(new
                                {
                                    IdMacroproceso = reader["idMacroproceso"],
                                    NombreMacroproceso = reader["nombreMacroproceso"]
                                });
                            }
                            return Ok(results);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al leer los Macroprocesos", Error = ex.Message });
                }
            }
        }

        // Endpoint: Actualizar Macroproceso
        [HttpPut("update/{idMacroproceso}")]
        public IActionResult UpdateMacroProceso(int idMacroproceso, [FromBody] string nombreMacroproceso)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Macroproceso_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "UPDATE");
                        command.Parameters.AddWithValue("@idMacroproceso", idMacroproceso);
                        command.Parameters.AddWithValue("@nombreMacroproceso", nombreMacroproceso);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new
                                {
                                    IdMacroproceso = reader["idMacroproceso"],
                                    NombreMacroproceso = reader["nombreMacroproceso"]
                                });
                            }
                            return NotFound(new { Message = "Macroproceso no encontrado" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al actualizar el Macroproceso", Error = ex.Message });
                }
            }
        }

        // Endpoint: Eliminar Macroproceso
        [HttpDelete("delete/{idMacroproceso}")]
        public IActionResult DeleteMacroProceso(int idMacroproceso)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Macroproceso_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "DELETE");
                        command.Parameters.AddWithValue("@idMacroproceso", idMacroproceso);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new { Message = reader["Message"].ToString() });
                            }
                            return NotFound(new { Message = "Macroproceso no encontrado" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al eliminar el Macroproceso", Error = ex.Message });
                }
            }
        }
    }
}