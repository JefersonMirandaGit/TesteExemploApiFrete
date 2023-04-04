using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;



namespace ConsoleApiFrete
{
    public class Frete
    {
        public string CodigoFrete { get; set; }
        public bool Ativo { get; set; }
        public int PermiteValor { get; set; }
        public bool NecessitaTranportadora { get; set; }
        public string DescricaoPortugues { get; set; }
        public string DescricaoIngles { get; set; }
        public string DescricaoEspanhol { get; set; }
            
        public Frete()
        {

        }
        /// <summary>
        /// Controe objeto frete
        /// </summary>
        /// <param name="codigoFrete">Utilizar o codigo de frete "CIF" ou "FOB"</param>
        /// <param name="descricaoPortugues">Descrição em Portugues</param>
        /// <param name="descricaoIngles">Descrição em Ingles</param>
        /// <param name="descricaoEspanhol">Descrição em Espanhol</param>
        /// <param name="permiteValor">PermiteValor Indica se o frete permite valor: 0 - Não permite, 1 - Permite(opcional), 2 - Permite(obrigatório)</param>
        /// 
        public Frete(string codigoFrete, string descricaoPortugues, string descricaoIngles, string descricaoEspanhol, int permiteValor = 2)
        {

            Ativo = true;
            NecessitaTranportadora = true;
            CodigoFrete = codigoFrete;
            DescricaoPortugues = descricaoPortugues;
            DescricaoIngles = descricaoIngles;
            DescricaoEspanhol = descricaoEspanhol;

        }

        public void SetDescricaoPortugues(string descricao)
        {
            if (string.IsNullOrEmpty(descricao) || descricao.Length < 5 || descricao.Length > 50)
            {
                new InvalidOperationException("Atenção parametro invalidos!\nDescrição Português não pode ser nulo. Menor que 10 e maior que 50 caracteres. ");
            }

        }


    }
    public class RetornoPostFrete
    {
        public int Status { get; set; }
        public string Mensagem { get; set; }
        public int Id { get; set; }
    }



    public class UserApi
    {
        public string Usuario { get; set; } = "Teste";
        public string Senha { get; set; } = "senhaProcessoSeletivo@ibid";
    }
    public class TokenApi
    {
        public string Token { get; set; }
        public DateTime Expirationexpiration { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine(ObterToken().Result.Token);
            PostAsync();
          
            Console.Write("Post:" );
            RunAsync();
            Console.Write("Get:" );
            Console.ReadKey();
           
        }

        static void ShowFrete(Frete frete)
        {
            //Console.WriteLine(ObterToken().Result.Token);          
        }

        static async Task<TokenApi> ObterToken()
        {
            var user = new UserApi();
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            client.BaseAddress = new System.Uri("https://wsibid.portaldecompras.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = await client.PostAsync("API/v1/Token", content);

            return JsonConvert.DeserializeObject<TokenApi>(response.Content.ReadAsStringAsync().Result);

        }

        static async Task RunAsync()
        {
            var tokenApi = ObterToken();
            var client = new HttpClient();


            client.BaseAddress = new System.Uri("https://wsibid.portaldecompras.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenApi.Result.Token);
            HttpResponseMessage response = await client.GetAsync("api/v1/Cadastro/Frete/?codigoERP=FOB");

            if (response.IsSuccessStatusCode)
            {  //GET

                var json = await response.Content.ReadAsStringAsync();
                var frete = JsonConvert.DeserializeObject<Frete>(json);
                Console.WriteLine(JsonConvert.SerializeObject(frete));
                Console.ReadKey();
            }

        }

        static async Task PostAsync()
        {
            var tokenApi = ObterToken();
            var client = new HttpClient();
            var frete = new Frete("FOB", "Teste02", "Test02", "Prueba");


            var content = new StringContent(JsonConvert.SerializeObject(frete), Encoding.UTF8, "application/json");


            client.BaseAddress = new System.Uri("https://wsibid.portaldecompras.co/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenApi.Result.Token);
            HttpResponseMessage response = await client.PostAsync("api/v1/Cadastro/Frete", content);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                var retornoPostFrete = JsonConvert.DeserializeObject<RetornoPostFrete>(json);
                Console.WriteLine(JsonConvert.SerializeObject(retornoPostFrete));
                Console.ReadKey();
            }
        }



    }
}
