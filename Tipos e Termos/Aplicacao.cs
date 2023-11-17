using System;

namespace Termos{
       public class Aplicacao : Termo{
        public Termo Funcao { get; }
        public Termo Argumento { get; }

        public Aplicacao(Termo funcao, Termo argumento) : base("aplicacao")
        {
            Funcao = funcao;
            Argumento = argumento;
        }
    }
}