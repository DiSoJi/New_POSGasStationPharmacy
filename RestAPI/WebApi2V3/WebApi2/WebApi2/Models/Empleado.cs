using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApi2.Models
{
    public class Empleado
    {
        postgresEntities DBConectionPrimaria = new postgresEntities(); //Objecto para acceder a la base primaria
        postgresEntities1 DBConectionSecundaria = new postgresEntities1(); //Objeto para acceder a la base secundaria

        //Metodo para obtener todos los empleados por compania 
        public JObject TodosEmpleados(int idcomp)
        { 
            JArray empleados = new JArray();
            JObject respuesta;
            try
            {
                dynamic data = DBConectionPrimaria.empleado.Where(e => e.activo == 1 && e.idcompania==idcomp).ToList(); //Coneccion con la base primaria 
                foreach (empleado e in data)
                {
                    JObject temp = new JObject();
                    temp.Add("contraseña", e.contraseña);
                    temp.Add("sucursal", e.sucursal1.nombre);
                    temp.Add("compania", e.compania.nombre);
                    temp.Add("nombre1", e.nombre1);
                    temp.Add("nombre2", e.nombre2);
                    temp.Add("apellido1", e.apellido1);
                    temp.Add("apellido2", e.apellido2);
                    temp.Add("cedula", e.cedula);
                    temp.Add("fNacimiento", e.fnacimiento);
                    temp.Add("provincia", e.provincia);
                    temp.Add("canton", e.canton);
                    temp.Add("distrito", e.distrito);
                    temp.Add("indicaciones", e.indicaciones);
                    temp.Add("telefono", e.telefono);
                    empleados.Add(temp);
                }
                respuesta = new JObject(
                        new JProperty("Empleados", empleados),
                        new JProperty("codigo", 200),
                        new JProperty("descripcion", "Exito")
                );
            }
            catch (Exception ex)
            { //Error en la base primaria, se intentara en la base secundaria
                Console.WriteLine(ex);
                try
                {
                    dynamic data = DBConectionSecundaria.empleado.Where(e => e.activo == 1 && e.idcompania == idcomp).ToList(); //Coneccion con la base secundaria
                    foreach (empleado e in data)
                    {
                        JObject temp = new JObject();
                        temp.Add("contraseña", e.contraseña);
                        temp.Add("sucursal", e.sucursal1.nombre);
                        temp.Add("compania", e.compania.nombre);
                        temp.Add("nombre1", e.nombre1);
                        temp.Add("nombre2", e.nombre2);
                        temp.Add("apellido1", e.apellido1);
                        temp.Add("apellido2", e.apellido2);
                        temp.Add("cedula", e.cedula);
                        temp.Add("fNacimiento", e.fnacimiento);
                        temp.Add("provincia", e.provincia);
                        temp.Add("canton", e.canton);
                        temp.Add("distrito", e.distrito);
                        temp.Add("indicaciones", e.indicaciones);
                        temp.Add("telefono", e.telefono);
                        empleados.Add(temp);
                    }
                    respuesta = new JObject(
                            new JProperty("Empleados", empleados),
                            new JProperty("codigo", 200),
                            new JProperty("descripcion", "Exito")
                    );
                }
                catch (Exception ex1)
                { //Error de conexion con la base secundaria 
                    Console.WriteLine(ex1); //Se imprime el error 
                    respuesta = new JObject(
                            new JProperty("codigo", 201),
                            new JProperty("descripcion", "Error")
                    );
                }
            }
            return respuesta;
        }

        //Funcion encargada de revisar el login de empleados 
        //Le dice al HTML cual viste debe cargar (Admin/Cajero)
        public JObject Login(int ced, string pass) {
            JObject respuesta = new JObject();
            JArray roles = new JArray();
            try {
                //Conexion a la base primaria para obtener la informacion del empleado
                dynamic data = DBConectionPrimaria.empleado.Where(e => e.activo == 1 && e.cedula == ced && e.contraseña == pass).SingleOrDefault();
                //Conexion a la base primaria para obtener la informacion de los roles 
                dynamic rol_empleado = 0;
                if (data != null)
                { //Si el login es correcto compruba
                    rol_empleado = DBConectionPrimaria.rol_empleado.Where(r => r.cedulaempleado == ced && r.activo == 1).ToList();
                    int adm = 0;
                    int caj = 0;
                    foreach (rol_empleado r in rol_empleado)
                    {//Recorre los roles que pertenecen al empleado
                        if (r.rol.descripcion == "Administrador") //Si el empleado es Administrador
                        {
                            adm = 1;
                            //Se conecta a la base primaria para obtener la informacion de la sucursal que administra
                            dynamic sucursal = DBConectionPrimaria.sucursal.Where(s => s.activo == 1 && s.cedadmin == ced).SingleOrDefault();
                            //Datos referentes a la sucursal 
                            respuesta.Add("AdmSucursal", sucursal.nombre);
                            respuesta.Add("AdmSucursalID", sucursal.id);
                            respuesta.Add("AdmSucursalProvincia", sucursal.provincia);
                            respuesta.Add("AdmSucursalCanton", sucursal.canton);
                            respuesta.Add("AdmSucursalDistrito", sucursal.distrito);
                            respuesta.Add("AdmSucursalIndicaciones", sucursal.indicaciones);
                            respuesta.Add("AdmSucursalDescripcion", sucursal.descripcion);
                        }
                        else if (r.rol.descripcion == "Cajero") caj = 1; //Si el empleado es cajero
                        JObject temp = new JObject();
                        temp.Add("idrol", r.rol.id);
                        temp.Add("rol", r.rol.descripcion);
                        roles.Add(temp);
                    }
                    if (adm == 1 && caj == 1) respuesta.Add("tipo", 3); //Tipo administrador y dependiente
                    else if (adm == 1) respuesta.Add("tipo", 1);//Tipo administrador
                    else if (caj == 1) respuesta.Add("tipo", 2);//Tipo dependiente
                    else if (caj == 0 && adm == 0) respuesta.Add("tipo", 0);//Tipo no tiene acceso a vistas

                    respuesta.Add("roles", roles); //Inserta los roles
                                                   //Datos referentes al empleado
                    respuesta.Add("sucursal", data.sucursal1.nombre);
                    respuesta.Add("idsucursal", data.sucursal1.id);
                    respuesta.Add("compania", data.compania.nombre);
                    respuesta.Add("idcompania", data.compania.id);
                    respuesta.Add("nombre1", data.nombre1);
                    respuesta.Add("nombre2", data.nombre2);
                    respuesta.Add("apellido1", data.apellido1);
                    respuesta.Add("apellido2", data.apellido2);
                    respuesta.Add("cedula", data.cedula);
                    respuesta.Add("fNacimiento", data.fnacimiento);
                    respuesta.Add("provincia", data.provincia);
                    respuesta.Add("canton", data.canton);
                    respuesta.Add("distrito", data.distrito);
                    respuesta.Add("indicaciones", data.indicaciones);
                    respuesta.Add("telefono", data.telefono);
                    respuesta.Add("codigo", 200);
                    respuesta.Add("descripcion", "Exito");
                }
                else {
                    respuesta = new JObject(
                          new JProperty("codigo", 201),
                          new JProperty("descripcion", "Error")
                    );

                }
               
            }
            catch (Exception ex1) {
                Console.Write(ex1);
                //Conexion a la base secundaria para obtener la informacion del empleado
                dynamic data = DBConectionSecundaria.empleado.Where(e => e.activo == 1 && e.cedula == ced && e.contraseña == pass).SingleOrDefault();
                //Conexion a la base secundaria para obtener la informacion de los roles 
                dynamic rol_empleado = 0;
                if (data != null)
                { //Si el login es correcto compruba
                    rol_empleado = DBConectionSecundaria.rol_empleado.Where(r => r.cedulaempleado == ced && r.activo == 1).ToList();
                    int adm = 0;
                    int caj = 0;
                    foreach (rol_empleado r in rol_empleado)
                    {//Recorre los roles que pertenecen al empleado
                        if (r.rol.descripcion == "Administrador") //Si el empleado es Administrador
                        {
                            adm = 1;
                            //Se conecta a la base secundaria para obtener la informacion de la sucursal que administra
                            dynamic sucursal = DBConectionSecundaria.sucursal.Where(s => s.activo == 1 && s.cedadmin == ced).SingleOrDefault();
                            //Datos referentes a la sucursal 
                            respuesta.Add("AdmSucursal", sucursal.nombre);
                            respuesta.Add("AdmSucursalID", sucursal.id);
                            respuesta.Add("AdmSucursalProvincia", sucursal.provincia);
                            respuesta.Add("AdmSucursalCanton", sucursal.canton);
                            respuesta.Add("AdmSucursalDistrito", sucursal.distrito);
                            respuesta.Add("AdmSucursalIndicaciones", sucursal.indicaciones);
                            respuesta.Add("AdmSucursalDescripcion", sucursal.descripcion);
                        }
                        else if (r.rol.descripcion == "Cajero") caj = 1; //Si el empleado es cajero
                        JObject temp = new JObject();
                        temp.Add("idrol", r.rol.id);
                        temp.Add("rol", r.rol.descripcion);
                        roles.Add(temp);
                    }
                    if (adm == 1 && caj == 1) respuesta.Add("tipo", 3); //Tipo administrador y dependiente
                    else if (adm == 1) respuesta.Add("tipo", 1);//Tipo administrador
                    else if (caj == 1) respuesta.Add("tipo", 2);//Tipo dependiente
                    else if (caj == 0 && adm == 0) respuesta.Add("tipo", 0);//Tipo no tiene acceso a vistas

                    respuesta.Add("roles", roles); //Inserta los roles
                    //Datos referentes al empleado
                    respuesta.Add("sucursal", data.sucursal1.nombre);
                    respuesta.Add("idsucursal", data.sucursal1.id);
                    respuesta.Add("compania", data.compania.nombre);
                    respuesta.Add("idcompania", data.compania.id);
                    respuesta.Add("nombre1", data.nombre1);
                    respuesta.Add("nombre2", data.nombre2);
                    respuesta.Add("apellido1", data.apellido1);
                    respuesta.Add("apellido2", data.apellido2);
                    respuesta.Add("cedula", data.cedula);
                    respuesta.Add("fNacimiento", data.fnacimiento);
                    respuesta.Add("provincia", data.provincia);
                    respuesta.Add("canton", data.canton);
                    respuesta.Add("distrito", data.distrito);
                    respuesta.Add("indicaciones", data.indicaciones);
                    respuesta.Add("telefono", data.telefono);
                    respuesta.Add("codigo", 200);
                    respuesta.Add("descripcion", "Exito");
                }
                else
                {
                    respuesta = new JObject(
                          new JProperty("codigo", 201),
                          new JProperty("descripcion", "Error")
                    );

                }
            }
            return respuesta; 
        }

        //Metodo para obtener la informacion relacionada a un empleado
        public JObject ObtenerEmpleado(int ced)
        {
            JObject respuesta = new JObject();
            JArray roles = new JArray();
            try
            {
                //Conexion a la base primaria para obtener la informacion del empleado
                dynamic data = DBConectionPrimaria.empleado.Where(e => e.activo == 1 && e.cedula == ced).SingleOrDefault();
                //Conexion a la base primaria para obtener la informacion de los roles 
                dynamic rol_empleado = 0;
                if (data != null)
                { //Si encontro el empleado
                    rol_empleado = DBConectionPrimaria.rol_empleado.Where(r => r.cedulaempleado == ced && r.activo == 1).ToList();
                    foreach (rol_empleado r in rol_empleado)
                    {//Recorre los roles que pertenecen al empleado
                        JObject temp = new JObject();
                        temp.Add("idrol", r.rol.id);
                        temp.Add("rol", r.rol.descripcion);
                        roles.Add(temp);
                    }
                    //Inserta los roles
                    respuesta.Add("roles", roles);
                    //Datos referentes al empleado
                    respuesta.Add("contraseña", data.contraseña);
                    respuesta.Add("sucursal", data.sucursal1.nombre);
                    respuesta.Add("idsucursal", data.sucursal1.id);
                    respuesta.Add("compania", data.compania.nombre);
                    respuesta.Add("idcompania", data.compania.id);
                    respuesta.Add("nombre1", data.nombre1);
                    respuesta.Add("nombre2", data.nombre2);
                    respuesta.Add("apellido1", data.apellido1);
                    respuesta.Add("apellido2", data.apellido2);
                    respuesta.Add("cedula", data.cedula);
                    respuesta.Add("fNacimiento", data.fnacimiento);
                    respuesta.Add("provincia", data.provincia);
                    respuesta.Add("canton", data.canton);
                    respuesta.Add("distrito", data.distrito);
                    respuesta.Add("indicaciones", data.indicaciones);
                    respuesta.Add("telefono", data.telefono);
                    respuesta.Add("codigo", 200);
                    respuesta.Add("descripcion", "Exito");
                }
                else
                {
                    respuesta = new JObject(
                          new JProperty("codigo", 201),
                          new JProperty("descripcion", "Error")
                    );

                }

            }
            catch (Exception ex1)
            {
                Console.Write(ex1);
                //Conexion a la base secundaria para obtener la informacion del empleado
                dynamic data = DBConectionSecundaria.empleado.Where(e => e.activo == 1 && e.cedula == ced).SingleOrDefault();
                //Conexion a la base secundaria para obtener la informacion de los roles 
                dynamic rol_empleado = 0;
                if (data != null)
                { //Si encontro el empleado
                    rol_empleado = DBConectionSecundaria.rol_empleado.Where(r => r.cedulaempleado == ced && r.activo == 1).ToList();
                    foreach (rol_empleado r in rol_empleado)
                    {//Recorre los roles que pertenecen al empleado
                        JObject temp = new JObject();
                        temp.Add("idrol", r.rol.id);
                        temp.Add("rol", r.rol.descripcion);
                        roles.Add(temp);
                    }
                    //Inserta los roles
                    respuesta.Add("roles", roles);
                    //Datos referentes al empleado
                    respuesta.Add("contraseña", data.contraseña);
                    respuesta.Add("sucursal", data.sucursal1.nombre);
                    respuesta.Add("compania", data.compania.nombre);
                    respuesta.Add("idcompania", data.compania.id);
                    respuesta.Add("nombre1", data.nombre1);
                    respuesta.Add("nombre2", data.nombre2);
                    respuesta.Add("apellido1", data.apellido1);
                    respuesta.Add("apellido2", data.apellido2);
                    respuesta.Add("cedula", data.cedula);
                    respuesta.Add("fNacimiento", data.fnacimiento);
                    respuesta.Add("provincia", data.provincia);
                    respuesta.Add("canton", data.canton);
                    respuesta.Add("distrito", data.distrito);
                    respuesta.Add("indicaciones", data.indicaciones);
                    respuesta.Add("telefono", data.telefono);
                    respuesta.Add("codigo", 200);
                    respuesta.Add("descripcion", "Exito");
                }
                else
                {
                    respuesta = new JObject(
                          new JProperty("codigo", 201),
                          new JProperty("descripcion", "Error")
                    );

                }
            }
            return respuesta;
        }

        //Metodo para eliminar un empleado (activo = 0)
        public JObject EliminarEmpleado(int cedula) {
            JObject respuesta;
            try
            {
                dynamic empleado = DBConectionPrimaria.empleado.Where(e => e.activo == 1 && e.cedula == cedula).SingleOrDefault(); //Se obtiene el empleado de la base primaria
                //Si se elimina un empleado que es administrador de una sucursal se debe setear tambien el atributo cedadmin al administrador default (cedadmin = 0) 
                dynamic sucursal = DBConectionPrimaria.sucursal.Where(s => s.activo == 1 && s.cedadmin == cedula).SingleOrDefault(); //Se obtiene la sucursal de la base primaria
                if (sucursal != null) {
                    sucursal.cedadmin = 0;
                }
                //Se setean los nuevos parametros
                empleado.activo = 0;
                dynamic empleado1 = DBConectionSecundaria.cliente.Where(c => c.activo == 1 && c.cedula == cedula).SingleOrDefault(); //Se obtiene el empleado de la base secundaria
                //Si se elimina un empleado que es administrador de una sucursal se debe setear tambien el atributo cedadmin al administrador default (cedadmin = 0) 
                dynamic sucursal1 = DBConectionSecundaria.sucursal.Where(s => s.activo == 1 && s.cedadmin == cedula).SingleOrDefault(); //Se obtiene la sucursal de la base primaria
                if (sucursal != null)
                {
                    //Se setean los nuevos parametros
                    sucursal.cedadmin = 0;
                }
                //Se setean los nuevos parametros
                empleado1.activo = 0;
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

        //Metodo para actualizar la informacion de un empleado
        public JObject ActualizarEmpleado(JObject x) {
            dynamic datos = x;
            JObject respuesta;
            int cedula = (int)datos.cedula;
            try
            {
                dynamic empleado = DBConectionPrimaria.empleado.Where(e => e.activo == 1 && e.cedula == cedula).SingleOrDefault(); ; //Se obtiene el objeto de la base primaria
                //Se setean los nuevos parametros
                empleado.nombre1 = (string)datos.nombre1;
                empleado.nombre2 = (string)datos.nombre2;
                empleado.apellido1 = (string)datos.apellido1;
                empleado.apellido2 = (string)datos.apellido2;
                empleado.fnacimiento = (DateTime)datos.fNacimiento;
                empleado.provincia = (string)datos.provincia;
                empleado.canton = (string)datos.canton;
                empleado.distrito = (string)datos.distrito;
                empleado.indicaciones = (string)datos.indicaciones;
                empleado.telefono = (int)datos.telefono;
                empleado.contraseña = (string)datos.contraseña;
                empleado.idsucursal = (int)datos.idsucursal;
                dynamic empleado1 = DBConectionSecundaria.empleado.Where(e => e.activo == 1 && e.cedula == cedula).SingleOrDefault(); ; //Se obtiene el objecto de la base secundaria
                //Se setean los nuevos parametros
                empleado1.nombre1 = (string)datos.nombre1;
                empleado1.nombre2 = (string)datos.nombre2;
                empleado1.apellido1 = (string)datos.apellido1;
                empleado1.apellido2 = (string)datos.apellido2;
                empleado1.fnacimiento = (DateTime)datos.fNacimiento;
                empleado1.provincia = (string)datos.provincia;
                empleado1.canton = (string)datos.canton;
                empleado1.distrito = (string)datos.distrito;
                empleado1.indicaciones = (string)datos.indicaciones;
                empleado1.telefono = (int)datos.telefono;
                empleado1.contraseña = (string)datos.contraseña;
                empleado1.idsucursal = (int)datos.idsucursal;
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

        public JObject InsertarEmpleado(JObject x) {
            JObject respuesta;
            dynamic datos = x;
            try
            {
                //Genera el objeto para insertar en la base primaria 
                DBConectionPrimaria.empleado.Add(new empleado
                {
                    cedula = datos.cedula,
                    contraseña = datos.contraseña,
                    idsucursal = datos.idsucursal,
                    idcompania = datos.idcompania,
                    nombre1 = datos.nombre1,
                    nombre2 = datos.nombre2,
                    apellido1 = datos.apellido1
                  ,
                    apellido2 = datos.apellido2,
                    fnacimiento = datos.fNacimiento,
                    provincia = datos.provincia
                  ,
                    canton = datos.canton,
                    distrito = datos.distrito,
                    indicaciones = datos.indicaciones
                  ,
                    telefono = datos.telefono,
                    activo = 1
                });

                //Genera el objeto para insertar en la base secundaria (RAID)
                DBConectionSecundaria.empleado.Add(new empleado
                {
                    cedula = datos.cedula,
                    contraseña = datos.contraseña,
                    idsucursal = datos.idsucursal,
                    idcompania = datos.idcompania,
                    nombre1 = datos.nombre1,
                    nombre2 = datos.nombre2,
                    apellido1 = datos.apellido1
                  ,
                    apellido2 = datos.apellido2,
                    fnacimiento = datos.fNacimiento,
                    provincia = datos.provincia
                  ,
                    canton = datos.canton,
                    distrito = datos.distrito,
                    indicaciones = datos.indicaciones
                  ,
                    telefono = datos.telefono,
                    activo = 1
                });

                DBConectionPrimaria.SaveChanges();//Inserta en la base primaria
                DBConectionSecundaria.SaveChanges();//Inserta en la base secundaria(RAID)
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