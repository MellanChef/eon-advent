using System.Text;

string operationsFile = "operations.txt";

var signal = 1;
var cycleDictionary = new Dictionary<int, int>();
var cycle = 0;
int[] cyclesToLog = [20, 60, 100, 140, 180, 220];

var crt = new StringBuilder();
var currentRow = "";
var position = 1;

List<Tuple<string, int?>> ParseOperations(string input) {
    var dict = new List<Tuple<string, int?>>();
    var arr = input.Split("\n");

    foreach(var op in arr) {
        if (op.StartsWith("noop")) {
            dict.Add(new Tuple<string, int?>(op, null));
            continue;
        }
        
        var opAndVal = op.Split(" ");

        var operation = opAndVal[0];
        var value = int.Parse(opAndVal[1]);
        dict.Add(new Tuple<string, int?>(operation, value));
    }

    return dict;
}

void LogCycle(int cycle, int signal) {
    if (cyclesToLog.Contains(cycle)) {
            var signalStrength = cycle * signal;
            Console.WriteLine($"{cycle}: {signalStrength}");
    }
}

bool shouldAddToDictionary(int cycle) => cyclesToLog.Contains(cycle);

void Draw() {
    var signalToPosition = signal - position;
    if (signalToPosition <= 0 & signalToPosition > -3)
        currentRow += "#";
    else
        currentRow += ".";

    position++;
    if (position == 40) {
        crt.AppendLine(currentRow);
        position = 0;
        currentRow = "";
    }
}

// Run
string operationsText = File.ReadAllText(operationsFile);
var operationsList = ParseOperations(operationsText);

for (int i = 0; i < operationsList.Count; i++) {    
    LogCycle(cycle, signal);
    Draw();
    cycle++;

    if (shouldAddToDictionary(cycle))
        cycleDictionary.Add(cycle, cycle * signal);

    var operation = operationsList[i];
    if (operation.Item1 == "noop")
        continue;

    LogCycle(cycle, signal);    
    Draw();
    cycle++;

    if (shouldAddToDictionary(cycle)) {
        cycleDictionary.Add(cycle, cycle * signal);
    }
        
    signal += operation.Item2.Value;
}

var total = cycleDictionary.Sum(x => x.Value);
Console.WriteLine($"Total: {total}");
Console.WriteLine("-------------------");

/* foreach (var row in crt) {
    Console.WriteLine(row);
} */

Console.Write(crt.ToString());