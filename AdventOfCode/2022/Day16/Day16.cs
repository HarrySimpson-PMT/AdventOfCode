﻿using System.Collections;

namespace AdventOfCode.Year2022 {
    //TODO - Finish my implementation - Need to rewrite implementation using a map of all paths with precalulated shortest distances to each point, toxic but faster than mass simulation lol.

    public class Day16 : Day {

        public Day16(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Solution solution = new();
            //result = solution.PartOne(string.Join("\n", data)).ToString();
            Console.WriteLine(result);
        }

        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Solution solution = new();
            //result = solution.PartTwo(string.Join("\n", data)).ToString();
            Console.WriteLine(result);
        }
    }

    public class VolcanoPressureControlSystem {
        public int VariationsTested { get; set; } = 0;
        public int MaxSimulationOutput { get; set; } = 0;
        public int PressuerizedNodes { get; set; } = 0;

        record Map(int[,] distances, Valve[] valves);
        record Valve(int id, string name, int flowRate, string[] tunnels);
        record Player(Valve Valve, int Distance);

        public Dictionary<string, ControlNode> Nodes { get; set; } = new();
        public SortedSet<string> ValveStateMap { get; set; } = new();
        public Dictionary<string, (int total, int time)> ValveState { get; set; } = new();

        public VolcanoPressureControlSystem(string[] data) {
            foreach (string line in data) {
                Nodes.Add(line.Split(" ")[1], new ControlNode(line));
            }
            foreach (KeyValuePair<string, ControlNode> node in Nodes) {
                if (node.Value.FlowRate > 0)
                    PressuerizedNodes++;
                node.Value.MakeConnections(Nodes);
            }
            CreateValveStateMap();
        }
        public void CreateValveStateMap() {
            foreach (KeyValuePair<string, ControlNode> node in Nodes) {
                if (node.Value.FlowRate > 0) {
                    ValveStateMap.Add(node.Value.Name);
                }
            }
        }
        public string CurrentState {
            get {
                string state = "";
                foreach (string node in ValveStateMap)
                    state += Nodes[node].ValveOpen ? "1" : "0";
                return state;
            }
        }

        [Obsolete("This method SUCKS.")]
        public void BacktrackingMaximizeOpeningSimulation(int total, int currentflow, string currentlocation, int time) {

            VariationsTested++;

            if (VariationsTested % 1000000 == 0) {
                Console.WriteLine($"Variations tested: {VariationsTested}");
            }

            string state = currentlocation + CurrentState;
            if (ValveState.ContainsKey(state)) {
                if (ValveState[state].total >= total) {
                    if (ValveState[state].time <= time)
                        return;
                }
                else
                    ValveState[state] = (total, time);
            }
            else {
                ValveState.Add(state, (total, time));
            }

            if (time == 30) {
                MaxSimulationOutput = Math.Max(MaxSimulationOutput, total);
                return;
            }

            if (Nodes[currentlocation].FlowRate > 0 && !Nodes[currentlocation].ValveOpen) {
                Nodes[currentlocation].ValveOpen = true;
                PressuerizedNodes--;
                BacktrackingMaximizeOpeningSimulation(total + currentflow, currentflow + Nodes[currentlocation].FlowRate, currentlocation, time + 1);
                PressuerizedNodes++;
                Nodes[currentlocation].ValveOpen = false;
            }

            if (PressuerizedNodes != 0) {
                foreach (ControlNode connection in Nodes[currentlocation].NodeConnections) {
                    if (connection.Visited < 3) {
                        connection.Visited++;
                        BacktrackingMaximizeOpeningSimulation(total + currentflow, currentflow, connection.Name, time + 1);
                        connection.Visited--;
                    }
                }
            }
            else {
                total += currentflow * (30 - time);
                if (total > MaxSimulationOutput) {
                    MaxSimulationOutput = total;
                }
                return;
            }
        }
        public class ControlNode {
            public List<string> Connections { get; set; } = new();
            public List<ControlNode> NodeConnections { get; set; } = new();
            public string Name { get; set; }
            public int FlowRate { get; set; }
            public bool Entered = false;
            public bool Exited = false;
            public int Visited = 0;
            public bool ValveOpen { get; set; } = false;
            public ControlNode(string raw) {
                //raw like "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB""
                string[] split = raw.Split(" ");
                Name = split[1];
                FlowRate = int.Parse(split[4].Split("=")[1].TrimEnd(';'));
                for (int i = 9; i < split.Length; i++) {
                    Connections.Add(split[i].TrimEnd(','));
                }
            }
            public override string ToString() {

                return $"{ValveOpen} and has flow rate {FlowRate} and connects to {string.Join(", ", Connections)}";
            }

            public void MakeConnections(Dictionary<string, ControlNode> nodes) {
                foreach (string connection in Connections) {
                    NodeConnections.Add(nodes[connection]);
                }
            }
        }

    }
    class Solution {

        public object PartOne(string input) {
            // fast enough
            return Solve(input, true, 30);
        }
        public object PartTwo(string input) {
            // takes about 10 seconds
            return Solve(input, false, 26);
        }

        int Solve(string input, bool singlePlayer, int time) {
            // initialize and run MaxFlow()

            Map map = Parse(input);

            Valve start = map.valves.Single(x => x.name == "AA");

            BitArray valvesToOpen = new(map.valves.Length);
            for (int i = 0; i < map.valves.Length; i++) {
                if (map.valves[i].flowRate > 0) {
                    valvesToOpen[i] = true;
                }
            }

            if (singlePlayer) {
                // int.MaxValue is used here as a dummy player that doesn't really do anything, it just
                // walks towards the start node
                return MaxFlow(map, 0, 0, new Player(start, 0), new Player(start, int.MaxValue), valvesToOpen, time);
            }
            else {
                return MaxFlow(map, 0, 0, new Player(start, 0), new Player(start, 0), valvesToOpen, time);
            }
        }

        record Map(int[,] distances, Valve[] valves);
        record Valve(int id, string name, int flowRate, string[] tunnels);
        record Player(Valve valve, int distance);

        // Recursively find the maximum available flow in the map by moving the players, opening valves and advancing
        // time according to the task description
        int MaxFlow(
            Map map,               // this is our map as per task input.
            int maxFlow,           // is the current maximum we found (call with 0), this is used internally to shortcut
            int currentFlow,       // the flow produced by the currently investigated steps (on the stack)
            Player player0,        // this is the 'human' player
            Player player1,        // this can be a second player, use distance = int.MaxValue to make it inactive
            BitArray valvesToOpen, // these valves can still be open
            int remainingTime      // and the remaining time
        ) {

            // briefly: we advance the simulation and collect what states the players can go -> recurse
            // use lots of early exits to make this approach practical (Residue is an important concept)

            // One of the players is standing next to a valve:
            if (player0.distance != 0 && player1.distance != 0) {
                throw new ArgumentException();
            }

            // Compute the next states for each player:
            Player[][] nextStatesByPlayer = new Player[2][];

            for (int iplayer = 0; iplayer < 2; iplayer++) {

                Player player = iplayer == 0 ? player0 : player1;

                if (player.distance > 0) {
                    // this player just steps forward towards the valve
                    nextStatesByPlayer[iplayer] = new[] { player with { distance = player.distance - 1 } };

                }
                else if (valvesToOpen[player.valve.id]) {
                    // the player is next to the valve, the valve is still closed, let's open:
                    // (this takes 1 time, so multiply with remainingTime -1)
                    currentFlow += player.valve.flowRate * (remainingTime - 1);

                    if (currentFlow > maxFlow) {
                        maxFlow = currentFlow;
                    }

                    valvesToOpen = new BitArray(valvesToOpen); // copy on write
                    valvesToOpen[player.valve.id] = false;

                    // in the next round this player will take some new target, 
                    // but it already used up it's 1 second this round for opening the valve
                    nextStatesByPlayer[iplayer] = new[] { player };

                }
                else {
                    // the valve is already open, let's try each valves that are still closed:
                    // this is where brancing happens

                    List<Player> nextStates = new();

                    for (int i = 0; i < valvesToOpen.Length; i++) {
                        if (valvesToOpen[i]) {
                            Valve nextValve = map.valves[i];
                            int distance = map.distances[player.valve.id, nextValve.id];
                            // the player moves in this time slot towards the valve, so use distance - 1 here
                            nextStates.Add(new Player(nextValve, distance - 1));
                        }
                    }

                    nextStatesByPlayer[iplayer] = nextStates.ToArray();
                }
            }

            // ran out of time, cannot improve maxFlow
            remainingTime--;
            if (remainingTime < 1) {
                return maxFlow;
            }

            // the is not enough juice left for the remaining time to improve on maxFlow
            // we can shortcut here
            if (currentFlow + Residue(valvesToOpen, map, remainingTime) <= maxFlow) {
                return maxFlow;
            }

            // all is left is going over every possible step combinations for each players:
            for (int i0 = 0; i0 < nextStatesByPlayer[0].Length; i0++) {
                for (int i1 = 0; i1 < nextStatesByPlayer[1].Length; i1++) {

                    player0 = nextStatesByPlayer[0][i0];
                    player1 = nextStatesByPlayer[1][i1];

                    // there is no point in walking to the same valve
                    // if one of the players has other thing to do:
                    if ((nextStatesByPlayer[0].Length > 1 || nextStatesByPlayer[1].Length > 1) && player0.valve == player1.valve) {
                        continue;
                    }

                    // this is an other optimization, if both players are walking
                    // we can advance time until one of them reaches target:
                    int advance = 0;
                    if (player0.distance > 0 && player1.distance > 0) {
                        advance = Math.Min(player0.distance, player1.distance);
                        player0 = player0 with { distance = player0.distance - advance };
                        player1 = player1 with { distance = player1.distance - advance };
                    }

                    maxFlow = MaxFlow(
                        map,
                        maxFlow,
                        currentFlow,
                        player0,
                        player1,
                        valvesToOpen,
                        remainingTime - advance
                    );
                }
            }

            return maxFlow;
        }

        int Residue(BitArray valvesToOpen, Map map, int remainingTime) {
            // Some upper bound for the possible amount of flow that we can
            // still produce in the remaining time. 

            // IT'S JUST AN UPPER BOUND. HEURISTICAL

            // The valves are in decreasing order of flowRate. We open the 
            // first two (we have two players), this takes 1 time then we 
            // step to the next two valves supposing that each valve is 
            // just one step away. Open these as well. Continue until we run 
            // out of time.

            int flow = 0;
            for (int i = 0; i < valvesToOpen.Length; i++) {
                if (valvesToOpen[i]) {
                    if (remainingTime <= 0) {
                        break;
                    }

                    flow += map.valves[i].flowRate * (remainingTime - 1);

                    if ((i & 1) == 0) {
                        remainingTime--;
                    }
                }
            }
            return flow;
        }

        Map Parse(string input) {
            // Valve BB has flow rate=0; tunnels lead to valve CC
            // Valve CC has flow rate=10; tunnels lead to valves DD, EE
            List<Valve> valveList = new();
            foreach (string line in input.Split("\n")) {
                string name = Regex.Match(line, "Valve (.*) has").Groups[1].Value;
                int flow = int.Parse(Regex.Match(line, @"\d+").Groups[0].Value);
                string[] tunnels = Regex.Match(line, "to valves? (.*)").Groups[1].Value.Split(", ").ToArray();
                valveList.Add(new Valve(0, name, flow, tunnels));
            }
            Valve[] valves = valveList
                .OrderByDescending(valve => valve.flowRate)
                .Select((v, i) => v with { id = i })
                .ToArray();

            return new Map(ComputeDistances(valves), valves);
        }

        int[,] ComputeDistances(Valve[] valves) {
            // Bellman-Ford style distance calculation for every pair of valves
            int[,] distances = new int[valves.Length, valves.Length];
            for (int i = 0; i < valves.Length; i++) {
                for (int j = 0; j < valves.Length; j++) {
                    distances[i, j] = int.MaxValue;
                }
            }
            foreach (Valve valve in valves) {
                foreach (string target in valve.tunnels) {
                    Valve targetNode = valves.Single(x => x.name == target);
                    distances[valve.id, targetNode.id] = 1;
                    distances[targetNode.id, valve.id] = 1;
                }
            }

            int n = distances.GetLength(0);
            bool done = false;
            while (!done) {
                done = true;
                for (int source = 0; source < n; source++) {
                    for (int target = 0; target < n; target++) {
                        if (source != target) {
                            for (int through = 0; through < n; through++) {
                                if (distances[source, through] == int.MaxValue || distances[through, target] == int.MaxValue) {
                                    continue;
                                }
                                int cost = distances[source, through] + distances[through, target];
                                if (cost < distances[source, target]) {
                                    done = false;
                                    distances[source, target] = cost;
                                    distances[target, source] = cost;
                                }
                            }
                        }
                    }
                }
            }
            return distances;
        }

    }
}