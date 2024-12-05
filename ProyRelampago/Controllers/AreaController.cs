using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RelampagoP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AreaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para obtener la conexión a la base de datos desde el appsettings.json
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConn");
            return new SqlConnection(connectionString);
        }

        // Endpoint: Crear Área
        [HttpPost("create")]
        public IActionResult CreateArea([FromBody] string nombreArea)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Area_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "CREATE");
                        command.Parameters.AddWithValue("@nombreArea", nombreArea);

                        var result = command.ExecuteScalar();
                        return Ok(new { Message = "Área creada exitosamente", NewID = result });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al crear el Área", Error = ex.Message });
                }
            }
        }

        // Endpoint: Leer todas las Áreas o una específica
        [HttpGet("read")]
        public IActionResult ReadArea(int? idArea = null)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Area_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "READ");
                        if (idArea.HasValue)
                            command.Parameters.AddWithValue("@idArea", idArea.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            var results = new List<object>();
                            while (reader.Read())
                            {
                                results.Add(new
                                {
                                    IdArea = reader["idArea"],
                                    NombreArea = reader["nombreArea"]
                                });
                            }
                            return Ok(results);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al leer las Áreas", Error = ex.Message });
                }
            }
        }

        // Endpoint: Actualizar Área
        [HttpPut("update/{idArea}")]
        public IActionResult UpdateArea(int idArea, [FromBody] string nombreArea)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Area_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "UPDATE");
                        command.Parameters.AddWithValue("@idArea", idArea);
                        command.Parameters.AddWithValue("@nombreArea", nombreArea);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new
                                {
                                    IdArea = reader["idArea"],
                                    NombreArea = reader["nombreArea"]
                                });
                            }
                            return NotFound(new { Message = "Área no encontrada" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al actualizar el Área", Error = ex.Message });
                }
            }
        }

        // Endpoint: Eliminar Área
        [HttpDelete("delete/{idArea}")]
        public IActionResult DeleteArea(int idArea)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("ejj.sp_Area_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "DELETE");
                        command.Parameters.AddWithValue("@idArea", idArea);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new { Message = reader["Message"].ToString() });
                            }
                            return NotFound(new { Message = "Área no encontrada" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error al eliminar el Área", Error = ex.Message });
                }
            }
        }
    }
}