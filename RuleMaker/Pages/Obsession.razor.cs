using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using NRules;
using NRules.RuleSharp;
using RuleMaker.Models;
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

        private async Task<ISession> Setup()
        {
            var repository = new RuleRepository();
            repository.AddNamespace("System");
            repository.AddNamespace("System.Linq");
            repository.AddNamespace("System.Collections.Generic");
            repository.AddNamespace("RuleMaker");
            repository.AddNamespace("RuleMaker.Models");
            repository.AddReference(typeof(ScoringGame).Assembly);
            repository.AddReference(typeof(Row).Assembly);


            var data = await Http.GetStringAsync(string.Format("data/obsession.rul?{0}", DateTime.UtcNow.Ticks));

            repository.LoadText(data);

            var factory = repository.Compile();
            var session = factory.CreateSession();

            return session;
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

                Game = JsonConvert.DeserializeObject<ScoringGame>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ScoreClicked()
        {
            var session = await Setup();

            try
            {
                session.Insert(Game);

                // For each player, calculate their score
                foreach (var p in Players)
                {
                    session.Insert(p);
                    session.InsertAll(p.Rows);
                    session.Fire();

                    Console.WriteLine(string.Format("Player {0}'s score was: {1}", p.Name, p.Score));

                    session.Retract(p);
                    session.RetractAll(p.Rows);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            StateHasChanged();
        }
    }
}