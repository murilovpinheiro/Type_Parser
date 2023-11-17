using System;

namespace Tipagem{

    public class Seta : Tipo
    {
        public Tipo Tipo1 { get; }
        public Tipo Tipo2 { get; }

        public Seta(Tipo tipo1, Tipo tipo2) : base("seta")
        {
            Tipo1 = tipo1;
            Tipo2 = tipo2;
        }
    }
}