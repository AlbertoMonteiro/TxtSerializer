using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TxtFileGenerator;

namespace Test
{
    [TestClass]
    public class DadoUmTxtSerializer
    {
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

            const string expected = 
@"|1|Alberto|22|
|1|10,9|
|2|11,9|";

            Assert.IsTrue(textSerialized.Contains(expected));
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
            
            const string expected =
@"|I10|1|Alberto|
|I20|2|Alberto1|
|I20|3|Alberto2|";

            var textSerialized = txtSerializer.Serialize(pessoa);
            Assert.IsTrue(textSerialized.Contains(expected));
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
