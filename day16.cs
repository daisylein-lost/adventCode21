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
                var package = new Packet(trans);

                package.AnalyseTransmission();

                Console.WriteLine("Sum of Version Numbers: {0}", package.getSumOfVersionNumbers());
                Console.WriteLine("Value of transmission: {0}", package.value);
            }
        }

        private class Packet
        {
            private operationType packetType;

            public int packetVersion;

            public long value;

            public string binary;

            List<Packet> subPackages = new List<Packet>();

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
                var allpackages = binary.Substring((int) bitStartIndex.lenghtIndex + (int)bitLength.subPacketNumberLength);

                while(subPackages.Count() < subPacketNumber )
                {
                    allpackages = createSubpackage(allpackages);
                }

                binary = binary.Substring(0, binary.Length- allpackages.Length);
            }

            private void AnalyseSubPacketsByTotalLenght()
            {
                var rawLength = binary.Substring((int) bitStartIndex.lenghtIndex, (int)bitLength.totalPacketLength);
                var length = Convert.ToInt32(rawLength, 2);
                var allpackages = binary.Substring((int) bitStartIndex.lenghtIndex + (int)bitLength.totalPacketLength, length);

                while(!String.IsNullOrEmpty(allpackages))
                {
                    allpackages = createSubpackage(allpackages);
                }

                binary = binary.Substring(0, (int) bitStartIndex.lenghtIndex + (int)bitLength.totalPacketLength + length);

            }

            private string createSubpackage(string allpackages)
            {
                var newPackage = new Packet(allpackages);
                newPackage.AnalyseTransmission();
                allpackages = allpackages.Substring(newPackage.binary.Length);
                subPackages.Add(newPackage);

                return allpackages;
            }

            private void setOperationValue()
            {
                switch (packetType)
                {
                    case operationType.sum:
                                                value = subPackages.Select(s => s.value).Sum();
                                                break;
                    case operationType.product:    
                                                value = subPackages.Count > 1 ? subPackages.Select(s => s.value).Aggregate((a, x) => a * x) : subPackages[0].value;
                                                break;
                    case operationType.minimum:
                                                value = subPackages.Select(s => s.value).Min();
                                                break;
                    case operationType.maximum:
                                                value = subPackages.Select(s => s.value).Max();
                                                break;
                    case operationType.literal:
                                                value = getLiteralBinary();
                                                break;
                    case operationType.greaterThan:
                                                value = subPackages[0].value > subPackages[1].value ? 1 : 0;
                                                break;
                    case operationType.lessThan:
                                                value = subPackages[0].value < subPackages[1].value ? 1 : 0;
                                                break;
                    case operationType.equalTo:
                                                value = subPackages[0].value == subPackages[1].value ? 1 : 0;
                                                break;
                    default:
                            throw new ArgumentOutOfRangeException();
                }
            }

            private long getLiteralBinary()
            {
                var transmission = binary.Substring((int)bitStartIndex.literalIndex);
                var packageString = String.Empty;
                int bitPrefix;
                var packageNumber = 0;
                do
                {
                    var bitGroup =  transmission.Substring(packageNumber*(int)bitLength.literalBitLength, (int)bitLength.literalBitLength);
                    bitPrefix = int.Parse(bitGroup[0].ToString());
                    packageString +=  bitGroup.Substring(1);
                    packageNumber++;
                } while (bitPrefix != 0);

                binary = binary.Substring(0, (int)bitStartIndex.literalIndex+packageNumber*(int)bitLength.literalBitLength);

                return Convert.ToInt64(packageString, 2);
            }

            public int getSumOfVersionNumbers()
            { 
                return subPackages.Any() ? packetVersion + subPackages.Select(sp => sp.getSumOfVersionNumbers()).Sum() : packetVersion;
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