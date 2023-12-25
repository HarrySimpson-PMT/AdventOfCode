namespace AdventOfCode.Year2022 {
    public class Day13 : Day {
        public Day13(int today) : base(today) { }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            List<Packet> packets = new();
            int sum = 0;
            int set = 1;

            bool reset = false;
            for (int i = 0; i < data.Length; i++) {
                packets.Add(new ListPacket(data[i]));
                if (reset) {
                    sum += packets[i - 1].CompareTo(packets[i]) < 0 ? set : 0;
                    set++;
                }
                reset = !reset;
            }
            result = sum.ToString();
            Console.WriteLine(result);

        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            int sum = 1;
            List<Packet> packets = new();
            data = data.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < data.Length; i++) {
                packets.Add(new ListPacket(data[i]));
            }
            Packet A = new ListPacket("[[2]]");
            Packet B = new ListPacket("[[6]]");
            packets.Add(A);
            packets.Add(B);

            packets.Sort();

            //Console.WriteLine(packets.IndexOf(A));
            //Console.WriteLine(packets.IndexOf(B));

            sum *= packets.IndexOf(A) + 1;
            sum *= packets.IndexOf(B) + 1;

            result = sum.ToString();

        }
    }
    public abstract class Packet : IComparable<Packet> {
        public override string ToString() {
            return base.ToString() ?? throw new NotImplementedException();
        }
        public int CompareTo(Packet? other) {
            if (this is ItemPacket && other is ItemPacket) {
                return ((ItemPacket)this).CompareTo((ItemPacket)other);
            }
            else if (this is ListPacket && other is ListPacket) {
                return ((ListPacket)this).CompareTo((ListPacket)other);
            }

            ListPacket A;
            ListPacket B;

            if (this is ItemPacket)
                A = new ListPacket($"[{(ItemPacket)this}]");
            else
                A = (ListPacket)this;

            if (other == null)
                B = new ListPacket("");
            else if (other is ItemPacket)
                B = new ListPacket($"[{((ItemPacket)other)}]");
            else
                B = (ListPacket)other;

            return A.CompareTo(B);
        }
    }
    public class ItemPacket : Packet {
        public int? Value { get; set; }
        public ItemPacket(int? value) {
            Value = value;
        }
        public override string ToString() => Value.ToString() ?? "NULL";
        public int CompareTo(ItemPacket? other) {
            if (Value == null && other!.Value == null) return 0;
            if (Value != null && other!.Value != null) return ((int)Value).CompareTo((int)other.Value);
            if (other!.Value != null) return -1;
            return 1;
        }
    }
    public class ListPacket : Packet {
        public List<Packet> Packets { get; set; }
        public ListPacket(string value) {
            Packets = new List<Packet>();
            if (value == "") {
                Packets.Add(new ItemPacket(null));
            }
            for (int i = 1; i < value.Length - 1; i++) {
                if (value[i] == '[') {
                    //find closing bracket
                    int j = i;
                    int count = 1;
                    while (count > 0) {
                        j++;
                        if (value[j] == '[') count++;
                        if (value[j] == ']') count--;
                    }
                    //create new listpacket
                    Packets.Add(new ListPacket(value.Substring(i, j - i + 1)));
                    i = j;

                }
                //if value is number get all numbers and parse into int
                else if (char.IsDigit(value[i])) {
                    int j = i;
                    while (char.IsDigit(value[j])) {
                        j++;
                    }
                    Packets.Add(new ItemPacket(int.Parse(value.Substring(i, j - i))));
                    i = j;
                }
            }
        }
        public int CompareTo(ListPacket? other) {
            if (Packets.Count == 0 && other!.Packets.Count == 0) return 0;
            if (Packets.Count != 0 && other!.Packets.Count != 0) {
                int i = 0;
                while (i < Packets.Count && i < other.Packets.Count) {
                    int result = Packets[i].CompareTo(other.Packets[i]);
                    if (result != 0) return result;
                    i++;
                }
                if (i < Packets.Count) return 1;
                if (i < other.Packets.Count) return -1;
                return 0;
            }
            if (Packets.Count == 0) return -1;
            return 1;
        }
        public override string ToString() {
            StringBuilder sb = new();
            sb.Append("[");
            foreach (Packet packet in Packets) {
                sb.Append(packet.ToString());
                sb.Append(",");
            }
            //check if last char is comma
            if (sb[sb.Length - 1] == ',') {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}

