using System;

namespace Tipagem{

    public class Tipo{
            public string Nome_tipo;

            public Tipo(string nome_tipo)
            {
                string[] tipos = { "bool", "nat", "sem_tipo", "seta"};
                if (tipos.Contains(nome_tipo))
                {
                    Nome_tipo = nome_tipo;
                }
                else
                {
                    throw new ArgumentException("!");
                }
            }
    }
}