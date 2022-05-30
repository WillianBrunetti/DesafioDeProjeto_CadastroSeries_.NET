using System;
using System.Collections.Generic;
using DIO.Series;

class Program
{
    static SerieRepositorio repositorio = new SerieRepositorio();
    static void Main(string[] args)
    {
        string opcaoUsuario = ObterOpcaoUsuario();

        while (opcaoUsuario.ToUpper() != "X")
        {
            switch (opcaoUsuario.ToUpper())
            {
                case "1":
                    ListarSeries();
                    break;
                case "2":
                    InserirSeries();
                    break;
                case "3":
                    AtualizarSerie();
                    break;
                case "4":
                    ExcluirSerie();
                    break;
                case "5":
                    VizualizarSerie();
                    break;
                case "C":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Opção invalida!!.");
                    break;
            }
            opcaoUsuario = ObterOpcaoUsuario();
        }
        Console.WriteLine("Obrigado por utilizar nossos serviços.");
        Console.ReadLine();
    }

    private static void ListarSeries()
    {
        System.Console.WriteLine("Listar Series");

        var lista = repositorio.Lista();

        if (!lista.Any())
        {
            System.Console.WriteLine("Nenhuma Serie Cadastrada.");
            return;
        }

        foreach (var serie in lista)
        {
            var excluido = serie.retornaExcluido();
            System.Console.WriteLine("#ID {0}: {1} {2}", serie.retornaId(), serie.retornaTitulo(), (excluido ? "*Excluido*" : ""));
        }
    }

    private static string ListarGeneros()
    {
        foreach (int i in Enum.GetValues(typeof(Genero)))
        {
            System.Console.WriteLine("{0} {1}", i, Enum.GetName(typeof(Genero), i));
        }

        System.Console.WriteLine("Digite o Genero entre as opções acima: ");
        return Console.ReadLine();
    }
    private static void Erro()
    {
        System.Console.WriteLine("Opção inválida.");
        System.Console.WriteLine("Precione qualquer botão.");
        Console.ReadKey();
        Console.Clear();
    }

    private static string EntradaAno()
    {
        System.Console.WriteLine("Digite o Ano de inicio da Série: ");
        return Console.ReadLine();
    }

    private static (Genero, string, string, int) AtribuirInformacoesSerie()
    {
        string entradaGenero;
        string entradaAno;
        string entradaTitulo;
        string entradaDescricao;
        int genero;
        int ano;
        do
        {
            entradaGenero = ListarGeneros();
            if (!int.TryParse(entradaGenero, out genero))
            {
                Erro();
            }
            else if (genero < 1 || genero > 13)
            {
                Erro();
            }
            else
                break;
        } while (true);

        do
        {
            entradaTitulo = ValidarTitulo();
            if (string.IsNullOrWhiteSpace(entradaTitulo))
                Erro();
            else
                break;
        } while (true);

        do
        {
            entradaAno = EntradaAno();
            if (!int.TryParse(entradaAno, out ano))
            {
                Erro();
            }
            else if (ano < 1900 || ano > DateTime.Now.Year)
            {
                Erro();
            }
            else
                break;
        } while (true);

        do
        {
            entradaDescricao = ValidarDescricao();
            if (string.IsNullOrWhiteSpace(entradaDescricao))
                Erro();
            else
                break;
        } while (true);

        return ((Genero)genero, entradaTitulo, entradaDescricao, ano);
    }

    private static void InserirSeries()
    {
        System.Console.WriteLine("Inserir Serie Nova");
        var (genero, entradaTitulo, entradaDescricao, entradaAno) = AtribuirInformacoesSerie();
        Serie novaSerie = new Serie(id: repositorio.ProximoId(),
                                    genero: (Genero)genero,
                                    titulo: entradaTitulo,
                                    ano: entradaAno,
                                    descricao: entradaDescricao);
        repositorio.Insere(novaSerie);

    }

