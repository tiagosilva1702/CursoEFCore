using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //InserirDados();
            //InserirEmMassa();
            //CadastrarPedido();
            //ConsultarAdiantado();
            //RemoverRegistro();
            //AtualizarDados();
        }

        public static void AtualizarDados(){
            using var db = new Date.ApplicationContext();
            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Tiago Santos";

            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        public static void RemoverRegistro(){
            using var db = new Date.ApplicationContext();
            var cliente = db.Clientes.Find(2);

            db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry<Cliente>(cliente).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public static void ConsultarAdiantado(){
            using var db = new Date.ApplicationContext();
            var pedidos = db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(p => p.Produto)
            .ToList();
        }

        public static void CadastrarPedido()
        {
            using var db = new Date.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = ValueObjects.StatusPedido.Analise,
                TipoFrete = ValueObjects.TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id, Desconto = 0, Quantidade = 1, Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        public static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Date.ApplicationContext();
            db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry<Produto>(produto).State = EntityState.Added;
            //db.Add(produto);

            var registro = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registro}");
        }

        public static void InserirEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Em Massa",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = ValueObjects.TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Tiago Silva",
                Telefone = "987192606",
                CEP = "41611540",
                Estado = "BA",
                Cidade = "Salvador"
            };

            using var db = new Date.ApplicationContext();
            db.AddRange(produto, cliente);

            var registro = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registro}");
        }

        public static void ConsultarDados()
        {
            using var db = new Date.ApplicationContext();
            var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();

            var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();
        }
    }
}
