using System;

namespace Tree_Parallel.Models
{
    public class Node
    {
        private int altura;
        private Int64 informacao;
        private Node esquerda;
        private Node direita;

        public Int64 Informacao
        {
            get { return informacao; }
            set { informacao = value; }
        }
        
        public int Altura
        {
            get { return altura; }
            set { altura = value; }
        }
        
        public Node Esquerda
        {
            get { return esquerda; }
            set { esquerda = value; }
        }
        
        public Node Direita
        {
            get { return direita; }
            set { direita = value; }
        }

    }
}