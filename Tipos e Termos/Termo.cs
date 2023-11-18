using System;
using System.Collections.Generic;

namespace Termos{
    public class Termo
    {
        public string Nome_termo;

        public Termo(string nome_termo)
        {
            List<string> termos = new List<string> { "true", "false", "suc", "pred", "ehzero",
                                                     "numero", "if", "lambda", "aplicacao", "var"};
            if (termos.Contains(nome_termo))
            {
                Nome_termo = nome_termo;
            }
            else
            {
                throw new ArgumentException("!");
            }
        }
    }
}