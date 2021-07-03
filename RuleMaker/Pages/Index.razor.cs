using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using CardboardKnight.Scoring.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RuleMaker.Pages
{
    public partial class Index
    {
        private ScoringType scoringType;
        private Winner winnerType;

        [Inject]
        protected HttpClient Http { get; set; }

        public ThingResponse.Item Game { get; set; }
        public string GameId { get; set; }
        public string JsonOutput { get; set; }

        public string RulOutput { get; set; }

        public ScoringType ScoringType
        {
            get => scoringType;
            set
            {
                scoringType = value;
                Transform();
            }
        }

        public Winner WinnerType
        {
            get => winnerType;
            set
            {
                winnerType = value;
                Transform();
            }
        }



        private void FetchClicked()
        {
            Fetch(int.Parse(GameId));
        }

        public async Task Fetch(int id)
        {
            var _bggApiClient = new BoardGameGeekXmlApi2Client(Http);

            ThingRequest request = new ThingRequest(new[] { id });
            ThingResponse response = await _bggApiClient.GetThingAsync(request);
            ThingResponse.Item game = response.Result.FirstOrDefault();

            Game = game;
            Transform();
        }

        /// <summary>
        /// Takes the game object, settings, and transforms it into JSON+RUL. Call Fetch first
        /// </summary>
        /// <returns></returns>
        public async Task Transform()
        {
            if (Game == null)
                return;

            var scoringGame = new ScoringGame()
            {
                Name = Game.Name,
                MinPlayers = Game.MinPlayers,
                MaxPlayers = Game.MaxPlayers,
                BggId = Game.Id.ToString(),
                Version = 0.1,
                Rows = new System.Collections.Generic.List<Row>()
                {
                    new Row()
                    {
                        Name="",
                    }
                }
            };

            scoringGame.ScoringType = ScoringType;
            scoringGame.Winner = WinnerType;

            JsonOutput = JsonConvert.SerializeObject(scoringGame, Formatting.Indented);
            RulOutput = await Http.GetStringAsync(string.Format("data/BlankRule.txt?{0}", DateTime.UtcNow.Ticks));
            StateHasChanged();
        }
    }
}