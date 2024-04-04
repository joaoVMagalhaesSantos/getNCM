using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using getNCM.Class;
using Newtonsoft.Json;

namespace getNCM.Class
{
    public class Ncm
    {
        public string? codigo { get; set; }
        public string? descricao { get; set; }
        public string? data_inicio { get; set; }
        public string? data_fim { get; set; }
        public string? tipo_ato_ini { get; set; }
        public string? nomero_ato_ini { get; set; }
        public int? ano_ato_ini { get; set; }

    }

    
}


public class GetNCM
{   
    public static async Task<List<Ncm>> ObterNomenclaturasAsync(string apiUrl)
    {
        List<Ncm> nomenclaturas = null;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    dynamic jsonObject = JsonConvert.DeserializeObject(json);

                    var nomenclaturasList = jsonObject.Nomenclaturas;

                    nomenclaturas = new List<Ncm>();

                    foreach (var nomenclatura in nomenclaturasList)
                    {
                        var ncm = JsonConvert.DeserializeObject<Ncm>(nomenclatura.ToString());

                        nomenclaturas.Add(ncm);
                    }

                }
                else
                {
                    Console.WriteLine("Erro ao acessar a API: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
        return nomenclaturas;
    }
}