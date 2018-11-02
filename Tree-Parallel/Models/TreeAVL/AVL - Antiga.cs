using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tree_Parallel.Models.TreeAVL
{
    public class AVL_Antiga
    {
        public Leaf root;

        public string logTela;

        public string logConsole;

        public int pulos;

        public void Initialize()
        {
            root = null;
        }

        public async Task<bool> SearchThread(Leaf folha, Int64 informacao)
        {
            if (informacao < folha.Value)
            {
                MultiThreadin(folha, informacao, 0);
            }
            else if (informacao > folha.Value)
            {
                MultiThreadin(folha, informacao, 1);
            }
            

            return true;
            
        }

        private Boolean MultiThreadin(Leaf folha, Int64 informacao, int direcao)
        {
            Task task0 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folha, informacao, 0, 0);
            });

            task0.Start();

            Leaf folhaAux = folha;

            folhaAux =  AdvanceNode(folhaAux, 8, direcao);

            Task task1 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folhaAux, informacao, 0, 1);
                
            });

            task1.Start();


            folhaAux =  AdvanceNode(folhaAux, 8, direcao);

            Task task2 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folhaAux, informacao, 0, 2);
            });

            task2.Start();


            folhaAux =  AdvanceNode(folhaAux, 8, direcao);

            Task task3 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folhaAux, informacao, 0, 3);
            });

            task3.Start();

            task0.Wait();
            task1.Wait();
            task2.Wait();
            task3.Wait();

            return true;
        }

        public Leaf AdvanceNode(Leaf folhaAux, int contador, int direcao)
        {
            for (int i = 0; i < contador; i++)
            {
                if (direcao == 0)
                {
                    Int64 x = folhaAux.Value;
                    folhaAux = folhaAux.Left;
                    x = folhaAux.Value;
                }
                else
                { 
                    Int64 x = folhaAux.Value;
                    folhaAux = folhaAux.Right;
                    x = folhaAux.Value;
                }
            }

            return folhaAux;
        }

        public Boolean Search(Leaf folha, Int64 informacao, int contPulos, int thread)
        {
            //MessageBox.Show("Elemento: " + informacao.ToString() + "Árvore elemento: " + folha.Informacao.ToString());

            if (folha != null && folha.Value == informacao)
            {
                this.pulos = contPulos;
                MessageBox.Show("Elemento encontrado pela Thread: " + thread.ToString() + " pulos: " + pulos.ToString());
                return true;
            }

            else if (folha.Value > informacao)
            {
                if (folha.Left != null)
                   return Search(folha.Left, informacao, contPulos + 1, thread);
                else
                    return false;
            }
            else
            {
                if (folha.Right != null)
                  return Search(folha.Right, informacao, contPulos + 1, thread);
                else
                    return false;
            }
        }

        private int Retorna_Altura(Leaf folha)
        {
            if (folha == null)
                return -1;
            else
                return folha.Height;
        }

        private int Retorna_Maior(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        private Leaf Rotacional_direita(Leaf folha)
        {
            //Node folhaAuxiliar = folha.Esquerda;
            //folha.Esquerda = folhaAuxiliar.Direita;
            //folhaAuxiliar.Direita = folha;

            //Node q, temp;
            //q = folha.Esquerda;
            //temp = q.Direita;
            //q.Direita = folha;
            //folha.Esquerda = temp;
            //folha = q;

            Leaf tmp = folha.Left;
            folha.Left = tmp.Right;
            tmp.Right = folha;
            
            folha.Height = 1 + Retorna_Maior(Retorna_Altura(folha.Left), Retorna_Altura(folha.Right));

            return tmp;
            //return folhaAuxiliar;
            //return folha;
        }

        // Funcao que faz uma rotacao simples para a esquerda
        private Leaf Rotacional_esquerda(Leaf folha)
        {
            //Node folhaAuxiliar = folha.Direita;
            //folha.Direita = folhaAuxiliar.Esquerda;
            //folhaAuxiliar.Esquerda = folha;

            //Node q, temp;
            //q = folha.Direita;
            //temp = q.Esquerda;
            //q.Esquerda = folha;
            //folha.Direita = temp;
            //folha = q;

            Leaf tmp = folha.Right;
            folha.Right = tmp.Left;
            tmp.Left = folha;
            
            folha.Height = 1 + Retorna_Maior(Retorna_Altura(folha.Left), Retorna_Altura(folha.Right));
            return tmp;
            //return folhaAuxiliar;
            //return folha;
        }

        // Funcao que faz uma rotacao dupla, esquerda/direita
        private Leaf Rotacional_esquerdadireita(Leaf folha)
        {
            //folha.Esquerda = Rotacional_esquerda(folha.Esquerda);
            //folha = Rotacional_direita(folha);
            folha.Left = Rotacional_direita(folha.Left);
            folha = Rotacional_esquerda(folha);
            return folha;
        }

        // Funcao que faz uma rotacao dupla, direita/esquerda
        private Leaf Rotacional_direitaesquerda(Leaf folha)
        {
            //folha.Direita = Rotacional_direita(folha.Direita);
            //folha = Rotacional_esquerda(folha);
            folha.Right = Rotacional_esquerda(folha.Right);
            folha = Rotacional_direita(folha);
            return folha;
        }

        public void insert(Int64 valor, string log, int i)
        {
            root = Insert(root, valor, log);
        }

        private Leaf Insert(Leaf node, Int64 informacao, string log)
        {
            if (node == null)
            {
                Leaf novo = new Leaf();
                novo.Left = null;
                novo.Right = null;
                novo.Value = informacao;
                novo.Height = 0;
                root = novo;

                log = log
                    + Environment.NewLine
                    + "(" + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) + ") "
                    + "- Inserido: "
                    + informacao.ToString();
                logTela = log;

                return novo; 
            }
            else if (informacao == node.Value)
            {
                log = log 
                    + Environment.NewLine 
                    + "(" + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) 
                    + ") " 
                    + "- O elemento: " 
                    + informacao.ToString() 
                    + " já existe na árvore e não foi inserido.";
                logTela = log;

                return node;
            }
            else
            {
                if (informacao < node.Value)
                {

                    node.Left = Insert(node.Left, informacao, log);

                    if (Retorna_Altura(node.Left) - Retorna_Altura(node.Right) == 2)
                    {
                        if (informacao < node.Left.Value)
                            node = Rotacional_direita(node);
                        else
                            node = Rotacional_esquerdadireita(node);
                    }

                }
                else if (informacao > node.Value)
                {
                    node.Right = Insert(node.Right, informacao, log);
                    if (Retorna_Altura(node.Left) - Retorna_Altura(node.Right) == 2)
                    {
                        //int *info_nodo_dir = (int*)nodo->dir->info;
                        if (informacao >= node.Right.Value)
                            node = Rotacional_esquerda(node);
                        else
                            node = Rotacional_direitaesquerda(node);
                    }
                }
                node.Height = 1 + Retorna_Maior(Retorna_Altura(node.Left), Retorna_Altura(node.Right));
                return node;
            }


            //else
            //{
            //    Node RaizAtual = raiz;

            //    if (informacao < node.Informacao)
            //    {
            //        if(node.Esquerda != null)
            //        {
            //            Inserir(node.Esquerda, informacao, log);
            //        }
            //        else
            //        {
            //            Node novo = new Node();
            //            novo.Esquerda = null;
            //            novo.Direita = null;
            //            novo.Informacao = informacao;
            //            novo.Altura = 0;
            //            node.Esquerda = novo;

            //            log = log 
            //                + Environment.NewLine 
            //                + "(" 
            //                + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) 
            //                + ") " 
            //                + "- Inserido: " 
            //                + informacao.ToString() 
            //                + " a <- de: " 
            //                + node.Informacao.ToString();
            //            logTela = log;

            //            if (Retorna_Altura(novo.Esquerda) - Retorna_Altura(novo.Direita) == 2)
            //            {
            //                if (informacao < novo.Informacao)
            //                    node = Rotacional_direita(node);
            //                else
            //                    node = Rotacional_esquerdadireita(node);
            //            }

            //        }
            //    }

            //    else if (informacao > node.Informacao)
            //    {
            //        if (node.Direita != null)
            //        {
            //            Inserir(node.Direita, informacao, log);
            //        }
            //        else
            //        {
            //            Node novo = new Node();
            //            novo.Esquerda = null;
            //            novo.Direita = null;
            //            novo.Informacao = informacao;
            //            node.Direita = novo;

            //            log = log 
            //                + Environment.NewLine 
            //                + "(" 
            //                + (DateTime.Now.TimeOfDay.ToString()).Substring(0, 8) 
            //                + ") " 
            //                + "- Inserido: " 
            //                + informacao.ToString() 
            //                + " a -> de: " 
            //                + node.Informacao.ToString();
            //            logTela = log;
            //        }
            //    }
            //}
        }
        
        public void PrintOut(Leaf raiz, string log)
        {
            if (raiz != null)
            {
                PrintOut(raiz.Left, log);

                // MessageBox.Show(raiz.Conteudo.ToString());

                log = "Informação: " + raiz.Value.ToString();
                logConsole =  logConsole + Environment.NewLine + log;

                PrintOut(raiz.Right, log);
            }
        }
    }
}