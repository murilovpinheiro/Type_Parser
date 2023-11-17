using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tipagem;
using Termos;
using ParserNamespace;

namespace Program{
    class Program{
        static void Main(string[] args)
        {
            try
            {
                string[] array_de_tokens;
                var linha = Console.ReadLine();

                if (linha != null) array_de_tokens = linha.Split(' ');
                else throw new ArgumentException("!");
                
                List<string> lista_de_tokens = new List<string>(array_de_tokens);
                
                try{
                    Parser p = new Parser();
                    var (result , listaRestante) = p.TratarTokens(lista_de_tokens);

                    if(listaRestante.Count > 0) throw new ArgumentException("!");
                    
                    Tipo tipos = p.Tipar(result, new List<(Var, Tipo)>());

                    string saidaFinal = p.PrintTipos(tipos);

                    Console.WriteLine(saidaFinal);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
