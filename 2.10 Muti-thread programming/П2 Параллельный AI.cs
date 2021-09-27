using System;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot
{ 
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {            
            var bestMoves = new List<Tuple<Turn, double>>();
            var finishedThreads = 0;

            for (var i = 0; i < threadsCount; i++)
            {
                var action = new Action(() =>
                {
                    var rand = new Random();
                    var bestMoveInThread = SearchBestMove(rocket,
                        random, iterationsCount / threadsCount);
                    lock (bestMoves)
                    {
                        bestMoves.Add(bestMoveInThread);
                    }
                    finishedThreads++;
                });
                
                action.BeginInvoke(null, null);
            }
            
            while (finishedThreads != threadsCount) {}
            var bestMove = bestMoves.OrderBy(x => x.Item2).First();
            return rocket.Move(bestMove.Item1, level);
        }
    }
}