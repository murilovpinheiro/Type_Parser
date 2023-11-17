using System;

namespace Termos{
    
// Define conjuntamente pois uma depende diretamente da outra e achei melhor assim

    public class Var
    {
        public string Nome { get; }

        public Var(string nome)
        {
            Nome = nome;
        }
    }
    
    public class VarTermo : Termo
    {
        public Var Var { get; }

        public VarTermo(Var var) : base("var")
        {
            Var = var;
        }
    }
}