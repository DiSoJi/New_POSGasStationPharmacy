using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApi2.Models;

namespace PostGasStationFarmacy.Controllers
{
    public class ClienteController : ApiController
    {

        Cliente modelCliente = new Cliente();
        public JObject Get() {
            JObject respuesta = new JObject();
            respuesta = modelCliente.TodosClientes();
            return respuesta;
        }

        public JObject Post(int codigo, JObject x) {
            dynamic data = x;
            int cedula = data.cedula;
            Console.WriteLine(cedula);
            JObject respuesta = new JObject();
            if (codigo == 1) //Solicita un cliente especifico 
            {
                respuesta = modelCliente.ObtenerCliente(data.cedula);
            }
            else if (codigo == 2) //Inserta un cliente especifico 
            {
                respuesta = modelCliente.InsertarCliente(data);
            }
            return respuesta;


        }
    }
}
