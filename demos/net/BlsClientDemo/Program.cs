using Gna.Bl.Service.Client;

namespace BlsClientDemo
{
    internal partial class Program
    {
        const string url = "https://test.bls.avionworx.aero/";
        const string env = "CrewAppHst";
        const string blueprint = "master";
        const string apiKey = "764a3255-b55b-435e-adab-fa7eaeca8dea";
        const string email = "abc@crewapp.is";

        /// Demonstration of Gna Bls client usage
        static async Task Main(string[] args)
        { 
            // Create BLS client
            var legalityServiceClient = new GnaBlsClient(url, env, blueprint)
            {
                ApiKey = apiKey,
                Email = email
            };

            // Create evaluation parameters
            // where RosterActivity is data model
            // and Guid is the key type
            var evaluateParameters = new EvaluateParameters<RosterActivity, Guid>();

            // Create some dummy roster data
            Dictionary<Guid, RosterActivity> rosters = DummyRoster.Create("CrewA", "CrewB", "CrewC");

            List<RosterActivity[]> rosterData = [];
            foreach(var roster in rosters.Values.GroupBy(r=>r.CrewNumer))
                rosterData.Add(roster.ToArray());

            // In the below example we send multiple rosters in one call
            
            // set data to evaluate in evaluation parameters 
            evaluateParameters.Data = rosterData.ToArray();
            
            // set keys, so you can match BLS results with your data
            evaluateParameters.Keys = rosterData.Select(r=>r.Select(ra=>ra.ActivityId).ToArray()).ToArray();

            // set what to evaluate (list of definitions)
            evaluateParameters.Labels = [ "FDPlimit", "DutyLength", "RestAfter"];

            // Evaluate
            var ev = await legalityServiceClient.Evaluate<RosterActivity, Guid, DateTime>(evaluateParameters);

            // Print out results
            foreach (var crewResult in ev.Result.GroupBy(r=>rosters[r.Key].CrewNumer))
            {
                Console.WriteLine($"Crew {crewResult.Key}");
                foreach (var result in crewResult)
                {
                    var actCode = rosters[result.Key].Code;
                    Console.WriteLine($"[{actCode}][{result.Index:yyyyMMMdd HH:mm}] {result.Label} = {result.Value} ");
                }
                Console.WriteLine();
            }

        }
    }
}
