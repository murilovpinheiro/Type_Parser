using System;
using Tipagem;

namespace Termos{
    public class Lambda : Termo
    {
        public Var Var { get; }
        public Tipo Tipo { get; }
        public Termo Corpo { get; }

        public Lambda(Var var, Tipo tipo, Termo corpo) : base("lambda")
        {
            Var = var;
            Tipo = tipo;
            Corpo = corpo;
        }
    }
}