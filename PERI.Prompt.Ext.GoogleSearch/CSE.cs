using System;

using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using System.Collections.Generic;

namespace PERI.Prompt.Ext.GoogleSearch
{
    public class CSE
    {
        public string Query { get; set; }
        public string ApiKey { get; set; }
        public string CseId { get; set; }

        public IEnumerable<Result> Search()
        {
            string apiKey = this.ApiKey;
            string searchEngineId = this.CseId;
            CustomsearchService customSearchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = apiKey });
            CseResource.ListRequest listRequest = customSearchService.Cse.List(this.Query ?? string.Empty);
            listRequest.Cx = searchEngineId;
            Search search = listRequest.Execute();
            return search.Items;
        }
    }
}
