using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthGold.Types
{
    public struct CPF
    {
        private readonly string _value;

        private CPF(string value)
        {
            _value = value;
        }

        public static CPF Parse(string value)
        {
            if (TryParse(value, out var result))
            {
                return result;
            }
            throw new ArgumentException();
        }

        public static bool TryParse(string value, out CPF cpf)
        {
            // Implement validation
            cpf = new CPF(value);
            return true;
        }

        public override string ToString()
            => _value;

        public string ToFormattedString()
        {
            // Implement here
            return _value;
        }

        public static implicit operator  CPF(string input)
            => Parse(input);

        public static implicit operator CPF(long input)
        {
            var result = input.ToString();
            if(result.Length == 11)
            {
                return result;
            }
            return "";
        }
    }
}
