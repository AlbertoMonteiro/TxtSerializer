﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TxtFileGenerator;

namespace Test
{
    [TestClass]
    public class DadoUmTxtSerializer
    {
        private string line = Environment.NewLine;

        [TestMethod]
        public void SerializaçãoDeObjetosDeveSerSeparadoPorPipe()
        {
            var txtSerializer = new TxtSerializer();

            var pessoa = new
            {
                Id = 1,
                Nome = "Alberto",
                Idade = 22
            };

            var textSerialized = txtSerializer.Serialize(pessoa);
            Assert.IsTrue(textSerialized.Contains("|1|Alberto|22|"));
        }

        [TestMethod]
        public void SerializaçãoDeObjetosDeveConsiderarPropriedadesComplexasNaLinhaAbaixo()
        {
            var txtSerializer = new TxtSerializer();

            var pessoa = new
            {
                Id = 1,
                Nome = "Alberto",
                Idade = 22,
                Casas = new List<object>
                {
                    new { Id=1,Tamanho = 10.9},
                    new { Id=2,Tamanho = 11.9}
                }
            };

            var textSerialized = txtSerializer.Serialize(pessoa);

            string expected = "|1|Alberto|22|" + line + 
                              "|1|10,9|" + line + 
                              "|2|11,9|" + line;

            Assert.AreEqual(expected, textSerialized);
        }

        [TestMethod]
        public void SerializaçãoDeObjetosDeveInserirOIndicadorDeRegistroAPartirDoAtributoTxtRegisterNameCasoHouver()
        {
            var txtSerializer = new TxtSerializer();

            var pessoa = new Pessoa
            {
                Id = 1,
                Nome = "Alberto",
                Pessoas = new List<Pessoa2>
                {
                    new Pessoa2 {Id = 2, Nome = "Alberto1"},
                    new Pessoa2 {Id = 3, Nome = "Alberto2"},
                }
            };

            var expected = "|I10|1|Alberto|" + line + "|I20|2|Alberto1|" + line + "|I20|3|Alberto2|" + line;

            var textSerialized = txtSerializer.Serialize(pessoa);
            Assert.IsTrue(textSerialized.Contains(expected));
        }

        [TestMethod]
        public void PossoGerarUmBloco()
        {
            var bloco = new Bloco0
            {
                Reg0001 = { IND_DAD = 0 }, 
                Reg0007 = { COD_ENT_REF = "NenhumaInscricao" }, 
                Reg0020 = { IND_DEC = IndicadorCentralizacao.Matriz }, 
                Reg0990 = { QdeDeLinhas = 1 }
            };

            var txtSerializer = new TxtSerializer();

            var serialize = txtSerializer.Serialize(bloco);

            string expected =
@"|0001|0|
|0007|NenhumaInscricao||
|0020|Matriz|||||||
|0990|
";
            Assert.AreEqual(expected,serialize);
        }

        [TxtRegisterName("I10")]
        class Pessoa
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public List<Pessoa2> Pessoas { get; set; }
        }

        [TxtRegisterName("I20")]
        class Pessoa2
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
    }
}
