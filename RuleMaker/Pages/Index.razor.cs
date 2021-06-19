using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using static BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2.CollectionResponse;

namespace RuleMaker.Pages
{
    public partial class Index
    {

        [Inject]
        protected HttpClient Http { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Fetch(220308);
        }

        public async Task Fetch(int id)
        {
            var _bggApiClient = new BoardGameGeekXmlApi2Client(Http);

            ThingRequest request = new ThingRequest(new[] { id });
            ThingResponse response = await _bggApiClient.GetThingAsync(request);
            ThingResponse.Item game = response.Result.FirstOrDefault();

            Console.WriteLine(game.Name);

        }
    }
}
