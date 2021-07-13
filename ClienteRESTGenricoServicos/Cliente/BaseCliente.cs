using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ClienteRESTGenricoServicos.Interface;

namespace ClienteRESTGenricoServicos.Cliente
{
    public class BaseCliente<T> : HttpClient, IBaseCliente<T> where T : class
    {
               
        private string baseclientePath;
        private const string MEDIA_TYPE = "application/json";

        public BaseCliente(string baseAddress, string basePath)
        {
            BaseAddress = new Uri(baseAddress);
            this.baseclientePath = basePath;
        }
      
        public async Task<T> Get(int? id)
        {
            try
            {
                SetupHeaders();

                var response = await GetAsync(baseclientePath + $"/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var returnModel = JsonConvert.DeserializeObject<T>(result);

                    return returnModel;
                }
                else
                {
                    throw new Exception
                        ("Falha ao recuperar o id do item: " + id + $" returned " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                SetupHeaders();

                var response = await GetAsync(baseclientePath);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var returnModel = JsonConvert.DeserializeObject<List<T>>(result);

                    return returnModel;
                }
                else
                {
                    throw new Exception
                        ($"Falha ao recuperar os itens devolvidos {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Create(T item)
        {
            try
            {
                SetupHeaders();
                var serializedJson = GetSerializedObject(item);
                var bodyContent = GetBodyContent(serializedJson);

                var response = await PostAsync(baseclientePath, bodyContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception
                    ($"Falha ao criar o recurso devolvido {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Update(int? id, T item)
        {
            try
            {
                SetupHeaders();
                var serializedJson = GetSerializedObject(item);
                var bodyContent = GetBodyContent(serializedJson);

                var response = await PutAsync(baseclientePath + $"/{id}", bodyContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Falha ao atualizar o id do recurso: {id}, returned {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                SetupHeaders();

                var response = await DeleteAsync(baseclientePath + $"/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception
                    ($"Falha ao excluir o ID do recurso: {id}, returned {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region Métodos de ajuda do cliente
        protected virtual void SetupHeaders()
        {
            DefaultRequestHeaders.Clear();

            //Define request data format  
            DefaultRequestHeaders.Accept.Add
                (new MediaTypeWithQualityHeaderValue
                (MEDIA_TYPE));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected virtual string GetSerializedObject(object obj)
        {
            var serializedJson = JsonConvert.SerializeObject(obj);

            return serializedJson;
        }

        protected virtual StringContent GetBodyContent(string serializedJson)
        {
            var bodyContent = new StringContent
                (serializedJson, Encoding.UTF8, "application/json");

            return bodyContent;
        }
        #endregion


    }
}
