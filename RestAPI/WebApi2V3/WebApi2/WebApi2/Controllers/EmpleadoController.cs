using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApi2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmpleadoController : ApiController
    {
        Empleado modelEmpleado = new Empleado();
        //Ingresan las peticiones Get
        public JObject Get(int x)
        {
            //dynamic z = Guid.NewGuid().ToString();
            //Console.Write(z);
            JObject respuesta = new JObject();
            respuesta = modelEmpleado.TodosEmpleados(x);
            return respuesta;
        }
        //Ingresan las peticiones Post
        public JObject Post(JObject x, int codigo) {
            dynamic data = x;
            JObject respuesta = new JObject();
            if (codigo == 1)//Comprobacion de Login
            {
                respuesta = modelEmpleado.Login((int)data.cedula, (string)data.pass);
            }
            else if (codigo == 2)//Obtener la informacion de un empleado
            {
                respuesta = modelEmpleado.ObtenerEmpleado((int)data.cedula);
            }
            else if (codigo == 3) {
                respuesta = modelEmpleado.InsertarEmpleado(data);
            }
            return respuesta;

        }
        //Ingresan las peticiones Put
        public JObject Put(JObject x) {
            dynamic data = x;
            JObject respuesta = modelEmpleado.ActualizarEmpleado(data);
            return respuesta;
        }
        //Ingresan las peticiones Delete
        public JObject Delete(int cedula) {
            JObject respuesta = modelEmpleado.EliminarEmpleado(cedula);
            return respuesta;
        }
    }
}
