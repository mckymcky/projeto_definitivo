using academicomodel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace academicocontroller
{
    public class Periodos_act
    {

        private List<Periodo> bancoPeriodos = new List<Periodo>();
        private Periodo itemPeriodo;

        private string caminhoBanco;
        private string nomeBancoPeriodos;
        private string caminho;

        private string connectionString = "Server=DESKTOP-MVALNKO\\SQLEXPRESS;Database=SISACADEMICO;Integrated Security=True;";

        public Periodos_act()
        {

            caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
            if (caminhoBanco == null)
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }

            nomeBancoPeriodos = ConfigurationManager.AppSettings["nomeBancoPeriodos"];
            if (string.IsNullOrEmpty(nomeBancoPeriodos) == false)
            {
                nomeBancoPeriodos = "periodos.csv";
            }

            caminho = caminhoBanco + nomeBancoPeriodos;

            bancoPeriodos = CarregarPeriodosDoCsv();

        }

        public void SalvarPeriodosEmCsv()
        {
            string caminho = caminhoBanco + nomeBancoPeriodos;

            try
            {
                using (StreamWriter writer = new StreamWriter(caminho))
                {
                    writer.WriteLine("perid,pernome,persigla");

                    foreach (var item in bancoPeriodos)
                    {
                        writer.WriteLine(
                            $"{item.perid},{item.pernome},{item.persigla}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro" + ex.Message);
            }
        }

        public List<Periodo> CarregarPeriodosDoCsv()
        {
            var periodos = new List<Periodo>();

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
                            if (partes.Length == 3)
                            {
                                int perid = int.Parse(partes[0]);
                                string pernome = partes[1];
                                string persigla = partes[2];
                                periodos.Add(new Periodo
                                {
                                    perid = perid,
                                    pernome = pernome,
                                    persigla = persigla
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
            return periodos;
        }

        /*
        public void inserir(Disciplina disciplina)
        {
            bancoDisciplinas.Add(disciplina);
        }
        */

        public void inserir(Periodo periodo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TBLPERIODOS (PERID, PERNOME, PERSIG) VALUES (@PERID, @PERNOME, @PERSIG)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PERID", periodo.perid);
                command.Parameters.AddWithValue("@PERNOME", periodo.pernome);
                command.Parameters.AddWithValue("@PERSIG", periodo.persigla);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*public void alterar(string sigla, Periodo periodo)
        { 
            foreach (var item in bancoPeriodos)
            {
                if (item.persigla == sigla.ToString().Trim())
                {
                    item.persigla = Console.ReadLine();
                    item.pernome = Console.ReadLine();
                    break;
                }
            }
        }*/

        public void alterar(string sigla, Periodo periodo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE TBLPERIODOS SET PERNOME = @PERNOME, PERSIG = @PERSIG WHERE PERSIG = @sigla";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sigla", sigla);
                command.Parameters.AddWithValue("@PERNOME", periodo.pernome);
                command.Parameters.AddWithValue("@PERSIG", periodo.persigla);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void excluir(string sigla)
        {
            foreach (var item in bancoPeriodos)
            {
                if (item.persigla == sigla.ToString().Trim())
                {
                    bancoPeriodos.Remove(item);
                    break;
                }
            }      
        }
        */

        public void excluir(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TBLPERIODOS WHERE PERSIG = @PERSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PERSIG", sigla);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        /*
        public void pesquisar(string sigla)
        {
            foreach (var item in bancoPeriodos)
            {
                if (item.persigla == sigla.ToString().Trim())
                {
                    Console.WriteLine(item.perid.ToString()
                        + " - " + item.pernome.ToString()
                        + " - " + (item.persigla.ToString()));
                }
            }
        }
        */

        public void pesquisar(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT PERID, PERNOME, PERSIG FROM TBLPERIODOS WHERE PERSIG = @PERSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PERSIG", sigla);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Periodo periodo = null;
                if (reader.Read())
                {
                    periodo = new Periodo
                    {
                        perid = reader.GetInt32(0),
                        pernome = reader.GetString(1),
                        persigla = reader.GetString(2)
                    };
                    Console.WriteLine($"ID: {reader.GetInt32(0)}, Nome: {reader.GetString(1)}, Sigla: {reader.GetString(2)}");
                }
                reader.Close();
                connection.Close();
            }
        }

        /*
        public void exibirTodos()
        {
            foreach (var item in bancoPeriodos)
            {
                Console.WriteLine(item.perid.ToString()
                    + " - " + item.pernome.ToString()
                    + " - " + (item.persigla.ToString()));
            }
        }
        */

        public void exibirTodos()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT PERID, PERNOME, PERSIG FROM TBLPERIODOS";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetInt32(0)}, Nome: {reader.GetString(1)}, Sigla: {reader.GetString(2)}");
                }
                reader.Close();
                connection.Close();
            }


        }

    }
}
