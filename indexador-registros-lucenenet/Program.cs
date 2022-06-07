using System;
using System.Collections.Generic;
using System.IO;
using indexador.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace indexador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("+------------------------------------------------+");
            Console.WriteLine("| Indexador de Registro - Lucene.Net - Vrs 1.0.0 |");
            Console.WriteLine("+------------------------------------------------+");
            
            //Ensures index backward compatibility
            const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

            // Construct a machine-independent path for the index
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var indexPath = Path.Combine(basePath, "index");
            Console.WriteLine($"Indexação: {indexPath}");

            using var dir = FSDirectory.Open(new DirectoryInfo(indexPath));
            
            // Create an analyzer to process the text
            var analyzer = new StandardAnalyzer(AppLuceneVersion);

            // Create an index writer
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);

            var listaIndexacao = RecuperarListaIndexacao();
            foreach (var indexacao in listaIndexacao)
            {
                var doc = new Document
                {
                    new Int32Field("id.prestador", indexacao.Id, Field.Store.NO),
                    new Int64Field("codigo.prestador", indexacao.Codigo, Field.Store.NO),
                    new StringField("nome.prestador", indexacao.Nome, Field.Store.YES),
                    new StringField("cpfcnpj.prestador", indexacao.CpfCnpj, Field.Store.YES),
                    new StringField("tipo.prestador", indexacao.TipoPrestador, Field.Store.YES),
                    new StringField("cidade.prestador", indexacao.Cidade, Field.Store.YES),
                    new StringField("estado.prestador", indexacao.Estado, Field.Store.YES),
                    new TextField("especialidades.prestador", string.Join(" ", indexacao.Especialidades), Field.Store.YES),
                    new TextField("busca.prestador", indexacao.ToString(), Field.Store.YES)
                };
                writer.AddDocument(doc);
                writer.Flush(triggerMerge: false, applyAllDeletes: false);
            }

            string query = null;
            while (string.IsNullOrEmpty(query))
            {

                Console.WriteLine("Informe um filtro de pesquisa ou exit (para sair): ");
                query = Console.ReadLine();
                if (query.ToLower() == "exit")
                    break;

                // Search with a phrase
                var phrase = new MultiPhraseQuery()
                {
                    new Term("busca.prestador", query),
                };
                
                using var reader = writer.GetReader(applyAllDeletes: true);
                var searcher = new IndexSearcher(reader);
                var hits = searcher.Search(phrase, 20).ScoreDocs;

                // Display the output in a table
                Console.WriteLine($"{"Score",10}" + $" {"Nome",-40}" + $" {"Cidade",-15}");
                foreach (var hit in hits)
                {
                    var foundDoc = searcher.Doc(hit.Doc);
                    Console.WriteLine($"{hit.Score:f8}" +
                                      $" {foundDoc.Get("nome.prestador"),-40}" +
                                      $" {foundDoc.Get("cidade.prestador"),-15}");
                }
                
                query = null;
                Console.WriteLine("+------------------------------------------------+");
                
            }
            
        }

        /// <summary>
        /// Recupera a lista de Prestadores para indexação do conteúdo.
        /// </summary>
        /// <returns></returns>
        private static List<Prestador> RecuperarListaIndexacao()
        {
            //Carga de exemplo
            var lista = new List<Prestador>();
            lista.Add(new Prestador()
            {
                Id = 1,
                Codigo = 1001,
                Nome = "Hospital 9 de Julho",
                CpfCnpj = "01.001.001/0001-01",
                TipoPrestador = "Hospital",
                Cidade = "São Paulo",
                Estado = "SP",
                Especialidades = new List<Especialidade>(){
                    new Especialidade()
                    {
                        Id = 2,
                        Codigo = 2002,
                        Nome = "Oftalmologia",
                        Descricao = "Oftalmologia",
                        Tags = new List<string>()
                        {
                            "oftalmo", "olhos", "visão", "enxergar", "vista"
                        },
                        Eventos = new List<EventoSaude>()
                        {
                            new EventoSaude()
                            {
                                Id = 3,
                                Estrutura = "3.3.3.3.1",
                                Nome = "Cirurgia de catarata",
                                Tags = new List<string>()
                                {
                                    "visão turva", "visão embaçada", "visão desfocada"
                                }
                            }
                        }
                    }
                }
            });
            return lista;
        }
    }
}