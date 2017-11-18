using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;

namespace WebApi2.Models
{
    public class Cliente
    {
        postgresEntities DBConectionPrimaria = new postgresEntities(); //Objecto para acceder a la base primaria
        postgresEntities1 DBConectionSecundaria = new postgresEntities1(); //Objeto para acceder a la base secundaria

        //Metodo para obtener todos los clientes 
        public JObject TodosClientes() {
            JArray clientes = new JArray();
            JObject respuesta;
            JObject temp = new JObject();
            try { 
                dynamic data = DBConectionPrimaria.cliente.ToList(); //Coneccion con la base primaria 
                foreach (cliente c in data)
                {
                    temp.Add("nombre1", c.nombre1);
                    temp.Add("nombre2", c.nombre2);
                    temp.Add("apellido1", c.apellido1);
                    temp.Add("apellido2", c.apellido2);
                    temp.Add("cedula", c.cedula);
                    temp.Add("fNacimiento", c.fnacimiento);
                    temp.Add("provincia", c.provincia);
                    temp.Add("canton", c.canton);
                    temp.Add("distrito", c.distrito);
                    temp.Add("indicaciones", c.indicaciones);
                    clientes.Add(temp);
                    temp.RemoveAll();
                }
                respuesta = new JObject(
                        new JProperty("Clientes", clientes),
                        new JProperty("codigo", 200),
                        new JProperty("descripcion", "Exito")
                );
            }
            catch (Exception ex) { //Error en la base primaria, se intentara en la base secundaria
                Console.WriteLine(ex);
                try {
                    dynamic data = DBConectionSecundaria.cliente.ToList(); //Conexion con la base secundaria 
                    foreach (cliente c in data)
                    {
                        temp.Add("nombre1", c.nombre1);
                        temp.Add("nombre2", c.nombre2);
                        temp.Add("apellido1", c.apellido1);
                        temp.Add("apellido2", c.apellido2);
                        temp.Add("cedula", c.cedula);
                        temp.Add("fNacimiento", c.fnacimiento);
                        temp.Add("provincia", c.provincia);
                        temp.Add("canton", c.canton);
                        temp.Add("distrito", c.distrito);
                        temp.Add("indicaciones", c.indicaciones);
                        clientes.Add(temp);
                        temp.RemoveAll();
                    }
                    respuesta = new JObject(
                            new JProperty("Clientes", clientes),
                            new JProperty("codigo", 200),
                            new JProperty("descripcion", "Exito")
                    );
                }
                catch (Exception ex1) { //Error de conexion con la base secundaria 
                    Console.WriteLine(ex1); //Se imprime el error 
                    respuesta = new JObject(
                            new JProperty("codigo", 201),
                            new JProperty("descripcion", "Error")
                    );
                }
            }
            return respuesta;
        }

        //Metodo para obtener un cliente por numero de cedula 
        public JObject ObtenerCliente(int cedula)
        {
            JObject respuesta;
            dynamic cliente = DBConectionPrimaria.cliente.Find(cedula, 1); ;
            respuesta = new JObject(
                    new JProperty("Nombre1", cliente.nombre1),
                    new JProperty("Nombre2", cliente.nombre2),
                    new JProperty("Apellido1", cliente.apellido1),
                    new JProperty("Apellido2", cliente.apellido2),
                    new JProperty("Cedula", cliente.cedula),
                    new JProperty("FNacimiento", cliente.fnacimiento),
                    new JProperty("Provincia", cliente.provincia),
                    new JProperty("Canton", cliente.canton),
                    new JProperty("Canton", cliente.distrito),
                    new JProperty("Canton", cliente.canton),
                    new JProperty("codigo", 200),
                    new JProperty("descripcion", "Exito")
            );
            return respuesta;
        }

        //Metodo para insertar un Cliente 
        public JObject InsertarCliente(JObject x)
        {
            JObject respuesta;
            dynamic datos = x;
            int cedula = datos.cedula;
            Console.WriteLine(cedula);
            try {
                //Genera el objeto para insertar en la base primaria 
                DBConectionPrimaria.cliente.Add(new cliente
                { cedula = datos.cedula, nombre1 = datos.nombre1, nombre2 = datos.nombre2, apellido1 = datos.apellido1 
                  , apellido2 = datos.apellido2, fnacimiento = datos.fNacimiento, provincia = datos.provincia 
                  , canton = datos.canton , distrito = datos.distrito, indicaciones = datos.indicaciones
                  , telefono = datos.telefono, activo = 1
                });
                
                //Genera el objeto para insertar en la base secundaria (RAID)
                DBConectionSecundaria.cliente.Add(new cliente
                { cedula = datos.cedula, nombre1 = datos.nombre1, nombre2 = datos.nombre2, apellido1 = datos.apellido1
                  , apellido2 = datos.apellido2, fnacimiento = datos.fNacimiento, provincia = datos.provincia
                  , canton = datos.canton, distrito = datos.distrito, indicaciones = datos.indicaciones
                  , telefono = datos.telefono, activo = 1
                });

                DBConectionPrimaria.SaveChanges();//Inserta en la base primaria
                DBConectionSecundaria.SaveChanges();//Inserta en la base secundaria(RAID)
                respuesta = new JObject(
                    new JProperty("codigo", 200),
                    new JProperty("descripcion", "Exito")
                );
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                respuesta = new JObject(
                    new JProperty("codigo", 201),
                    new JProperty("descripcion", "Error")
                );
            }
            return respuesta;
        }
    }
}