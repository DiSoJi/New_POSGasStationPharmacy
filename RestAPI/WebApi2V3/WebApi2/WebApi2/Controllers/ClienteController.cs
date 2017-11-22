using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApi2.Models;
using System.Web.Http.Cors;

namespace PostGasStationFarmacy.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClienteController : ApiController
    {

        Cliente modelCliente = new Cliente(); 
       //Ingresan las peticiones Get
        public JObject Get() {
            JObject respuesta = new JObject();
            respuesta = modelCliente.TodosClientes();
            return respuesta;
        }
        //Ingresan las peticiones Post
        public JObject Post(int codigo, JObject x) {
            dynamic data = x;
            JObject respuesta = new JObject();
            if (codigo == 1) //Solicita un cliente especifico 
            {
                respuesta = modelCliente.ObtenerCliente((int)data.cedula);
            }
            else if (codigo == 2) //Inserta un cliente especifico 
            {
                respuesta = modelCliente.InsertarCliente(data);
            }
            return respuesta;
        }
        //Ingresan las peticiones Put
        public JObject Put(JObject x)
        {
            dynamic data = x;
            JObject respuesta = modelCliente.ActualizarCliente(data);
            return respuesta;
        }

        //Ingresan las peticiones Delete
        public JObject Delete(JObject x) {
            dynamic datos = x;
            int cedula = (int)datos.cedula;
            JObject respuesta = modelCliente.EliminarCliente(cedula);
            return respuesta;

        }
    }
}
