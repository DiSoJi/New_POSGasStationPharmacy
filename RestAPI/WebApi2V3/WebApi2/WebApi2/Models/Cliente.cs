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
            
            try { 
                dynamic data = DBConectionPrimaria.cliente.Where(c => c.activo == 1).ToList(); //Coneccion con la base primaria 
                foreach (cliente c in data)
                {
                    JObject temp = new JObject();
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
                    temp.Add("telefono", c.telefono);
                    clientes.Add(temp);
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
                    dynamic data = DBConectionSecundaria.cliente.Where(c => c.activo == 1).ToList(); //Conexion con la base secundaria 
                    foreach (cliente c in data)
                    {
                        JObject temp = new JObject();
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
                        temp.Add("telefono", c.telefono);
                        clientes.Add(temp);
                      
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
            try {
                //dynamic cliente = DBConectionPrimaria.cliente.Find(cedula, 1);
                dynamic cliente = DBConectionPrimaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault();
                respuesta = new JObject(
                   new JProperty("nombre1", cliente.nombre1),
                   new JProperty("nombre2", cliente.nombre2),
                   new JProperty("apellido1", cliente.apellido1),
                   new JProperty("apellido2", cliente.apellido2),
                   new JProperty("cedula", cliente.cedula),
                   new JProperty("fNacimiento", cliente.fnacimiento),
                   new JProperty("provincia", cliente.provincia),
                   new JProperty("canton", cliente.canton),
                   new JProperty("distrito", cliente.distrito),
                   new JProperty("indicaciones", cliente.indicaciones),
                   new JProperty("telefono", cliente.telefono),
                   new JProperty("codigo", 200),
                   new JProperty("descripcion", "Exito")
                );
                
            }
            catch (Exception ex) {
                Console.WriteLine(ex); //Se imprime el error 
                try {
                    dynamic cliente = DBConectionSecundaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault();
                    respuesta = new JObject(
                       new JProperty("nombre1", cliente.nombre1),
                       new JProperty("nombre2", cliente.nombre2),
                       new JProperty("apellido1", cliente.apellido1),
                       new JProperty("apellido2", cliente.apellido2),
                       new JProperty("cedula", cliente.cedula),
                       new JProperty("fNacimiento", cliente.fnacimiento),
                       new JProperty("provincia", cliente.provincia),
                       new JProperty("canton", cliente.canton),
                       new JProperty("distrito", cliente.distrito),
                       new JProperty("indicaciones", cliente.indicaciones),
                       new JProperty("telefono", cliente.telefono),
                       new JProperty("codigo", 200),
                       new JProperty("descripcion", "Exito")
                    );

                } catch (Exception ex1) {
                    Console.WriteLine(ex1); //Se imprime el error 
                    respuesta = new JObject(
                            new JProperty("codigo", 201),
                            new JProperty("descripcion", "Error")
                    );
                }
            }
            return respuesta;
        }

        //Metodo para insertar un Cliente 
        public JObject InsertarCliente(JObject x)
        {
            JObject respuesta;
            dynamic datos = x;
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

        //Metodo para actualizar un Cliente 
        public JObject ActualizarCliente(JObject x)
        {
            JObject respuesta;
            dynamic datos = x;
            int cedula = (int)datos.cedula;
            try
            {
                dynamic cliente = DBConectionPrimaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault(); ; //Se obtiene el objeto de la base primaria
                //Se setean los nuevos parametros
                cliente.nombre1 = (string)datos.nombre1;
                cliente.nombre2 = (string)datos.nombre2;
                cliente.apellido1 = (string)datos.apellido1;
                cliente.apellido2 = (string)datos.apellido2;
                cliente.fnacimiento = (DateTime)datos.fNacimiento;
                cliente.provincia = (string)datos.provincia;
                cliente.canton = (string)datos.canton;
                cliente.distrito = (string)datos.distrito;
                cliente.indicaciones = (string)datos.indicaciones;
                cliente.telefono = (int)datos.telefono;
                dynamic cliente1 = DBConectionSecundaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault(); ; //Se obtiene el objecto de la base secundaria
                //Se setean los nuevos parametros
                cliente1.nombre1 = (string)datos.nombre1;
                cliente1.nombre2 = (string)datos.nombre2;
                cliente1.apellido1 = (string)datos.apellido1;
                cliente1.apellido2 = (string)datos.apellido2;
                cliente1.fnacimiento = (DateTime)datos.fNacimiento;
                cliente1.provincia = (string)datos.provincia;
                cliente1.canton = (string)datos.canton;
                cliente1.distrito = (string)datos.distrito;
                cliente1.indicaciones = (string)datos.indicaciones;
                cliente1.telefono = (int)datos.telefono;
                //Guardar cambios 
                DBConectionPrimaria.SaveChanges(); //Se guardan los cambios en la base de datos primaria 
                DBConectionSecundaria.SaveChanges(); //Se guardan los cambios en la base de datos secundaria(RAID)
                respuesta = new JObject(
                    new JProperty("codigo", 200),
                    new JProperty("descripcion", "Exito")
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                respuesta = new JObject(
                    new JProperty("codigo", 201),
                    new JProperty("descripcion", "Error")
                );
            }
            return respuesta;
        }

        //Metodo para Eliminar un Cliente 
        public JObject EliminarCliente(int cedula)
        {
            JObject respuesta;
            try
            {
                dynamic cliente = DBConectionPrimaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault(); //Se obtiene el objeto de la base primaria
                //Se setean los nuevos parametros
                cliente.activo = 0;
                dynamic cliente1 = DBConectionSecundaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault(); //Se obtiene el objecto de la base secundaria
                //Se setean los nuevos parametros
                cliente1.activo = 0;
                DBConectionPrimaria.SaveChanges(); //Se guardan los cambios en la base de datos primaria 
                DBConectionSecundaria.SaveChanges(); //Se guardan los cambios en la base de datos secundaria(RAID)
                respuesta = new JObject(
                    new JProperty("codigo", 200),
                    new JProperty("descripcion", "Exito")
                );
            }
            catch (Exception ex)
            {
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