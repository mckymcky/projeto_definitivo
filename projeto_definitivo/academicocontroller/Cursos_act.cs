using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using academicomodel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;


namespace academicocontroller
{
    public class Cursos_act
    {
        private List<Curso> bancoCursos = new List<Curso>();
        private Curso itemCurso;

        private string caminhoBanco;
        private string nomeBancoCursos;
        private string caminho;

        private string connectionString = "Server=DESKTOP-MVALNKO\\SQLEXPRESS;Database=SISACADEMICO;Integrated Security=True;";

        public Cursos_act()
        {

            caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
            if (caminhoBanco == null)
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }

            nomeBancoCursos = ConfigurationManager.AppSettings["nomeBancoCursos"];
            if (string.IsNullOrEmpty(nomeBancoCursos) == false)
            {
                nomeBancoCursos = "cursos.csv";
            }

            caminho = caminhoBanco + nomeBancoCursos;

            bancoCursos = CarregarCursosDoCsv();

        }


        public void SalvarCursosEmCsv()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(caminho))
                {
                    writer.WriteLine("cursoid,cursonome,cursosig,cursoobs");

                    foreach (var item in bancoCursos)
                    {
                        writer.WriteLine(
                            $"{item.cursoid},{item.cursonome},{item.cursosig},{item.cursoobs}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro" + ex.Message);
            }
        }

        public List<Curso> CarregarCursosDoCsv()
        {
            var cursos = new List<Curso>();

            try
            {
                if (File.Exists(caminho) == true)
                {
                    using (StreamReader reader = new StreamReader(caminho))
                    {
                        string linha = reader.ReadLine();
                        while ((linha = reader.ReadLine()) != null)
                        {
                            var partes = linha.Split(',');
                            if (partes.Length == 4)
                            {
                                int cursoid = int.Parse(partes[0]);
                                string cursonome = partes[1];
                                string cursosig = partes[2];
                                string cursoobs = partes[3];
                                cursos.Add(new Curso
                                {
                                    cursoid = cursoid,
                                    cursonome = cursonome,
                                    cursosig = cursosig,
                                    cursoobs = cursoobs
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro" + ex.Message); throw;
            }
            return cursos;
        }


        /*
        public void inserir(Curso curso)
        {
            bancoCursos.Add(curso);
        }
        */

        public void inserir(Curso curso)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TBLCURSOS (CURSOID, CURSONOME, CURSOSIG, CURSOOBS) VALUES (@CURSOID, @CURSONOME, @CURSOSIG, @CURSOOBS)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CURSOID", curso.cursoid);
                command.Parameters.AddWithValue("@CURSONOME", curso.cursonome);
                command.Parameters.AddWithValue("@CURSOSIG", curso.cursosig);
                command.Parameters.AddWithValue("@CURSOOBS", curso.cursoobs);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void alterar(string sigla, Curso curso)
        {

            foreach (var item in bancoCursos)
            {
                if (item.cursosig == sigla.ToString().Trim())
                {
                    item.cursosig = curso.cursosig;
                    item.cursonome = curso.cursonome;
                    item.cursoobs = curso.cursoobs;
                    break;
                }
            }
        }
        */

        public void alterar(string sigla, Curso curso)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE TBLCURSOS SET CURSONOME = @CURSONOME, CURSOSIG = @CURSOSIG, CURSOOBS = @CURSOOBS  WHERE CURSOSIG = @sigla";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sigla", sigla);
                command.Parameters.AddWithValue("@CURSONOME", curso.cursonome);
                command.Parameters.AddWithValue("@CURSOSIG", curso.cursosig);
                command.Parameters.AddWithValue("@CURSOOBS", curso.cursoobs);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void excluir(string sigla)
        {
            foreach (var item in bancoCursos)
            {
                if (item.cursosig == sigla.ToString().Trim())
                {
                    bancoCursos.Remove(item);
                    break;
                }
            }
        }
        */

        public void excluir(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TBLCURSOS WHERE CURSOSIG = @CURSOSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CURSOSIG", sigla);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void pesquisar(string sigla)
        {
            foreach (var item in bancoCursos)
            {
                if (item.cursosig == sigla.ToString().Trim())
                {
                    Console.WriteLine(item.cursoid.ToString()
                        + " - " + item.cursonome.ToString()
                        + " - " + (item.cursosig.ToString()
                        + " - " + (item.cursoobs.ToString())));
                    break;
                }
            }
        }
        */

        public void pesquisar(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CURSOID, CURSONOME, CURSOSIG, CURSOOBS FROM TBLCURSOS WHERE CURSOSIG = @CURSOSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CURSOSIG", sigla);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Curso curso = null;
                if (reader.Read())
                {
                    curso = new Curso
                    {
                        cursoid = reader.GetInt32(0),
                        cursonome = reader.GetString(1),
                        cursosig = reader.GetString(2),
                        cursoobs = reader.GetString(3)
                    };
                    Console.WriteLine($"ID: {reader.GetInt32(0)}, Nome: {reader.GetString(1)}, Sigla: {reader.GetString(2)}, Obs: {reader.GetString(3)}");
                }
                reader.Close();
                connection.Close();
            }
        }

        /*
        public void exibirTodos()
        {
            foreach (var item in bancoCursos)
            {
                Console.WriteLine(item.cursoid.ToString()
                    + " - " + item.cursonome.ToString()
                    + " - " + (item.cursosig.ToString()
                    + " - " + (item.cursoobs.ToString())));
            }
        }
        */

        public void exibirTodos()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CURSOID, CURSONOME, CURSOSIG, CURSOOBS FROM TBLCURSOS";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetInt32(0)}, Nome: {reader.GetString(1)}, Sigla: {reader.GetString(2)}, Obs: {reader.GetString(3)}");
                }
                reader.Close();
                connection.Close();
            }
        }


    }
}

