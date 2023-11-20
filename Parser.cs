using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Termos;
using Tipagem;

namespace ParserNamespace {

    public class Parser{

        public Parser(){}
        public bool ValidarVariavel(string var){
            // Verificar se a variável não é uma palavra reservada
            string[] reserved = {"true", "false", "if", "then", "else", "endif", "suc", "pred",
                                "ehzero", "lambda", "Nat", "bool", "end", ":", ".", "->", "Bool", "End"};
            if (reserved.Contains(var))
            {
                return false;
            }

            // Verificar se a variável não é um número
            if (int.TryParse(var, out _))
            {
                return false;
            }

            // Se não for uma palavra reservada nem um número, a variável é válida
            return true;
        }

        public (Tipo, List<string>) TratarLambda(List<string> token_list)
        {
            string token = token_list.First();
            token_list.RemoveAt(0);
            switch (token)
            {
                case "(":
                    var (term1, new_tokens) = TratarLambda(token_list);
                    var (term2, final_tokens) = TratarLambda(new_tokens);
                    if (final_tokens.First() == ")")
                    {
                        final_tokens.RemoveAt(0);
                        return (new Seta(term1, term2), final_tokens);
                    }
                    else
                    {
                        throw new ArgumentException("!");
                    }
                case "->":
                    return TratarLambda(token_list);
                case "Bool":
                    return (new Tipo("bool"), token_list);
                case "Nat":
                    return (new Tipo("nat"), token_list);

                default:
                    throw new ArgumentException("!");
            }
        }

        public Tipo TiparVariavel(Var variavel, List<(Var, Tipo)> list_var)
        {
            if (list_var == null)
            {
                return new Tipo("sem_tipo");
            }
            else
            {
                var (v, t) = list_var.FirstOrDefault(item => item.Item1.Nome == variavel.Nome);
                list_var.RemoveAt(0);

                if (v != null)
                {
                    return t;
                }
                else
                {
                    return TiparVariavel(variavel, list_var);
                }
            }
        }

