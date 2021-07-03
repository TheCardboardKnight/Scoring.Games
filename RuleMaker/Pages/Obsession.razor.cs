using CardboardKnight.Scoring;
using CardboardKnight.Scoring.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RuleMaker.Pages
{
    public partial class Obsession
    {
        [Inject]
        protected HttpClient Http { get; set; }

        public ScoringGame Game { get; set; }
        public List<ScoringPlayer> Players { get; set; }

        public Obsession()
        {
        }

        protected override Task OnInitializedAsync()
        {
            Players = new List<ScoringPlayer>();
            Load();
            return base.OnInitializedAsync();
        }

        public void AddPlayersClicked()
        {
            var rows = Game.Rows.Select(r => new Row() { Name = r.Name }).ToList();
            Players.Add(
                new ScoringPlayer()
                {
                    Rows = rows
                });
        }

        public async Task Load()
        {
            try
            {
                var data = await Http.GetStringAsync(string.Format("data/obsession.json?{0}", DateTime.UtcNow.Ticks));
                Game = ScoreMaster.CreateGame(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ScoreClicked()
        {
            var data = await Http.GetStringAsync(string.Format("data/obsession.rul?{0}", DateTime.UtcNow.Ticks));
            ScoreMaster.Score(Game, Players.ToArray(), data);
            StateHasChanged();
        }
    }
}