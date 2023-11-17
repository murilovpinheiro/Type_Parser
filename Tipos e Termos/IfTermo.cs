using System;

namespace Termos{
    public class If : Termo
    {
        public Termo Condicao { get; }
        public Termo RamoTrue { get; }
        public Termo RamoFalse { get; }

        public If(Termo condicao, Termo ramoTrue, Termo ramoFalse) : base("if")
        {
            Condicao = condicao;
            RamoTrue = ramoTrue;
            RamoFalse = ramoFalse;
        }
    }
}