        public Tipo Tipar(Termo termo, List<(Var, Tipo)> list_var)
        {
            switch (termo.Nome_termo)
            {
                case "true":
                    return new Tipo("bool");
                case "false":
                    return new Tipo("bool");
                case "numero":
                    return new Tipo("nat");
                case "suc":
                    return new Seta(new Tipo("nat"), new Tipo("nat"));
                case "pred":
                    return new Seta(new Tipo("nat"), new Tipo("nat"));
                case "ehzero":
                    return new Seta(new Tipo("nat"), new Tipo("bool"));
                case "if":
                    if (termo is If iff)
                    {
                        Termo t1 = iff.Condicao;
                        Termo t2 = iff.RamoTrue;
                        Termo t3 = iff.RamoFalse;
                        Tipo tipo1 = Tipar(t1, list_var);
                        Tipo tipo2 = Tipar(t2, list_var);
                        Tipo tipo3 = Tipar(t3, list_var);
                        if (tipo1.Nome_tipo == "bool" && tipo2.Nome_tipo == tipo3.Nome_tipo)
                        {
                            return tipo2;
                        }
                        else
                        {
                            return new Tipo("sem_tipo");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("!");
                    }
                case "var":
                    if (termo is VarTermo vt)
                    {
                        return TiparVariavel(vt.Var, list_var);
                    }
                    else
                    {
                        throw new ArgumentException("!");
                    }
                case "aplicacao":
                    if (termo is Aplicacao app)
                    {
                        var tipo_funcao = Tipar(app.Funcao, list_var);
                        var tipo_argumento = Tipar(app.Argumento, list_var);
                        
                        if(tipo_funcao is Seta seta){

                            if (seta.Tipo1.Nome_tipo == tipo_argumento.Nome_tipo)
                            {
                                return seta.Tipo2;
                            }
                            else
                            {
                                return new Tipo("sem_tipo");
                            }
                        }
                        else {
                            return new Tipo("sem_tipo");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("!");
                    }
                case "lambda":
                    if (termo is Lambda lamb)
                    {
                        Var v = lamb.Var;
                        Tipo t = lamb.Tipo;
                        Termo c = lamb.Corpo;
                        list_var.Insert(0, (v, t));
                        return new Seta(t, Tipar(c, list_var));
                    }
                    else
                    {
                        throw new ArgumentException("!");
                    }
                default:
                    return new Tipo("sem_tipo");
            }
        }

        public (Termo, List<string>) TratarTokens(List<string> token_list)
        {
        if (token_list.Count == 0)
        {
            // Tratar erro: Lista de tokens vazia
            throw new ArgumentException("!");
        }

        string token = token_list.First();
        token_list.RemoveAt(0);

        switch (token)
        {
            case "(":
                var (term1, new_tokens) = TratarTokens(token_list);
                var (term2, final_tokens) = TratarTokens(new_tokens);

                if (final_tokens.Count > 0 && final_tokens.First() == ")")
                {
                    final_tokens.RemoveAt(0);
                    return (new Aplicacao(term1, term2), final_tokens);
                }
                else
                {
                    // Tratar erro: Parêntese de fechamento ausente ou posição inválida
                    throw new ArgumentException("!");
                }

            case "lambda":
                if (token_list.Count < 3)
                {
                    // Tratar erro: Expressão lambda incompleta
                    throw new ArgumentException("!");
                }

                var variable = token_list.First();
                token_list.RemoveAt(0);

                if (!ValidarVariavel(variable))
                {
                    // Tratar erro: Variável inválida
                    throw new ArgumentException("!");
                }

                if (token_list.Count < 2 || token_list[0] != ":")
                {
                    // Tratar erro: Dois pontos ausentes após a variável na expressão lambda
                    throw new ArgumentException("!");
                }

                token_list.RemoveAt(0); // Remover ":"

                var (lambda_type, lambda_tokens) = TratarLambda(token_list);

                if (lambda_tokens.Count > 0 && lambda_tokens.First() == ".")
                {
                    lambda_tokens.RemoveAt(0);
                    var (lambda_term, lambda_tokens2) = TratarTokens(lambda_tokens);

                    if (lambda_tokens2.Count > 0 && lambda_tokens2.First() == "end")
                    {
                        lambda_tokens2.RemoveAt(0);
                        return (new Lambda(new Var(variable), lambda_type, lambda_term), lambda_tokens2);
                    }
                    else
                    {
                        // Tratar erro: "end" ausente após o corpo da expressão lambda
                        throw new ArgumentException("!");
                    }
                }
                else
                {
                    // Tratar erro: Ponto (.) ausente após o tipo na expressão lambda
                    throw new ArgumentException("!");
                }

            case "if":
                var (if_term, if_tokens) = TratarTokens(token_list);

                if (if_tokens.Count > 0 && if_tokens.First() == "then")
                {
                    if_tokens.RemoveAt(0);

                    var (if_term2, if_tokens2) = TratarTokens(if_tokens);

                    if (if_tokens2.Count > 0 && if_tokens2.First() == "else")
                    {
                        if_tokens2.RemoveAt(0);
                        var (if_term3, iffinal_tokens) = TratarTokens(if_tokens2);

                        if (iffinal_tokens.Count > 0 && iffinal_tokens.First() == "endif")
                        {
                            iffinal_tokens.RemoveAt(0);
                            return (new If(if_term, if_term2, if_term3), iffinal_tokens);
                        }
                        else
                        {
                            // Tratar erro: "endif" ausente após o bloco else do if
                            throw new ArgumentException("!");
                        }
                    }
                    else
                    {
                        // Tratar erro: Bloco else ausente no if
                        throw new ArgumentException("!");
                    }
                }
                else
                {
                    // Tratar erro: Bloco then ausente no if
                    throw new ArgumentException("!");
                }

            case "true":
                return (new Termo("true"), token_list);
            case "false":
                return (new Termo("false"), token_list);
            case "suc":
                return (new Termo("suc"), token_list);
            case "ehzero":
                return (new Termo("ehzero"), token_list);
            case "pred":
                return (new Termo("pred"), token_list);

            default:
                if (int.TryParse(token, out int resultado))
                {
                    return (new Numero(resultado), token_list);
                }
                else if (ValidarVariavel(token))
                {
                    return (new VarTermo(new Var(token)), token_list);
                }
                else
                {
                    // Tratar erro: Token inesperado
                    throw new ArgumentException("!");
                }
        }
    }

        public string PrintTipos(Tipo t){
            if (t == null)
            {
                // Tratar erro: Tipo nulo
                throw new ArgumentNullException("!");
            }

            switch (t.Nome_tipo)
            {
                case "bool":
                    return "Bool";
                case "nat":
                    return "Nat";
                case "seta":
                    if (t is Seta s)
                    {
                        string saida1 = PrintTipos(s.Tipo1);
                        string saida2 = PrintTipos(s.Tipo2);
                        return "( " + saida1 + " -> " + saida2 + " )";
                    }
                    else
                    {
                        // Tratar erro: Tipo 'seta' não é uma instância de 'Seta'
                        throw new ArgumentException("!");
                    }
                case "sem_tipo":
                    // Tratar erro: Tipo sem nome
                    throw new ArgumentException("-");
                default:
                    // Tratar erro: Nome de tipo desconhecido
                    throw new ArgumentException("!");
            }
        }
    }
}