    private static void AtualizarSerie()
    {
        string indiceSerie;
        int iserie;
        Serie? serie = null;
        do
        {
            indiceSerie = IndicaSerie();
            if (!int.TryParse(indiceSerie, out iserie) || string.IsNullOrWhiteSpace(indiceSerie))
            {
                Erro();
            }
            else
            {
                serie = repositorio.BuscarPorId(iserie);
                if (serie == null)
                    Erro();
                else
                    break;
            }
        } while (true);

        System.Console.WriteLine("Atualizar série");
        var (genero, entradaTitulo, entradaDescricao, entradaAno) = AtribuirInformacoesSerie();

        serie.Atualizar(genero, entradaTitulo, entradaDescricao, entradaAno);
        repositorio.Atualiza(iserie, serie);

    }

    private static string IndicaSerie()
    {
        System.Console.WriteLine("Digite o Id da série: ");
        return Console.ReadLine();

    }

    private static void ExcluirSerie()
    {
        string indiceSerie;
        int Idexclui;
        Serie? serie = null;
        do
        {
            indiceSerie = ExcluindoSerie();
            if (!int.TryParse(indiceSerie, out Idexclui) || string.IsNullOrWhiteSpace(indiceSerie))
            {
                Erro();
                continue;
            }

            serie = repositorio.BuscarPorId(Idexclui);
            if (serie == null)
            {
                Erro();
                continue;
            }
            else if (serie.IsExcluido())
            {
                System.Console.WriteLine("Erro!!");
                System.Console.WriteLine("Série ja excluida. Pressione qualquer tecla");
                Console.ReadKey();
                Console.Clear();
                break;
            }

            repositorio.Exclui(Idexclui);
            System.Console.WriteLine("Serie Excluida com sucesso!");
            break;
        } while (true);

    }

    private static string ExcluindoSerie()
    {
        System.Console.WriteLine("Digite o id da série");
        //int indiceSerie = int.Parse(Console.ReadLine();
        return Console.ReadLine();
    }

    private static void VizualizarSerie()
    {
        string indiceVisualizar;
        int idSerie;
        Serie? serie = null;
        //tratar exeption caso usuario der uma entrada com valor invalido
        //var serie = repositorio.RetornaId(idSerie);
        do
        {
            indiceVisualizar = VisualizarId();
            if (!int.TryParse(indiceVisualizar, out idSerie) || string.IsNullOrWhiteSpace(indiceVisualizar))
            {
                Erro();
                continue;
            }
            serie = repositorio.BuscarPorId(idSerie);
            if (serie == null)
            {
                Erro();
                continue;
            }
            repositorio.BuscarPorId(idSerie);
            Console.WriteLine(serie);
            break;

        } while (true);
    }

    private static string VisualizarId()
    {
        System.Console.WriteLine("Digite o id da série:");
        return Console.ReadLine();
    }

    private static string ValidarTitulo()
    {
        System.Console.WriteLine("Digite o Titulo: ");
        return Console.ReadLine();
    }

    private static string ValidarDescricao()
    {
        System.Console.WriteLine("Digite a Descrição da Série: ");
        return Console.ReadLine();
    }

    private static string ObterOpcaoUsuario()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("DIO Series a seu dispor!!");
        System.Console.WriteLine("Informe a opção desejada: ");

        System.Console.WriteLine("1 - Lista Séries.");
        System.Console.WriteLine("2 - Inserir Série.");
        System.Console.WriteLine("3 - Atualizar Série. ");
        System.Console.WriteLine("4 - Excluir Série. ");
        System.Console.WriteLine("5 - Visualizar Série. ");
        System.Console.WriteLine("C - Limpar tela. ");
        System.Console.WriteLine("X - Sair. ");
        System.Console.WriteLine();

        string opcaoUsuario = Console.ReadLine();

        System.Console.WriteLine();
        return opcaoUsuario;
    }
}