using Nest;
using System.Collections.Generic;

namespace SafeDevelopment
{
    public class ElasticsearchService
    {
        private readonly ElasticClient _client;

        public ElasticsearchService(string connectionString)
        {
            var settings = new ConnectionSettings(new Uri(connectionString));
            _client = new ElasticClient(settings);
        }

        public void IndexBook(Book book)
        {
            _client.Index(book);
        }

        public List<Book> SearchBooks(string query)
        {
            var searchResponse = _client.Search<Book>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Match(m => m
                                .Field(f => f.Title)
                                .Query(query)),
                            sh => sh.Match(m => m
                                .Field(f => f.Description)
                                .Query(query))
                        )
                    )
                )
            );

            return searchResponse.Documents.ToList();
        }
    }
}
