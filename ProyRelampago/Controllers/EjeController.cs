using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RelampagoP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EjeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EjeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("create")]
        public IActionResult CreateEje([FromBody] string nombreEjeEstrategico)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConn")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[ejj].[sp_Eje_CRUD]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "CREATE");
                        command.Parameters.AddWithValue("@nombreEjeEstrategico", nombreEjeEstrategico);

                        var result = command.ExecuteScalar();
                        return Ok(new { NewID = result });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("read")] // Leer todos los registros
        public IActionResult GetEjes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConn")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[ejj].[sp_Eje_CRUD]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "READ");
                        command.Parameters.AddWithValue("@idEje", DBNull.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable table = new DataTable();
                            table.Load(reader);
                            return Ok(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("read/{idEje}")] // Leer un registro específico
        public IActionResult GetEjeById(int idEje)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConn")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[ejj].[sp_Eje_CRUD]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "READ");
                        command.Parameters.AddWithValue("@idEje", idEje);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                return Ok(table);
                            }
                            else
                            {
                                return NotFound("Eje no encontrado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{idEje}")] // Actualizar un registro
        public IActionResult UpdateEje(int idEje, [FromBody] string nombreEjeEstrategico)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConn")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[ejj].[sp_Eje_CRUD]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "UPDATE");
                        command.Parameters.AddWithValue("@idEje", idEje);
                        command.Parameters.AddWithValue("@nombreEjeEstrategico", nombreEjeEstrategico);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                return Ok(table);
                            }
                            else
                            {
                                return NotFound("Eje no encontrado para actualizar.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete/{idEje}")] // Eliminar un registro
        public IActionResult DeleteEje(int idEje)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConn")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[ejj].[sp_Eje_CRUD]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", "DELETE");
                        command.Parameters.AddWithValue("@idEje", idEje);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                return Ok(new { Message = reader["Message"].ToString() });
                            }
                            else
                            {
                                return NotFound("Eje no encontrado para eliminar.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
