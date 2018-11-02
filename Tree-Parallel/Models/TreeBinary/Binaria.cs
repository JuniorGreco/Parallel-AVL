using Tree_Parallel.Views;
using System;
using System.Windows.Forms;

namespace Tree_Parallel.Models.TreeBinary
{
    public class Binaria
    {
        public Leaf raiz;

        public string logTela;

        public string logConsole;

        public void Inicia()
        {
            raiz = null;
        }

        public Boolean Busca(Leaf folha, Int64 informacao)
        {
            if (folha != null && folha.Value == informacao)
                return true;

            else if (folha.Value > informacao)
            {
                if (folha.Left != null)
                   return Busca(folha.Left, informacao);
                else
                    return false;
            }
            else
            {
                if (folha.Right != null)
                  return Busca(folha.Right, informacao);
                else
                    return false;
            }
                
        }

        public void inserir(Int64 valor, string log)
        {
            inserir(raiz, valor, log);
        }

        private void inserir(Leaf nodo, Int64 informacao, string log)
        {
            if (raiz == null)
            {
                var node = new Leaf();
                node.Left = null;
                node.Right = null;
                node.Value = informacao;
                raiz = node;

                log = log + Environment.NewLine + "(" + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) + ") " + "- Inserido: " + informacao.ToString() + " na raíz";
                logTela = log;
            }
            else
            {
                if (informacao < nodo.Value)
                {
                    if(nodo.Left != null)
                    {
                        inserir(nodo.Left, informacao, log);
                    }
                    else
                    {
                        var node = new Leaf();
                        node.Left = null;
                        node.Right = null;
                        node.Value = informacao;
                        nodo.Left = node;

                        log = log + Environment.NewLine + "(" + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) + ") " + "- Inserido: " + informacao.ToString() + " a <- de: " + nodo.Value.ToString();
                        logTela = log;
                    }
                }

                else if (informacao > nodo.Value)
                {
                    if (nodo.Right != null)
                    {
                        inserir(nodo.Right, informacao, log);
                    }
                    else
                    {
                        var node = new Leaf();
                        node.Left = null;
                        node.Right = null;
                        node.Value = informacao;
                        nodo.Right = node;

                        log = log + Environment.NewLine + "(" + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) + ") " + "- Inserido: " + informacao.ToString() + " a -> de: " + nodo.Value.ToString();
                        logTela = log;
                    }
                }
            }
            
        }

        public void Imprime(Leaf raiz, string log)
        {
            if (raiz != null)
            {
                Imprime(raiz.Left, log);

                // MessageBox.Show(raiz.Conteudo.ToString());

                log = "Informação: " + raiz.Value.ToString();
                logConsole =  logConsole + Environment.NewLine + log;

                Imprime(raiz.Right, log);
            }
        }

    }
}