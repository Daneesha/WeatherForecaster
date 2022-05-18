using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ForecasterWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            ArrayList Summaries = new ArrayList();

            string query = @"Select Summary from dbo.Summary";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WeatherForecastAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    foreach (DataRow dr in table.Rows)
                    {
                        Summaries.Add(Convert.ToString(dr["Summary"]));
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Count)].ToString()
            })
            .ToArray();
        }
    }
}