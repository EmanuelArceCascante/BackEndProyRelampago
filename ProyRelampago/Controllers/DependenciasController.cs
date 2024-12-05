using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RelampagoP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DependenciasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DependenciasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para obtener la conexión a la base de datos desde el appsettings.json
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConn");
            return new SqlConnection(connectionString);
        }

        // Endpoint: Crear Dependencia
        [HttpPost("create")]
        public IActionResult CreateDependencia([FromBody] string nombreDependencia)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Dependencia_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "CREATE");
                        command.Parameters.AddWithValue("@nombreDependencia", nombreDependencia);

                        var result = command.ExecuteScalar();
                        return Ok(new { Message = "Dependencia creada exitosamente", NewID = result });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al crear la Dependencia", Error = ex.Message });
                }
            }
        }

        // Endpoint: Leer todas las Dependencias o una específica
        [HttpGet("read")]
        public IActionResult ReadDependencia(int? idDependencia = null)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Dependencia_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "READ");
                        if (idDependencia.HasValue)
                            command.Parameters.AddWithValue("@idDependencia", idDependencia.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            var results = new List<object>();
                            while (reader.Read())
                            {
                                results.Add(new
                                {
                                    IdDependencia = reader["idDependencia"],
                                    NombreDependencia = reader["nombreDependencia"]
                                });
                            }
                            return Ok(results);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al leer las Dependencias", Error = ex.Message });
                }
            }
        }

        // Endpoint: Actualizar Dependencia
        [HttpPut("update/{idDependencia}")]
        public IActionResult UpdateDependencia(int idDependencia, [FromBody] string nombreDependencia)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Dependencia_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "UPDATE");
                        command.Parameters.AddWithValue("@idDependencia", idDependencia);
                        command.Parameters.AddWithValue("@nombreDependencia", nombreDependencia);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new
                                {
                                    IdDependencia = reader["idDependencia"],
                                    NombreDependencia = reader["nombreDependencia"]
                                });
                            }
                            return NotFound(new { Message = "Dependencia no encontrada" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al actualizar la Dependencia", Error = ex.Message });
                }
            }
        }

        // Endpoint: Eliminar Dependencia
        [HttpDelete("delete/{idDependencia}")]
        public IActionResult DeleteDependencia(int idDependencia)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Dependencia_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "DELETE");
                        command.Parameters.AddWithValue("@idDependencia", idDependencia);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new { Message = reader["Message"].ToString() });
                            }
                            return NotFound(new { Message = "Dependencia no encontrada" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al eliminar la Dependencia", Error = ex.Message });
                }
            }
        }
    }
}
