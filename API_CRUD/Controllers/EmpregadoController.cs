using API_CRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpregadoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmpregadoController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select IdEmpregado, NomeEmpregado, Departamento,
                            convert(varchar(10),DataInicio,120) as DataInicio,NomeArquivoFoto
                            from
                            dbo.Empregado
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Empregado emp)
        {
            string query = @"
                           insert into dbo.Empregado
                           (NomeEmpregado,Departamento,DataInicio,NomeArquivoFoto)
                    values (@NomeEmpregado,@Departamento,@DataInicio,@NomeArquivoFoto)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NomeEmpregado", emp.NomeEmpregado);
                    myCommand.Parameters.AddWithValue("@Departamento", emp.Departamento);
                    myCommand.Parameters.AddWithValue("@DataInicio", emp.DataInicio);
                    myCommand.Parameters.AddWithValue("@NomeArquivoFoto", emp.NomeArquivoFoto);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Registo incluído com sucesso!");
        }

        [HttpPut]
        public JsonResult Put(Empregado emp)
        {
            string query = @"
                           update dbo.Empregado
                           set NomeEmpregado= @NomeEmpregado,
                            Departamento=@Departamento,
                            DataInicio=@DataInicio,
                            NomeArquivoFoto=@NomeArquivoFoto
                            where IdEmpregado=@IdEmpregado
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdEmpregado", emp.IdEmpregado);
                    myCommand.Parameters.AddWithValue("@NomeEmpregado", emp.NomeEmpregado);
                    myCommand.Parameters.AddWithValue("@Departamento", emp.Departamento);
                    myCommand.Parameters.AddWithValue("@DataInicio", emp.DataInicio);
                    myCommand.Parameters.AddWithValue("@NomeArquivoFoto", emp.NomeArquivoFoto);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Registro atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Empregado
                            where IdEmpregado=@IdEmpregado
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@IdEmpregado", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Registro deletado com sucesso");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
