using System;

namespace Termos{
    public class Numero : Termo
    {
        public int Valor { get; }

        public Numero(int valor) : base("numero")
        {
            Valor = valor;
        }
    }
}