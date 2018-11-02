using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tree_Parallel.Models.TreeAVL
{
    public class AVL3
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

            folhaAux = AdvanceNode(folhaAux, 8, direcao);

            Task task1 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folhaAux, informacao, 0, 1);

            });

            task1.Start();


            folhaAux = AdvanceNode(folhaAux, 8, direcao);

            Task task2 = new Task(() =>
            {
                Int64 value = folha.Value;

                Search(folhaAux, informacao, 0, 2);
            });

            task2.Start();


            folhaAux = AdvanceNode(folhaAux, 8, direcao);

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

        private Leaf init_node(Int64 informacao)
        {
            Leaf node = new Leaf();

            node.Left = null;
            node.Right = null;
            node.Value = informacao;

            return node;
        }

        public void insert(Int64 valor, string log, int i)
        {
            root = Insert(root, valor, log);
        }

        private Leaf Insert(Leaf root, Int64 informacao, string log)
        {
            Leaf novo_node_ptr = null;
            Leaf next_ptr = null;
            Leaf last_ptr = null;

            if (root == null)
            {
                root = init_node(informacao);
            }
            else
            {
                next_ptr = root;

                while (next_ptr != null)
                {
                    last_ptr = next_ptr;

                    if (informacao < next_ptr.Value)
                    {
                        next_ptr = next_ptr.Left;

                    }
                    else if (informacao > next_ptr.Value)
                    {
                        next_ptr = next_ptr.Right;
                    }
                    else if (informacao == next_ptr.Value)
                    {
                        MessageBox.Show("Valor já inserido!");
                    }
                }

                novo_node_ptr = init_node(informacao);

                if (informacao < last_ptr.Value)
                    last_ptr.Left = novo_node_ptr;

                if (informacao > last_ptr.Value)
                    last_ptr.Right = novo_node_ptr;

            }

            balancear_tree(root);

            return root;
        }

        private void balancear_tree(Leaf root)
        {
            Leaf nova_raiz = null;

            nova_raiz = balancear_node(root);
            Int64 xfdsafd = nova_raiz.Value;

            if (nova_raiz != root)
            {
                root = nova_raiz;
            }
        }

        private Leaf balancear_node(Leaf node)
        {
            Leaf node_balanceado = null;

            if (node.Left != null)
                node.Left = balancear_node(node.Left);

            if (node.Right != null)
                node.Right = balancear_node(node.Right);

            Int64 fator = fator_bal(node);

            if (fator >= 2)
            {
                /* pesando pra esquerda */

                if (fator_bal(node.Left) <= -1)
                    node_balanceado = rotationar_esq_dir(node);
                else
                    node_balanceado = rotacionar_esq_esq(node);

            }
            else if (fator <= -2)
            {
                /* pesando pra direita */

                if (fator_bal(node.Right) >= 1)
                    node_balanceado = rotacionar_dir_esq(node);
                else
                    node_balanceado = rotacionar_dir_dir(node);
            }
            else
            {
                node_balanceado = node;
            }

            return node_balanceado;
        }

        private Int64 fator_bal(Leaf node)
        {
            Int64 fator = 0;

            if (node.Left != null)
                fator += altura(node.Left);

            if (node.Right != null)
                fator -= altura(node.Right);

            return fator;
        }
        
        private Int64 altura(Leaf node)
        {
            Int64 altura_esq = 0;
            Int64 altura_dir = 0;

            if (node.Left != null)
                altura_esq = altura(node.Left);

            if (node.Right != null)
                altura_dir = altura(node.Right);

            return max(altura_dir, altura_esq) + 1;
        }

        private Int64 max(Int64 x, Int64 y)
        {
            if (x > y)
                return x;
            return y;
        }

        private Leaf rotacionar_dir_dir(Leaf node)
        {
            //Node b = node;
            //Node a = b.Esquerda;
            //b.Esquerda = a.Direita;
            //a.Direita = b;
            //node = a;

            //return node;

            Leaf temp_ptr = node;
            Leaf dir_ptr = temp_ptr.Right;

            temp_ptr.Right = dir_ptr.Left;
            dir_ptr.Left = temp_ptr;

            return dir_ptr;
        }

        private Leaf rotacionar_esq_esq(Leaf node)
        {
            //Node a = node;
            //Node b = a.Direita;
            //a.Direita = b.Esquerda;
            //b.Esquerda = a;
            //node = b;

            //return node;

            Leaf temp_ptr = node;
            Leaf esq_ptr = temp_ptr.Left;

            temp_ptr.Left = esq_ptr.Right;
            esq_ptr.Right = temp_ptr;

            return esq_ptr;
        }

        private Leaf rotationar_esq_dir(Leaf node)
        {
            Leaf temp_ptr = node;
            Leaf esq_ptr = temp_ptr.Left;
            Leaf dir_ptr = esq_ptr.Right;

            temp_ptr.Left = dir_ptr.Right;
            esq_ptr.Right = dir_ptr.Left;
            dir_ptr.Left = esq_ptr;
            dir_ptr.Right = temp_ptr;

            return dir_ptr;
        }

        private Leaf rotacionar_dir_esq(Leaf node)
        {
            Leaf temp_ptr = node;
            Leaf dir_ptr = temp_ptr.Right;
            Leaf esq_ptr = dir_ptr.Left;

            temp_ptr.Right = esq_ptr.Left;
            dir_ptr.Left = esq_ptr.Right;
            esq_ptr.Right = dir_ptr;
            esq_ptr.Left = temp_ptr;

            return esq_ptr;
        }

        public void PrintOut(Leaf raiz, string log)
        {
            if (raiz != null)
            {
                PrintOut(raiz.Left, log);

                // MessageBox.Show(raiz.Conteudo.ToString());

                log = "Informação: " + raiz.Value.ToString();
                logConsole = logConsole + Environment.NewLine + log;

                PrintOut(raiz.Right, log);
            }
        }
    }
}