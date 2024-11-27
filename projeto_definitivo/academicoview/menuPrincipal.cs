using academicomodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using academicocontroller;

namespace academicoview
{
    public class menuPrincipal
    {
        private Cursos_act cursos = new Cursos_act();
        private Curso curso;

        private Disciplinas_act disciplinas = new Disciplinas_act();
        private Disciplina disciplina;

        private Periodos_act periodos = new Periodos_act();
        private Periodo periodo;

        public menuPrincipal()
        {
            int opcao = 0;

            while (opcao != 9)
            {
                Console.WriteLine("Sistema: Escolas e Faculdades");
                Console.WriteLine("1. Periodos");
                Console.WriteLine("2. Cursos");
                Console.WriteLine("3. Disciplinas");
                Console.WriteLine("9. Sair");
                Console.Write("Digite a opcao: ");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 1)
                {
                    submenuPeriodos();
                }

                if (opcao == 2)
                {
                    submenuCursos();
                }

                if (opcao == 3)
                {
                    submenuDisciplinas();
                }
            }
        }

        private void submenuCursos()
        {
            int subopcao = 0;
            String sigla = Console.ReadLine();
            subopcao = 0;

            while (subopcao != 29)
            {
                Console.WriteLine("20. Inserir");
                Console.WriteLine("21. Alterar");
                Console.WriteLine("22. Excluir");
                Console.WriteLine("23. Pesquisar");
                Console.WriteLine("24. Exibir");
                Console.WriteLine("25. Salvar em Banco");
                Console.WriteLine("29. Sair");
                Console.Write("Digite a subopcao: ");
                subopcao = int.Parse(Console.ReadLine());

                if (subopcao == 20)
                {
                    curso = new Curso();
                    curso.cursoid = int.Parse(Console.ReadLine());
                    curso.cursonome = Console.ReadLine();
                    curso.cursosig = Console.ReadLine();
                    curso.cursoobs = Console.ReadLine();

                    cursos.inserir(curso);

                    break;
                }

                if (subopcao == 21)
                {
                    sigla = Console.ReadLine();
                    curso = new Curso();
                    curso.cursosig = Console.ReadLine();
                    curso.cursonome = Console.ReadLine();
                    curso.cursoobs = Console.ReadLine();

                    cursos.alterar(sigla, curso);
                }

                if (subopcao == 22)
                {
                    sigla = Console.ReadLine();

                    cursos.excluir(sigla);
                }

                if (subopcao == 23)
                {
                    sigla = Console.ReadLine();

                    cursos.pesquisar(sigla);
                }

                if (subopcao == 24)
                {
                    cursos.exibirTodos();
                }

                if (subopcao == 25)
                {
                    cursos.SalvarCursosEmCsv();
                }
            }

        }

        private void submenuPeriodos ()
        {
            int subopcao = 0;
            String sigla = Console.ReadLine();
            subopcao = 0;

            while (subopcao != 19)
            {
                Console.WriteLine("10. Inserir");
                Console.WriteLine("11. Alterar");
                Console.WriteLine("12. Excluir");
                Console.WriteLine("13. Pesquisar");
                Console.WriteLine("14. Exibir");
                Console.WriteLine("15. Salvar banco");
                Console.WriteLine("19. Sair");
                Console.Write("Digite a subopcao: ");
                subopcao = int.Parse(Console.ReadLine());

                if (subopcao == 10)
                {
                    periodo = new Periodo();
                    periodo.perid = int.Parse(Console.ReadLine());
                    periodo.pernome = Console.ReadLine();
                    periodo.persigla = Console.ReadLine();

                    periodos.inserir(periodo);
                }

                if (subopcao == 11)
                {

                    sigla = Console.ReadLine();
                    periodo = new Periodo();
                    periodo.persigla = Console.ReadLine();
                    periodo.pernome = Console.ReadLine();
                    periodos.alterar(sigla, periodo);

                }

                if (subopcao == 12)
                {

                    sigla = Console.ReadLine();

                    periodos.excluir(sigla);

                }

                if (subopcao == 13)
                {

                    sigla = Console.ReadLine();
                    periodos.pesquisar(sigla);
                }

                if (subopcao == 14)
                {
                    periodos.exibirTodos();
                }

                if (subopcao == 15)
                {
                    periodos.SalvarPeriodosEmCsv();
                }
            }
        }

        private void submenuDisciplinas ()
        {
            int subopcao = 0;
            String sigla = Console.ReadLine();
            subopcao = 0;

            while (subopcao != 39)
            {
                Console.WriteLine("30. Inserir");
                Console.WriteLine("31. Alterar");
                Console.WriteLine("32. Excluir");
                Console.WriteLine("33. Pesquisar");
                Console.WriteLine("34. Exibir");
                Console.WriteLine("35. Salvar banco");
                Console.WriteLine("39. Sair");
                Console.Write("Digite a subopcao: ");
                subopcao = int.Parse(Console.ReadLine());

                if (subopcao == 30)
                {
                    disciplina = new Disciplina();
                    disciplina.disid = int.Parse(Console.ReadLine());
                    disciplina.disnome = Console.ReadLine();
                    disciplina.dissig = Console.ReadLine();
                    disciplina.disobs = Console.ReadLine();

                    disciplinas.inserir(disciplina);

                    break;
                }

                if (subopcao == 31)
                {
                    sigla = Console.ReadLine();
                    disciplina = new Disciplina();
                    disciplina.dissig = Console.ReadLine();
                    disciplina.disnome = Console.ReadLine();
                    disciplina.disobs = Console.ReadLine();

                    disciplinas.alterar(sigla, disciplina);
                }

                if (subopcao == 32)
                {
                    sigla = Console.ReadLine();

                    disciplinas.excluir(sigla);
                }

                if (subopcao == 33)
                {
                    sigla = Console.ReadLine();

                    disciplinas.pesquisar(sigla);
                }

                if (subopcao == 34)
                {
                    disciplinas.exibirTodos();
                }

                if (subopcao == 35)
                {
                    disciplinas.SalvarDisciplinasEmCsv();

                }

            }
        }
    }
}
