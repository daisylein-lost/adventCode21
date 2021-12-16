namespace adventCode21
{
    public class day16
    {
        private static bool real = true;
        
        private string file = real ? "day16_input.txt" : "day16_inputTest.txt";
        
        private static Dictionary<char,string> hexToBinary = new Dictionary<char,string> {
            {'0', "0000"},
            {'1', "0001"},
            {'2', "0010"},
            {'3', "0011"},
            {'4', "0100"},
            {'5', "0101"},
            {'6', "0110"},
            {'7', "0111"},
            {'8', "1000"},
            {'9', "1001"},
            {'A', "1010"},
            {'B', "1011"},
            {'C', "1100"},
            {'D', "1101"},
            {'E', "1110"},
            {'F', "1111"},
        };

        public void execute()
        {
            var rawTransmissions = InputConverter.getInput(file);

            var transmissions = rawTransmissions.Select(t => String.Join(String.Empty,t.ToCharArray().Select(c => hexToBinary[c]))).ToList();

            foreach (var trans in transmissions)
            {
                var packet = new Packet(trans);

                packet.AnalyseTransmission();

                Console.WriteLine("Sum of Version Numbers: {0}", packet.getSumOfVersionNumbers());
                Console.WriteLine("Value of transmission: {0}", packet.value);
            }
        }

        private class Packet
        {
            private operationType packetType;

            public int packetVersion;

            public long value;

            public string binary;

            List<Packet> subPackets = new List<Packet>();

            public Packet(string transmission)
            {
                binary = transmission;
                packetVersion = Convert.ToInt32(binary.Substring(0,3), 2);
                var rawType = Convert.ToInt32(binary.Substring(3,3), 2);
                packetType = (operationType)rawType;
            }

            public void AnalyseTransmission()
            {
                if(packetType != operationType.literal)
                {
                    var lengthTypeId = int.Parse(binary[(int)bitStartIndex.lengthIdIndex].ToString());

                    if(lengthTypeId == 0)
                    {
                        AnalyseSubPacketsByTotalLenght();
                    }
                    else
                    {
                        AnalyseSubPacketsByNumber();
                    }
                }

                setOperationValue();
            }

            private void AnalyseSubPacketsByNumber()
            {
                var rawLength = binary.Substring((int) bitStartIndex.lenghtIndex, (int)bitLength.subPacketNumberLength);
                var subPacketNumber = Convert.ToInt32(rawLength, 2);
                var allpackets = binary.Substring((int) bitStartIndex.lenghtIndex + (int)bitLength.subPacketNumberLength);

                while(subPackets.Count() < subPacketNumber )
                {
                    allpackets = createFirstSubpacketInString(allpackets);
                }

                binary = binary.Substring(0, binary.Length- allpackets.Length);
            }

            private void AnalyseSubPacketsByTotalLenght()
            {
                var rawLength = binary.Substring((int) bitStartIndex.lenghtIndex, (int)bitLength.totalPacketLength);
                var length = Convert.ToInt32(rawLength, 2);
                var allpackets = binary.Substring((int) bitStartIndex.lenghtIndex + (int)bitLength.totalPacketLength, length);

                while(!String.IsNullOrEmpty(allpackets))
                {
                    allpackets = createFirstSubpacketInString(allpackets);
                }

                binary = binary.Substring(0, (int) bitStartIndex.lenghtIndex + (int)bitLength.totalPacketLength + length);

            }

            private string createFirstSubpacketInString(string allpackets)
            {
                var newPackets = new Packet(allpackets);
                newPackets.AnalyseTransmission();
                allpackets = allpackets.Substring(newPackets.binary.Length);
                subPackets.Add(newPackets);

                return allpackets;
            }

            private void setOperationValue()
            {
                switch (packetType)
                {
                    case operationType.sum:
                                                value = subPackets.Select(s => s.value).Sum();
                                                break;
                    case operationType.product:    
                                                value = subPackets.Count > 1 ? subPackets.Select(s => s.value).Aggregate((a, x) => a * x) : subPackets[0].value;
                                                break;
                    case operationType.minimum:
                                                value = subPackets.Select(s => s.value).Min();
                                                break;
                    case operationType.maximum:
                                                value = subPackets.Select(s => s.value).Max();
                                                break;
                    case operationType.literal:
                                                value = getLiteralBinary();
                                                break;
                    case operationType.greaterThan:
                                                value = subPackets[0].value > subPackets[1].value ? 1 : 0;
                                                break;
                    case operationType.lessThan:
                                                value = subPackets[0].value < subPackets[1].value ? 1 : 0;
                                                break;
                    case operationType.equalTo:
                                                value = subPackets[0].value == subPackets[1].value ? 1 : 0;
                                                break;
                    default:
                            throw new ArgumentOutOfRangeException();
                }
            }

            private long getLiteralBinary()
            {
                var transmission = binary.Substring((int)bitStartIndex.literalIndex);
                var binaryString = String.Empty;
                int bitPrefix;
                var bitgroupsNumber = 0;
                do
                {
                    var bitGroup =  transmission.Substring(bitgroupsNumber*(int)bitLength.literalBitLength, (int)bitLength.literalBitLength);
                    bitPrefix = int.Parse(bitGroup[0].ToString());
                    binaryString +=  bitGroup.Substring(1);
                    bitgroupsNumber++;
                } while (bitPrefix != 0);

                binary = binary.Substring(0, (int)bitStartIndex.literalIndex+bitgroupsNumber*(int)bitLength.literalBitLength);

                return Convert.ToInt64(binaryString, 2);
            }

            public int getSumOfVersionNumbers()
            { 
                return subPackets.Any() ? packetVersion + subPackets.Select(sp => sp.getSumOfVersionNumbers()).Sum() : packetVersion;
            }

            enum operationType
            {
                sum = 0,
                product = 1,
                minimum = 2,
                maximum = 3,
                literal = 4,

                greaterThan = 5,

                lessThan = 6,

                equalTo = 7
            }

            enum bitLength
            {
                literalBitLength = 5,
                totalPacketLength = 15,

                subPacketNumberLength = 11,
            }

            enum bitStartIndex
            {
                versionIndex = 0,
                typeIdIndex = 3,
                lengthIdIndex = 6,
                literalIndex = 6,
                lenghtIndex = 7,
            }
        }
    }
}