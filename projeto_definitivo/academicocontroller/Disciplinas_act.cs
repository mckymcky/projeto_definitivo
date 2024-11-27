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
    public class Disciplinas_act
    {

        private List<Disciplina> bancoDisciplinas = new List<Disciplina>();
        private Disciplina itemDisciplina;

        private string caminhoBanco;
        private string nomeBancoDisciplinas;
        private string caminho;

        private string connectionString = "Server=DESKTOP-MVALNKO\\SQLEXPRESS;Database=SISACADEMICO;Integrated Security=True;";

        public Disciplinas_act()
        {

            caminhoBanco = ConfigurationManager.AppSettings["caminhoBanco"];
            if (caminhoBanco == null)
            {
                caminhoBanco = AppDomain.CurrentDomain.BaseDirectory;
            }

            nomeBancoDisciplinas = ConfigurationManager.AppSettings["nomeBancoDisciplinas"];
            if (string.IsNullOrEmpty(nomeBancoDisciplinas) == false)
            {
                nomeBancoDisciplinas = "disciplinas.csv";
            }

            caminho = caminhoBanco + nomeBancoDisciplinas;

            bancoDisciplinas = CarregarDisciplinasDoCsv();

        }

        public void SalvarDisciplinasEmCsv()
        {
            string caminho = caminhoBanco + nomeBancoDisciplinas;

            try
            {
                using (StreamWriter writer = new StreamWriter(caminho))
                {
                    writer.WriteLine("disid,disnome,dissig,disobs");

                    foreach (var item in bancoDisciplinas)
                    {
                        writer.WriteLine(
                            $"{item.disid},{item.disnome},{item.dissig},{item.disobs}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro" + ex.Message);
            }
        }

        public List<Disciplina> CarregarDisciplinasDoCsv()
        {
            var disciplinas = new List<Disciplina>();

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
                                int disid = int.Parse(partes[0]);
                                string disnome = partes[1];
                                string dissig = partes[2];
                                string disobs = partes[3];
                                disciplinas.Add(new Disciplina
                                {
                                    disid = disid,
                                    disnome = disnome,
                                    dissig = dissig,
                                    disobs = disobs
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
            return disciplinas;
        }

        /*
        public void inserir(Disciplina disciplina)
        {
            bancoDisciplinas.Add(disciplina);
        }
        */

        public void inserir(Disciplina disciplina)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TBLDISCIPLINAS (DISID, DISNOME, DISSIG, DISOBS) VALUES (@DISID, @DISNOME, @DISSIG, @DISOBS)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DISID", disciplina.disid);
                command.Parameters.AddWithValue("@DISNOME", disciplina.disnome);
                command.Parameters.AddWithValue("@DISSIG", disciplina.dissig);
                command.Parameters.AddWithValue("@DISOBS", disciplina.disobs);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void alterar(string sigla, Disciplina disciplina)
        {
            foreach (var item in bancoDisciplinas)
            {
                if (item.dissig == sigla.ToString().Trim())
                {
                    item.dissig = Console.ReadLine();
                    item.disnome = Console.ReadLine();
                    item.disobs = Console.ReadLine();
                    break;
                }
            }
        }
        */

        public void alterar(string sigla, Disciplina disciplina)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE TBLDISCIPLINAS SET DISNOME = @DISNOME, DISSIG = @DISSIG, DISOBS = @DISOBS  WHERE DISSIG = @sigla";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sigla", sigla);
                command.Parameters.AddWithValue("@DISNOME", disciplina.disnome);
                command.Parameters.AddWithValue("@DISSIG", disciplina.dissig);
                command.Parameters.AddWithValue("@DISOBS", disciplina.disobs);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void excluir(string sigla)
        {
            foreach (var item in bancoDisciplinas)
            {
                if (item.dissig == sigla.ToString().Trim())
                {
                    bancoDisciplinas.Remove(item);
                    break;
                }
            }
        }
        */

        public void excluir(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TBLDISCIPLINAS WHERE DISSIG = @DISSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DISSIG", sigla);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /*
        public void pesquisar(string sigla)
        {

            foreach (var item in bancoDisciplinas)
            {
                if (item.dissig == sigla.ToString().Trim())
                {
                    Console.WriteLine(item.disid.ToString()
                        + " - " + item.disnome.ToString()
                        + " - " + (item.dissig.ToString()
                        + " - " + (item.disobs.ToString())));
                }
            }
        }
        */

        public void pesquisar(string sigla)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DISID, DISNOME, DISSIG, DISOBS FROM TBLDISCIPLINAS WHERE DISSIG = @DISSIG";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DISSIG", sigla);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Disciplina disciplina = null;
                if (reader.Read())
                {
                    disciplina = new Disciplina
                    {
                        disid = reader.GetInt32(0),
                        disnome = reader.GetString(1),
                        dissig = reader.GetString(2),
                        disobs = reader.GetString(3)
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
            foreach (var item in bancoDisciplinas)
            {
                Console.WriteLine(item.disid.ToString()
                    + " - " + item.disnome.ToString()
                    + " - " + (item.dissig.ToString()
                    + " - " + (item.disobs.ToString())));
            }
        }
        */

        public void exibirTodos()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DISID, DISNOME, DISSIG, DISOBS FROM TBLDISCIPLINAS";

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
