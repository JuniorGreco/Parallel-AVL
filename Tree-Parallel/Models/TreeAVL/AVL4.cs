using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tree_Parallel.Models.TreeAVL
{
    public class AVL4
    {
        public Node root;

        public string logTela;

        public string logConsole;

        public int pulos;

        public int numNodesThreads = 16;

        public void Initialize()
        {
            root = null;
        }

        public async Task<bool> SearchThread1(Node folha, Int64 informacao)
        {
            if (informacao < folha.Informacao)
            {
                MultiThreadin1(folha, informacao, 0);
            }
            else if (informacao > folha.Informacao)
            {
                MultiThreadin1(folha, informacao, 1);
            }

            return true;
        }

        public async Task<bool> SearchThread2(Node raiz, Int64 informacao)
        {
            if (informacao < raiz.Informacao)
            {
                MultiThreadin2(raiz, informacao, 0);
            }
            else if (informacao > raiz.Informacao)
            {
                MultiThreadin2(raiz, informacao, 1);
            }

            return true;
        }

        private Boolean MultiThreadin1(Node raiz, Int64 informacao, int direcao)
        {
            int height = root.Altura;
            int numThread = 0;

            Task task0 = new Task(() =>
            {
                Int64 value = raiz.Informacao;

                Search(raiz, informacao, 0, 0);
            });

            task0.Start();

            Node folhaAux = raiz;

            while (height >= numNodesThreads)
            {
                numThread += 1;

                folhaAux = AdvanceNode1(folhaAux, 8, direcao);

                Task task = new Task(() => Search(raiz, informacao, 0, numThread));

                task.Start();

                height -= numNodesThreads;
            }

            task0.Wait();
            return true;
        }

        private Boolean MultiThreadin2(Node raiz, Int64 informacao, int direcao)
        {
            int numThread = 2;

            Task threadPrincipal = new Task(() =>
            {
                Search(raiz, informacao, 0, 1);
            });

            threadPrincipal.Start();

            Node raizTemporaria = raiz;

            raizTemporaria = AdvanceNode2(raizTemporaria, informacao, direcao);

            Task threadAuxiliar = new Task(() => Search(raiz, informacao, 0, numThread)); 

            if (raizTemporaria != null)
                threadAuxiliar.Start();
           
            threadPrincipal.Wait();

            //if (raizTemporaria != null)
            //threadAuxiliar.Wait();

            return true;
        }

        public Node AdvanceNode1(Node folhaAux, int contador, int direcao)
        {
            for (int i = 0; i < contador; i++)
            {
                if (direcao == 0)
                {
                    Int64 x = folhaAux.Informacao;
                    folhaAux = folhaAux.Esquerda;
                    x = folhaAux.Informacao;
                }
                else
                {
                    Int64 x = folhaAux.Informacao;
                    folhaAux = folhaAux.Direita;
                    x = folhaAux.Informacao;
                }
            }

            return folhaAux;
        }

        public Node AdvanceNode2(Node raiz, Int64 informacao, int direcao)
        {
            int altura = raiz.Altura;

            Node raizAuxiliar = raiz;

            for (int i = 0; i < raiz.Altura; i++)
            {
                altura /= 2;

                if (direcao == 0)
                {
                    Int64 x = raiz.Informacao;

                    for (int j = 0; j < altura; j++)
                        raizAuxiliar = raizAuxiliar.Esquerda;

                    if (informacao < raizAuxiliar.Informacao)
                    {
                        Node raizAuxiliar2 = raizAuxiliar;

                        altura /= 2;

                        for (int j = 0; j < altura; j++)
                            raizAuxiliar2 = raizAuxiliar2.Esquerda;

                        if (informacao < raizAuxiliar2.Informacao)
                        {
                            for (int j = 0; j < altura; j++)
                            {
                                if (informacao < raizAuxiliar2.Informacao)
                                    raizAuxiliar2 = raizAuxiliar2.Esquerda;
                                else
                                    return raizAuxiliar2;
                            }
                        }
                        else
                            return raizAuxiliar;

                    }
                    else
                    {
                        altura /= 2;

                        Node raizAuxiliar2 = raiz;
                        raizAuxiliar = raizAuxiliar2;

                        for (int j = 0; j < altura; j++)
                            raizAuxiliar = raizAuxiliar.Esquerda;

                        if (informacao < raizAuxiliar.Informacao)
                        {
                            for (int j = 0; j < altura + 1; j++)
                            {
                                if (informacao < raizAuxiliar2.Informacao)
                                    raizAuxiliar2 = raizAuxiliar2.Esquerda;
                                else
                                    return raizAuxiliar;
                            }
                        }
                    }
                }
                else if (direcao == 1)
                {
                    Int64 x = raiz.Informacao;

                    for (int j = 0; j < altura; j++)
                        raizAuxiliar = raizAuxiliar.Direita;

                    if (informacao > raizAuxiliar.Informacao)
                    {
                        Node raizAuxiliar2 = raizAuxiliar;

                        altura /= 2;

                        for (int j = 0; j < altura; j++)
                            raizAuxiliar2 = raizAuxiliar2.Direita;

                        if (informacao > raizAuxiliar2.Informacao)
                        {
                            for (int j = 0; j < altura; j++)
                            {
                                if (informacao < raizAuxiliar2.Informacao)
                                    raizAuxiliar2 = raizAuxiliar2.Direita;
                                else
                                    return raizAuxiliar2;
                            }
                        }
                        else
                            return raizAuxiliar;

                    }
                    else
                    {
                        altura /= 2;

                        Node raizAuxiliar2 = raiz;
                        raizAuxiliar = raizAuxiliar2;

                        for (int j = 0; j < altura; j++)
                            raizAuxiliar = raizAuxiliar.Direita;

                        if (informacao > raizAuxiliar.Informacao)
                        {
                            for (int j = 0; j < altura + 1; j++)
                            {
                                if (informacao > raizAuxiliar2.Informacao)
                                    raizAuxiliar2 = raizAuxiliar2.Direita;
                                else
                                    return raizAuxiliar;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public Boolean Search(Node folha, Int64 informacao, int contPulos, int thread)
        {
            //MessageBox.Show("Elemento: " + informacao.ToString() + "Árvore elemento: " + folha.Informacao.ToString());

            if (folha != null && folha.Informacao == informacao)
            {
                this.pulos = contPulos;
                MessageBox.Show("Elemento encontrado pela Thread: " + thread.ToString() + " pulos: " + pulos.ToString());
                return true;
            }

            else if (folha.Informacao > informacao)
            {
                if (folha.Esquerda != null)
                    return Search(folha.Esquerda, informacao, contPulos + 1, thread);
                else
                    return false;
            }
            else
            {
                if (folha.Direita != null)
                    return Search(folha.Direita, informacao, contPulos + 1, thread);
                else
                    return false;
            }
        }

        private int Retorna_Altura(Node folha)
        {
            if (folha == null)
                return -1;
            else
                return folha.Altura;
        }

        private int Retorna_Maior(int a, int b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        private Node Rotacional_direita(Node folha)
        {
            Node tmp = folha.Esquerda;
            folha.Esquerda = tmp.Direita;
            tmp.Direita = folha;

            folha.Altura = 1 + Retorna_Maior(Retorna_Altura(folha.Esquerda), Retorna_Altura(folha.Direita));

            return tmp;
        }

        // Funcao que faz uma rotacao simples para a esquerda
        private Node Rotacional_esquerda(Node folha)
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

            Node tmp = folha.Direita;
            folha.Direita = tmp.Esquerda;
            tmp.Esquerda = folha;

            folha.Altura = 1 + Retorna_Maior(Retorna_Altura(folha.Esquerda), Retorna_Altura(folha.Direita));
            return tmp;
            //return folhaAuxiliar;
            //return folha;
        }

        // Funcao que faz uma rotacao dupla, esquerda/direita
        private Node Rotacional_esquerdadireita(Node folha)
        {
            //folha.Esquerda = Rotacional_esquerda(folha.Esquerda);
            //folha = Rotacional_direita(folha);
            folha.Esquerda = Rotacional_direita(folha.Esquerda);
            folha = Rotacional_esquerda(folha);
            return folha;
        }

        // Funcao que faz uma rotacao dupla, direita/esquerda
        private Node Rotacional_direitaesquerda(Node folha)
        {
            //folha.Direita = Rotacional_direita(folha.Direita);
            //folha = Rotacional_esquerda(folha);
            folha.Direita = Rotacional_esquerda(folha.Direita);
            folha = Rotacional_direita(folha);
            return folha;
        }


        private Node init_node(Int64 informacao)
        {
            Node node = new Node();

            node.Esquerda = null;
            node.Direita = null;
            node.Informacao = informacao;

            return node;
        }

        private Node Insert(Node root, Int64 informacao, string log)
        {
            Node novo_node_ptr = null;
            Node next_ptr = null;
            Node last_ptr = null;

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

                    if (informacao < next_ptr.Informacao)
                    {
                        next_ptr = next_ptr.Esquerda;

                    }
                    else if (informacao > next_ptr.Informacao)
                    {
                        next_ptr = next_ptr.Direita;
                    }
                    else if (informacao == next_ptr.Informacao)
                    {
                        MessageBox.Show("Valor já inserido!");
                    }
                }

                novo_node_ptr = init_node(informacao);

                if (informacao < last_ptr.Informacao)
                    last_ptr.Esquerda = novo_node_ptr;

                if (informacao > last_ptr.Informacao)
                    last_ptr.Direita = novo_node_ptr;

            }

            balancear_tree(root);

            return root;
        }

        private void balancear_tree(Node root)
        {
            Node nova_raiz = null;

            nova_raiz = balancear_node(root);
            Int64 xfdsafd = nova_raiz.Informacao;

            if (nova_raiz != root)
            {
                root = nova_raiz;
            }
        }

        private Node balancear_node(Node node)
        {
            Node node_balanceado = null;

            if (node.Esquerda != null)
                node.Esquerda = balancear_node(node.Esquerda);

            if (node.Direita != null)
                node.Direita = balancear_node(node.Direita);

            Int64 fator = fator_bal(node);

            if (fator >= 2)
            {
                /* pesando pra esquerda */

                if (fator_bal(node.Esquerda) <= -1)
                    node_balanceado = rotationar_esq_dir(node);
                else
                    node_balanceado = rotacionar_esq_esq(node);

            }
            else if (fator <= -2)
            {
                /* pesando pra direita */

                if (fator_bal(node.Direita) >= 1)
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

        private Int64 fator_bal(Node node)
        {
            Int64 fator = 0;

            if (node.Esquerda != null)
                fator += altura(node.Esquerda);

            if (node.Direita != null)
                fator -= altura(node.Direita);

            return fator;
        }

        private Int64 altura(Node node)
        {
            Int64 altura_esq = 0;
            Int64 altura_dir = 0;

            if (node.Esquerda != null)
                altura_esq = altura(node.Esquerda);

            if (node.Direita != null)
                altura_dir = altura(node.Direita);

            return max(altura_dir, altura_esq) + 1;
        }

        private Int64 max(Int64 x, Int64 y)
        {
            if (x > y)
                return x;
            return y;
        }

        private Node rotacionar_dir_dir(Node node)
        {
            Node temp_ptr = node;
            Node dir_ptr = temp_ptr.Direita;

            temp_ptr.Direita = dir_ptr.Esquerda;
            dir_ptr.Esquerda = temp_ptr;

            return dir_ptr;
        }

        private Node rotacionar_esq_esq(Node node)
        {
            Node temp_ptr = node;
            Node esq_ptr = temp_ptr.Esquerda;

            temp_ptr.Esquerda = esq_ptr.Direita;
            esq_ptr.Direita = temp_ptr;

            return esq_ptr;
        }

        private Node rotationar_esq_dir(Node node)
        {
            Node temp_ptr = node;
            Node esq_ptr = temp_ptr.Esquerda;
            Node dir_ptr = esq_ptr.Direita;

            temp_ptr.Esquerda = dir_ptr.Direita;
            esq_ptr.Direita = dir_ptr.Esquerda;
            dir_ptr.Esquerda = esq_ptr;
            dir_ptr.Direita = temp_ptr;

            return dir_ptr;
        }

        private Node rotacionar_dir_esq(Node node)
        {
            Node temp_ptr = node;
            Node dir_ptr = temp_ptr.Direita;
            Node esq_ptr = dir_ptr.Esquerda;

            temp_ptr.Direita = esq_ptr.Esquerda;
            dir_ptr.Esquerda = esq_ptr.Direita;
            esq_ptr.Direita = dir_ptr;
            esq_ptr.Esquerda = temp_ptr;

            return esq_ptr;
        }

        public void PrintOut(Node raiz, string log)
        {
            if (raiz != null)
            {
                PrintOut(raiz.Esquerda, log);

                // MessageBox.Show(raiz.Conteudo.ToString());

                log = "Informação: " + raiz.Informacao.ToString();
                logConsole = logConsole + Environment.NewLine + log;

                PrintOut(raiz.Direita, log);
            }
        }



        /** Retorna a altura da árvore */
        private static int height(Node Node)
        {
            return Node == null ? -1 : Node.Altura;
        }

        /**
        * Retorna o maior valor ente lhs e rhs.
        */
        private static int max(int lhs, int rhs)
        {
            return lhs > rhs ? lhs : rhs;
        }

        /** Retorna o fator de balanceamento da árvore com raiz t */
        private int getFactor(Node t)
        {
            return height(t.Esquerda) - height(t.Direita);
        }

        public Boolean insert2(Int64 x)
        {
            root = insert(x, root);
            return true;
        }

        private Node insert(Int64 informacao, Node node)
        {
            if (node == null)
            {
                node = init_node(informacao);
            }

            else if (informacao < node.Informacao) node.Esquerda = insert(informacao, node.Esquerda);
            else if (informacao > node.Informacao) node.Direita = insert(informacao, node.Direita);

            node = balance(node);

            return node;
        }

        public Node balance(Node node)
        {
            if (getFactor(node) == 2)
            {
                if (getFactor(node.Esquerda) > 0) node = doRightRotation(node);
                else node = doDoubleRightRotation(node);
            }
            else if (getFactor(node) == -2)
            {
                if (getFactor(node.Direita) < 0) node = doLeftRotation(node);
                else node = doDoubleLeftRotation(node);
            }
            node.Altura = max(height(node.Esquerda), height(node.Direita)) + 1;
            return node;
        }

        /** Faz Rotação simples a direita */
        private static Node doRightRotation(Node k2)
        {
            Node k1 = k2.Esquerda;
            k2.Esquerda = k1.Direita;
            k1.Direita = k2;
            k2.Altura = max(height(k2.Esquerda), height(k2.Direita)) + 1;
            k1.Altura = max(height(k1.Esquerda), k2.Altura) + 1;
            return k1;
        }

        /** Rotação simples à esquerda */
        private static Node doLeftRotation(Node k1)
        {
            Node k2 = k1.Direita;
            k1.Direita = k2.Esquerda;
            k2.Esquerda = k1;
            k1.Altura = max(height(k1.Esquerda), height(k1.Direita)) + 1;
            k2.Altura = max(height(k2.Direita), k1.Altura) + 1;
            return k2;
        }

        /** Rotação dupla à direita */
        private static Node doDoubleRightRotation(Node k3)
        {
            k3.Esquerda = doLeftRotation(k3.Esquerda);
            return doRightRotation(k3);
        }

        /** Rotação dupla à esquerda */
        private static Node doDoubleLeftRotation(Node k1)
        {
            k1.Direita = doRightRotation(k1.Direita);
            return doLeftRotation(k1);
        }
    }
}